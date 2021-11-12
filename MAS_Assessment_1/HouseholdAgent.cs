using ActressMas;
using System;
using System.Collections.Generic;

namespace MAS_Assessment_1
{
    class HouseholdAgent : Agent
    {

        private int MinPriceSellToHousehold;
        private int MaxPiceBuyFromHousehold;
        private int PriceTosellToUtility;
        private int PriceToBuyFromUtility;
        private int Demand;
        private int Generation;
        private int CurrentBid;
        private bool Participating;
        private bool Buyer;
        private int FinalBalance;
        private List<string> ParametersList = new List<string>();

        public HouseholdAgent()
        {

        }

        public override void Setup()
        {
        }

        public override void Act(Message message)
        {
            try
            {
                int highestBid = 0;

                message.Parse(out string action, out string parameters);
                Console.WriteLine($"\r\n\t{message.Format()}");
                switch (action)
                {
                    case "start":
                        HandleStart();
                        break;
                    case "information":
                        HandleInformation(parameters);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void HandleInformation(string parameters)
        {
            string[] splittedString = parameters.Split(' ');
            foreach (string parameter in splittedString)
            {
                ParametersList.Add(parameter);
            }
            Demand = Convert.ToInt32(ParametersList[0]);
            Generation = Convert.ToInt32(ParametersList[1]);
            PriceToBuyFromUtility = Convert.ToInt32(ParametersList[2]);
            PriceTosellToUtility = Convert.ToInt32(ParametersList[3]);

            CalculateProsumerValues();

            Console.WriteLine("Demand: {0}, Generation: {1}, PriceToBuyFromUtility: {2}, PriceTosellToUtility: {3}, Buyer: {4}, Participating: {5}",
                Demand, Generation, PriceToBuyFromUtility, PriceTosellToUtility, Buyer, Participating);

        }

        private void HandleStart()
        {
            Send("environmentAgent", $"RequestInformation");
        }

        private void HandleBid(int receivedBid)
        {
            //int next = receivedBid + Settings.Increment;
            //if (receivedBid >= _currentBid && next <= _valuation)
            //{
            //    _currentBid = next;
            //    Broadcast($"bid {next}");
            //}
        }

        private void HandleWinner(string winner)
        {
            //if (winner == Name)
            //    Console.WriteLine($"[{Name}]: I have won with {_currentBid}");

            //Stop();
        }

        private void CalculateProsumerValues()
        {
            if (Demand - Generation < 0) { Buyer = false; Participating = true; }
            if (Demand - Generation > 0) { Buyer = true; Participating = true; }
            if (Demand - Generation == 0) { Participating = false; }
        }
    }
}