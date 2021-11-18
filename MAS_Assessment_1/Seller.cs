using System.Collections.Generic;

namespace MAS_Assessment_1
{
    public class Seller
    {
        public string ID { get; set; }
        public int AmountkWhToSell { get; set; }
        public double MinPriceSellToHousehold { get; set; }
        public double PriceToSellToUtility { get; set; }
        public double TotalEarned { get; set; }
        public List<double> Sales { get; set; }
        public Seller(string iD, int amountkWHToSell, double minPriceToSellToUtility, double priceTosellToUtility)
        {
            ID = iD;
            AmountkWhToSell = amountkWHToSell;
            MinPriceSellToHousehold = minPriceToSellToUtility;
            PriceToSellToUtility = priceTosellToUtility;
            TotalEarned = 0;
            Sales = new List<double>();
        }
    }
}