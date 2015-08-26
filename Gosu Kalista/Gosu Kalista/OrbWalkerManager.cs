using System;
using System.Linq;
using LeagueSharp.Common;


namespace Gosu_Kalista
{
    internal class OrbWalkerManager
    {
        public static void EventCheck()
        {
            if (Properties.MainMenu.Item("doHuman").GetValue<bool>())
            {
                if (!Humanizer.CheckDelay("rendDelay")) // Wait for rend delay
                    return;
            }

            // Console.WriteLine("Checking Epic Monsters");
            if (Properties.MainMenu.Item("bUseEToKillEpics").GetValue<bool>() && Properties.Champion.E.IsReady())
                CheckEpicMonsters();
            if (Properties.MainMenu.Item("bUseEToKillBuffs").GetValue<bool>() && Properties.Champion.E.IsReady())
                CheckBuffMonsters();
            // Console.WriteLine("Checking Champions for kills");
            if (Properties.MainMenu.Item("bUseEToAutoKill").GetValue<bool>() && Properties.Champion.E.IsReady())
                CheckEnemies();
        }

        private static void CheckEpicMonsters()
        {
            // ReSharper disable once UnusedVariable
            foreach (var mob in MinionManager.GetMinions(Properties.PlayerHero.ServerPosition,
                Properties.Champion.E.Range,
                MinionTypes.All,
                MinionTeam.Neutral,
                MinionOrderTypes.MaxHealth).Where(mob => mob.Name.Contains("Baron") || mob.Name.Contains("Dragon")).Where(mob => DamageCalc.GetRendDamage(mob) > mob.Health))
            {
                Properties.Champion.E.Cast();
            }
        }

        private static void CheckBuffMonsters()
        {
            // ReSharper disable once UnusedVariable
            foreach (var mob in MinionManager.GetMinions(Properties.PlayerHero.ServerPosition,
                Properties.Champion.E.Range,
                MinionTypes.All,
                MinionTeam.Neutral,
                MinionOrderTypes.MaxHealth).Where(mob => mob.Name.Contains("Red") || mob.Name.Contains("Blue")).Where(mob => DamageCalc.GetRendDamage(mob) > mob.Health))
            {
                Properties.Champion.E.Cast();
            }
        }

        private static void CheckEnemies()
        {
            // ReSharper disable once UnusedVariable
            if (!(from target in HeroManager.Enemies
                where target.IsValid
                where !DamageCalc.CheckNoDamageBuffs(target)
                where Properties.Champion.E.IsInRange(target)
                where !(DamageCalc.GetRendDamage(target) < target.Health)
                select target).Any()) return;

            Console.WriteLine("Killing champion(s)");
            Properties.Champion.E.Cast();
        }

        public static void DoTheWalk()
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
            if (Properties.MainMenu.Item("bUseQCombo").GetValue<bool>() && Properties.Champion.Q.IsReady())
            {
                var target = TargetSelector.GetTarget(Properties.Champion.Q.Range, TargetSelector.DamageType.Physical);
                var predictionPosition = Properties.Champion.Q.GetPrediction(target);
                if (predictionPosition.Hitchance >= HitChance.VeryHigh)
                    if (Properties.PlayerHero.IsWindingUp || Properties.PlayerHero.IsDashing())
                        Properties.Champion.Q.Cast(predictionPosition.CastPosition);
            }
            if (!Properties.MainMenu.Item("bUseECombo").GetValue<bool>() || !Properties.Champion.E.IsReady()) return;

            CheckEnemies();
        }

        private static void Mixed()
        {
            if (Properties.MainMenu.Item("bUseQMixed").GetValue<bool>() && Properties.Champion.Q.IsReady())
            {
                var target = TargetSelector.GetTarget(Properties.Champion.Q.Range, TargetSelector.DamageType.Physical);
                var predictionPosition = Properties.Champion.Q.GetPrediction(target);
                if (predictionPosition.Hitchance >= HitChance.VeryHigh)
                    if (Properties.PlayerHero.IsWindingUp || Properties.PlayerHero.IsDashing())
                        Properties.Champion.Q.Cast(predictionPosition.CastPosition);
            }
            if (!Properties.MainMenu.Item("bUseEMixed").GetValue<bool>()) return;

            // ReSharper disable once UnusedVariable
            foreach (var stacks in from target in HeroManager.Enemies where target.IsValid where target.IsValidTarget(Properties.Champion.E.Range) where !DamageCalc.CheckNoDamageBuffs(target) select target.GetBuffCount("kalistaexpungemarker") into stacks where stacks >= Properties.MainMenu.Item("sMixedStacks").GetValue<Slider>().Value select stacks)
            {
                Properties.Champion.E.Cast();
            }

        }
    

        private static void LaneClear()
        {

        }

        private static void LastHit()
        {
           
        }
    }
}
