using System.Collections.Generic;
using Server.ContextMenus;
using Server.Gumps;
using Server.Multis;
using Server.Targeting;

namespace Server.Items
{
    public interface IDyable
    {
        bool Dye(Mobile from, DyeTub sender);
    }

    public class DyeTub : Item, ISecurable
    {
        private int m_DyedHue;
        private bool m_Redyable;

        [Constructible]
        public DyeTub() : base(0xFAB)
        {
            Weight = 10.0;
            m_Redyable = true;
        }

        public DyeTub(Serial serial) : base(serial)
        {
        }

        public virtual CustomHuePicker CustomHuePicker => null;

        public virtual bool AllowRunebooks => false;

        public virtual bool AllowFurniture => false;

        public virtual bool AllowStatuettes => false;

        public virtual bool AllowLeather => false;

        public virtual bool AllowDyables => true;

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual bool Redyable
        {
            get => m_Redyable;
            set => m_Redyable = value;
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int DyedHue
        {
            get => m_DyedHue;
            set
            {
                if (m_Redyable)
                {
                    m_DyedHue = value;
                    Hue = value;
                }
            }
        }

        // Three metallic tubs now.
        public virtual bool MetallicHues => false;

        // Select the clothing to dye.
        public virtual int TargetMessage => 500859;

        // You can not dye that.
        public virtual int FailMessage => 1042083;

        [CommandProperty(AccessLevel.GameMaster)]
        public SecureLevel Level { get; set; }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version

            writer.Write((int)Level);
            writer.Write(m_Redyable);
            writer.Write(m_DyedHue);
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    {
                        Level = (SecureLevel)reader.ReadInt();
                        goto case 0;
                    }
                case 0:
                    {
                        m_Redyable = reader.ReadBool();
                        m_DyedHue = reader.ReadInt();

                        break;
                    }
            }
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);
            SetSecureLevelEntry.AddTo(from, this, list);
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(GetWorldLocation(), 1))
            {
                from.SendLocalizedMessage(TargetMessage);
                from.Target = new InternalTarget(this);
            }
            else
            {
                from.SendLocalizedMessage(500446); // That is too far away.
            }
        }

        private class InternalTarget : Target
        {
            private readonly DyeTub m_Tub;

            public InternalTarget(DyeTub tub) : base(1, false, TargetFlags.None) => m_Tub = tub;

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Item item)
                {
                    if (item.QuestItem)
                    {
                        from.SendLocalizedMessage(1151836); // You may not dye toggled quest items.
                    }
                    else if (item is IDyable dyable && m_Tub.AllowDyables)
                    {
                        if (!from.InRange(m_Tub.GetWorldLocation(), 1) || !from.InRange(item.GetWorldLocation(), 1))
                            from.SendLocalizedMessage(500446); // That is too far away.
                        else if (item.Parent is Mobile)
                            from.SendLocalizedMessage(500861); // Can't Dye clothing that is being worn.
                        else if (dyable.Dye(from, m_Tub))
                            from.PlaySound(0x23E);
                    }
                    else if ((FurnitureAttribute.Check(item) || item is PotionKeg) && m_Tub.AllowFurniture)
                    {
                        if (!from.InRange(m_Tub.GetWorldLocation(), 1) || !from.InRange(item.GetWorldLocation(), 1))
                        {
                            from.SendLocalizedMessage(500446); // That is too far away.
                        }
                        else
                        {
                            var okay = item.IsChildOf(from.Backpack);

                            if (!okay)
                            {
                                if (item.Parent == null)
                                {
                                    var house = BaseHouse.FindHouseAt(item);

                                    if (house == null || !house.HasLockedDownItem(item) && !house.HasSecureItem(item))
                                        from.SendLocalizedMessage(501022); // Furniture must be locked down to paint it.
                                    else if (!house.IsCoOwner(from))
                                        from.SendLocalizedMessage(501023); // You must be the owner to use this item.
                                    else
                                        okay = true;
                                }
                                else
                                {
                                    from.SendLocalizedMessage(
                                        1048135
                                    ); // The furniture must be in your backpack to be painted.
                                }
                            }

                            if (okay)
                            {
                                item.Hue = m_Tub.DyedHue;
                                from.PlaySound(0x23E);
                            }
                        }
                    }
                    else if ((item is Runebook || item is RecallRune) && m_Tub.AllowRunebooks)
                    {
                        if (!from.InRange(m_Tub.GetWorldLocation(), 1) || !from.InRange(item.GetWorldLocation(), 1))
                        {
                            from.SendLocalizedMessage(500446); // That is too far away.
                        }
                        else if (!item.Movable)
                        {
                            from.SendLocalizedMessage(1049776); // You cannot dye runes or runebooks that are locked down.
                        }
                        else
                        {
                            item.Hue = m_Tub.DyedHue;
                            from.PlaySound(0x23E);
                        }
                    }
                    else if (item is MonsterStatuette && m_Tub.AllowStatuettes)
                    {
                        if (!from.InRange(m_Tub.GetWorldLocation(), 1) || !from.InRange(item.GetWorldLocation(), 1))
                        {
                            from.SendLocalizedMessage(500446); // That is too far away.
                        }
                        else if (!item.Movable)
                        {
                            from.SendLocalizedMessage(1049779); // You cannot dye statuettes that are locked down.
                        }
                        else
                        {
                            item.Hue = m_Tub.DyedHue;
                            from.PlaySound(0x23E);
                        }
                    }
                    else if ((item is BaseArmor armor &&
                              (armor.MaterialType == ArmorMaterialType.Leather ||
                               armor.MaterialType == ArmorMaterialType.Studded) || item is ElvenBoots ||
                              item is WoodlandBelt) && m_Tub.AllowLeather)
                    {
                        if (!from.InRange(m_Tub.GetWorldLocation(), 1) || !from.InRange(item.GetWorldLocation(), 1))
                        {
                            from.SendLocalizedMessage(500446); // That is too far away.
                        }
                        else if (!item.Movable)
                        {
                            from.SendLocalizedMessage(1042419); // You may not dye leather items which are locked down.
                        }
                        else if (item.Parent is Mobile)
                        {
                            from.SendLocalizedMessage(500861); // Can't Dye clothing that is being worn.
                        }
                        else
                        {
                            item.Hue = m_Tub.DyedHue;
                            from.PlaySound(0x23E);
                        }
                    }
                    else
                    {
                        from.SendLocalizedMessage(m_Tub.FailMessage);
                    }
                }
                else
                {
                    from.SendLocalizedMessage(m_Tub.FailMessage);
                }
            }
        }
    }
}
