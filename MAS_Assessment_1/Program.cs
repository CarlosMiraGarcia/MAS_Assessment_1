using ActressMas;
using System;

namespace MAS_Assessment_1
{
    public class Program
    {
        static void Main(string[] args)
        {
            var env = new EnvironmentMas(randomOrder: false, parallel: false);

            var auctioneerAgent = new AuctioneerAgent();
            env.Add(auctioneerAgent, "auctioneer");
            var environmentAgent = new EnvironmentAgent();
            env.Add(environmentAgent, "environmentAgent");

            for (int i = 1; i <= Settings.NumberOfHouseholds; i++)
            {
                var householdAgent = new HouseholdAgent();
                env.Add(householdAgent, $"householdAgent" + i);
            }

            env.Start();
            Console.ReadLine();

        }
    }
}
