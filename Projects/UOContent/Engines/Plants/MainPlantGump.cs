using System;
using Server.Engines.Help;
using Server.Gumps;
using Server.Items;
using Server.Network;

namespace Server.Engines.Plants
{
    public class MainPlantGump : Gump
    {
        private readonly PlantItem m_Plant;

        public MainPlantGump(PlantItem plant) : base(20, 20)
        {
            m_Plant = plant;

            DrawBackground();

            DrawPlant();

            AddButton(71, 67, 0xD4, 0xD4, 1); // Reproduction menu
            AddItem(59, 68, 0xD08);

            var system = plant.PlantSystem;

            AddButton(71, 91, 0xD4, 0xD4, 2); // Infestation
            AddItem(8, 96, 0x372);
            AddPlus(95, 92, system.Infestation);

            AddButton(71, 115, 0xD4, 0xD4, 3); // Fungus
            AddItem(58, 115, 0xD16);
            AddPlus(95, 116, system.Fungus);

            AddButton(71, 139, 0xD4, 0xD4, 4); // Poison
            AddItem(59, 143, 0x1AE4);
            AddPlus(95, 140, system.Poison);

            AddButton(71, 163, 0xD4, 0xD4, 5); // Disease
            AddItem(55, 167, 0x1727);
            AddPlus(95, 164, system.Disease);

            AddButton(209, 67, 0xD2, 0xD2, 6); // Water
            AddItem(193, 67, 0x1F9D);
            AddPlusMinus(196, 67, system.Water);

            AddButton(209, 91, 0xD4, 0xD4, 7); // Poison potion
            AddItem(201, 91, 0xF0A);
            AddLevel(196, 91, system.PoisonPotion);

            AddButton(209, 115, 0xD4, 0xD4, 8); // Cure potion
            AddItem(201, 115, 0xF07);
            AddLevel(196, 115, system.CurePotion);

            AddButton(209, 139, 0xD4, 0xD4, 9); // Heal potion
            AddItem(201, 139, 0xF0C);
            AddLevel(196, 139, system.HealPotion);

            AddButton(209, 163, 0xD4, 0xD4, 10); // Strength potion
            AddItem(201, 163, 0xF09);
            AddLevel(196, 163, system.StrengthPotion);

            AddImage(48, 47, 0xD2);
            AddLevel(54, 47, (int)m_Plant.PlantStatus);

            AddImage(232, 47, 0xD2);
            AddGrowthIndicator(239, 47);

            AddButton(48, 183, 0xD2, 0xD2, 11); // Help
            AddLabel(54, 183, 0x835, "?");

            AddButton(232, 183, 0xD4, 0xD4, 12); // Empty the bowl
            AddItem(219, 180, 0x15FD);
        }

        private void DrawBackground()
        {
            AddBackground(50, 50, 200, 150, 0xE10);

            AddItem(45, 45, 0xCEF);
            AddItem(45, 118, 0xCF0);

            AddItem(211, 45, 0xCEB);
            AddItem(211, 118, 0xCEC);
        }

        private void DrawPlant()
        {
            var status = m_Plant.PlantStatus;

            if (status < PlantStatus.FullGrownPlant)
            {
                AddImage(110, 85, 0x589);

                AddItem(122, 94, 0x914);
                AddItem(135, 94, 0x914);
                AddItem(120, 112, 0x914);
                AddItem(135, 112, 0x914);

                if (status >= PlantStatus.Stage2)
                {
                    AddItem(127, 112, 0xC62);
                }

                if (status is PlantStatus.Stage3 or PlantStatus.Stage4)
                {
                    AddItem(129, 85, 0xC7E);
                }

                if (status >= PlantStatus.Stage4)
                {
                    AddItem(121, 117, 0xC62);
                    AddItem(133, 117, 0xC62);
                }

                if (status >= PlantStatus.Stage5)
                {
                    AddItem(110, 100, 0xC62);
                    AddItem(140, 100, 0xC62);
                    AddItem(110, 130, 0xC62);
                    AddItem(140, 130, 0xC62);
                }

                if (status >= PlantStatus.Stage6)
                {
                    AddItem(105, 115, 0xC62);
                    AddItem(145, 115, 0xC62);
                    AddItem(125, 90, 0xC62);
                    AddItem(125, 135, 0xC62);
                }
            }
            else
            {
                var typeInfo = PlantTypeInfo.GetInfo(m_Plant.PlantType);
                var hueInfo = PlantHueInfo.GetInfo(m_Plant.PlantHue);

                // The large images for these trees trigger a client crash, so use a smaller, generic tree.
                if (m_Plant.PlantType is PlantType.CypressTwisted or PlantType.CypressStraight)
                {
                    AddItem(130 + typeInfo.OffsetX, 96 + typeInfo.OffsetY, 0x0CCA, hueInfo.Hue);
                }
                else
                {
                    AddItem(130 + typeInfo.OffsetX, 96 + typeInfo.OffsetY, typeInfo.ItemID, hueInfo.Hue);
                }
            }

            if (status != PlantStatus.BowlOfDirt)
            {
                var message = m_Plant.PlantSystem.GetLocalizedHealth();

                switch (m_Plant.PlantSystem.Health)
                {
                    case PlantHealth.Dying:
                        {
                            AddItem(92, 167, 0x1B9D);
                            AddItem(161, 167, 0x1B9D);

                            AddHtmlLocalized(136, 167, 42, 20, message, 0xFC00);

                            break;
                        }
                    case PlantHealth.Wilted:
                        {
                            AddItem(91, 164, 0x18E6);
                            AddItem(161, 164, 0x18E6);

                            AddHtmlLocalized(132, 167, 42, 20, message, 0xC207);

                            break;
                        }
                    case PlantHealth.Healthy:
                        {
                            AddItem(96, 168, 0xC61);
                            AddItem(162, 168, 0xC61);

                            AddHtmlLocalized(129, 167, 42, 20, message, 0x8200);

                            break;
                        }
                    case PlantHealth.Vibrant:
                        {
                            AddItem(93, 162, 0x1A99);
                            AddItem(162, 162, 0x1A99);

                            AddHtmlLocalized(129, 167, 42, 20, message, 0x83E0);

                            break;
                        }
                }
            }
        }

        private void AddPlus(int x, int y, int value)
        {
            switch (value)
            {
                case 1:
                    {
                        AddLabel(x, y, 0x35, "+");
                        break;
                    }
                case 2:
                    {
                        AddLabel(x, y, 0x21, "+");
                        break;
                    }
            }
        }

        private void AddPlusMinus(int x, int y, int value)
        {
            switch (value)
            {
                case 0:
                    {
                        AddLabel(x, y, 0x21, "-");
                        break;
                    }
                case 1:
                    {
                        AddLabel(x, y, 0x35, "-");
                        break;
                    }
                case 3:
                    {
                        AddLabel(x, y, 0x35, "+");
                        break;
                    }
                case 4:
                    {
                        AddLabel(x, y, 0x21, "+");
                        break;
                    }
            }
        }

        private void AddLevel(int x, int y, int value)
        {
            AddLabel(x, y, 0x835, value.ToString());
        }

        private void AddGrowthIndicator(int x, int y)
        {
            if (!m_Plant.IsGrowable)
            {
                return;
            }

            switch (m_Plant.PlantSystem.GrowthIndicator)
            {
                default:
                case PlantGrowthIndicator.None:
                case PlantGrowthIndicator.InvalidLocation:
                    {
                        AddLabel(x, y, 0x21, "!");
                        break;
                    }
                case PlantGrowthIndicator.NotHealthy:
                    {
                        AddLabel(x, y, 0x21, "-");
                        break;
                    }
                case PlantGrowthIndicator.Delay:
                    {
                        AddLabel(x, y, 0x35, "-");
                        break;
                    }
                case PlantGrowthIndicator.Grown:
                    {
                        AddLabel(x, y, 0x3, "+");
                        break;
                    }
                case PlantGrowthIndicator.DoubleGrown:
                    {
                        AddLabel(x, y, 0x3F, "+");
                        break;
                    }
            }
        }

        public override void OnResponse(NetState sender, in RelayInfo info)
        {
            var from = sender.Mobile;

            if (info.ButtonID == 0 || m_Plant.Deleted || m_Plant.PlantStatus >= PlantStatus.DecorativePlant)
            {
                return;
            }

            if ((info.ButtonID >= 6 && info.ButtonID <= 10 || info.ButtonID == 12) &&
                !from.InRange(m_Plant.GetWorldLocation(), 3))
            {
                from.LocalOverheadMessage(MessageType.Regular, 0x3E9, 500446); // That is too far away.
                return;
            }

            if (!m_Plant.IsUsableBy(from))
            {
                m_Plant.LabelTo(from, 1061856); // You must have the item in your backpack or locked down in order to use it.
                return;
            }

            switch (info.ButtonID)
            {
                case 1: // Reproduction menu
                    {
                        if (m_Plant.PlantStatus > PlantStatus.BowlOfDirt)
                        {
                            from.SendGump(new ReproductionGump(m_Plant));
                        }
                        else
                        {
                            from.SendLocalizedMessage(1061885); // You need to plant a seed in the bowl first.

                            from.SendGump(new MainPlantGump(m_Plant));
                        }

                        break;
                    }
                case 2: // Infestation
                    {
                        from.NetState.SendDisplayHelpTopic(HelpTopic.InfestationLevel);

                        from.SendGump(new MainPlantGump(m_Plant));

                        break;
                    }
                case 3: // Fungus
                    {
                        from.NetState.SendDisplayHelpTopic(HelpTopic.FungusLevel);

                        from.SendGump(new MainPlantGump(m_Plant));

                        break;
                    }
                case 4: // Poison
                    {
                        from.NetState.SendDisplayHelpTopic(HelpTopic.PoisonLevel);

                        from.SendGump(new MainPlantGump(m_Plant));

                        break;
                    }
                case 5: // Disease
                    {
                        from.NetState.SendDisplayHelpTopic(HelpTopic.DiseaseLevel);

                        from.SendGump(new MainPlantGump(m_Plant));

                        break;
                    }
                case 6: // Water
                    {
                        BaseBeverage bev = null;

                        foreach (var beverage in from.Backpack.FindItemsByType<BaseBeverage>())
                        {
                            if (beverage.IsEmpty && beverage.Pourable && beverage.Content == BeverageType.Water)
                            {
                                bev = beverage;
                                break;
                            }
                        }

                        if (bev == null)
                        {
                            from.Target = new PlantPourTarget(m_Plant);

                            // CastSpellOnTarget the container you wish to use to water the ~1_val~.
                            from.SendLocalizedMessage(1060808, $"#{m_Plant.GetLocalizedPlantStatus()}");
                        }
                        else
                        {
                            m_Plant.Pour(from, bev);
                        }

                        from.SendGump(new MainPlantGump(m_Plant));

                        break;
                    }
                case 7: // Poison potion
                    {
                        AddPotion(from, PotionEffect.PoisonGreater, PotionEffect.PoisonDeadly);

                        break;
                    }
                case 8: // Cure potion
                    {
                        AddPotion(from, PotionEffect.CureGreater);

                        break;
                    }
                case 9: // Heal potion
                    {
                        AddPotion(from, PotionEffect.HealGreater);

                        break;
                    }
                case 10: // Strength potion
                    {
                        AddPotion(from, PotionEffect.StrengthGreater);

                        break;
                    }
                case 11: // Help
                    {
                        from.NetState.SendDisplayHelpTopic(HelpTopic.PlantGrowing);

                        from.SendGump(new MainPlantGump(m_Plant));

                        break;
                    }
                case 12: // Empty the bowl
                    {
                        from.SendGump(new EmptyTheBowlGump(m_Plant));

                        break;
                    }
            }
        }

        private void AddPotion(Mobile from, params PotionEffect[] effects)
        {
            var item = GetPotion(from, effects);

            if (item != null)
            {
                m_Plant.Pour(from, item);
            }
            else
            {
                if (m_Plant.ApplyPotion(effects[0], true, out var message))
                {
                    from.SendLocalizedMessage(1061884); // You don't have any strong potions of that type in your pack.

                    from.Target = new PlantPourTarget(m_Plant);

                    // CastSpellOnTarget the container you wish to use to water the ~1_val~.
                    from.SendLocalizedMessage(1060808, $"#{m_Plant.GetLocalizedPlantStatus()}");

                    return;
                }

                m_Plant.LabelTo(from, message);
            }

            from.SendGump(new MainPlantGump(m_Plant));
        }

        public static Item GetPotion(Mobile from, PotionEffect[] effects)
        {
            if (from.Backpack == null)
            {
                return null;
            }

            foreach (var item in from.Backpack.FindItems())
            {
                if (item is BasePotion potion)
                {
                    if (Array.IndexOf(effects, potion.PotionEffect) >= 0)
                    {
                        return potion;
                    }
                }
                else if (item is PotionKeg keg && keg.Held > 0 && Array.IndexOf(effects, keg.Type) >= 0)
                {
                    return keg;
                }
            }

            return null;
        }
    }
}
