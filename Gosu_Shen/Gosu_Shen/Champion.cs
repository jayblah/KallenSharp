using LeagueSharp;
using LeagueSharp.Common;

namespace Gosu_Shen
{
    class Champion
    {
        private static SpellSlot _ignite;
        private static SpellSlot _flash;
        private static SpellSlot _smite;
        private static readonly Obj_AI_Hero Player = ObjectManager.Player;

        public static Spell Q, W, E, R;

        public void LoadChampion()
        {
            _ignite = GlobalProperties.GetHero.GetSpellSlot("SummonerDot");
            _flash = GlobalProperties.GetHero.GetSpellSlot("SummonerFlash");
            _smite = GlobalProperties.GetHero.GetSpellSlot("SummonerSmite");

            if (_ignite == SpellSlot.Unknown) 

        }

        public static float IgniteDamage(Obj_AI_Hero target)
        {
            if (_ignite == SpellSlot.Unknown || Player.Spellbook.CanUseSpell(_ignite) != SpellState.Ready)
                return 0f;
            return (float)Player.GetSummonerSpellDamage(target, Damage.SummonerSpell.Ignite);
        }

        public static SpellSlot GetIgniteSlot()
        {
            return _ignite;
        }

        public static void SetIgniteSlot(SpellSlot nSpellSlot)
        {
            _ignite = nSpellSlot;
        }

    }
}
