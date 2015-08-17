using System;

using Plugin_Voiceroid;

namespace Test
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var voiceroid = VoiceroidFactory.CreateAoi();
            var voiceroid2 = VoiceroidFactory.CreateShouta();

            string text = "えびちゃんわーい";
            voiceroid2.PrepareTalk();
            if (voiceroid.PrepareTalk())
            {
                for (int i = 0; i < 10; ++i)
                {
                    //plugin.Talk("abcdﾊﾝｶｸ！！");
                    voiceroid.Talk(text + i);
                    //plugin.Talk("湖湖湖湖湖湖湖湖");

                    System.Threading.Thread.Sleep(2400);
                }
            }
        }
    }
}
