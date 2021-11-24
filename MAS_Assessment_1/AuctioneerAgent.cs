using ActressMas;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MAS_Assessment_1
{
    public class AuctioneerAgent : Agent
    {
        private List<Seller> sellerList = new List<Seller>(); //list of sellers used by the auctioneer agent to conduct the auctions
        private List<Seller> orderedSellerList = new List<Seller>(); //ordered copy of the sellerList
        private List<Buyer> buyerList = new List<Buyer>(); //list of buyers used by the auctioneer agent to conduct the auctions
        private List<Buyer> orderedBuyerList = new List<Buyer>(); //ordered copy of the buyerList
        private int counter = 0; //counter used to keep track of the number of household agents that have been added to the list
        private Statistics statistics = new Statistics(); //creates instance of the Statistics class

        public override void Setup()
        {
            Console.WriteLine("[auctioneer]: Agents, start collecting information"); //Writes in the console information to let us know what is happening
            Broadcast("Start"); //broadcasts to every agent
        }

        public override void Act(Message message)
        {
            Console.WriteLine($"\t{message.Format()}");  //this line can be commented out so not all the messages are printed on the console
            message.Parse(out string action, out string parameters); //parses the incoming message to a string
            switch (action)
            {
                case "Ready":
                    HandleStart(); //when the message Ready is sent to the auctioneer, run the method HandleStart
                    break;

                case "Seller":
                    HandleSeller(message, parameters); //when the message Seller is sent to the auctioneer, run the method HandleSeller
                    break;                             //passing the message and parameters that came with it

                case "Buyer":
                    HandleBuyer(message, parameters); //when the message Buyer is sent to the auctioneer, run the method HandleBuyer
                    break;                            //passing the message and parameters that came with it

                case "NoParticipating":
                    HandleNoParticipating(message); //when the message NoParticipating is sent to the auctioneer, run the method HandleNoParticipating
                    break;                          //passing just the message

                default:
                    break;
            }
        }

        private void HandleStart()
        {
            Console.WriteLine("\n[auctioneer]: Collecting Asks and Bids"); //Writes in the console information to let us know what is happening
            Broadcast("BuyerOrSeller"); //broadcast BuyerOrSeller to each agent to know whether they are selling to or buying from the auction
        }

        public override void ActDefault()
        {
            if (counter == EnvironmentAgent.NumberOfHouseholds) //checks if all the household agents have been added to either the seller or buyer list
            {
                counter = 0; //sets counter back to 0, just in case we want to run the program in a loop
                OrderSellers(); //calls the method OrdersSellers
                OrderBuyers(); //calls the method OrderBuyers
                HandleAuction(); //calls the method HandlAuction
            }
        }

        private void HandleSeller(Message message, string parameters) //adds each of the sellers into a seller list
        {
            string[] parameteresArray = parameters.Split(' '); //splits the parameters string into an string array
            Seller seller = new Seller(message.Sender, Convert.ToInt32(parameteresArray[0]), Convert.ToDouble(parameteresArray[1]), Convert.ToDouble(parameteresArray[2]));
            sellerList.Add(seller); //adds the seller object to the sellerList
            counter++; //increases the counter
        }

        private void HandleBuyer(Message message, string parameters) //adds each of the buyers into a buyer list
        {
            string[] parameteresArray = parameters.Split(' '); //split sthe parameters string into an string array
            Buyer buyer = new Buyer(message.Sender, Convert.ToInt32(parameteresArray[0]), Convert.ToDouble(parameteresArray[1]), Convert.ToDouble(parameteresArray[2]), Convert.ToDouble(parameteresArray[3]));
            buyerList.Add(buyer); //adds the buyer object to the buyerList
            counter++; //increases the counter
        }

        private void HandleNoParticipating(Message message) //handles the no participant message
        {
            counter++; //increases the counter
        }

        private void HandleAuction() //handles the auctions
        {
            Console.WriteLine("\n[auctioneer]: Auctions started"); //Writes in the console information to let us know what is happening
            while (orderedSellerList.Count() != 0 && orderedBuyerList.Count() != 0) //while the orderedSellerList and orderedBuyerList have some objects, keep looping
            {
                //checks if the seller's min price to sell to household is less or equal to the buyer's max price to buy from household
                if (orderedSellerList[0].MinPriceSellToHousehold <= orderedBuyerList[counter].MaxPriceToBuyFromHousehold)
                {
                    var nextSeller = 0; //sets nextSeller index to 0, which is the first on the list, which is the current seller selected for auction
                    var nextBuyer = counter; //sets the nextBuyer to counter, which is the current buyer selected for auction

                    //if the amount of buyers in orderedBuyerList is greater to 1, set the nextBuyer to the next on the list,
                    //does the same for the seller list.
                    //The reason for this is because we are using a Vickrey auction, where the price is calculated using the next agent on the list
                    //if the lists contain only one agent, that agent's value will be used for calculating the final price
                    if (orderedBuyerList.Count() > 1 && orderedBuyerList.Count() > counter) { nextBuyer++; }
                    if (orderedSellerList.Count() > 1) { nextSeller++; }

                    //the final priced is set using the Vickrey's auction algorithm
                    double pricePaid = Math.Round((orderedBuyerList[nextBuyer].MaxPriceToBuyFromHousehold + orderedSellerList[nextSeller].MinPriceSellToHousehold) / 2, 2);

                    orderedSellerList[counter].AmountkWhToSell -= 1; //decreases the ammount left to sell by 1
                    orderedSellerList[counter].Sales.Add(pricePaid); //adds the sale to the ordered list of sellers
                    orderedSellerList[counter].TotalEarned += pricePaid; //updates the total earned variable
                    Send($"{orderedSellerList[0].ID}", $"UpdateSeller {1} {pricePaid}"); //sends a message to the seller agent with the ammount of energy sold and the money earnt from it

                    orderedBuyerList[counter].AmountkWhToBuy -= 1; //decreases the ammount left to buy by 1
                    orderedBuyerList[counter].Purchases.Add(pricePaid); //adds the purchase to the ordered list of buyers
                    orderedBuyerList[counter].TotalSpent += pricePaid; //updates the total spent variable
                    Send($"{orderedBuyerList[counter].ID}", $"UpdateBuyer {1} {pricePaid}"); //sends a message to the buyer agent with the ammount of energy bought and the money spent for it

                    if (orderedSellerList[0].AmountkWhToSell == 0) //if current seller has no more energy to sell, update the seller list
                    {
                        UpdateSellerList();
                    }

                    if (orderedBuyerList[counter].AmountkWhToBuy == 0) //if current buyer has no more energy to buy, update the buyer list
                    {
                        UpdateBuyerList(counter);
                    }
                }
                else
                {
                    counter++; //increases counter

                    if (counter == orderedBuyerList.Count() - 1) //if the counter equals the total of buyers in the list minus 1, update the sellers list
                                                                 //because it means that the seller didn't match with any of the buyers
                    {
                        UpdateSellerList();
                        counter = 0;
                    }
                }
            }

            Environment.Continue(1); //waits one iteration before continuing. This helps to keep the messages in order in the console
                                     //it's only needed for demostration of the messages passed between agents. It can be commented out for
                                     //better perfomance
            HandleFinish();
            statistics.CreateStatistics(buyerList, sellerList); //calls the statistics method CreateStatistics, passing the buyer and seller lists
        }

        private void HandleFinish()
        {
            Console.WriteLine("\n[auctioneer]: Auctions are finished"); //Writes in the console information to let us know what is happening
            Broadcast("Finished"); //broadcasts every agent informing the auctions have finished
            
            Environment.Continue(1);
            Stop(); //stops the execution of the auctioneer agent and its removed from the environment
        }

        private void OrderSellers()
        {
            //orders the sellerList in ascending order using the MinPriceSellToHousehold variable
            orderedSellerList = sellerList.OrderBy(x => x.MinPriceSellToHousehold).ToList();
        }

        private void OrderBuyers()
        {
            //orders the buyerList in descending order using the MaxPriceToBuyFromHousehold variable
            orderedBuyerList = buyerList.OrderBy(x => x.MaxPriceToBuyFromHousehold).Reverse().ToList();
        }

        private void UpdateSellerList()
        {
            //removes the current seller from the orderedSellerList
            orderedSellerList.Remove(orderedSellerList[0]);
        }

        private void UpdateBuyerList(int counter)
        {
            //removes the current buyer from the orderedBuyerList
            orderedBuyerList.Remove(orderedBuyerList[counter]);
        }
    }
}