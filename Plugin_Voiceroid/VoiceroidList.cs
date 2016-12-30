using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Runtime.InteropServices;

namespace Plugin_Voiceroid
{
    /// <summary>
    /// ボイスロイドのリストを扱います。
    /// </summary>
    public static class VoiceroidList
    {
        /// <summary>
        /// ボイスロイドの一覧テーブルです。
        /// </summary>
        public static Voiceroid[] VoiceroidTable = new Voiceroid[]
        {
            VoiceroidFactory.CreateAi(),
            VoiceroidFactory.CreateShouta(),
            VoiceroidFactory.CreateTomoe(),
            VoiceroidFactory.CreateTomoe2(),
            VoiceroidFactory.CreateYukari(),
            VoiceroidFactory.CreateYukari2(),
            VoiceroidFactory.CreateZunko(),
            VoiceroidFactory.CreateAkane(),
            VoiceroidFactory.CreateAoi(),
        };

        /// <summary>
        /// 対応するボイスロイドを返します。
        /// </summary>
        /// <remarks>
        /// 複数のボイスタグがある場合に、一番最後にあるボイスロイドを選択します。
        /// </remarks>
        public static Voiceroid DetectVoiceroid(string sourceText)
        {
            Voiceroid result = null;
            int maxIndex = -1;

            // タグによる読み上げを実行します。
            foreach (Voiceroid voiceroid in Voiceroid.VoiceroidTable)
            {
                var index = voiceroid.FindVoiceTag(sourceText);
                if (index < 0)
                {
                    continue;
                }

                if (!voiceroid.PrepareTalk())
                {
                    continue;
                }

                // より後にあるタグを優先します。
                if (index > maxIndex)
                {
                    maxIndex = index;
                    result = voiceroid;
                }
            }

            return result;
        }

        /// <summary>
        /// すべてのボイスタグを削除します。
        /// </summary>
        public static string RemoveVoiceTag(string sourceText)
        {
            foreach (Voiceroid voiceroid in Voiceroid.VoiceroidTable)
            {
                sourceText = voiceroid.RemoveVoiceTag(sourceText);
            }

            return sourceText;
        }
    }
}
