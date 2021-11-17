﻿/*
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

        private const int MinGeneration = 5; //min possible generation from renewable energy on a day for a household (in kWh)
        private const int MaxGeneration = 15; //max possible generation from renewable energy on a day for a household (in kWh)
        private const int MinDemand = 5; //min possible demand on a day for a household (in kWh)
        private const int MaxDemand = 15; //max possible demand on a day for a household (in kWh)
        private const int MinPriceToBuyFromUtility = 12; //min possible price to buy 1kWh from the utility company (in pence)
        private const int MaxPriceToBuyFromUtility = 22; //max possible price to buy 1kWh from the utility company (in pence)
        private const int MinPriceToSellToUtility = 2; //min possible price to sell 1kWh to the utility company (in pence)
        private const int MaxPriceToSellToUtility = 5; //max possible price to sell 1kWh to the utility company (in pence)
        private int counter = 0;

        public override void Act(Message message)

        {
            switch (message.Content)
            {
                case "RequestInformation": //this agent only responds to "information" messages
                    SendInformation(message);
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
            double priceToBuyFromUtility = Convert.ToDouble(rand.Next(MinPriceToBuyFromUtility * 10, MaxPriceToBuyFromUtility * 10)) / 10; //what the household's utility company
                                                                                                                                           //charges to buy 1kWh from it
            double priceToSellToUtility = Convert.ToDouble(rand.Next(MinPriceToSellToUtility * 10, MaxPriceToSellToUtility * 10)) / 10;    //what the household's utility company
                                                                                                                                           //offers to buy 1kWh of renewable energy for
            string content = $"Information {demand} {generation} {priceToBuyFromUtility} {priceToSellToUtility}";
            Send(senderID, content); //send the message with this information back to the household agent that requested it
            counter++;
        }

        public override void ActDefault()
        {
            if (counter == Settings.NumberOfHouseholds)
            {
                Send("auctioneer", "Start");
                counter = 0;
            }
        }
    }
}