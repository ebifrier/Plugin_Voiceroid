using System;
using System.Collections.Generic;
using System.Text;

namespace Plugin_Voiceroid
{
    /// <summary>
    /// ボイスロイドの種別です。
    /// </summary>
    public enum VoiceroidType
    {
        /// <summary>
        /// 特になし。
        /// </summary>
        None,
        /// <summary>
        /// 月読アイ
        /// </summary>
        Ai,
        /// <summary>
        /// 月読ショウタ
        /// </summary>
        Shouta,
        /// <summary>
        /// 民安ともえ
        /// </summary>
        Tomoe,
        /// <summary>
        /// 結月ゆかり
        /// </summary>
        Yukari,
        /// <summary>
        /// 東北ずん子
        /// </summary>
        Zunko,
    }

    /// <summary>
    /// ボイスロイドのファクトリです。
    /// </summary>
    public static class VoiceroidFactory
    {
        /// <summary>
        /// 月読アイを作成します。
        /// </summary>
        public static Voiceroid CreateAi()
        {
            return new Voiceroid(
                new VoiceroidInfo(
                    VoiceroidType.Ai,
                    "月読アイ",
                    @"\b(ai|あい|アイ)\b[)）]\s*",
                    "VOICEROID - アイ",
                    10, 9));
        }

        /// <summary>
        /// 月読ショウタを作成します。
        /// </summary>
        public static Voiceroid CreateShouta()
        {
            return new Voiceroid(
                new VoiceroidInfo(
                    VoiceroidType.Shouta,
                    "月読ショウタ",
                    @"\b(sho(u)?(ta)?|syo(u)?(ta)?|しょうた?|ショウタ?)\b[)）]\s*",
                    "VOICEROID - ショウタ",
                    10, 9));
        }

        /// <summary>
        /// 民安ともえを作成します。
        /// </summary>
        public static Voiceroid CreateTomoe()
        {
            return new Voiceroid(
                new VoiceroidInfo(
                    VoiceroidType.Tomoe,
                    "民安ともえ",
                    @"\b(tomoe|ともえ|トモエ)\b[)）]\s*",
                    "VOICEROID＋ 民安ともえ",
                    9, 8));
        }

        /// <summary>
        /// 結月ゆかりを作成します。
        /// </summary>
        public static Voiceroid CreateYukari()
        {
            return new Voiceroid(
                new VoiceroidInfo(
                    VoiceroidType.Yukari,
                    "結月ゆかり",
                    @"\b(yukari|ゆかり|ユカリ)\b[)）]\s*",
                    "VOICEROID＋ 結月ゆかり",
                    9, 8));
        }

        /// <summary>
        /// 東北ずん子を作成します。
        /// </summary>
        public static Voiceroid CreateZunko()
        {
            return new Voiceroid(
                new VoiceroidInfo(
                    VoiceroidType.Zunko,
                    "東北ずん子",
                    @"\b(zu(n)?nko|ずん子|ずんこ)\b[)）]\s*",
                    "VOICEROID＋ 東北ずん子",
                    10, " 再生", 11, " 停止",
                    "WindowsForms10.BUTTON.app.0.17ad52b",
                    8, "WindowsForms10.RichEdit20W.app.0.17ad52b"));
        }
    }
}
