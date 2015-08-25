using System.Linq;
using LeagueSharp.Common;


namespace Gosu_Kalista
{
    internal class OrbWalkerManager
    {
        public void EventCheck()
        {
            CheckEpicMonsters();
            CheckEnemies();
        }

        private static void CheckEpicMonsters()
        {
            foreach (var mob in MinionManager.GetMinions(Properties.PlayerHero.ServerPosition,
                Properties.Champion.E.Range,
                MinionTypes.All,
                MinionTeam.Neutral,
                MinionOrderTypes.MaxHealth).Where(mob => mob.Name.Contains("Baron") || mob.Name.Contains("Dragon")).Where(mob => DamageCalc.GetRendDamage(mob) > mob.Health))
            {
                Properties.Champion.E.Cast();
            }
        }

        private static void CheckEnemies()
        {
            foreach (var target in from target in HeroManager.Enemies where target != null where !target.IsDead where !DamageCalc.CheckNoDamageBuffs(target) where Properties.Champion.E.IsInRange(target) where !(DamageCalc.GetRendDamage(target) < target.Health) select target)
            {
                Properties.Champion.E.Cast();
            }
        }

        public static void OrbWalk()
        {

            switch (Properties.LukeOrbWalker.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Combo:
                    Combo();
                    break;

                case Orbwalking.OrbwalkingMode.Mixed:
                    Mixed();
                    break;

                case Orbwalking.OrbwalkingMode.LaneClear:
                    LaneClear();
                    break;

                case Orbwalking.OrbwalkingMode.LastHit:
                    LastHit();
                    break;
            }
        }

        private static void Combo()
        {

        }

        private static void Mixed()
        {
            if (Properties.MainMenu.Item("useQinMixed").GetValue<bool>() &&Properties.Champion.Q.IsReady())
            {
                var target = TargetSelector.GetTarget(Properties.Champion.Q.Range, TargetSelector.DamageType.Physical);
                var predictionPosition = Properties.Champion.Q.GetPrediction(target);
                if (predictionPosition.Hitchance >= HitChance.VeryHigh)
                    if (Properties.PlayerHero.IsWindingUp || Properties.PlayerHero.IsDashing())
                        Properties.Champion.Q.Cast(predictionPosition.CastPosition);   
            }
            if (!Properties.MainMenu.Item("useEinMixed").GetValue<bool>()) return;

            foreach (var target in HeroManager.Enemies)
            {
                if (target == null || !target.IsValid) continue;
                if (!target.IsValidTarget(Properties.Champion.E.Range)) continue;
                if (DamageCalc.CheckNoDamageBuffs(target)) continue;
                var stacks = target.GetBuffCount("kalistaexpungemarker");
                if (stacks < Properties.MainMenu.Item("minStacksHarass").GetValue<Slider>().Value) continue;
                Properties.Champion.E.Cast();
            }

        }
    

    private static void LaneClear()
        {

        }
        private static void LastHit()
        {
            // Handles itself?
            if (!Properties.MainMenu.Item("useEToLastHit").GetValue<bool>()) return;

        }
    }
}
