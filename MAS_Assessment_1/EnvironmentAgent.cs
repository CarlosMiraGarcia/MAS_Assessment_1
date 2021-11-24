/*
 * Author: Simon Powers
 * An Environment Agent that sends information to a Household Agent
 * about that household's demand, generation, and prices to buy and sell
 * from the utility company, on that day. Responds whenever pinged
 * by a Household Agent with a "start" message.
 */

using ActressMas;
using System;

namespace MAS_Assessment_1
{
    public class EnvironmentAgent : Agent
    {
        private Random rand = new Random();
        public const int NumberOfHouseholds = 10000;
        private const int MinGeneration = 5; //min possible generation from renewable energy on a day for a household (in kWh)
        private const int MaxGeneration = 15; //max possible generation from renewable energy on a day for a household (in kWh)
        private const int MinDemand = 5; //min possible demand on a day for a household (in kWh)
        private const int MaxDemand = 15; //max possible demand on a day for a household (in kWh)
        private const int MinPriceToBuyFromUtility = 12; //min possible price to buy 1kWh from the utility company (in pence)
        private const int MaxPriceToBuyFromUtility = 22; //max possible price to buy 1kWh from the utility company (in pence)
        private const int MinPriceToSellToUtility = 2; //min possible price to sell 1kWh to the utility company (in pence)
        private const int MaxPriceToSellToUtility = 5; //max possible price to sell 1kWh to the utility company (in pence)
        private int counter = 0; //used to inform the environment agent when all the household agents have had their information

        public override void Act(Message message)

        {
            Console.WriteLine($"\t{message.Format()}");
            message.Parse(out string action, out string parameters);
            switch (message.Content)
            {
                case "RequestInformation": //household agents request information about their attributes such as demand and generation
                    SendInformation(message);
                    break;

                case "Finished": //informs the environment agent that all the auctions are finished
                    HandleFinish();
                    break;

                default:
                    break;
            }
        }

        private void SendInformation(Message message)
        {
            string senderID = message.Sender; //get the sender's name so we can reply to them
            int demand = rand.Next(MinDemand, MaxDemand); //the household's demand in kWh
            int generation = rand.Next(MinGeneration, MaxGeneration); //the household's generation in kWh
            double renewableEnergyPreference; //sets the energy preference for the household agents

            double priceToBuyFromUtility = Math.Round(Convert.ToDouble(rand.Next(MinPriceToBuyFromUtility * 10, MaxPriceToBuyFromUtility * 10)), 2) / 10; //what the household's utility company
                                                                                                                                                          //charges to buy 1kWh from it
            double priceToSellToUtility = Math.Round(Convert.ToDouble(rand.Next(MinPriceToSellToUtility * 10, MaxPriceToSellToUtility * 10)), 2) / 10;    //what the household's utility company
                                                                                                                                                          //charges to buy 1kWh from it
            renewableEnergyPreference = CalculateRenewableEnergyPreference(); //calculates the energy preference setting for each household agent
            string content = $"Information {demand} {generation} {priceToBuyFromUtility} {priceToSellToUtility} {renewableEnergyPreference}"; //send the information to the household agent
            Send(senderID, content); //send the message with this information back to the household agent that requested it
            counter++; //updates counter when the current household agent has had its information sent
        }

        public override void ActDefault()
        {
            if (counter == NumberOfHouseholds)
            {
                Send("auctioneer", "Ready"); //when all the household agents have had their information sent, informs the auction about this
                counter = 0; //sets counter to 0 just in case we want, in the future, run the program in a loop
            }
        }

        private void HandleFinish()
        {
            Stop(); //stops the execution of the environment agent and its removed from the environment
        }

        private double CalculateRenewableEnergyPreference()
        {
            WeightedRandomizer<string> preferences = new WeightedRandomizer<string>(); //creates an instance of the WeightedRandomizer class
            preferences.NewItem("Indifferent", 80); //adds items to the WeightedRandomizer class, and adds the weight for each of them
            preferences.NewItem("MildlyPreferred", 15);
            preferences.NewItem("GreatlyPreferred", 5);

            WeightedRandomizer<string>.Item prefered = preferences.GetRandomItem(); //gets a random item from the list of type of buyers
            switch (prefered.ItemName) //switches between the different types and returns a value according to this. This value will be used
            {                          //to calculate the buyer's bid
                case "Indifferent":
                    return 1.00;

                case "MildlyPreferred":
                    return 1.15;

                case "GreatlyPreferred":
                    return 1.30;

                default:
                    return 1.00;
            }
        }
    }
}