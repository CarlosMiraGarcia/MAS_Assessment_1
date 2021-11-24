using ActressMas;
using System;

namespace MAS_Assessment_1
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var env = new EnvironmentMas(randomOrder: false, parallel: false);

            for (int i = 1; i <= EnvironmentAgent.NumberOfHouseholds; i++) //creates all the household agents
            {
                var householdAgent = new HouseholdAgent(); //creates a new householdAgent object
                env.Add(householdAgent, $"householdAgent" + i); //adds the new agent to the environment with a unique name
            }

            var auctioneerAgent = new AuctioneerAgent(); //creates a new auctioneerAgent object
            env.Add(auctioneerAgent, "auctioneer"); //adds the auctioneer agent to the environment
            var environmentAgent = new EnvironmentAgent(); //creates a new environmentAgent object
            env.Add(environmentAgent, "environmentAgent");//adds the environment agent to the environment

            env.Start(); //starts the simulation
            Console.ReadLine();
        }
    }
}