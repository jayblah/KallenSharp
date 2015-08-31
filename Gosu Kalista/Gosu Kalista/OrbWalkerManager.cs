using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using LeagueSharp;
using LeagueSharp.Common;



namespace Gosu_Kalista
{
    internal class OrbWalkerManager
    {

        public static void EventCheck()
        {
            if (!Humanizer.CheckDelay("rendDelay")) // Wait for rend delay
                    return;

            if (!Properties.Champion.E.IsReady()) return;

            if (Properties.MainMenu.Item("bUseEToKillEpics").GetValue<bool>())
                if (CheckEpicMonsters()) return;

            if (Properties.MainMenu.Item("bUseEToKillBuffs").GetValue<bool>())
                if(CheckBuffMonsters())return;

            if (Properties.MainMenu.Item("bUseEToAutoKill").GetValue<bool>())
                if (CheckEnemies()) return;

            if (Properties.MainMenu.Item("bUseEToAutoKillMinions").GetValue<bool>())
                if (CheckMinions()) return;
            if (Properties.MainMenu.Item("bAutoEOnStacksAndMinions").GetValue<bool>())
                if(AutoEOnStacksAndMinions())return;

            //Sentinel
            if (!Properties.MainMenu.Item("bSentinel").GetValue<KeyBind>().Active) return;
            if (Properties.MainMenu.Item("bSentinelDragon").GetValue<bool>())
                AutoSentinels(true);
            if (Properties.MainMenu.Item("bSentinelBaron").GetValue<bool>())
                AutoSentinels(false);
        }
            
        public static void CheckNonKillables(AttackableUnit minion)
        {
            if(!Properties.MainMenu.Item("bUseENonKillables").GetValue<bool>() || !Properties.Champion.E.IsReady()) return;
            if (!(minion.Health <= DamageCalc.GetRendDamage((Obj_AI_Base) minion))) return;
            if (!Humanizer.CheckDelay("rendDelay")) return;
            Properties.Champion.E.Cast();
        }

        private static bool AutoEOnStacksAndMinions()
        {
            if (!(from stacks in (from target in HeroManager.Enemies
                where target.IsValid
                where target.IsValidTarget(Properties.Champion.E.Range)
                where !DamageCalc.CheckNoDamageBuffs(target)
                select target.GetBuffCount("kalistaexpungemarker")
                into stacks
                where stacks >= Properties.MainMenu.Item("sUseEOnChampStacks").GetValue<Slider>().Value
                select stacks)
                select 0
                into count
                let minions =
                    MinionManager.GetMinions(Properties.PlayerHero.ServerPosition, Properties.Champion.E.Range)
                select
                    count + minions.Count(minion => minion.Health <= DamageCalc.GetRendDamage(minion) && minion.IsValid)
                into count
                where Properties.MainMenu.Item("sUseEOnMinionKilled").GetValue<Slider>().Value <= count
                select count).Any()) return false;

            Properties.Champion.E.Cast();
            return true;
        }

        private static void AutoSentinels(bool dragon)
        {
            if (!Properties.Champion.W.IsReady()) return;

            if (dragon)
            {
                //if (!ObjectManager.Get<Obj_AI_Minion>()
                //    .Where(minion => minion.CharData.BaseSkinName.Contains("Dragon"))
                //    .Any(minion => minion.Team == GameObjectTeam.Neutral && !minion.IsDead)) return;
                if (!(ObjectManager.Player.Distance(SummonersRift.River.Dragon) <= Properties.Champion.W.Range)) return;

                Properties.Champion.W.Cast(SummonersRift.River.Dragon);
            }
            else
            {
                //if (!ObjectManager.Get<Obj_AI_Minion>()
                //    .Where(minion => minion.CharData.BaseSkinName.Contains("Baron"))
                //    .Any(minion => minion.Team == GameObjectTeam.Neutral && !minion.IsDead)) return;
                if (!(ObjectManager.Player.Distance(SummonersRift.River.Baron) <= Properties.Champion.W.Range)) return;

                Properties.Champion.W.Cast(SummonersRift.River.Baron);
            }

        }

        private static bool CheckEpicMonsters()
        {
            // ReSharper disable once UnusedVariable
            if (!MinionManager.GetMinions(Properties.PlayerHero.ServerPosition,
                Properties.Champion.E.Range,
                MinionTypes.All,
                MinionTeam.Neutral,
                MinionOrderTypes.MaxHealth)
                .Where(mob => mob.Name.Contains("Baron") || mob.Name.Contains("Dragon")).Any(mob => DamageCalc.GetRendDamage(mob) > mob.Health)) return false;

            Properties.Champion.E.Cast();
            return true;
        }
                
        private static bool CheckBuffMonsters()
        {
            // ReSharper disable once UnusedVariable
            if (!MinionManager.GetMinions(Properties.PlayerHero.ServerPosition,
                Properties.Champion.E.Range,
                MinionTypes.All,
                MinionTeam.Neutral,
                MinionOrderTypes.MaxHealth)
                .Where(mob => mob.CharData.BaseSkinName.Contains("SRU_Red") || mob.CharData.BaseSkinName.Contains("SRU_Blue")).Any(mob => DamageCalc.GetRendDamage(mob) > mob.Health)) return false;
            Properties.Champion.E.Cast();
            return true;
        }

        private static bool CheckEnemies()
        {
            // ReSharper disable once UnusedVariable
            if (!(from target in HeroManager.Enemies
                where target.IsValid
                where !DamageCalc.CheckNoDamageBuffs(target)
                where Properties.Champion.E.IsInRange(target)
                where !(DamageCalc.GetRendDamage(target) < target.Health)
                select target).Any()) return false;

           
            Properties.Champion.E.Cast();
            return true;
        }

        private static bool CheckMinions()
        {
            var count = 0;
            
            var minions = MinionManager.GetMinions(Properties.PlayerHero.ServerPosition, Properties.Champion.E.Range);
            count += minions.Count(minion => minion.Health <= DamageCalc.GetRendDamage(minion) && minion.IsValid);
            if (Properties.MainMenu.Item("sAutoEMinionsKilled").GetValue<Slider>().Value > count) return false;
            Properties.Champion.E.Cast();
            return true;
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

        private static HitChance GetHitChance()
        {
            switch (Properties.MainMenu.Item("slQprediction").GetValue<StringList>().SelectedIndex)
            {
                case 0:
                    return HitChance.VeryHigh;
                case 1:
                    return HitChance.High;
                case 2:
                    return HitChance.Dashing;
            }
            return HitChance.VeryHigh;
        }
        private static void Combo()
        {
            if (Properties.MainMenu.Item("bUseQCombo").GetValue<bool>() && Properties.Champion.Q.IsReady())
            {
                var target = TargetSelector.GetTarget(Properties.Champion.Q.Range, TargetSelector.DamageType.Physical);
                var predictionPosition = Properties.Champion.Q.GetPrediction(target);
                if (predictionPosition.Hitchance >= GetHitChance())
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
                if (predictionPosition.Hitchance >= GetHitChance())
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
