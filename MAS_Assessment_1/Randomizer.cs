using System;
namespace MAS_Assessment_1
{
    public class Randomizer
    {
        private static Random rand = new Random((int)Environment.TickCount);

        public static int GetRandom(discre)
        {
            return rand.Next(1, 100);
        }
    }
}
