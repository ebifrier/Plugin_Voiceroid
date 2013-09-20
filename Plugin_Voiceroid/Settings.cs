using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using FNF;
using FNF.Utility;
using FNF.XmlSerializerSetting;

namespace Plugin_Voiceroid
{
    /// <summary>
    /// publicフィールドが自動的に保存されます。
    /// </summary>
    public sealed class Settings : SettingsBase
    {
        /// <summary>
        /// 唯一のグローバルインスタンス。
        /// </summary>
        public static Settings Default = new Settings();

        #region Voiceroid
        private VoiceroidType defaultVoiceroidType = VoiceroidType.None;

        /// <summary>
        /// Voiceroidコマンドを有効にするかどうかを取得または設定します。
        /// </summary>
        public bool IsUseVoiceroid = true;

        /// <summary>
        /// タグが無かったときに使われるデフォルトのボイスロイドを
        /// 取得または設定します。
        /// </summary>
        public VoiceroidType DefaultVoiceroidType
        {
            get
            {
                return this.defaultVoiceroidType;
            }
            set
            {
                if (this.defaultVoiceroidType != value)
                {
                    this.defaultVoiceroidType = value;

                    UpdateDefaultVoiceroid();
                }
            }
        }

        /// <summary>
        /// タグが無かったときに使われるデフォルトのボイスロイドを
        /// 取得または設定します。
        /// </summary>
        [XmlIgnore()]
        public Voiceroid DefaultVoiceroid
        {
            get;
            private set;
        }
        #endregion

        /// <summary>
        /// デフォルトで使われるボイスロイドを更新します。
        /// </summary>
        private void UpdateDefaultVoiceroid()
        {
            foreach (Voiceroid voiceroid in Voiceroid.VoiceroidTable)
            {
                if (voiceroid.Info.Type == this.defaultVoiceroidType)
                {
                    DefaultVoiceroid = voiceroid;
                    return;
                }
            }

            // 見つからなかったらnullを設定します。
            DefaultVoiceroid = null;
        }

        /// <summary>
        /// 設定ファイル読み込み時に呼ばれます。
        /// </summary>
        public override void ReadSettings()
        {
            UpdateDefaultVoiceroid();
        }

        /// <summary>
        /// 設定ファイル書き込み時に呼ばれます。
        /// </summary>
        public override void WriteSettings()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private Settings()
        {
            DefaultVoiceroid = null;
        }
    }
}
