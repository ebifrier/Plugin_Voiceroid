using System;

using Plugin_Voiceroid;

namespace Test
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Voiceroid voiceroid = VoiceroidFactory.CreateZunko();

            string text = "えびちゃんわーい";
            if (voiceroid.PrepareTalk())
            {
                for (int i = 0; i < 100; ++i)
                {
                    //plugin.Talk("abcdﾊﾝｶｸ！！");
                    voiceroid.Talk(text);
                    //plugin.Talk("湖湖湖湖湖湖湖湖");

                    System.Threading.Thread.Sleep(2400);
                }
            }
        }
    }
}
