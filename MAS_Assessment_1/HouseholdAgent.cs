using ActressMas;
using System;
using System.Collections.Generic;

namespace MAS_Assessment_1
{
    public class HouseholdAgent : Agent
    {
        private double minPriceSellToHousehold; //min price to sell to household
        private double maxPiceBuyFromHousehold; //max price to buy from household
        private double priceToSellToUtility; //price to sell to utility
        private double priceToBuyFromUtility; //price to buy from utility
        private int demand; //energy demand
        private int generated; //generated energy
        private int energyBalance; //energy left to sell or to buy
        private double financialBalance; //total sold or spent
        private double renewableEnergyPreference; //preference for renewable energy
        private readonly List<string> ParametersList = new List<string>(); //list to store the parameters
        private bool IsParticipating; //bool to indicate whether the agent is participating in the auction or no
        private bool IsBuyer; //bool to indicate if the agent is a buyer
        private bool IsSeller; //bool to indicate if the agent is a seller

        public override void Act(Message message)
        {
            try
            {
                Console.WriteLine($"\t{message.Format()}"); //this line can be commented out so not all the messages are printed on the console
                message.Parse(out string action, out string parameters); //parses the incoming message to a string
                switch (action)
                {
                    case "Start":
                        HandleStart(); //when the message Start is sent to the agent, run the method HandleStart
                        break;

                    case "Information":
                        HandleInformation(parameters); //when the message Information is sent to the agent, run the method HandleInformation and pass the parameters
                        break;

                    case "BuyerOrSeller":
                        SendInformation(); //when the message BuyerOrSeller is sent to the agent, run the method SendInformation
                        break;

                    case "UpdateSeller":
                        UpdateSellerInfo(parameters); //when the message UpdateSeller is sent to the agent, run the method UpdateSellerInfo and pass the parameters
                        break;

                    case "UpdateBuyer":
                        UpdateBuyerInfo(parameters); //when the message UpdateBuyer is sent to the agent, run the method UpdateBuyerInfo and pass the parameters
                        break;

                    case "Finished":
                        HandleAuctionEnd(); //when the message Finished is sent to the agent, run the method HandleAuctionEnd
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
            ParametersList.Clear(); //clear the list with parameters before adding new parameters
            string[] splittedString = parameters.Split(' '); //splits the parameters string into an string array
            foreach (string parameter in splittedString)
            {
                ParametersList.Add(parameter); //adds the parameters from the array to the ParametersList
            }
        }

        private void HandleStart()
        {
            Send("environmentAgent", "RequestInformation"); //sends the message RequestInformation to the environment agent
        }

        private void HandleInformation(string parameters)
        {
            PopulateParameterList(parameters); //calls the method PopulateParameterList and passes the parameters

            demand = Convert.ToInt32(ParametersList[0]); //sets the demand for this agent
            generated = Convert.ToInt32(ParametersList[1]); //sets the generated energy for this agent
            priceToBuyFromUtility = Math.Round(Convert.ToDouble(ParametersList[2]), 2); //sets the price to buy from the utility company for this agent
            priceToSellToUtility = Math.Round(Convert.ToDouble(ParametersList[3]), 2); //sets the price to set to the utility company for this agent
            renewableEnergyPreference = Convert.ToDouble(ParametersList[4]); //sets the renewable energy preference for this agent

            CalculateEnergyNeeds(); //calls the method CalculateEnergyNeeds
            CalculateProsumerValues(); //calls the method CalculateProsumerValues
        }

        private void SendInformation()
        {
            if (IsParticipating && IsSeller) //checks if the agent is participating in the auction and if it's a seller
            {
                CalculatePriceSellToHousehold(); //calls the method CalculatePriceSellToHousehold
                Send("auctioneer", $"Seller {energyBalance} {minPriceSellToHousehold} {priceToSellToUtility}"); //sends a message to the auctioneer with the parameters for the auction
            }
            else if (IsParticipating && IsBuyer)
            {
                CalculatePriceBuyFromHousehold(); //calls the method CalculatePriceBuyFromHousehold
                Send("auctioneer", $"Buyer {Math.Abs(energyBalance)} {maxPiceBuyFromHousehold} {priceToBuyFromUtility} {renewableEnergyPreference}"); //sends a message to the auctioneer with the parameters for the auction
            }
            else
            {
                Send("auctioneer", "NoParticipating"); //send a message to the auctioneer to indicate the agent is not participating in the auction
            }
        }

        private void UpdateSellerInfo(string parameters)
        {
            PopulateParameterList(parameters); //calls the method PopulateParameterList
            energyBalance = energyBalance - Convert.ToInt32(ParametersList[0]); //updates the energyBalance
            financialBalance = Math.Round(financialBalance + Convert.ToDouble(ParametersList[1]), 2); //updates the finalBalance
        }

        private void UpdateBuyerInfo(string parameters)
        {
            PopulateParameterList(parameters); //calls the method PopulateParameterList
            energyBalance = energyBalance + Convert.ToInt32(ParametersList[0]); //updates the energyBalance
            financialBalance = Math.Round(financialBalance - Convert.ToDouble(ParametersList[1]), 2); //updates the finalBalance
        }

        private void CalculateProsumerValues()
        {
            // sets the booleans IsSeller, IsBuyer, and IsParticipating depending on the energyBalance
            if (energyBalance > 0) { IsSeller = true; IsParticipating = true; }
            if (energyBalance < 0) { IsBuyer = true; IsParticipating = true; }
            if (energyBalance == 0) { IsParticipating = false; }
        }

        public void CalculateEnergyNeeds()
        {
            energyBalance = generated - demand; //calculates the energyBalance
        }

        public void CalculatePriceSellToHousehold()
        {
            minPriceSellToHousehold = Math.Round(priceToSellToUtility, 2); //sets the min price to sell to household
        }

        public void CalculatePriceBuyFromHousehold()
        {
            maxPiceBuyFromHousehold = Math.Round(priceToBuyFromUtility * renewableEnergyPreference, 2); //calculates the max price to buy from household using the value from renewableEnergyPreference
        }

        private void HandleAuctionEnd()
        {
            //when the energyBalance is different than 0, checks if it's a buyer or a seller and buys or sells the left energy from the utility company
            while (energyBalance != 0)
            {
                if (IsBuyer)
                {
                    financialBalance -= priceToBuyFromUtility;
                    energyBalance++;
                }
                else if (IsSeller)
                {
                    financialBalance += priceToSellToUtility;
                    energyBalance--;
                }
            }
            Stop(); //stops the execution of the household agent and its removed from the environment
        }
    }    
}