using ActressMas;
using System;

namespace MAS_Assessment_1
{
    public class AuctioneerAgent : Agent
    {
        public AuctioneerAgent()
        {

        }
        public override void Setup()
        {
            Broadcast("start");
        }

        public override void Act(Message message)
        {
            switch(message.Content)
            {
                case "start":
                    HandleStart();
                    break;
                default:
                    break;
            }
        }

        private void HandleStart()
        {
            Console.WriteLine("Autions will start soon");
        }
    }
}
