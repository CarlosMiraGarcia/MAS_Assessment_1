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
            double totalPurchaseOnlyUtility = 0;
            double totalSavingsBuyers = 0;
            int totalAsksUnsold = 0;
            int totalBidsUnsold = 0;

            List<double> purchasesList = new List<double>();
            List<double> salesList = new List<double>();
            int line = 0;

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

            FlushConsole();

            Console.Clear();
            WriteAt("########################################", 2, line++);
            WriteAt("############## STATISTICS ##############", 2, line++);
            WriteAt("########################################", 2, line++);
            WriteAt("Transactions", 2, ++line);
            line++;
            WriteAt("Count:", 5, ++line);
            WriteAt("Sale Value:", 15, line);
            WriteAt("Purchase Value:", 30, line);
            for (int i = 0; i < purchasesList.Count; i++)
            {
                WriteAt(Convert.ToString(i + 1), 7, ++line);
                WriteAt("£" + salesList[i], 18, line);
                WriteAt("£" + purchasesList[i], 35, line);

                if (line > 30000)
                {
                    FlushConsole();
                    line = 0;
                }
            }

            line++;
            WriteAt("Total sellers: ", 2, ++line);
            WriteAt(Convert.ToString(sellerList.Count), 25, line);
            WriteAt("Total buyers: ", 2, ++line);
            WriteAt(Convert.ToString(buyerList.Count), 25, line);
            WriteAt("Total non participant: ", 2, ++line);
            WriteAt(Convert.ToString(Settings.NumberOfHouseholds - sellerList.Count - buyerList.Count), 25, line);
            WriteAt("Total households: ", 2, ++line);
            WriteAt(Convert.ToString(Settings.NumberOfHouseholds), 25, line);
            WriteAt("Total transactions: ", 2, ++line);
            WriteAt(Convert.ToString(salesList.Count), 25, line);

            FlushConsole();
            line = 0;

            var averageTransaction = Math.Round(totalSales / totalNumberSales, 2);

            WriteAt("#######################", 2, line++);
            WriteAt("####### SELLERS #######", 2, line++);
            WriteAt("#######################", 2, line++);
            WriteAt("ID:", 5, ++line);
            WriteAt("Number of Sales:", 25, line);
            WriteAt("Value:", 48, line);
            WriteAt("Unsold Items:", 60, line);
            foreach (Seller seller in sellerList)
            {
                WriteAt(seller.ID, 2, ++line);
                WriteAt(Convert.ToString(seller.Sales.Count), 30, line);
                WriteAt("£" + Math.Round(seller.TotalEarned, 2), 48, line);
                WriteAt(Convert.ToString(seller.AmountkWhToSell), 65, line);

                if (line > 30000)
                {
                    FlushConsole();
                    line = 0;
                }
            }

            line++;
            WriteAt("Total:", 2, ++line);
            WriteAt(Convert.ToString(totalNumberSales), 30, line);
            WriteAt("£" + Math.Round(totalSales, 2), 48, line);
            WriteAt(Convert.ToString(totalAsksUnsold), 65, line);

            FlushConsole();
            line = 0;

            WriteAt("#######################", 2, line++);
            WriteAt("####### BUYERS ########", 2, line++);
            WriteAt("#######################", 2, line++);
            WriteAt("ID:", 5, ++line);
            WriteAt("Purchased:", 25, line);
            WriteAt("To buy left:", 40, line);
            WriteAt("Total Spent:", 57, line);
            WriteAt("Total Spent + Utility:", 74, line);
            WriteAt("Total if Only Utility:", 102, line);
            WriteAt("Saved:", 132, line);
            foreach (Buyer buyer in buyerList)
            {
                double purchasesBidsPlusUtility = buyer.AmountkWhToBuy * buyer.PriceToBuyFromUtility + buyer.TotalSpent;
                totalPurchasesWithUtility += purchasesBidsPlusUtility;
                double purchasesOnlyUtility = (buyer.AmountkWhToBuy + buyer.Purchases.Count) * buyer.PriceToBuyFromUtility;
                totalPurchaseOnlyUtility += purchasesOnlyUtility;
                totalSavingsBuyers += purchasesOnlyUtility - purchasesBidsPlusUtility;

                WriteAt(buyer.ID, 2, ++line);
                WriteAt(Convert.ToString(buyer.Purchases.Count), 29, line);
                WriteAt(Convert.ToString(buyer.AmountkWhToBuy), 45, line);
                WriteAt("£" + Math.Round(buyer.TotalSpent, 2), 60, line);
                WriteAt("£" + Convert.ToString(Math.Round(purchasesBidsPlusUtility, 2)), 83, line);
                WriteAt("£" + Convert.ToString(Math.Round(purchasesOnlyUtility, 2)), 109, line);
                WriteAt("£" + Convert.ToString(Math.Round(purchasesOnlyUtility - purchasesBidsPlusUtility, 2)), 132, line);
            }
            line++;
            WriteAt("Total:", 2, ++line);
            WriteAt(Convert.ToString(totalNumberPurchases), 29, line);
            WriteAt(Convert.ToString(totalBidsUnsold), 45, line);
            WriteAt("£" + Math.Round(totalPurchases, 2), 60, line);
            WriteAt("£" + Math.Round(totalPurchasesWithUtility, 2), 83, line);
            WriteAt("£" + Math.Round(totalPurchaseOnlyUtility, 2), 109, line);
            WriteAt("£" + Math.Round(totalSavingsBuyers, 2), 132, line);


            FlushConsole();
            line = 0;
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
            int saveBufferWidth = Console.BufferWidth;
            Console.SetBufferSize(saveBufferWidth, short.MaxValue - 1);
            Console.Clear();
        }
    }
}



