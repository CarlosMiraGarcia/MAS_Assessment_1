using ActressMas;
using System;
using System.Collections.Generic;

namespace MAS_Assessment_1
{
    public class AuctioneerAgent : Agent
    {
        private List<string> SellerList = new List<string>();
        private List<string> BuyerList = new List<string>();
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
            switch(message.Content)
            {
                case "Start":
                    HandleStart();
                    break;
                case "Seller":
                    HandleSeller(message);
                    break;
                case "Buyer":
                    HandleBuyer(message);
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
                foreach (string seller in SellerList)
                {
                    Console.WriteLine(seller);
                }
                Console.WriteLine("Buyers:");
                foreach (string buyer in BuyerList)
                {
                    Console.WriteLine(buyer);
                }
                counter = 0;
            }
        }
        private void HandleSeller(Message message)
        {
            SellerList.Add(message.Sender);
            counter++;
        }
        private void HandleBuyer(Message message)
        {
            BuyerList.Add(message.Sender);
            counter++;
        }
        private void HandleNoParticipating(Message message)
        {
            counter++;
        }
    }
}
