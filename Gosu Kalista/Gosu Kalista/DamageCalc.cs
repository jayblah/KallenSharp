using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

namespace Gosu_Kalista
{
    class DamageCalc
    {

        private static float CalculateRendDamage(Obj_AI_Base target)
        {
            var defuffer = 1f;

            if (target.HasBuff("FerociousHowl"))
                defuffer *= .7f;

            if (target.Name.Contains("Baron") && Properties.PlayerHero.HasBuff("barontarget"))
                    defuffer *= 0.5f;
            

            if (target.Name.Contains("Dragon") && Properties.PlayerHero.HasBuff("s5test_dragonslayerbuff"))
                defuffer *= (1 - (.07f* Properties.PlayerHero.GetBuffCount("s5test_dragonslayerbuff")));
            

            if (Properties.PlayerHero.HasBuff("summonerexhaust"))
                defuffer *= .4f;

            return Properties.Champion.E.GetDamage(target)*defuffer;
        }

        public static bool CheckNoDamageBuffs(Obj_AI_Hero target)// From Asuna
        {
            // Tryndamere R
            if (target.ChampionName != "Tryndamere" || !target.Buffs.Any(
                b => b.Caster.NetworkId == target.NetworkId && b.IsValidBuff() && b.DisplayName == "Undying Rage"))
            {
                if (!target.Buffs.Any(b => b.IsValidBuff() && b.DisplayName == "Chrono Shift"))
                {
                    if (!target.Buffs.Any(b => b.IsValidBuff() && b.DisplayName == "JudicatorIntervention"))
                    {
                        switch (target.ChampionName)
                        {
                            case "Poppy":
                                if (
                                    HeroManager.Allies.Any(
                                        o =>
                                            !o.IsMe
                                            && o.Buffs.Any(
                                                b =>
                                                    b.Caster.NetworkId == target.NetworkId && b.IsValidBuff()
                                                    && b.DisplayName == "PoppyDITarget")))
                                {
                                    return true;
                                }
                                break;
                        }

                        //Banshee's Veil, Sivir E or Noc W
                        return target.Buffs.Any(b => b.IsValidBuff() && b.DisplayName == "bansheesveil") ||
                               (target.Buffs.Any(b => b.IsValidBuff() && b.DisplayName == "SivirE") ||
                                (target.Buffs.Any(b => b.IsValidBuff() && b.DisplayName == "NocturneW") ||
                                 (target.HasBuffOfType(BuffType.Invulnerability)
                                  || target.HasBuffOfType(BuffType.SpellImmunity)
                                  || target.HasBuffOfType(BuffType.SpellShield))));
                    }

                    // Poppy R
                    return true;
                }

                // Kayle R
                return true;
            }

            // Zilean R
            return true;
        }

        public static float GetRendDamage(Obj_AI_Base target)
        {
            return !Properties.Champion.E.IsReady() ? 0f : CalculateRendDamage(target);
        }
    }
}
