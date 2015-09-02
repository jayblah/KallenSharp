// <copyright file="AutoEventManager.cs" company="Kallen">
//   Copyright (C) 2015 LeagueSharp Kallen
//   
//             This program is free software: you can redistribute it and/or modify
//             it under the terms of the GNU General Public License as published by
//             the Free Software Foundation, either version 3 of the License, or
//             (at your option) any later version.
//   
//             This program is distributed in the hope that it will be useful,
//             but WITHOUT ANY WARRANTY; without even the implied warranty of
//             MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//             GNU General Public License for more details.
//   
//             You should have received a copy of the GNU General Public License
//             along with this program.  If not, see <http://www.gnu.org/licenses/>.
// </copyright>
// <summary>
//   Assembly to be use with LeagueSharp for champion Kalista
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using LeagueSharp;
using LeagueSharp.Common;
using System;
using System.Linq;

namespace S_Class_Kalista
{
    internal class AutoEventManager
    {
        #region Public Functions

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

            if (!Properties.Time.CheckRendDelay()) return;

            if (!Properties.Champion.E.IsReady()) return;

            if (Properties.MainMenu.Item("bUseEToKillEpics").GetValue<bool>())
                if (CheckEpicMonsters()) return;

            if (Properties.MainMenu.Item("bUseEToKillBuffs").GetValue<bool>())
                if (CheckBuffMonsters()) return;

            if (Properties.MainMenu.Item("bUseEToAutoKill").GetValue<bool>())
                if (CheckEnemies()) return;

            if (Properties.MainMenu.Item("bUseEToAutoKillMinions").GetValue<bool>())
                if (CheckMinions()) return;

            if (Properties.MainMenu.Item("bAutoEOnStacksAndMinions").GetValue<bool>())
                if (AutoEOnStacksAndMinions()) return;

            
           
            // ReSharper disable once RedundantJumpStatement
            if (Properties.MainMenu.Item("bUseEOnLeave").GetValue<bool>() && AutoEOnLeave()) return;
        }

        public static void CheckNonKillables(AttackableUnit minion)
        {
            if (!Properties.Time.CheckNonKillable()) return;
            if (!Properties.MainMenu.Item("bUseENonKillables").GetValue<bool>() || !Properties.Champion.E.IsReady()) return;
            if (!(minion.Health <= DamageCalc.GetRendDamage((Obj_AI_Base)minion)) || minion.Health > 60) return;

            Console.WriteLine("Killing NonKillables");
            Properties.Champion.UseNonKillableRend();
        }

        public static bool CheckEnemies()
        {
            if (!Properties.Time.CheckRendDelay()) return false;

            foreach (var target in HeroManager.Enemies)
            {
                if (!target.IsValid) continue;
                if (!target.IsValidTarget(Properties.Champion.E.Range)) continue;
                if (DamageCalc.CheckNoDamageBuffs(target)) continue;
                if (!Properties.Champion.E.IsInRange(target)) continue;
                if (DamageCalc.GetRendDamage(target) < target.Health) continue;
                if (target.IsDead) continue;
                Console.WriteLine("Using Killing Enemy E:{0}", Properties.Time.TickCount);
                Properties.Champion.UseRend();
                return true;
            }
            return false;
        }

        #endregion Public Functions

        #region Private Functions

        // ReSharper disable once UnusedMethodReturnValue.Local
        private static bool AutoEOnStacksAndMinions()
        {

            if (!Properties.Time.CheckRendDelay()) return false;

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
                  select count).Any())
                return false;

            Properties.Champion.UseRend();
            Console.WriteLine("Using Stacks And Minions E:{0}", Environment.TickCount);
            return true;
        }

        private static void AutoSentinels(bool dragon)
        {
            if (!Properties.Champion.W.IsReady()) return;

            if (dragon)
            {
                if (!(ObjectManager.Player.Distance(SummonersRift.River.Dragon) <= Properties.Champion.W.Range)) return; Properties.Champion.W.Cast(SummonersRift.River.Dragon);
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

            if (!Properties.Time.CheckRendDelay()) return false;

            if (!MinionManager.GetMinions(Properties.PlayerHero.ServerPosition,
                Properties.Champion.E.Range,
                MinionTypes.All,
                MinionTeam.Neutral,
                MinionOrderTypes.MaxHealth)
                .Where(mob => mob.Name.Contains("Baron") || mob.Name.Contains("Dragon")).Any(mob => DamageCalc.GetRendDamage(mob) > mob.Health))
                return false;

            Properties.Champion.UseRend();
            Console.WriteLine("Using Baron and Dragon E:{0}", Properties.Time.TickCount);
            return true;
        }
        private static bool AutoEOnLeave()
        {
            if (!Properties.Time.CheckRendDelay()) return false;

            foreach (var target in HeroManager.Enemies)
            {
                if (!target.IsValid) continue;
                if (!target.IsValidTarget(Properties.Champion.E.Range)) continue;
                if (DamageCalc.CheckNoDamageBuffs(target)) continue;
                if (!Properties.Champion.E.IsInRange(target)) continue;
                if (target.IsDead) continue;
                if (target.Distance(Properties.PlayerHero) < Properties.Champion.E.Range - 100)continue;
                if (!Properties.Time.CheckRendDelay()) continue;
                var stacks = target.GetBuffCount("kalistaexpungemarker");
                if(stacks <= Properties.MainMenu.Item("sStacksOnLeave").GetValue<Slider>().Value)continue;
                Console.WriteLine("Using Rend On Long  E:{0}", Properties.Time.TickCount);
                Properties.Champion.UseRend();
                return true;
            }
            return false;
        }
        private static bool CheckBuffMonsters()
        {
            // ReSharper disable once UnusedVariable
            if (!Properties.Time.CheckRendDelay()) return false;

            foreach (var monster in MinionManager.GetMinions(Properties.PlayerHero.ServerPosition,
                Properties.Champion.E.Range,
                MinionTypes.All,
                MinionTeam.Neutral,
                MinionOrderTypes.MaxHealth))
            {
                if (!monster.CharData.BaseSkinName.Equals("SRU_Red") &&
                    !monster.CharData.BaseSkinName.Equals("SRU_Blue"))
                    continue;

                if (!(DamageCalc.GetRendDamage(monster) > monster.Health)) continue;
                if (!Properties.Time.CheckRendDelay()) return false;
                Console.WriteLine("Using Buff E:{0}", Properties.Time.TickCount);
                Properties.Champion.UseRend();
                return true;
            }
            return false;
        }

        private static bool CheckMinions()
        {
            if (!Properties.Time.CheckRendDelay()) return false;
            var count = 0;

            var minions = MinionManager.GetMinions(Properties.PlayerHero.ServerPosition, Properties.Champion.E.Range);
            count += minions.Count(minion => minion.Health <= DamageCalc.GetRendDamage(minion) && minion.IsValid);
            if (Properties.MainMenu.Item("sAutoEMinionsKilled").GetValue<Slider>().Value > count) return false;
            Console.WriteLine("Using Minion E:{0}", Properties.Time.TickCount);
            Properties.Champion.UseRend();
            return true;
        }

        #endregion Private Functions
    }
}