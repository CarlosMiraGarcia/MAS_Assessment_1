﻿using ActressMas;
using System;
using System.Collections.Generic;

namespace MAS_Assessment_1
{
    class HouseholdAgent : Agent
    {
        private double MinPriceSellToHousehold;
        private double MaxPiceBuyFromHousehold;
        private int PriceTosellToUtility;
        private int PriceToBuyFromUtility;
        private int Demand;
        private int Generated;
        private int EnergyBalance;
        private bool Participating;
        private bool Buyer;
        private double FinancialBalance;
        private List<string> ParametersList = new List<string>();
        private double renewableEnergyPreference;
        public double RenewableEnergyPreference
        {
            get { return renewableEnergyPreference; }
            set
            {
                if (value == 0) { renewableEnergyPreference = 1.00; }
                if (value == 1) { renewableEnergyPreference = 1.05; }
                if (value == 2) { renewableEnergyPreference = 1.10; }
            }
        }

        public HouseholdAgent()
        {
            CalculateRenewableEnergyPreference();
        }

        private void CalculateRenewableEnergyPreference()
        {
            WeightedRandomizer<string> preferences = new WeightedRandomizer<string>();
            preferences.NewItem("Indiferent", 70);
            preferences.NewItem("MildyPrefered", 20);
            preferences.NewItem("GreatlyPrefered", 10);

            WeightedRandomizer<string>.Item prefered = preferences.GetRandomItem();
            switch (prefered.itemName)
            {
                case "Indiferent":
                    RenewableEnergyPreference = 0;
                    break;
                case "MildyPrefered":
                    RenewableEnergyPreference = 1;
                    break;
                case "GreatlyPrefered":
                    RenewableEnergyPreference = 2;
                    break;
                default:
                    RenewableEnergyPreference = 0;
                    break;
            }
        }

        public override void Act(Message message)
        {
            try
            {
                message.Parse(out string action, out string parameters);
                Console.WriteLine($"\r\n\t{message.Format()}");
                switch (action)
                {
                    case "Start":
                        HandleStart();
                        break;
                    case "Information":
                        HandleInformation(parameters);
                        break;
                    case "BuyerOrSeller":
                        SendInformation();
                        break;
                    case "UpdateSeller":
                        UpdateSellerInfo(parameters);
                        break;
                    case "UpdateBuyer":
                        UpdateBuyerInfo(parameters);
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


        private void PopulateParameterList(string parameters)
        {
            ParametersList.Clear();
            string[] splittedString = parameters.Split(' ');
            foreach (string parameter in splittedString)
            {
                ParametersList.Add(parameter);
            }
        }
        private void SendInformation()
        {
            if (Participating && !Buyer)
            {
                CalculatePriceSellToHousehold();
                Send("auctioneer", $"Seller {EnergyBalance} {MinPriceSellToHousehold}");
            }

            else if (Participating && Buyer)
            {
                CalculatePriceBuyFromHousehold();
                Send("auctioneer", $"Buyer {Math.Abs(EnergyBalance)} {MaxPiceBuyFromHousehold}");
            }
            else
            {
                Send("auctioneer", "NoParticipating");
            }
        }
        private void UpdateSellerInfo(string parameters)
        {
            PopulateParameterList(parameters);
            EnergyBalance = Convert.ToInt32(ParametersList[0]);
            FinancialBalance = Convert.ToDouble(ParametersList[1]);
        }

        private void UpdateBuyerInfo(string parameters)
        {
            PopulateParameterList(parameters);
            EnergyBalance = Convert.ToInt32(ParametersList[0]);
            FinancialBalance = Convert.ToDouble(ParametersList[1]) * - 1;
        }
        private void HandleInformation(string parameters)
        {
            PopulateParameterList(parameters);

            Demand = Convert.ToInt32(ParametersList[0]);
            Generated = Convert.ToInt32(ParametersList[1]);
            PriceToBuyFromUtility = Convert.ToInt32(ParametersList[2]);
            PriceTosellToUtility = Convert.ToInt32(ParametersList[3]);
            CalculateEnergyNeeds();
            CalculateProsumerValues();

            Console.WriteLine("Demand: {0}, Generation: {1}, PriceToBuyFromUtility: {2}, PriceTosellToUtility: {3}, Buyer: {4}, Participating: {5}",
                Demand, Generated, PriceToBuyFromUtility, PriceTosellToUtility, Buyer, Participating);
        }

        private void HandleStart()
        {
            Send("environmentAgent", $"RequestInformation");
        }

        private void CalculateProsumerValues()
        {
            if (EnergyBalance > 0) { Buyer = false; Participating = true; }
            if (EnergyBalance < 0) { Buyer = true; Participating = true; }
            if (EnergyBalance == 0) { Participating = false; }
        }
        public void CalculateEnergyNeeds()
        {
            EnergyBalance = Generated - Demand;
        }
        public void CalculatePriceSellToHousehold()
        {
            MinPriceSellToHousehold = Math.Round(PriceTosellToUtility / renewableEnergyPreference, 1);
        }
        public void CalculatePriceBuyFromHousehold()
        {
            MaxPiceBuyFromHousehold = Math.Round(PriceToBuyFromUtility * renewableEnergyPreference, 1);
        }

    }
}