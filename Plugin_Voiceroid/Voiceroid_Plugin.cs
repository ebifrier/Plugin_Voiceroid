using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

using FNF;
using FNF.Utility;
using FNF.BouyomiChanApp;
using FNF.XmlSerializerSetting;

namespace Plugin_Voiceroid
{
    /// <summary>
    /// 月読アイ、ショウタなどにしゃべらせるためのプラグインです。
    /// </summary>
    public sealed class Voiceroid_Plugin : IPlugin
    {
        /// <summary>
        /// 設定ファイルの保存場所です。
        /// </summary>
        private readonly string SettingFilePath = Path.Combine(
            Base.CallAsmPath,
            Base.CallAsmName + ".settings");

        private SettingFormData settingFormData;
        //private bool isSpeek = false;

        /// <summary>
        /// プラグインの説明を取得します。
        /// </summary>
        public string Caption
        {
            get
            {
                return "Voiceroid(月読アイ、ショウタなど)にしゃべってもらいます。";
            }
        }

        /// <summary>
        /// プラグイン名を取得します。
        /// </summary>
        public string Name
        {
            get
            {
                return "VoiceroidTalk";
            }
        }

        /// <summary>
        /// 設定用データ？を取得します。
        /// </summary>
        public ISettingFormData SettingFormData
        {
            get
            {
                return this.settingFormData;
            }
        }

        /// <summary>
        /// バージョンを取得します。
        /// </summary>
        public string Version
        {
            get
            {
                return "2012/05/31版";
            }
        }

        /// <summary>
        /// プラグインを開始します。
        /// </summary>
        public void Begin()
        {
            // 設定ファイルを読み込みます。
            Settings.Default.Load(SettingFilePath);

            this.settingFormData = new SettingFormData();

            Pub.FormMain.BC.TalkTaskStarted += BC_TalkTaskStarted;
        }

        /// <summary>
        /// プラグインを終了します。
        /// </summary>
        public void End()
        {
            if (Settings.Default != null)
            {
                Settings.Default.Save(SettingFilePath);
            }

            Pub.FormMain.BC.TalkTaskStarted -= BC_TalkTaskStarted;
            //this.isSpeek = false;
        }

        /// <summary>
        /// 受け取ったトークタスクを再生します。
        /// </summary>
        void BC_TalkTaskStarted(object sender, BouyomiChan.TalkTaskStartedEventArgs e)
        {
            if (e == null || string.IsNullOrEmpty(e.ReplaceWord))
            {
                return;
            }

            Settings settings = Settings.Default;
            if (settings != null && !settings.IsUseVoiceroid)
            {
                return;
            }

            // 再生タグが含まれていれば再生しません。
            if (HasPlayTag(e.ConvertUnified))
            {
                return;
            }

            try
            {
                string sourceText = e.ReplaceWord;

                Voiceroid voiceroid =
                    VoiceroidList.DetectVoiceroid(sourceText)
                    ?? (settings != null ? settings.DefaultVoiceroid : null);

                // タグによる読み上げを実行します。
                if (voiceroid != null)
                {
                    // すべてのボイスタグを削除します。
                    // 例) "ai) yukari) てすと" などの場合に対応。
                    sourceText = VoiceroidList.RemoveVoiceTag(sourceText);

                    // 発声に成功した場合は e.Cancel=true となります。
                    if (Talk(voiceroid, sourceText, e))
                    {
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Add(
                    "ERROR",
                    ex.Message,
                    LogType.Error);
            }
        }

        /// <summary>
        /// 再生タグの判別に使います。
        /// </summary>
        private static readonly Regex PlayTagRegex = new Regex(
            @"再生[(（].*[)）]", RegexOptions.IgnoreCase);

        /// <summary>
        /// 再生タグが含まれているか調べます。
        /// </summary>
        private bool HasPlayTag(string sourceText)
        {
            if (string.IsNullOrEmpty(sourceText))
            {
                return false;
            }

            return PlayTagRegex.IsMatch(sourceText);
        }

        /// <summary>
        /// 読み上げ時の待ち時間を計算します。
        /// </summary>
        private TimeSpan GetVoiceWaitTime(BouyomiChan.TalkTaskStartedEventArgs e)
        {
            int restCount = e.RestTalkTaskCount;

            /*if (restCount == 0)
            {
                return TimeSpan.FromMilliseconds(300);
            }*/
            if (restCount <= 2)
            {
                return TimeSpan.FromMilliseconds(250);
            }
            if (restCount <= 4)
            {
                return TimeSpan.FromMilliseconds(200);
            }
            if (restCount <= 7)
            {
                return TimeSpan.FromMilliseconds(150);
            }
            if (restCount <= 10)
            {
                return TimeSpan.FromMilliseconds(100);
            }
            
            return TimeSpan.FromMilliseconds(50);
        }

        /// <summary>
        /// 指定のボイスロイドで読み上げできるか試してみます。
        /// </summary>
        private bool Talk(Voiceroid voiceroid, string text,
                          BouyomiChan.TalkTaskStartedEventArgs e)
        {
            if (voiceroid == null)
            {
                return false;
            }

            if (voiceroid.PrepareTalk())
            {
                // 棒読みちゃんのおしゃべりをAIタンが話している間は
                // 止めておきます。そうしないと同時再生されてしまいます。
                e.Cancel = true;
                Pub.Pause = true;

                // しゃべらせます。
                voiceroid.Talk(text);

                // しゃべっている間は、棒読みちゃんに読ませません。
                TimeSpan wait = GetVoiceWaitTime(e);
                Thread.Sleep((int)(wait.TotalMilliseconds * text.Length));

                /*Log.Add(string.Format("{0} wait={1}ms",
                    e.RestTalkTaskCount,
                    (int)wait.TotalMilliseconds));*/

                // 一時停止を元に戻します。
                Pub.Pause = false;
                return true;
            }

            return false;
        }
    }
}
