namespace Server.Items
{
    public class HardenedWildStaff : WildStaff
    {
        [Constructible]
        public HardenedWildStaff() => Attributes.WeaponDamage = 5;

        public HardenedWildStaff(Serial serial) : base(serial)
        {
        }

        public override int LabelNumber => 1073552; // hardened wild staff

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadEncodedInt();
        }
    }
}
