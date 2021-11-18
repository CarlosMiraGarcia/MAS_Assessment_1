using System.Collections.Generic;

namespace MAS_Assessment_1
{
    public class Buyer
    {
        public string ID { get; set; }
        public int AmountkWhToBuy { get; set; }
        public double MaxPriceToBuyFromHousehold { get; set; }
        public double PriceToBuyFromUtility { get; set; }
        public double TotalSpent { get; set; }
        public List<double> Purchases { get; set; }
        public Buyer(string iD, int amountkWHToBuy, double maxPriceToBuyFromHousehold, double priceToBuyFromUtility)
        {
            ID = iD;
            AmountkWhToBuy = amountkWHToBuy;
            MaxPriceToBuyFromHousehold = maxPriceToBuyFromHousehold;
            PriceToBuyFromUtility = priceToBuyFromUtility;
            TotalSpent = 0;
            Purchases = new List<double>();
        }
    }
}