using ActressMas;

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
    }
}
