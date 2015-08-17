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
        /// <summary>
        /// 琴葉茜
        /// </summary>
        Akane,
        /// <summary>
        /// 琴葉葵
        /// </summary>
        Aoi,
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
                    10, " 再生", 11, " 停止", false,
                    "WindowsForms10.BUTTON.app.0.17ad52b",
                    8, "WindowsForms10.RichEdit20W.app.0.17ad52b"));
        }

        /// <summary>
        /// 琴葉茜を作成します。
        /// </summary>
        public static Voiceroid CreateAkane()
        {
            return new Voiceroid(
                new VoiceroidInfo(
                    VoiceroidType.Akane,
                    "琴葉茜",
                    @"\b(akane|茜|あかね)\b[)）]\s*",
                    "VOICEROID＋ 琴葉茜",
                    11, " 再生", 12, " 停止", true,
                    "WindowsForms10.BUTTON.app.0.33c0d9d",
                    9, "WindowsForms10.RichEdit20W.app.0.33c0d9d"));
        }

        /// <summary>
        /// 琴葉葵を作成します。
        /// </summary>
        public static Voiceroid CreateAoi()
        {
            return new Voiceroid(
                new VoiceroidInfo(
                    VoiceroidType.Aoi,
                    "琴葉葵",
                    @"\b(aoi|葵|あおい)\b[)）]\s*",
                    "VOICEROID＋ 琴葉葵",
                    11, " 再生", 12, " 停止", true,
                    "WindowsForms10.BUTTON.app.0.33c0d9d",
                    9, "WindowsForms10.RichEdit20W.app.0.33c0d9d"));
        }
    }
}
