using LeagueSharp;
using LeagueSharp.Common;
using System.Linq;

namespace S_Class_Kalista
{
    internal class DamageCalc
    {
        #region Public Functions

        public static bool CheckNoDamageBuffs(Obj_AI_Hero target)// From Asuna
        {
            foreach (var b in target.Buffs.Where(b => b.IsValidBuff()))
            {
                switch (b.DisplayName)
                {
                    case "Chrono Shift":
                        return true;

                    case "JudicatorIntervention":
                        return true;

                    case "Undying Rage":
                        if (target.ChampionName == "Tryndamere")
                            return true;
                        continue;

                    //Spell Shields
                    case "bansheesveil":
                        return true;

                    case "SivirE":
                        return true;

                    case "NocturneW":
                        return true;
                }
            }
            if (target.ChampionName == "Poppy" && HeroManager.Allies.Any(
                o =>
                {
                    return !o.IsMe
                           && o.Buffs.Any(
                               b =>
                                   b.Caster.NetworkId == target.NetworkId && b.IsValidBuff()
                                   && b.DisplayName == "PoppyDITarget");
                }))
            {
                return true;
            }

            return (target.HasBuffOfType(BuffType.Invulnerability)
                      || target.HasBuffOfType(BuffType.SpellImmunity)
                      || target.HasBuffOfType(BuffType.SpellShield));
        }

        public static float GetRendDamage(Obj_AI_Base target)
        {
            return !Properties.Champion.E.IsReady() ? 0f : CalculateRendDamage(target);
        }

        #endregion Public Functions

        #region Private Functions

        private static float CalculateRendDamage(Obj_AI_Base target)
        {
            var defuffer = 1f;

            if (target.HasBuff("FerociousHowl"))
                defuffer *= .7f;

            if (target.Name.Contains("Baron") && Properties.PlayerHero.HasBuff("barontarget"))
                defuffer *= 0.5f;

            if (target.Name.Contains("Dragon") && Properties.PlayerHero.HasBuff("s5test_dragonslayerbuff"))
                defuffer *= (1 - (.07f * Properties.PlayerHero.GetBuffCount("s5test_dragonslayerbuff")));

            if (Properties.PlayerHero.HasBuff("summonerexhaust"))
                defuffer *= .4f;

            return Properties.Champion.E.GetDamage(target) * defuffer;
        }

        #endregion Private Functions
    }
}