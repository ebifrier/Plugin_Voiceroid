using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Runtime.InteropServices;

namespace Plugin_Voiceroid
{
    /// <summary>
    /// 各ボイスロイドから発声させるためのクラスです。
    /// </summary>
    public sealed class Voiceroid
    {
        #region PInvoke
        private const uint WM_NULL = 0x0000;
        private const uint WM_COMMAND = 0x0111;
        private const uint BM_CLICK = 0x00f5;

        [return: MarshalAs(UnmanagedType.Bool)]
        delegate bool Win32Callback(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll")]
        static extern IntPtr GetMenu(IntPtr hWnd);
        [DllImport("user32.dll")]
        static extern uint GetMenuItemID(IntPtr hMenu, int nPos);
        [DllImport("user32.dll")]
        static extern IntPtr GetSubMenu(IntPtr hMenu, int nPos);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr FindWindowEx(IntPtr hWnd, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnumChildWindows(IntPtr parentHandle, Win32Callback callback, IntPtr lParam);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        static extern bool SendMessage(IntPtr hWnd, uint Msg, Int32 wParam, Int32 lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, string lParam);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, Int32 wParam, Int32 lParam);
        #endregion

        /// <summary>
        /// ボイスロイドの一覧テーブルです。
        /// </summary>
        public static Voiceroid[] VoiceroidTable = new Voiceroid[]
        {
            VoiceroidFactory.CreateAi(),
            VoiceroidFactory.CreateShouta(),
            VoiceroidFactory.CreateTomoe(),
            VoiceroidFactory.CreateYukari(),
            VoiceroidFactory.CreateZunko(),
            VoiceroidFactory.CreateAkane(),
            VoiceroidFactory.CreateAoi(),
        };

        private IntPtr windowHandle = IntPtr.Zero;
        private IntPtr playButtonHandle = IntPtr.Zero;
        private IntPtr stopButtonHandle = IntPtr.Zero;
        private IntPtr textBoxHandle = IntPtr.Zero;
        private uint undoMenuId = 0;
        private uint pasteMenuId = 0;
        private int handleCount = 0;

        /// <summary>
        /// 情報を取得します。
        /// </summary>
        public VoiceroidInfo Info
        {
            get;
            private set;
        }

        /// <summary>
        /// ボタンのクラス名が正しいか調べます。
        /// </summary>
        private bool ValidateClassName(IntPtr hWnd, string caption, string className)
        {
            StringBuilder sbStr = new StringBuilder(64);
            StringBuilder sbClassName = new StringBuilder(256);

            if (!string.IsNullOrEmpty(caption))
            {
                GetWindowText(hWnd, sbStr, sbStr.Capacity);
                if (sbStr.ToString() != caption)
                {
                    return false;
                }
            }

            // 戻り値は値の文字数。
            if (GetClassName(hWnd, sbClassName, sbClassName.Capacity) == 0)
            {
                return false;
            }

            return (sbClassName.ToString() == className);
        }

        /// <summary>
        /// 再生ボタン・停止ボタンを検索します。
        /// </summary>
        private bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam)
        {
            if (this.handleCount == Info.PlayButtonNo ||
                this.handleCount == Info.PlayButtonNo + 1)
            {
                if (ValidateClassName(hWnd, Info.PlayButtonCaption, Info.ButtonClassName))
                {
                    this.playButtonHandle = hWnd;
                }
            }
            
            if (this.handleCount == Info.StopButtonNo ||
                this.handleCount == Info.StopButtonNo + 1)
            {
                if (ValidateClassName(hWnd, Info.StopButtonCaption, Info.ButtonClassName))
                {
                    this.stopButtonHandle = hWnd;
                }
            }
            
            if (this.handleCount == Info.EditBoxNo)
            {
                if (ValidateClassName(hWnd, null, Info.EditBoxClassName))
                {
                    this.textBoxHandle = hWnd;
                }
            }

            this.handleCount += 1;
            return true;
        }

        /// <summary>
        /// 与えられた文字列にボイスタグが含まれているか調べます。
        /// </summary>
        public int FindVoiceTag(string text)
        {
            Match m = Info.TagRegex.Match(text);

            return (m.Success ? m.Index : -1);
        }

        /// <summary>
        /// 与えられた文字列にボイスタグが含まれていたらそれを削除します。
        /// </summary>
        public string RemoveVoiceTag(string text)
        {
            Match m = Info.TagRegex.Match(text);
            if (!m.Success)
            {
                return text;
            }

            return text.Remove(m.Index, m.Length);
        }

        /// <summary>
        /// 発声の前にウィンドウハンドルの取得などを行います。
        /// </summary>
        public bool PrepareTalk()
        {
            IntPtr windowHandle = FindWindowEx(
                IntPtr.Zero,
                IntPtr.Zero,
                null,
                Info.WindowCaption);
            if (windowHandle == IntPtr.Zero)
            {
                windowHandle = FindWindowEx(
                    IntPtr.Zero,
                    IntPtr.Zero,
                    null,
                    Info.WindowCaption + "*");

                if (windowHandle == IntPtr.Zero)
                {
                    return false;
                }
            }

            this.playButtonHandle = IntPtr.Zero;
            this.stopButtonHandle = IntPtr.Zero;
            this.textBoxHandle = IntPtr.Zero;
            this.handleCount = 0;

            // 各ボタンを検索します。
            EnumChildWindows(
                windowHandle,
                new Win32Callback(EnumWindowsProc),
                IntPtr.Zero);

            // 見つからなかったら失敗。
            if (this.playButtonHandle == IntPtr.Zero ||
                this.stopButtonHandle == IntPtr.Zero ||
                (Info.EditBoxNo > 0 && this.textBoxHandle == IntPtr.Zero))
            {
                return false;
            }

            // 東北ずん子の場合はやり方が違います。
            if (this.textBoxHandle == IntPtr.Zero)
            {
                // 戻ると貼り付けのメニューIDを検索します。
                IntPtr hMenu = GetMenu(windowHandle);
                IntPtr hSubMenu = GetSubMenu(hMenu, 1);

                // メニューアイテムの位置が変わったら要修正。
                this.undoMenuId = GetMenuItemID(hSubMenu, 0);
                this.pasteMenuId = GetMenuItemID(hSubMenu, 5);
                if (this.undoMenuId == 0 || this.pasteMenuId == 0)
                {
                    return false;
                }
            }

            this.windowHandle = windowHandle;
            return true;
        }

        /// <summary>
        /// 発声を中止します。
        /// </summary>
        public void StopTalk()
        {
            if (this.stopButtonHandle == IntPtr.Zero)
            {
                return;
            }

            // Sendすると処理が戻ってこなくなることがあります。
            // 主に結月ゆかりで。
            uint msg = (Info.EditBoxNo < 0 ? WM_NULL : BM_CLICK);
            PostMessage(this.stopButtonHandle, msg, 0, 0);
            Thread.Sleep(200);
        }

        /// <summary>
        /// テキストの読み上げに失敗する文字を消去します。
        /// </summary>
        private string ModityText(string sourceText)
        {
            return sourceText.Replace("\u200C", "").Replace("\u200E", "");
        }

        /// <summary>
        /// 実際にしゃべらせます。
        /// </summary>
        public void Talk(string sourceText)
        {
            if (string.IsNullOrEmpty(sourceText))
            {
                return;
            }

            if (this.windowHandle == IntPtr.Zero ||
                this.playButtonHandle == IntPtr.Zero ||
                this.stopButtonHandle == IntPtr.Zero ||
                (this.textBoxHandle == IntPtr.Zero &&
                 (this.undoMenuId == 0 || this.pasteMenuId == 0)))
            {
                throw new InvalidOperationException(
                    "Voiceroidアプリの確認に失敗しました。");
            }

            // 読み上げに失敗する文字は削除します。
            sourceText = ModityText(sourceText);

            KeyValuePair<string, object>? oldData = null;
            try
            {
                // クリップボードのデータを一時的に保存します。
                oldData = ClipboardUtil.SetData(sourceText);

                if (this.textBoxHandle != IntPtr.Zero)
                {
                    SendMessage(this.textBoxHandle, 12, IntPtr.Zero, sourceText);
                }
                else
                {
                    SendMessage(this.windowHandle, WM_COMMAND, (int)this.undoMenuId, 0);
                    SendMessage(this.windowHandle, WM_COMMAND, (int)this.pasteMenuId, 0);
                }

                // 一度再生を止めます。
                StopTalk();

                // 音声の再生を行います。
                uint msg = (Info.EditBoxNo < 0 ? WM_NULL : BM_CLICK);
                PostMessage(this.playButtonHandle, msg, 0, 0);
            }
            finally
            {
                // クリップボードの古いデータを復元します。
                if (oldData != null)
                {
                    ClipboardUtil.SetData(oldData);
                }
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Voiceroid(VoiceroidInfo info)
        {
            Info = info;
        }
    }
}
