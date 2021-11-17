namespace MAS_Assessment_1
{
    public class Seller
    {
        public string ID { get; set; }
        public int AmountkWhToSell { get; set; }
        public double MinPriceToSell { get; set; }
        public double TotalEarned { get; set; }

        public Seller(string iD, int amountkWHToSell, double minPriceToSell)
        {
            ID = iD;
            AmountkWhToSell = amountkWHToSell;
            MinPriceToSell = minPriceToSell;
            TotalEarned = 0;
        }
    }
}