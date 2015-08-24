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
            {
                defuffer *= (1 - (.07f* Properties.PlayerHero.GetBuffCount("s5test_dragonslayerbuff")));
            }

            if (Properties.PlayerHero.HasBuff("summonerexhaust"))
                defuffer *= .4f;

            return Properties.Champion.E.GetDamage(target)*defuffer;
        }

        public static float GetRendDamage(Obj_AI_Base target)
        {
            return !Properties.Champion.E.IsReady() ? 0f : CalculateRendDamage(target);
        }
    }
}
