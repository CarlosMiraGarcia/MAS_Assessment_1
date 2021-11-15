using System;
using System.Collections.Generic;
using System.Text;

namespace MAS_Assessment_1
{
    public class Buyer
    {
        public string ID { get; set; }
        public double AmountkWhToBuy { get; set; }
        public double MaxPriceToBuy { get; set; }
        public double TotalSpent { get; set; }

        public Buyer(string iD, double amountkWHToBuy, double maxPriceToBuy)
        {
            ID = iD;
            AmountkWhToBuy = amountkWHToBuy;
            MaxPriceToBuy = maxPriceToBuy;
            TotalSpent = 0;
        }
    }
}
