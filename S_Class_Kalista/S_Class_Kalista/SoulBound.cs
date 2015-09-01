using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

namespace S_Class_Kalista
{
    class SoulBound
    {

        public static void CheckSoulBoundHero()
        {
            if (!Properties.Champion.R.IsReady()) return;

            if (Properties.SoulBoundHero == null)
                Properties.SoulBoundHero = HeroManager.Allies.Find(ally=> ally.Buffs.Any(user => user.Caster.IsMe && user.Name.Contains("kalistacoopstrikeally")));

            if (!Properties.Champion.R.IsInRange(Properties.SoulBoundHero) || Properties.SoulBoundHero.IsDead) return;
            if (Properties.SoulBoundHero.ChampionName == "Blitzcrank" && Properties.MainMenu.Item("bBalista").GetValue<bool>())
            {
                foreach (var target in ObjectManager.Get<Obj_AI_Hero>().Where(enem => enem.IsValid && enem.IsEnemy && enem.Distance(ObjectManager.Player) <= 2450f).Where(target => target.Buffs != null && target.Health > 300 && Properties.SoulBoundHero.Distance(target) > 450f))
                {
                    for (var i = 0; i < target.Buffs.Count(); i++)
                    {
                        if (target.Buffs[i].Name != "rocketgrab2" || !target.Buffs[i].IsActive) continue;
                        Properties.Champion.R.Cast();
                    }
                }

            }
            else if (Properties.SoulBoundHero.HealthPercent <
                Properties.MainMenu.Item("sSoulBoundPercent").GetValue<Slider>().Value &&
                Properties.SoulBoundHero.CountEnemiesInRange(500) > 0)
                Properties.Champion.R.Cast();

        }
    }
}
