using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
    public class SBRanger : SBInfo
    {
        public override IShopSellInfo SellInfo { get; } = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo { get; } = new InternalBuyInfo();

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(Cat), 138, 20, 201, 0));
                Add(new AnimalBuyInfo(1, typeof(Dog), 181, 20, 217, 0));
                Add(new AnimalBuyInfo(1, typeof(PackLlama), 491, 20, 292, 0));
                Add(new AnimalBuyInfo(1, typeof(PackHorse), 606, 20, 291, 0));
                Add(new GenericBuyInfo(typeof(Bandage), 5, 20, 0xE21, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
        }
    }
}
