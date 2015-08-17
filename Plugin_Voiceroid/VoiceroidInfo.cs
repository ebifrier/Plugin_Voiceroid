using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Plugin_Voiceroid
{
    /// <summary>
    /// 各ボイスロイドに対応させます。
    /// </summary>
    public sealed class VoiceroidInfo
    {
        /// <summary>
        /// ボイスロイドの種別を取得します。
        /// </summary>
        public VoiceroidType Type
        {
            get;
            private set;
        }

        /// <summary>
        /// ボイスロイド名を取得します。
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// 棒読みちゃん用のタグを解析するための正規表現を取得します。
        /// </summary>
        public Regex TagRegex
        {
            get;
            private set;
        }

        /// <summary>
        /// ウィンドウ検索に使うタイトルを取得します。
        /// </summary>
        public string WindowCaption
        {
            get;
            private set;
        }

        /// <summary>
        /// ボタンクラス名を取得します。
        /// </summary>
        public string ButtonClassName
        {
            get;
            private set;
        }

        /// <summary>
        /// エディットボックスのコントロール番号(何番目にあるか)を取得します。
        /// </summary>
        public int EditBoxNo
        {
            get;
            private set;
        }

        /// <summary>
        /// エディットボックスのクラス名を取得します。
        /// </summary>
        public string EditBoxClassName
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool UseNextNo
        {
            get;
            private set;
        }

        /// <summary>
        /// 再生ボタンのコントロール番号(何番目にあるか)を取得します。
        /// </summary>
        public int PlayButtonNo
        {
            get;
            private set;
        }

        /// <summary>
        /// 再生ボタンのキャプションを取得します。
        /// </summary>
        public string PlayButtonCaption
        {
            get;
            private set;
        }

        /// <summary>
        /// 停止ボタンのコントロール番号(何番目にあるか)を取得します。
        /// </summary>
        public int StopButtonNo
        {
            get;
            private set;
        }

        /// <summary>
        /// 停止ボタンのキャプションを取得します。
        /// </summary>
        public string StopButtonCaption
        {
            get;
            private set;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public VoiceroidInfo(VoiceroidType type, string name,
                             string regex, string windowCaption,
                             int playButtonNo, string playButtonCaption,
                             int stopButtonNo, string stopButtonCaption,
                             bool useNextNo, string buttonClassName,
                             int editBoxNo, string editBoxClassName)
        {
            Type = type;
            Name = name;
            TagRegex = new Regex(regex, RegexOptions.IgnoreCase);

            WindowCaption = windowCaption;
            ButtonClassName = buttonClassName;
            EditBoxClassName = editBoxClassName;
            EditBoxNo = editBoxNo;

            PlayButtonCaption = playButtonCaption;
            PlayButtonNo = playButtonNo;
            StopButtonCaption = stopButtonCaption;
            StopButtonNo = stopButtonNo;
            UseNextNo = useNextNo;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public VoiceroidInfo(VoiceroidType type, string name, string regex,
                             string windowCaption,
                             int playButtonNo, int stopButtonNo)
            : this(type, name, regex, windowCaption,
                   playButtonNo, null, stopButtonNo, null,
                   false, "Button", -1, null)
        {
        }
    }
}
