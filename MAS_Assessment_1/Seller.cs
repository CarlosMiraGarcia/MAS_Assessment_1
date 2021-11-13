using System;
using System.Collections.Generic;
using System.Text;

namespace MAS_Assessment_1
{
    public class Seller
    {
        public string ID { get; set; }
        public int AmountkWhToSell { get; set; }
        public int MinPriceToSell { get; set; }
        public int TotalEarn { get; set; }

        public Seller(string iD, int amountkWHToSell, int minPriceToSell)
        {
            ID = iD;
            AmountkWhToSell = amountkWHToSell;
            MinPriceToSell = minPriceToSell;
            TotalEarn = 0;
        }
    }
}
