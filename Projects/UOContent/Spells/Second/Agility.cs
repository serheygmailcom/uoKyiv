using Server.Engines.ConPVP;
using Server.Targeting;

namespace Server.Spells.Second
{
    public class AgilitySpell : MagerySpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Agility",
            "Ex Uus",
            212,
            9061,
            Reagent.Bloodmoss,
            Reagent.MandrakeRoot
        );

        public AgilitySpell(Mobile caster, Item scroll = null) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Second;

        public void CastSpellOnTarget(Mobile m)
        {
            if (CheckBSequence(m))
            {
                SpellHelper.Turn(Caster, m);

                var length = SpellHelper.GetDuration(Caster, m);
                SpellHelper.AddStatBonus(Caster, m, StatType.Dex, length, false);

                m.FixedParticles(0x375A, 10, 15, 5010, EffectLayer.Waist);
                m.PlaySound(0x1e7);

                var percentage = (int)(SpellHelper.GetOffsetScalar(Caster, m, false) * 100);

                BuffInfo.AddBuff(m, new BuffInfo(BuffIcon.Agility, 1075841, length, m, percentage.ToString()));
            }
        }

        public override bool CheckCast()
        {
            if (DuelContext.CheckSuddenDeath(Caster))
            {
                Caster.SendMessage(0x22, "You cannot cast this spell when in sudden death.");
                return false;
            }

            return base.CheckCast();
        }

        public override void OnCastingAfterMantra()
        {
            Caster.Target = new SpellTarget<Mobile>(this, TargetFlags.Beneficial);
        }
    }
}
