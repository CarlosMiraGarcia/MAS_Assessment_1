using System;
using System.Collections.Generic;
using System.Text;

namespace MAS_Assessment_1
{
    public class Seller
    {
        public string ID { get; set; }
        public double AmountkWhToSell { get; set; }
        public double MinPriceToSell { get; set; }
        public double TotalEarned { get; set; }

        public Seller(string iD, double amountkWHToSell, double minPriceToSell)
        {
            ID = iD;
            AmountkWhToSell = amountkWHToSell;
            MinPriceToSell = minPriceToSell;
            TotalEarned = 0;
        }
    }
}
