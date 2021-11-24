using System.Collections.Generic;

namespace MAS_Assessment_1
{
    public class Buyer
    {
        public string ID { get; set; } //agent's ID
        public int AmountkWhToBuy { get; set; } //agent's amount to buy from the auction
        public double MaxPriceToBuyFromHousehold { get; set; } //maximum price to buy from the household
        public double PriceToBuyFromUtility { get; set; } //price to buy from utility company
        public double TotalSpent { get; set; } //total spent from auctions plus any left over energy bought from the utility company
        public List<double> Purchases { get; set; } //list of purchases from the auction
        public double Preference { get; set; } //buyer's renewable energy preference


        public Buyer(string iD, int amountkWHToBuy, double maxPriceToBuyFromHousehold, double priceToBuyFromUtility, double Preference)
        {
            ID = iD; //sets the ID
            AmountkWhToBuy = amountkWHToBuy; //sets the AmountkWhToBuy
            MaxPriceToBuyFromHousehold = maxPriceToBuyFromHousehold; //sets the MaxPriceToBuyFromHousehold
            PriceToBuyFromUtility = priceToBuyFromUtility; //sets the PriceToBuyFromUtility
            TotalSpent = 0; //sets the TotalSpent to 0
            this.Preference = Preference; //sets the preference
            Purchases = new List<double>(); //initialises the sales list
        }
    }
}