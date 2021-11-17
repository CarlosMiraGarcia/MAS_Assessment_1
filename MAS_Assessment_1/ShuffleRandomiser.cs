using System;
using System.Collections.Generic;
using System.Linq;

namespace MAS_Assessment_1
{
    public class ShuffleRandomiser<T>
    {
        private static Random rand = new Random();

        public List<T> ShuffleList(List<T> list)
        {
            List<T> shuffled = new List<T>();
            shuffled = list.OrderBy(x => rand.Next()).ToList();
            return shuffled;
        }
    }
}