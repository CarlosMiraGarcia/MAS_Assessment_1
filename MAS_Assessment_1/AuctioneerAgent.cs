using ActressMas;
using System;
using System.Collections.Generic;
using System.Linq;
using PLplot;
using System.Reflection;
using System.Drawing;

namespace MAS_Assessment_1
{
    public class AuctioneerAgent : Agent
    {
        private List<Seller> SellerList = new List<Seller>();
        private List<Seller> OrderedSellerList;
        private List<Buyer> BuyerList = new List<Buyer>();
        private List<Buyer> OrderedBuyerList;
        private int counter = 0;
        private double totalRequestsSell = 0;
        private double totalRequestsBuy = 0;
        private List<double> sellerRequest = new List<double>();
        private List<double> buyerRequest = new List<double>();
        private List<double> totalPaid = new List<double>();

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
            Console.WriteLine();
            Console.WriteLine("/////////////////////////");
            Console.WriteLine("Collecting Lots and Bids");
            Console.WriteLine("/////////////////////////");
            Broadcast("BuyerOrSeller");
        }

        public override void ActDefault()
        {
            if (counter == Settings.NumberOfHouseholds)
            {
                //Console.WriteLine();
                //Console.WriteLine("///////");
                //Console.WriteLine("Sellers");
                //Console.WriteLine("///////");
                //foreach (Seller seller in SellerList)
                //{
                //    Console.WriteLine($"ID: {seller.ID}, ToSell: {seller.AmountkWhToSell}, MinPrice: {seller.MinPriceToSell}");
                //}

                //Console.WriteLine();
                //Console.WriteLine("///////");
                //Console.WriteLine("Buyers");
                //Console.WriteLine("///////");
                //foreach (Buyer buyer in BuyerList)
                //{
                //    Console.WriteLine($"ID: {buyer.ID}, ToBuy: {buyer.AmountkWhToBuy}, MaxPrice: {buyer.MaxPriceToBuy}");
                //}

                counter = 0;

                OrderSellers();
                OrderBuyers();

                HandleAuction();
            }
        }

        private void OrderSellers()
        {
            OrderedSellerList = SellerList.OrderBy(x => x.MinPriceToSell).ToList();

            //Console.WriteLine("Seller order:");
            //foreach (Seller seller in ShuffledSellerList)
            //{
            //    Console.WriteLine(seller.ID);
            //}
        }

        private void OrderBuyers()
        {
            //ShuffleRandomiser<Buyer> buyerShuffler = new ShuffleRandomiser<Buyer>();
            //ShuffledBuyerList = buyerShuffler.ShuffleList(BuyerList);
            OrderedBuyerList = BuyerList.OrderBy(x => x.MaxPriceToBuy).Reverse().ToList();

            //Console.WriteLine("Buyer order:");
            //foreach (Buyer buyer in OrderedBuyerList)
            //{
            //    Console.WriteLine(buyer.ID);
            //}
        }

        private void HandleSeller(Message message, string parameters)
        {
            string[] parameteresArray = parameters.Split(' ');
            Seller seller = new Seller(message.Sender, Convert.ToInt32(parameteresArray[0]), Convert.ToDouble(parameteresArray[1]));
            SellerList.Add(seller);
            totalRequestsSell += seller.AmountkWhToSell;
            for (int i = seller.AmountkWhToSell; i > 0; i--)
            {
                sellerRequest.Add(seller.MinPriceToSell);
            }
            counter++;
        }

        private void HandleBuyer(Message message, string parameters)
        {
            string[] parameteresArray = parameters.Split(' ');
            Buyer buyer = new Buyer(message.Sender, Convert.ToInt32(parameteresArray[0]), Convert.ToDouble(parameteresArray[1]));
            BuyerList.Add(buyer);
            totalRequestsBuy += buyer.AmountkWhToBuy;
            for (int i = buyer.AmountkWhToBuy; i > 0; i--)
            {
                buyerRequest.Add(buyer.MaxPriceToBuy);
            }
            counter++;
        }

        private void HandleNoParticipating(Message message)
        {
            counter++;
        }

        private void UpdateSellerList()
        {
            OrderedSellerList.Remove(OrderedSellerList[0]);
        }

        private void UpdateBuyerList()
        {
            OrderedBuyerList.Remove(OrderedBuyerList[0]);
        }

        private void HandleAuction()
        {
            Console.WriteLine();
            Console.WriteLine("////////////////");
            Console.WriteLine("Auctions started");
            Console.WriteLine("/////////////////");
            while (OrderedSellerList.Count() != 0)
            {
                if (OrderedBuyerList.Count() > 0)
                {
                    if (OrderedSellerList[0].MinPriceToSell <= OrderedBuyerList[0].MaxPriceToBuy)
                    {
                        double pricePaid = Math.Round((OrderedBuyerList[0].MaxPriceToBuy + OrderedSellerList[0].MinPriceToSell) / 2, 2);

                        OrderedSellerList[0].AmountkWhToSell -= 1;
                        OrderedSellerList[0].TotalEarned += pricePaid;
                        Send($"{OrderedSellerList[0].ID}", $"UpdateSeller {1} {pricePaid}");
                        OrderedBuyerList[0].AmountkWhToBuy -= 1;
                        OrderedBuyerList[0].TotalSpent += pricePaid;
                        Send($"{OrderedBuyerList[0].ID}", $"UpdateBuyer {1} {pricePaid}");
                        totalPaid.Add(pricePaid);
                        if (OrderedSellerList[0].AmountkWhToSell == 0)
                        {
                            UpdateSellerList();
                        }
                        if (OrderedBuyerList[0].AmountkWhToBuy == 0)
                        {
                            UpdateBuyerList();
                        }
                    }
                }
                else
                {
                    //Console.WriteLine($"Sorry {ShuffledSellerList[0].ID}, no buyer was found for your auction");
                    break;
                }
            }
            Environment.Continue(1);
            Broadcast("Finished");
            Environment.Continue(1);

            Console.WriteLine(totalRequestsSell);
            Console.WriteLine(totalRequestsBuy);
            Console.WriteLine(sellerRequest.Count());
            Console.WriteLine(buyerRequest.Count());

            Plot.CreatePlot(sellerRequest, buyerRequest);
            Console.WriteLine(totalPaid.Sum() / totalPaid.Count());
            Stop();
        } 
    }
}