using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

using FNF;
using FNF.Utility;
using FNF.Controls;
using FNF.BouyomiChanApp;
using FNF.XmlSerializerSetting;

namespace Plugin_Voiceroid
{
    /// <summary>
    /// 設定画面表示用クラス
    /// </summary>
    public sealed class SettingFormData : ISettingFormData
    {
        // 棒読みちゃん側が把握できるようにpublicにする。
        public VoiceroidPropertyGrid voiceroidPropertyGrid;

        /// <summary>
        /// 設定画面のタイトルを取得します。
        /// </summary>
        public string Title
        {
            get
            {
                return "Voiceroidなどの設定画面";
            }
        }

        /// <summary>
        /// すべて展開しているかどうかを取得します。
        /// </summary>
        public bool ExpandAll
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// 設定オブジェクトを取得します。
        /// </summary>
        public SettingsBase Setting
        {
            get
            {
                return Settings.Default;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SettingFormData()
        {
            this.voiceroidPropertyGrid = new VoiceroidPropertyGrid();
        }
    }

    /// <summary>
    /// Voiceroidの設定グリッドコントロール
    /// </summary>
    public sealed class VoiceroidPropertyGrid : ISettingPropertyGrid
    {
        /// <summary>
        /// グリッドコントロールの表示名を取得します。
        /// </summary>
        public string GetName()
        {
            return "Voiceroid設定";
        }

        [Category("基本設定")]
        [DisplayName("01)Voiceroidタグを有効にする")]
        [Description("各Voiceroidのタグを使うかどうかです。")]
        public bool IsUseVoiceroid
        {
            get { return Settings.Default.IsUseVoiceroid; }
            set { Settings.Default.IsUseVoiceroid = value; }
        }

        [Category("基本設定")]
        [DisplayName("02)デフォルトのボイスロイド")]
        [Description("テキストにタグが設定されていなかった場合のデフォルトボイスロイドを設定します。")]
        public VoiceroidType DefaultVoiceroidType
        {
            get { return Settings.Default.DefaultVoiceroidType; }
            set { Settings.Default.DefaultVoiceroidType = value; }
        }
    }
}
