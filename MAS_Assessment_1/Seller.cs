using System.Collections.Generic;

namespace MAS_Assessment_1
{
    public class Seller
    {
        public string ID { get; set; } //agent's ID
        public int AmountkWhToSell { get; set; } //agent's amount to sell in the auction
        public double MinPriceSellToHousehold { get; set; } //minimum price to sell to the household
        public double PriceToSellToUtility { get; set; } //price to sell to utility company
        public double TotalEarned { get; set; } //total earned from auctions plus any left over energy sold to the utility company
        public List<double> Sales { get; set; } //list of sales from the auction

        public Seller(string iD, int amountkWHToSell, double minPriceToSellToUtility, double priceTosellToUtility)
        {
            ID = iD; //sets the ID
            AmountkWhToSell = amountkWHToSell; //sets the AmountkWhToSell
            MinPriceSellToHousehold = minPriceToSellToUtility; //sets the MinPriceSellToHousehold
            PriceToSellToUtility = priceTosellToUtility; //sets the PriceToSellToUtility
            TotalEarned = 0; //sets the TotalEarned to 0
            Sales = new List<double>(); //initialises the sales list
        }
    }
}