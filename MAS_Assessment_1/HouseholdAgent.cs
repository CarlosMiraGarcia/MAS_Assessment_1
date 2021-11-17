using ActressMas;
using System;
using System.Collections.Generic;

namespace MAS_Assessment_1
{
    internal class HouseholdAgent : Agent
    {
        private double MinPriceSellToHousehold;
        private double MaxPiceBuyFromHousehold;
        private double PriceTosellToUtility;
        private double PriceToBuyFromUtility;
        private int Demand;
        private int Generated;
        private int EnergyBalance;
        private bool IsParticipating;
        private bool IsBuyer;
        private bool IsSeller;
        private double FinancialBalance;
        private readonly List<string> ParametersList = new List<string>();
        private double renewableEnergyPreference;

        public double RenewableEnergyPreference
        {
            get { return renewableEnergyPreference; }
            set
            {
                if (value == 0) { renewableEnergyPreference = 1.00; }
                if (value == 1) { renewableEnergyPreference = 1.01; }
                if (value == 2) { renewableEnergyPreference = 1.02; }
            }
        }

        public HouseholdAgent()
        {
            CalculateRenewableEnergyPreference();
        }

        private void CalculateRenewableEnergyPreference()
        {
            WeightedRandomizer<string> preferences = new WeightedRandomizer<string>();
            preferences.NewItem("Indiferent", 85);
            preferences.NewItem("MildyPrefered", 10);
            preferences.NewItem("GreatlyPrefered", 5);

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

                    case "Finished":
                        HandleAuctionEnd();
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

        private void HandleAuctionEnd()
        {
            Stop();
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
            if (IsParticipating && IsSeller)
            {
                CalculatePriceSellToHousehold();
                Send("auctioneer", $"Seller {EnergyBalance} {MinPriceSellToHousehold}");
            }
            else if (IsParticipating && IsBuyer)
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
            EnergyBalance = EnergyBalance - Convert.ToInt32(ParametersList[0]);
            FinancialBalance = Math.Round(FinancialBalance + Convert.ToDouble(ParametersList[1]), 2);
        }

        private void UpdateBuyerInfo(string parameters)
        {
            PopulateParameterList(parameters);
            EnergyBalance = EnergyBalance + Convert.ToInt32(ParametersList[0]);
            FinancialBalance = Math.Round(FinancialBalance - Convert.ToDouble(ParametersList[1]), 2);
        }

        private void HandleInformation(string parameters)
        {
            PopulateParameterList(parameters);

            Demand = Convert.ToInt32(ParametersList[0]);
            Generated = Convert.ToInt32(ParametersList[1]);
            PriceToBuyFromUtility = Math.Round(Convert.ToDouble(ParametersList[2]), 2);
            PriceTosellToUtility = Math.Round(Convert.ToDouble(ParametersList[3]), 2);
            CalculateEnergyNeeds();
            CalculateProsumerValues();

            //Console.WriteLine($"{this.Name} - Demand: {Demand}, Generation: {Generated}, PriceToBuyFromUtility: {PriceToBuyFromUtility}, " +
            //    $"PriceTosellToUtility: {PriceTosellToUtility}, Buyer: {IsBuyer}, Seller: {IsSeller}, Participating: {IsParticipating}");
        }

        private void HandleStart()
        {
            Send("environmentAgent", $"RequestInformation");
        }

        private void CalculateProsumerValues()
        {
            if (EnergyBalance > 0) { IsSeller = true; IsParticipating = true; }
            if (EnergyBalance < 0) { IsBuyer = true; IsParticipating = true; }
            if (EnergyBalance == 0) { IsParticipating = false; }
        }

        public void CalculateEnergyNeeds()
        {
            EnergyBalance = Generated - Demand;
        }

        public void CalculatePriceSellToHousehold()
        {
            MinPriceSellToHousehold = Math.Round(PriceTosellToUtility / renewableEnergyPreference, 2);
        }

        public void CalculatePriceBuyFromHousehold()
        {
            MaxPiceBuyFromHousehold = Math.Round(PriceToBuyFromUtility * renewableEnergyPreference, 2);
        }
    }
}