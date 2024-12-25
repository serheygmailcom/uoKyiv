using System;
using Server.Multis;
using Server.Spells.Sixth;
using Server.Targeting;

namespace Server.SkillHandlers;

public static class Hiding
{
    public static bool CombatOverride { get; set; }

    public static void Initialize()
    {
        SkillInfo.Table[21].Callback = OnUse;
    }


    public static TimeSpan OnUse(Mobile m)
    {
        m.RevealingAction();

        if (m.Spell != null)
        {
            m.SendLocalizedMessage(501238); // You are busy doing something else and cannot hide.
            
        }

        Target.Cancel(m);

        if (m.Warmode)
        {
            m.SendLocalizedMessage(501237); // You can't seem to hide right now.
            return TimeSpan.FromSeconds(3.0);
        }

        new InternalTimer(m).Start();

        return TimeSpan.FromSeconds(3.5);
    }

    private class InternalTimer : Timer
    {
        private readonly Mobile m;
        private readonly DateTime m_StartTime;

        public InternalTimer(Mobile m) : base(TimeSpan.FromSeconds(2.0))
        {
            this.m = m;
            m_StartTime = Core.Now;
        }

        protected override void OnTick()
        {

            if (m.Spell != null)
            {
                m.SendLocalizedMessage(501238); // You are busy doing something else and cannot hide.
                Stop();
            }
            if (!m.CheckAlive())
            {
                m.SendLocalizedMessage(501237); // You can't seem to hide right now.
                Stop();
            }
            if (!m.Region.OnSkillUse(m, 21))
            {
                m.SendLocalizedMessage(501237); // You can't seem to hide right now.
                Stop();
            }
            if (!m.AllowSkillUse((SkillName)21))
            {
                m.SendLocalizedMessage(501237); // You can't seem to hide right now.
                Stop();
            }
            var de = m.FindMostRecentDamageEntry(false);
            if (de != null && de.LastDamage > m_StartTime)
            {
                m.SendLocalizedMessage(501237); // You can't seem to hide right now.
                Stop();
            }
            Target.Cancel(m);

            if (m.Warmode)
            {
                m.SendLocalizedMessage(501237); // You can't seem to hide right now.
                Stop();
            }


            else
            {
                m.Hidden = true;
                m.LocalOverheadMessage(MessageType.Regular, 0x1F4, 501240); // You have hidden yourself well.
                InvisibilitySpell.StopTimer(m);
                _ = Stealth.OnUse(m);// 10 sec dealay of stealth is not used
                Stop();
            }


        }

    }

}
