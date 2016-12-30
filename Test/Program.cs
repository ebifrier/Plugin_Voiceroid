using System;

using Plugin_Voiceroid;

namespace Test
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //var voiceroid1 = VoiceroidFactory.CreateAoi();
            //var voiceroid = VoiceroidFactory.CreateYukari();

            var text = "zunko) えびちゃんわーい";
            var voiceroid = VoiceroidList.DetectVoiceroid(text);
            if (voiceroid != null)
            {
                var modifiedText = VoiceroidList.RemoveVoiceTag(text);
                for (int i = 0; i < 10; ++i)
                {
                    //plugin.Talk("abcdﾊﾝｶｸ！！");
                    voiceroid.Talk(modifiedText + i);
                    //plugin.Talk("湖湖湖湖湖湖湖湖");

                    System.Threading.Thread.Sleep(2400);
                }
            }
        }
    }
}
