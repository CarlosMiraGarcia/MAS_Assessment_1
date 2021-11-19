using ActressMas;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace MAS_Assessment_1
{
    public class AuctioneerAgent : Agent
    {
        private List<Seller> sellerList = new List<Seller>();
        private List<Seller> orderedSellerList = new List<Seller>();
        private List<Buyer> buyerList = new List<Buyer>();
        private List<Buyer> orderedBuyerList = new List<Buyer>();
        private List<double> sellerRequest = new List<double>();
        private List<double> buyerRequest = new List<double>();
        private int counter = 0;
        private Plot plot = new Plot();
        private Statistics statistics = new Statistics();
        Stopwatch stopwatch = new Stopwatch();

        public override void Setup()
        {
            stopwatch.Start();
            Console.WriteLine("[auctioneer]: Agents, start collecting information");
            Broadcast("Start");
        }

        public override void Act(Message message)
        {
            Console.WriteLine($"\t{message.Format()}");
            message.Parse(out string action, out string parameters);
            switch (action)
            {
                case "Ready":
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
            Console.WriteLine("\n[auctioneer]: Collecting Asks and Bids");
            Broadcast("BuyerOrSeller");
        }

        public override void ActDefault()
        {
            if (counter == Settings.NumberOfHouseholds)
            {
                counter = 0;
                OrderSellers();
                OrderBuyers();
                HandleAuction();
            }
        }

        private void HandleSeller(Message message, string parameters)
        {
            string[] parameteresArray = parameters.Split(' ');
            Seller seller = new Seller(message.Sender, Convert.ToInt32(parameteresArray[0]), Convert.ToDouble(parameteresArray[1]), Convert.ToDouble(parameteresArray[2]));
            sellerList.Add(seller);
            for (int i = seller.AmountkWhToSell; i > 0; i--)
            {
                sellerRequest.Add(seller.MinPriceSellToHousehold);
            }
            counter++;
        }

        private void HandleBuyer(Message message, string parameters)
        {
            string[] parameteresArray = parameters.Split(' ');
            Buyer buyer = new Buyer(message.Sender, Convert.ToInt32(parameteresArray[0]), Convert.ToDouble(parameteresArray[1]), Convert.ToDouble(parameteresArray[2]));
            buyerList.Add(buyer);
            for (int i = buyer.AmountkWhToBuy; i > 0; i--)
            {
                buyerRequest.Add(buyer.MaxPriceToBuyFromHousehold);
            }
            counter++;
        }

        private void HandleNoParticipating(Message message)
        {
            counter++;
        }

        private void HandleAuction()
        {
            Console.WriteLine("\n[auctioneer]: Auctions started");
            while (orderedSellerList.Count() != 0)
            {
                if (orderedBuyerList.Count() > 0)
                {
                    if (orderedSellerList[0].MinPriceSellToHousehold <= orderedBuyerList[0].MaxPriceToBuyFromHousehold)
                    {
                        double pricePaid = Math.Round((orderedBuyerList[0].MaxPriceToBuyFromHousehold + orderedSellerList[0].MinPriceSellToHousehold) / 2, 2);

                        orderedSellerList[0].AmountkWhToSell -= 1;
                        orderedSellerList[0].Sales.Add(pricePaid);
                        orderedSellerList[0].TotalEarned += pricePaid;
                        Send($"{orderedSellerList[0].ID}", $"UpdateSeller {1} {pricePaid}");

                        orderedBuyerList[0].AmountkWhToBuy -= 1;
                        orderedBuyerList[0].Purchases.Add(pricePaid);
                        orderedBuyerList[0].TotalSpent += pricePaid;
                        Send($"{orderedBuyerList[0].ID}", $"UpdateBuyer {1} {pricePaid}");

                        if (orderedSellerList[0].AmountkWhToSell == 0)
                        {
                            UpdateSellerList();
                        }

                        if (orderedBuyerList[0].AmountkWhToBuy == 0)
                        {
                            UpdateBuyerList();
                        }
                    }
                }
                else
                {
                    break;
                }
            }

            Environment.Continue(1);
            HandleFinish();
            plot.CreatePlot(sellerRequest, buyerRequest);
            statistics.CreateStatistics(buyerList, sellerList);  
        }

        private void HandleFinish()
        {
            Console.WriteLine("\n[auctioneer]: Auctions are finished");
            Broadcast("Finished");
            Environment.Continue(1);
            Stop();

            stopwatch.Stop();
            Console.WriteLine("\n\nElapsed Time is {0} ms", stopwatch.ElapsedMilliseconds);
        }

        private void OrderSellers()
        {
            orderedSellerList = sellerList.OrderBy(x => x.MinPriceSellToHousehold).ToList();
        }

        private void OrderBuyers()
        {
            orderedBuyerList = buyerList.OrderBy(x => x.MaxPriceToBuyFromHousehold).Reverse().ToList();
        }

        private void UpdateSellerList()
        {
            orderedSellerList.Remove(orderedSellerList[0]);
        }

        private void UpdateBuyerList()
        {
            orderedBuyerList.Remove(orderedBuyerList[0]);
        }
    }
}