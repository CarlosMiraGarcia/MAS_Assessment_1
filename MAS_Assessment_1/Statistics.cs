using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            List<double> purchasesList = new List<double>();
            List<double> salesList = new List<double>();
            int line = 0;

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
            }

            purchasesList = purchasesList.OrderBy(x => x).ToList();
            salesList = salesList.OrderBy(x => x).ToList();

            Console.WriteLine("Please, make the console full screen to allow all statistic to fit the screen,");
            Console.WriteLine("otherwise, the console won't be able to display them.");
            FlushConsole();

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
            WriteAt(" Total sellers:                      ", 46, ++line);
            WriteAt(" " + Convert.ToString(sellerList.Count) + " ", 75, line);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            WriteAt(" Total buyers: ", 46, ++line);
            WriteAt(" " + Convert.ToString(buyerList.Count) + " ", 75, line);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            WriteAt(" Total non participant:              ", 46, ++line);
            WriteAt(" " + Convert.ToString(Settings.NumberOfHouseholds - sellerList.Count - buyerList.Count) + " ", 75, line);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            WriteAt(" Total households: ", 46, ++line);
            WriteAt(" " + Convert.ToString(Settings.NumberOfHouseholds) + " ", 75, line);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            WriteAt(" Total transactions:                 ", 46, ++line);
            WriteAt(" " + Convert.ToString(salesList.Count) + " ", 75, line);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            FlushConsole();
            line = 0;

            var averageTransaction = Math.Round(totalSales / totalNumberSales, 2);

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
            WriteAt(" ID " , 7, ++line);
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



