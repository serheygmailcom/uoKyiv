using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class IceMustang : BaseMount
    {
        public override string DefaultName => "an ice mustang";

        [Constructible]
        public IceMustang() : base(0x74, 0x3EA7, AIType.AI_Mage)
        {
            BaseSoundID = Core.AOS ? 0xA8 : 0x16A;

            Hue = 1288;
            // Publish 97
            if (Core.TOL)
            {
                if (Utility.RandomDouble() < 0.3)
                {
                    SetStr(140, 220);
                    ControlSlots = 2;
                }
                else
                {
                    SetStr(496, 525);
                    ControlSlots = 3;
                }
            }
            else
            {
                SetStr(280, 325);
                ControlSlots = 0;
            }

            SetDex(86, 105);
            SetInt(86, 125);

            SetHits(298, 315);

            SetDamage(16, 22);

            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Fire, 40);
            SetDamageType(ResistanceType.Energy, 20);

            SetResistance(ResistanceType.Physical, 55, 65);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.EvalInt, 10.4, 50.0);
            SetSkill(SkillName.Magery, 10.4, 50.0);
            SetSkill(SkillName.MagicResist, 85.3, 100.0);
            SetSkill(SkillName.Tactics, 97.6, 100.0);
            SetSkill(SkillName.Wrestling, 80.5, 92.5);

            Fame = 14000;
            Karma = 5000;

            VirtualArmor = 60;

            Tamable = true;
            MinTameSkill = 95.1;

            switch (Utility.Random(3))
            {
                case 0:
                    {
                        Body = 116;
                        ItemID = 16039;
                        break;
                    }
                case 1:
                    {
                        Body = 178;
                        ItemID = 16041;
                        break;
                    }
                case 2:
                    {
                        Body = 179;
                        ItemID = 16055;
                        break;
                    }
            }

            PackItem(new SulfurousAsh(Utility.RandomMinMax(3, 5)));
        }

        public override int StepsMax => 6400;
        public override string CorpseName => "an ice mustang corpse";
        public override int Meat => 5;
        public override int Hides => 2;
        public override HideType HideType => HideType.Barbed;
        public override FoodType FavoriteFood => FoodType.Meat;
        public override bool CanAngerOnTame => true;

        private static MonsterAbility[] _abilities = { MonsterAbilities.ColdBreath };
        public override MonsterAbility[] GetMonsterAbilities() => _abilities;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Average);
            AddLoot(LootPack.LowScrolls);
            AddLoot(LootPack.Potions);
        }

        public override int GetAngerSound()
        {
            if (!Controlled)
            {
                return 0x16A;
            }

            return base.GetAngerSound();
        }
    }
}
