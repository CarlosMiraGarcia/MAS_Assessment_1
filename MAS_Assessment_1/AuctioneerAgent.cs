using ActressMas;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MAS_Assessment_1
{
    public class AuctioneerAgent : Agent
    {
        private List<Seller> SellerList = new List<Seller>();
        private List<Seller> ShuffledSellerList;
        private List<Buyer> BuyerList = new List<Buyer>();
        private List<Buyer> OrderedBuyerList = new List<Buyer>();
        bool Started = false;
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

                ShuffleSellers();
                OrderBuyers();

                HandleAuction();
            }
        }

        private void ShuffleSellers()
        {
            ShuffleRandomiser<Seller> sellerShuffler = new ShuffleRandomiser<Seller>();
            if (Started == false)
            {
                ShuffledSellerList = sellerShuffler.ShuffleList(SellerList);
            }

            else
            {
                ShuffledSellerList = sellerShuffler.ShuffleList(ShuffledSellerList);
            }

            Console.WriteLine("Seller order:");
            foreach (Seller seller in ShuffledSellerList)
            {
                Console.WriteLine(seller.ID);
            }
        }
        private void OrderBuyers()
        {
            //ShuffleRandomiser<Buyer> buyerShuffler = new ShuffleRandomiser<Buyer>();
            //ShuffledBuyerList = buyerShuffler.ShuffleList(BuyerList);
            OrderedBuyerList = BuyerList.OrderBy(x => x.MaxPriceToBuy).Reverse().ToList();

            Console.WriteLine("Buyer order:");
            foreach (Buyer buyer in OrderedBuyerList)
            {
                Console.WriteLine(buyer.ID);
            }
        }

        private void HandleSeller(Message message, string parameters)
        {
            string[] parameteresArray = parameters.Split(' ');
            Seller seller = new Seller(message.Sender, Convert.ToDouble(parameteresArray[0]), Convert.ToDouble(parameteresArray[1]));
            SellerList.Add(seller);
            counter++;
        }
        private void HandleBuyer(Message message, string parameters)
        {
            string[] parameteresArray = parameters.Split(' ');
            Buyer buyer = new Buyer(message.Sender, Convert.ToDouble(parameteresArray[0]), Convert.ToDouble(parameteresArray[1]));
            BuyerList.Add(buyer);
            counter++;
        }
        private void HandleNoParticipating(Message message)
        {
            counter++;
        }

        private void UpdateSellerList()
        {
            ShuffledSellerList.Remove(ShuffledSellerList[0]);
        }

        private void UpdateBuyerList()
        {
            OrderedBuyerList.Remove(OrderedBuyerList[0]);
        }


        private void HandleAuction()
        {
            while (ShuffledSellerList.Count() != 0)
            {
                if (OrderedBuyerList.Count() > 0)
                {
                    double pricePaid = 0;

                    if (OrderedBuyerList.Count() > 1 && ShuffledSellerList[0].MinPriceToSell <= OrderedBuyerList[1].MaxPriceToBuy)
                    {
                        pricePaid = OrderedBuyerList[1].MaxPriceToBuy;
                    }

                    if (OrderedBuyerList.Count() == 1 && ShuffledSellerList[0].MinPriceToSell <= OrderedBuyerList[0].MaxPriceToBuy)
                    {
                        pricePaid = OrderedBuyerList[0].MaxPriceToBuy;
                    }

                    if (pricePaid != 0 )
                    {
                        ShuffledSellerList[0].AmountkWhToSell -= 1;
                        ShuffledSellerList[0].TotalEarned += pricePaid;
                        Send($"{ShuffledSellerList[0].ID}", $"UpdateSeller {ShuffledSellerList[0].AmountkWhToSell} {ShuffledSellerList[0].TotalEarned}");
                        OrderedBuyerList[0].AmountkWhToBuy -= 1;
                        OrderedBuyerList[0].TotalSpent += pricePaid;
                        Send($"{OrderedBuyerList[0].ID}", $"UpdateBuyer {OrderedBuyerList[0].AmountkWhToBuy} {OrderedBuyerList[0].TotalSpent}");

                        if (ShuffledSellerList[0].AmountkWhToSell == 0)
                        {
                            UpdateSellerList();
                        }
                        else if (OrderedBuyerList[0].AmountkWhToBuy == 0)
                        {
                            UpdateBuyerList();
                        }
                    }
                    Started = true;
                    ShuffleSellers();
                }
                else
                {
                    Console.WriteLine($"Sorry {ShuffledSellerList[0].ID}, no buyer was found for your auction");
                    break;
                }
            }
            Stop();
        }
    }
}
