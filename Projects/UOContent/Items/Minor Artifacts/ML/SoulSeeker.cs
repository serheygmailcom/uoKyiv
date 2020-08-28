namespace Server.Items
{
    public class SoulSeeker : RadiantScimitar
    {
        [Constructible]
        public SoulSeeker()
        {
            Hue = 0x38C;

            WeaponAttributes.HitLeechStam = 40;
            WeaponAttributes.HitLeechMana = 40;
            WeaponAttributes.HitLeechHits = 40;
            Attributes.WeaponSpeed = 60;
            Slayer = SlayerName.Repond;
        }

        public SoulSeeker(Serial serial) : base(serial)
        {
        }

        public override int LabelNumber => 1075046; // Soul Seeker

        public override int InitMinHits => 255;
        public override int InitMaxHits => 255;

        public override void GetDamageTypes(
            Mobile wielder, out int phys, out int fire, out int cold, out int pois,
            out int nrgy, out int chaos, out int direct
        )
        {
            cold = 100;

            pois = fire = phys = nrgy = chaos = direct = 0;
        }

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
