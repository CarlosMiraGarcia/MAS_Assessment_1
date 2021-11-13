using ActressMas;
using System;
using System.Collections.Generic;

namespace MAS_Assessment_1
{
    public class AuctioneerAgent : Agent
    {
        private List<Seller> SellerList = new List<Seller>();
        private List<Buyer> BuyerList = new List<Buyer>();
        private int counter = 0;
        public AuctioneerAgent()
        {

        }
        public override void Setup()
        {
            Broadcast("Start");
        }

        public override void Act(Message message)
        {
            message.Parse(out string action, out string parameters);
            switch (action)
            {
                case "Start":
                    HandleStart();
                    break;
                case "Seller":
                    HandleSeller(message, parameters);
                    break;
                case "Buyer":
                    HandleBuyer(message, parameters);
                    break;
                case "NoParticipating":
                    HandleNoParticipating(message);
                    break;
                default:
                    break;
            }
        }

        private void HandleStart()
        {
            Console.WriteLine("Auctions will start soon");
            Broadcast("BuyerOrSeller");
        }
        public override void ActDefault()
        {
            if (counter == Settings.NumberOfHouseholds)
            {
                Console.WriteLine("Sellers:");
                foreach (Seller seller in SellerList)
                {
                    Console.WriteLine($"ID: {seller.ID}, ToSell: {seller.AmountkWhToSell}, MinPrice: {seller.MinPriceToSell}");
                }
                Console.WriteLine("Buyers:");
                foreach (Buyer buyer in BuyerList)
                {
                    Console.WriteLine($"ID: {buyer.ID}, ToBuy: {buyer.AmountkWhToBuy}, MaxPrice: {buyer.MaxPriceToBuy}");
                }
                counter = 0;
            }
        }
        private void HandleSeller(Message message, string parameters)
        {
            string[] parameteresArray = parameters.Split(' ');
            Seller seller = new Seller(message.Sender, Convert.ToInt32(parameteresArray[0]), Convert.ToInt32(parameteresArray[1]));
            SellerList.Add(seller);
            counter++;
        }
        private void HandleBuyer(Message message, string parameters)
        {
            string[] parameteresArray = parameters.Split(' ');
            Buyer buyer = new Buyer(message.Sender, Convert.ToInt32(parameteresArray[0]), Convert.ToInt32(parameteresArray[1]));
            BuyerList.Add(buyer);
            counter++;
        }
        private void HandleNoParticipating(Message message)
        {
            counter++;
        }
    }
}
