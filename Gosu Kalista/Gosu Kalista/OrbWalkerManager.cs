using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;



namespace Gosu_Kalista
{
    internal class OrbWalkerManager
    {
        private static bool CheckRendDelay()
        {
            return !(Properties.Time.TickCount - Properties.Time.LastRendTick < 1000);
        }

        private static void UseRend()
        {
            Properties.Champion.E.Cast();
            Console.WriteLine("Last Rend Tick:{0} Current Tick{1}", Properties.Time.LastRendTick,
    Properties.Time.TickCount);
            Properties.Time.LastRendTick = Properties.Time.TickCount;

        }
    public static void EventCheck()
        {
            if (Properties.MainMenu.Item("bAutoSaveSoul").GetValue<bool>())
                SoulBound.CheckSoulBoundHero();

            //Sentinel
        if (Properties.MainMenu.Item("bSentinel").GetValue<KeyBind>().Active)
        {
            if (Properties.MainMenu.Item("bSentinelDragon").GetValue<bool>())
                AutoSentinels(true);
            if (Properties.MainMenu.Item("bSentinelBaron").GetValue<bool>())
                AutoSentinels(false);
        }

        if (!CheckRendDelay()) return;

            if (!Properties.Champion.E.IsReady()) return; 

            if (Properties.MainMenu.Item("bUseEToKillEpics").GetValue<bool>())
                if (CheckEpicMonsters()) return; 

            if (Properties.MainMenu.Item("bUseEToKillBuffs").GetValue<bool>())
                if(CheckBuffMonsters()) return; 

            if (Properties.MainMenu.Item("bUseEToAutoKill").GetValue<bool>())
                if (CheckEnemies()) return; 

            if (Properties.MainMenu.Item("bUseEToAutoKillMinions").GetValue<bool>())
                if (CheckMinions()) return; 

            if (Properties.MainMenu.Item("bAutoEOnStacksAndMinions").GetValue<bool>())
                if(AutoEOnStacksAndMinions()) return; 

        }
            
        public static void CheckNonKillables(AttackableUnit minion)
        {
            if(!Properties.MainMenu.Item("bUseENonKillables").GetValue<bool>() || !Properties.Champion.E.IsReady()) return;
            if (!(minion.Health <= DamageCalc.GetRendDamage((Obj_AI_Base) minion))) return;

            if (!CheckRendDelay()) // Wait for rend delay
                return;

            Console.WriteLine("Killing NonKillables");
            UseRend();
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

            UseRend();
            Console.WriteLine("Using Stacks And Minions E:{0}", Environment.TickCount);
            return true;
        }

        private static void AutoSentinels(bool dragon)
        {
            if (!Properties.Champion.W.IsReady()) return;

            if (dragon)
            {
                if (!(ObjectManager.Player.Distance(SummonersRift.River.Dragon) <= Properties.Champion.W.Range)) return;
                    Properties.Champion.W.Cast(SummonersRift.River.Dragon);
            }
            else
            {
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

            UseRend();
            Console.WriteLine("Using Baron and Dragon E:{0}", Properties.Time.TickCount);
            return true;
        }
                
        private static bool CheckBuffMonsters()
        {
            // ReSharper disable once UnusedVariable
            foreach (var monster in MinionManager.GetMinions(Properties.PlayerHero.ServerPosition,
                Properties.Champion.E.Range,
                MinionTypes.All,
                MinionTeam.Neutral,
                MinionOrderTypes.MaxHealth))
            {
                if (!monster.CharData.BaseSkinName.Equals("SRU_Red") &&
                    !monster.CharData.BaseSkinName.Equals("SRU_Blue")) continue;

                if (!(DamageCalc.GetRendDamage(monster) > monster.Health)) continue;

                Console.WriteLine("Using Buff E:{0}", Properties.Time.TickCount);
                UseRend();
                return true;
            }
            return false;
        }

        private static bool CheckEnemies()
        {
            // ReSharper disable once UnusedVariable
            //if (!(from target in HeroManager.Enemies
            //    where target.IsValid
            //    where !DamageCalc.CheckNoDamageBuffs(target)
            //    where Properties.Champion.E.IsInRange(target)
            //    where !(DamageCalc.GetRendDamage(target) < target.Health)
            //      where target.IsDead
            //    select target).Any()) return false;

            foreach (var target in HeroManager.Enemies)
            {
                if (!target.IsValid) continue;
                if (!target.IsValidTarget(Properties.Champion.E.Range)) continue;
                if (DamageCalc.CheckNoDamageBuffs(target)) continue;
                if(!Properties.Champion.E.IsInRange(target))continue;
                if (DamageCalc.GetRendDamage(target) < target.Health) continue;
                if (target.IsDead) continue;

                Console.WriteLine("Using Killing Enemy E:{0}", Properties.Time.TickCount);
                UseRend();
                return true;
            }
            return false;
        }

        private static bool CheckMinions()
        {
            var count = 0;
            
            var minions = MinionManager.GetMinions(Properties.PlayerHero.ServerPosition, Properties.Champion.E.Range);
            count += minions.Count(minion => minion.Health <= DamageCalc.GetRendDamage(minion) && minion.IsValid);
            if (Properties.MainMenu.Item("sAutoEMinionsKilled").GetValue<Slider>().Value > count) return false;

           
            Console.WriteLine("Using Minion E:{0}", Properties.Time.TickCount);
            UseRend();
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

            if (!CheckRendDelay()) // Wait for rend delay
                return;
            //Allow for auto E ?
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
                if (!CheckRendDelay()) // Wait for rend delay
                    continue;
                
                Console.WriteLine("Using Mixed E:{0}", Properties.Time.TickCount);
                UseRend();
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
