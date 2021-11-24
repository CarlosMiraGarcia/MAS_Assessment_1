using System;
using System.Collections.Generic;
using System.Linq;

namespace MAS_Assessment_1
{
    public class Statistics
    {
        public void CreateStatistics(List<Buyer> buyerList, List<Seller> sellerList)
        {
            int totalNumberSales = 0;
            int totalNumberPurchases = 0;
            double totalSales = 0;
            double totalPurchases = 0;
            double totalPurchasesWithUtility = 0;
            double totalSalesWithUtility = 0;
            double totalPurchaseOnlyUtility = 0;
            double totalSalesOnlyUtility = 0;
            double totalSavingsBuyers = 0;
            double totalExtraSellers = 0;
            int totalAsksUnsold = 0;
            int totalBidsUnsold = 0;
            double unsoldSeller = 0;
            double unsoldBuyer1 = 0;
            double unsoldBuyer11 = 0;
            double unsoldBuyer12 = 0;
            int line = 0;
            List<double> purchasesList = new List<double>();
            List<double> salesList = new List<double>();
            List<double> sellerAVG = new List<double>();
            List<double> preferenceBuyer1AVG = new List<double>();
            List<double> preferenceBuyer11AVG = new List<double>();
            List<double> preferenceBuyer12AVG = new List<double>();

            int saveBufferWidth = Console.BufferWidth;
            Console.SetBufferSize(saveBufferWidth, short.MaxValue - 1);

            foreach (Seller seller in sellerList)
            {
                totalNumberSales += seller.Sales.Count;
                totalAsksUnsold += seller.AmountkWhToSell;
                foreach (double sale in seller.Sales)
                {
                    salesList.Add(sale);
                    totalSales += sale;
                }

                if (seller.Sales.Count > 0)
                {
                    foreach (double sale in seller.Sales)
                    {
                        sellerAVG.Add(sale);
                    }
                }
                else { unsoldSeller++; }
            }

            foreach (Buyer buyer in buyerList)
            {
                totalNumberPurchases += buyer.Purchases.Count;
                totalBidsUnsold += buyer.AmountkWhToBuy;
                foreach (double purchase in buyer.Purchases)
                {
                    purchasesList.Add(purchase);
                    totalPurchases += purchase;
                }

                if (buyer.Preference == 1)
                {
                    if (buyer.Purchases.Count > 0)
                    {
                        foreach (double sale in buyer.Purchases)
                        {
                            preferenceBuyer1AVG.Add(sale);
                        }
                    }
                    else { unsoldBuyer1++; }
                }
                if (buyer.Preference == 1.15)
                {
                    if (buyer.Purchases.Count > 0)
                    {
                        foreach (double sale in buyer.Purchases)
                        {
                            preferenceBuyer11AVG.Add(sale);
                        }
                    }
                    else { unsoldBuyer11++; }
                }
                if (buyer.Preference == 1.3)
                {
                    if (buyer.Purchases.Count > 0)
                    {
                        foreach (double sale in buyer.Purchases)
                        {
                            preferenceBuyer12AVG.Add(sale);
                        }
                    }
                    else { unsoldBuyer12++; }
                }
            }

            purchasesList = purchasesList.OrderBy(x => x).ToList();
            salesList = salesList.OrderBy(x => x).ToList();

            Console.WriteLine("\n\n--> Please, make the console full screen to allow all the statistic to fit in the screen,");
            Console.WriteLine("--> otherwise, the console won't be able to display them.");
            FlushConsole();

            WriteAt("#######################", 50, line++);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            WriteAt("        SELLERS         ", 49, line++);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            WriteAt("#######################", 50, line++);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            line++;
            WriteAt(" ID ", 7, ++line);
            WriteAt(" Sold ", 25, line);
            WriteAt(" Unsold Lots ", 34, line);
            WriteAt(" Total Received ", 49, line);
            WriteAt(" Received + Utility ", 67, line);
            WriteAt(" Total if Only Utility ", 89, line);
            WriteAt(" Extra £ ", 117, line);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            foreach (Seller seller in sellerList)
            {
                double salesAsksPlusUtility = seller.AmountkWhToSell * seller.PriceToSellToUtility + seller.TotalEarned;
                totalSalesWithUtility += salesAsksPlusUtility;
                double salesOnlyUtility = (seller.AmountkWhToSell + seller.Sales.Count) * seller.PriceToSellToUtility;
                totalSalesOnlyUtility += salesOnlyUtility;
                totalExtraSellers += salesAsksPlusUtility - salesOnlyUtility;

                WriteAt(seller.ID, 2, ++line);
                WriteAt(" " + Convert.ToString(seller.Sales.Count) + " kWh ", 25, line);
                WriteAt(" " + Convert.ToString(seller.AmountkWhToSell) + " kWh ", 37, line);
                WriteAt(" £" + Math.Round(seller.TotalEarned, 2) + " ", 52, line);
                WriteAt(" £" + Convert.ToString(Math.Round(salesAsksPlusUtility, 2)) + " ", 72, line);
                WriteAt(" £" + Convert.ToString(Math.Round(salesOnlyUtility, 2)) + " ", 96, line);
                WriteAt(" £" + Convert.ToString(Math.Round(salesAsksPlusUtility - salesOnlyUtility, 2)) + " ", 118, line);
            }
            line++;
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            WriteAt(" Total: ", 2, ++line);
            WriteAt(" " + Convert.ToString(totalNumberSales) + " kWh ", 25, line);
            WriteAt(" " + Convert.ToString(totalAsksUnsold) + " kWh ", 36, line);
            WriteAt(" £" + Math.Round(totalSales, 2) + " ", 53, line);
            WriteAt(" £" + Math.Round(totalSalesWithUtility, 2) + " ", 72, line);
            WriteAt(" £" + Math.Round(totalSalesOnlyUtility, 2) + " ", 96, line);
            WriteAt(" £" + Math.Round(totalExtraSellers, 2) + " ", 118, line);

            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            line++;
            WriteAt(" Sold ", 25, ++line);
            WriteAt(" Unsold Lots ", 34, line);
            WriteAt(" Total Received ", 49, line);
            WriteAt(" Received + Utility ", 67, line);
            WriteAt(" Total if Only Utility ", 89, line);
            WriteAt(" Extra £ ", 117, line);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            FlushConsole();
            line = 0;

            WriteAt("#######################", 50, line++);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            WriteAt("         BUYERS          ", 49, line++);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            WriteAt("#######################", 50, line++);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            line++;
            WriteAt(" ID ", 7, ++line);
            WriteAt(" Purchased ", 22, line);
            WriteAt(" Unpurchased ", 34, line);
            WriteAt(" Total Spent ", 49, line);
            WriteAt(" Total Spent + Utility ", 64, line);
            WriteAt(" Total if Only Utility ", 89, line);
            WriteAt(" Saved £ ", 117, line);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            foreach (Buyer buyer in buyerList)
            {
                double purchasesBidsPlusUtility = buyer.AmountkWhToBuy * buyer.PriceToBuyFromUtility + buyer.TotalSpent;
                totalPurchasesWithUtility += purchasesBidsPlusUtility;
                double purchasesOnlyUtility = (buyer.AmountkWhToBuy + buyer.Purchases.Count) * buyer.PriceToBuyFromUtility;
                totalPurchaseOnlyUtility += purchasesOnlyUtility;
                totalSavingsBuyers += purchasesOnlyUtility - purchasesBidsPlusUtility;

                WriteAt(buyer.ID, 2, ++line);
                WriteAt(Convert.ToString(buyer.Purchases.Count) + " kWh", 25, line);
                WriteAt(Convert.ToString(buyer.AmountkWhToBuy) + " kWh", 38, line);
                WriteAt("£" + Math.Round(buyer.TotalSpent, 2), 52, line);
                WriteAt("£" + Convert.ToString(Math.Round(purchasesBidsPlusUtility, 2)), 72, line);
                WriteAt("£" + Convert.ToString(Math.Round(purchasesOnlyUtility, 2)), 98, line);
                WriteAt("£" + Convert.ToString(Math.Round(purchasesOnlyUtility - purchasesBidsPlusUtility, 2)), 118, line);
            }
            line++;
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            WriteAt(" Total: ", 2, ++line);
            WriteAt(" " + Convert.ToString(totalNumberPurchases) + " kWh ", 24, line);
            WriteAt(" " + Convert.ToString(totalBidsUnsold) + " kWh ", 37, line);
            WriteAt(" £" + Math.Round(totalPurchases, 2) + " ", 51, line);
            WriteAt(" £" + Math.Round(totalPurchasesWithUtility, 2) + " ", 71, line);
            WriteAt(" £" + Math.Round(totalPurchaseOnlyUtility, 2) + " ", 97, line);
            WriteAt(" £" + Math.Round(totalSavingsBuyers, 2) + " ", 117, line);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            line++;
            WriteAt(" Purchased ", 22, ++line);
            WriteAt(" Unpurchased ", 34, line);
            WriteAt(" Total Spent ", 49, line);
            WriteAt(" Total Spent + Utility ", 64, line);
            WriteAt(" Total if Only Utility ", 89, line);
            WriteAt(" Saved £ ", 117, line);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            FlushConsole();
            line = 0;

            WriteAt("#############################", 50, line++);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            WriteAt("           STATISTICS          ", 49, line++);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            WriteAt("#############################", 50, line++);

            line++;
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            WriteAt(" Total sellers:                          ", 44, ++line);
            WriteAt(" " + Convert.ToString(sellerList.Count) + " ", 72, line);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            WriteAt(" Total buyers:                          ", 44, ++line);
            WriteAt(" " + Convert.ToString(buyerList.Count) + " ", 72, line);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            WriteAt(" Total non participant:                  ", 44, ++line);
            WriteAt(" " + Convert.ToString(EnvironmentAgent.NumberOfHouseholds - sellerList.Count - buyerList.Count) + " ", 72, line);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            WriteAt(" Total households:                          ", 44, ++line);
            WriteAt(" " + Convert.ToString(EnvironmentAgent.NumberOfHouseholds) + " ", 72, line);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            WriteAt(" Total transactions:                     ", 44, ++line);
            WriteAt(" " + Convert.ToString(salesList.Count) + " ", 72, line);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            WriteAt(" Avg % Profit per Seller:                ", 44, ++line);
            WriteAt(" " + Math.Round(totalSalesWithUtility / totalSalesOnlyUtility * 100, 2) + "% ", 72, line);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            WriteAt(" Avg % Savings per Buyers:               ", 44, ++line);
            WriteAt(" " + Math.Round(totalPurchasesWithUtility / totalPurchaseOnlyUtility * 100, 2) + "% ", 72, line);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            WriteAt(" Max Value Transaction:                  ", 44, ++line);
            WriteAt(" £" + salesList.Max() + " ", 72, line);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            WriteAt(" Min Value Transaction:                  ", 44, ++line);
            WriteAt(" £" + salesList.Min() + " ", 72, line);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            WriteAt(" Total profit for sellers:               ", 44, ++line);
            WriteAt(" £" + Math.Round(totalExtraSellers, 2) + " ", 72, line);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            WriteAt(" Total savings for buyers:               ", 44, ++line);
            WriteAt(" £" + Math.Round(totalSavingsBuyers, 2) + " ", 72, line);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            WriteAt(" Avg profit per seller:                  ", 44, ++line);
            WriteAt(" £" + Math.Round(totalExtraSellers / sellerList.Count, 2) + " ", 72, line);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            WriteAt(" Avg savings per buyer:                  ", 44, ++line);
            WriteAt(" £" + Math.Round(totalSavingsBuyers / buyerList.Count, 2) + " ", 72, line);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            double sellerAVGTotal = Math.Round(sellerAVG.Sum() / sellerAVG.Count, 2);
            double preferenceBuyer1AVGTotal = Math.Round(preferenceBuyer1AVG.Sum() / preferenceBuyer1AVG.Count, 2);
            double preferenceBuyer11AVGTotal = Math.Round(preferenceBuyer11AVG.Sum() / preferenceBuyer11AVG.Count, 2);
            double preferenceBuyer12AVGTotal = Math.Round(preferenceBuyer12AVG.Sum() / preferenceBuyer12AVG.Count, 2);

            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            line += 5;
            WriteAt("Sellers:                                                                                ", 1, line);
            WriteAt(" AVG final ask value ", 27, line);
            WriteAt(" Unfinished asks ", 50, line);
            WriteAt(" % Unfinished asks ", 68, line);
            WriteAt(" Total Asks ", 90, line);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            WriteAt("£" + Convert.ToString(sellerAVGTotal), 35, ++line);
            WriteAt(Convert.ToString(unsoldSeller), 57, line);
            WriteAt(Convert.ToString(Math.Round((unsoldSeller / sellerAVG.Count() * 100), 2)) + "%", 77, line);
            WriteAt(Convert.ToString((sellerAVG.Count() + unsoldSeller)), 95, line);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            line++;
            WriteAt("Buyers:                                                                                ", 1, line);
            WriteAt(" AVG final bid value ", 27, line);
            WriteAt(" Unfinished bids ", 50, line);
            WriteAt(" % Unfinished bids ", 68, line);
            WriteAt(" Total bids ", 90, line);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            WriteAt(" Indifferent", 2, ++line);
            WriteAt("£" + Convert.ToString(preferenceBuyer1AVGTotal), 35, line);
            WriteAt(Convert.ToString(unsoldBuyer1), 57, line);
            WriteAt(Convert.ToString(Math.Round((unsoldBuyer1 / preferenceBuyer1AVG.Count() * 100), 2)) + "%", 77, line);
            WriteAt(Convert.ToString((preferenceBuyer1AVG.Count() + unsoldBuyer1)), 95, line);
            WriteAt(" Mildly Preferred", 2, ++line);
            WriteAt("£" + Convert.ToString(preferenceBuyer11AVGTotal), 35, line);
            WriteAt(Convert.ToString(unsoldBuyer11), 57, line);
            WriteAt(Convert.ToString(Math.Round((unsoldBuyer11 / preferenceBuyer11AVG.Count() * 100), 2)) + "%", 77, line);
            WriteAt(Convert.ToString((preferenceBuyer11AVG.Count() + unsoldBuyer11)), 95, line);
            WriteAt(" Greatly Preferred", 2, ++line);
            WriteAt("£" + Convert.ToString(preferenceBuyer12AVGTotal), 35, line);
            WriteAt(Convert.ToString(unsoldBuyer12), 57, line);
            WriteAt(Convert.ToString(Math.Round((unsoldBuyer12 / preferenceBuyer12AVG.Count() * 100), 2)) + "%", 77, line);
            WriteAt(Convert.ToString((preferenceBuyer12AVG.Count() + unsoldBuyer12)), 95, line);

            Console.WriteLine("\n\n\nPress any key to exit");
        }

        //https://docs.microsoft.com/en-us/dotnet/api/system.console.setcursorposition?view=net-5.0#System_Console_SetCursorPosition_System_Int32_System_Int32_
        protected static int origRow;

        protected static int origCol;

        protected static void WriteAt(string s, int x, int y)
        {
            try
            {
                Console.SetCursorPosition(origCol + x, origRow + y);
                Console.Write(s);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
            }
        }

        protected static void FlushConsole()
        {
            Console.WriteLine("\n\nPlease, press Enter to continue");
            Console.Out.Flush();
            Console.ReadLine();
            Console.Clear();
        }
    }
}