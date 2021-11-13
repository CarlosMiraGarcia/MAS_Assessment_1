using System;
using System.Collections.Generic;
using System.Text;

namespace MAS_Assessment_1
{
    class Buyer
    {
        public string ID { get; set; }
        public int AmountkWhToBuy { get; set; }
        public int MaxPriceToBuy { get; set; }
        public int TotalSpent { get; set; }

        public Buyer(string iD, int amountkWHToBuy, int minPriceToBuy)
        {
            ID = iD;
            AmountkWhToBuy = amountkWHToBuy;
            MaxPriceToBuy = minPriceToBuy;
            TotalSpent = 0;
        }
    }
}
