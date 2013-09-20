using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Plugin_Voiceroid
{
    /// <summary>
    /// クリップボードはSTAスレッド上からアクセスする必要があるので、
    /// それを行うためのクラスです。
    /// </summary>
    internal static class ClipboardUtil
    {
        private static readonly object SyncRoot = new object();
        private static Thread clipboardThread;
        private static KeyValuePair<string, object>? pasteData;
        private static KeyValuePair<string, object>? oldData = null;
        private static bool processing = false;

        /// <summary>
        /// クリップボードの保存＆復元に対応するフォーマットリストです。
        /// </summary>
        /// <remarks>
        /// すべてのデータを保存＆復元するためには、
        /// Streamを使ってデータを保存する必要があるらしい。
        /// そんな面倒なことはしたくないので、以下のファイルのみに
        /// 復元機能を対応させることにする。
        /// </remarks>
        public static readonly string[] SupportedFormats = new string[]
        {
            DataFormats.Bitmap,
            DataFormats.Text,
            DataFormats.WaveAudio,
            DataFormats.FileDrop,
        };

        /// <summary>
        /// クリップボードに文字列をコピーします。
        /// </summary>
        public static KeyValuePair<string, object>? SetData(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            return SetData(new KeyValuePair<string, object>(
                DataFormats.UnicodeText, text));
        }

        /// <summary>
        /// クリップボードに文字列をコピーします。
        /// </summary>
        public static KeyValuePair<string, object>? SetData(
            KeyValuePair<string, object>? data)
        {
            if (data == null)
            {
                return null;
            }

            lock (SyncRoot)
            {
                pasteData = data;

                processing = true;
                Monitor.PulseAll(SyncRoot);

                // 処理が終わるまで待ちます。
                while (processing)
                {
                    Monitor.Wait(SyncRoot);
                }

                return oldData;
            }
        }

        private static KeyValuePair<string, object>? GetDataInternal()
        {
            try
            {
                // 古いデータを取得し、後で戻せるようにします。
                var dataObject = Clipboard.GetDataObject();
                if (dataObject == null)
                {
                    return null;
                }

                // 対応するフォーマットが見つかれば、その形式で保存します。
                foreach (string dataFormat in dataObject.GetFormats(true))
                {
                    if (Array.IndexOf(SupportedFormats, dataFormat) >= 0)
                    {
                        return new KeyValuePair<string, object>(
                            dataFormat,
                            dataObject.GetData(dataFormat));
                    }
                }
            }
            catch (ExternalException)
            {
            }

            return null;
        }

        private static void SetDataInternal(KeyValuePair<string, object>? data)
        {
            if (data == null)
            {
                return;
            }

            try
            {
                KeyValuePair<string, object> value = data.Value;

                Clipboard.SetData(value.Key, value.Value);
            }
            catch (ExternalException)
            {
            }
        }

        /// <summary>
        /// クリップボードにデータをコピーするためのスレッド関数です。
        /// </summary>
        /// <remarks>
        /// 棒読みちゃんがSTAThreadではないので、クリップボードを扱うには
        /// 別スレッドで行う必要があります。
        /// </remarks>
        private static void ClipboardMain(object state)
        {
            while (true)
            {
                lock (SyncRoot)
                {
                    try
                    {
                        // データが設定されるまで待ちます。
                        while (!processing)
                        {
                            Monitor.Wait(SyncRoot);
                        }

                        // 古いデータを取得し、後で戻せるようにします。
                        oldData = GetDataInternal();

                        // クリップボードにデータをコピー
                        SetDataInternal(pasteData);
                    }
                    catch (ThreadAbortException)
                    {
                        break;
                    }
                    catch
                    {
                        // 無視
                    }

                    // 処理の終了を伝えます。
                    processing = false;
                    Monitor.PulseAll(SyncRoot);
                }
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static ClipboardUtil()
        {
            clipboardThread = new Thread(ClipboardMain)
            {
                Name = "クリップボード",
                IsBackground = true,
                Priority = ThreadPriority.AboveNormal,
            };

            // アパートメントはシングルスレッドに。
            clipboardThread.SetApartmentState(ApartmentState.STA);

            // スレッド開始
            clipboardThread.Start();
        }
    }
}
