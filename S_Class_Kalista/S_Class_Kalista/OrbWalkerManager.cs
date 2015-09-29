﻿// <copyright file="OrbWalkerManager.cs" company="Kallen">
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

using System;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using System.Collections.Generic;
using System.Linq;
using Collision = LeagueSharp.Common.Collision;

namespace S_Class_Kalista
{
    internal class OrbWalkerManager
    {
        #region Public Functions

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

        #endregion Public Functions

        #region Private Functions

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

        private static void OrbWalkMinionsV3()
        {

            var target = TargetSelector.GetTarget(Properties.Champion.E.Range * 1.2f, TargetSelector.DamageType.Physical);
            if (target != null)

            {
                var Minions = MinionManager.GetMinions(Properties.PlayerHero.Position, Orbwalking.GetRealAutoAttackRange(Properties.PlayerHero), MinionTypes.All, MinionTeam.NotAlly);

                var target2 = TargetSelector.GetTarget(700, TargetSelector.DamageType.Physical);
                foreach (var minion in Minions)
                {
                    if (target2 == null && (Vector3.Distance(ObjectManager.Player.ServerPosition, minion.Position) < Orbwalking.GetRealAutoAttackRange(Properties.PlayerHero) + 50))
                    {
                        Properties.PlayerHero.IssueOrder(GameObjectOrder.AttackUnit, minion);

                        break;
                    }
                }


                var rendTarget =
                    HeroManager.Enemies.Where(
                        x =>
                        x.IsValidTarget(Properties.Champion.E.Range) && Properties.Champion.E.GetDamage(x) > 1 && !DamageCalc.CheckNoDamageBuffs(x))
                        .OrderByDescending(x => Properties.Champion.E.GetDamage(x))
                        .FirstOrDefault();


                // E usage
                if ((Properties.Champion.E.Instance.State == SpellState.Ready || Properties.Champion.E.Instance.State == SpellState.Surpressed) && target.GetBuffCount("kalistaexpungemarker") > 0)
                {
                    // Target is not in range but has E stacks on
                    if (Properties.PlayerHero.Distance(target, true) > Math.Pow(Orbwalking.GetRealAutoAttackRange(target), 2))
                    {
                        // Get minions around
                        var minions = ObjectManager.Get<Obj_AI_Minion>().Where(m => m.IsValidTarget(Orbwalking.GetRealAutoAttackRange(m)));

                        // Check if a minion can die with the current E stacks
                        if (minions.Any(m => Properties.Champion.E.CanCast(m) && m.Health <= Properties.Champion.E.GetDamage(m)))
                        {
                            Properties.Champion.E.Cast();
                            Properties.Champion.E.LastCastAttemptT = Environment.TickCount;
                        }
                        else
                        {
                            // Check if a minion can die with one AA and E. Also, the AA minion has be be behind the player direction for a further leap
                            var minion = VectorHelper.GetDashObjects(minions).Find(m => m.Health > Properties.PlayerHero.GetAutoAttackDamage(m) && m.Health < Properties.PlayerHero.GetAutoAttackDamage(m) + DamageCalc.GetRendDamage(m));
                            if (minion != null)
                            {
                                Properties.LukeOrbWalker.ForceTarget(minion);
                            }
                        }
                    }
                    // Target is in range and has at least the set amount of E stacks on
                    else if (Properties.Champion.E.IsInRange(target) &&
                        target.Health < DamageCalc.GetRendDamage(target))
                    {
                        // Check if the target would die from E
                        if (rendTarget != null
                            && DamageCalc.GetRendDamage(rendTarget) >= (rendTarget.Health)
                            && !rendTarget.IsDead && Environment.TickCount - Properties.Champion.E.LastCastAttemptT > 500
                            && !DamageCalc.CheckNoDamageBuffs(rendTarget))
                        {
                            Properties.Champion.E.Cast();
                            Properties.Champion.E.LastCastAttemptT = Environment.TickCount;
                        }
                        else
                        {
                            // Check if target is about to leave our E range or the buff is about to run out
                            if (target.ServerPosition.Distance(Properties.PlayerHero.ServerPosition, true) > Math.Pow(Properties.Champion.E.Range * 0.8, 2))
                            {
                                Properties.Champion.E.Cast();
                                Properties.Champion.E.LastCastAttemptT = Environment.TickCount;
                            }
                        }
                    }
                }
            }
            else
            {
                var Minions = MinionManager.GetMinions(Properties.PlayerHero.Position, Orbwalking.GetRealAutoAttackRange(Properties.PlayerHero), MinionTypes.All, MinionTeam.NotAlly);

                foreach (var minion in Minions)
                {
                    if (
                        !(Vector3.Distance(ObjectManager.Player.ServerPosition, minion.Position) <
                          Orbwalking.GetRealAutoAttackRange(Properties.PlayerHero) + 50)) continue;
                    Properties.PlayerHero.IssueOrder(GameObjectOrder.AttackUnit, minion);

                    break;
                }

            }
        }

        private static bool OrbWalkMinionsV2()
        {
            //Return If there is a champ in auto range
            if (TargetSelector.GetTarget(Properties.PlayerHero.AttackRange, TargetSelector.DamageType.Physical) != null)
                return false;

            //No targets in range of E
            var nTarget = TargetSelector.GetTarget(Properties.Champion.E.Range, TargetSelector.DamageType.Physical);
            if (nTarget == null) return false;

            //Target within 200 range of Auto
            var targetToGetTo = TargetSelector.GetTarget(Properties.PlayerHero.AttackRange + 200, TargetSelector.DamageType.Physical);
            if (targetToGetTo == null) return false;

            //Every Minion in Auto Range
            foreach (var m in MinionManager.GetMinions(
               Properties.PlayerHero.Position, Orbwalking.GetRealAutoAttackRange(Properties.PlayerHero),
               MinionTypes.All, MinionTeam.NotAlly))
            {
                var damage = (float)Properties.PlayerHero.GetAutoAttackDamage(m);
                if(Properties.Champion.E.IsReady() && Properties.Time.CheckRendDelay())
                    damage += (float)(20 + (Properties.PlayerHero.GetAutoAttackDamage(m)) * .65);

                if(damage < m.Health)continue;
                
                if (Vector3.Distance(Properties.PlayerHero.Position, m.Position) > Orbwalking.GetRealAutoAttackRange(Properties.PlayerHero) + 50)
                    Properties.PlayerHero.IssueOrder(GameObjectOrder.AttackUnit, m);

                if(m.IsValidTarget(Properties.Champion.E.Range) && m.Health < DamageCalc.GetRendDamage(m))
                    Properties.Champion.UseRend();

                return true;
            }
            return false;
        }

        private static void OrbWalkMinions()
        {

            var target = TargetSelector.GetTarget(Properties.Champion.E.Range * 1.2f, TargetSelector.DamageType.Physical);

            if (target != null)
            {
                var Minions = MinionManager.GetMinions(Properties.PlayerHero.Position, Orbwalking.GetRealAutoAttackRange(Properties.PlayerHero), MinionTypes.All, MinionTeam.NotAlly);

                var target2 = TargetSelector.GetTarget(700, TargetSelector.DamageType.Physical);
                foreach (var minion in Minions.Where(minion => target2 == null).Where(minion => Vector3.Distance(ObjectManager.Player.ServerPosition, minion.Position) < Orbwalking.GetRealAutoAttackRange(Properties.PlayerHero) + 50))
                {
                    Properties.PlayerHero.IssueOrder(GameObjectOrder.AttackUnit, minion);
                    break;
                }


                var rendTarget =
                    HeroManager.Enemies.Where(
                        x =>
                        x.IsValidTarget(Properties.Champion.E.Range) && Properties.Champion.E.GetDamage(x) > 1 && !DamageCalc.CheckNoDamageBuffs(x))
                        .OrderByDescending(x => Properties.Champion.E.GetDamage(x))
                        .FirstOrDefault();

                if (target.GetBuffCount("kalistaexpungemarker") <= 0) return;
   
                if (Properties.PlayerHero.Distance(target, true) > Math.Pow(Orbwalking.GetRealAutoAttackRange(target), 2))
                {
                    // Get minions around
                    var minions = ObjectManager.Get<Obj_AI_Minion>().Where(m => m.IsValidTarget(Orbwalking.GetRealAutoAttackRange(m)));

                    // Check if a minion can die with the current E stacks
                    if (minions.Any(m => Properties.Champion.E.CanCast(m) && m.Health <= Properties.Champion.E.GetDamage(m)))
                    {
                        Properties.Champion.UseRend();
                    }
                    else
                    {
                        // Check if a minion can die with one AA and E. Also, the AA minion has be be behind the player direction for a further leap
                        var minion = VectorHelper.GetDashObjects(minions).Find(m => m.Health > Properties.PlayerHero.GetAutoAttackDamage(m) && m.Health < Properties.PlayerHero.GetAutoAttackDamage(m) + DamageCalc.GetRendDamage(m));
                        if (minion != null)
                        {
                            Properties.LukeOrbWalker.ForceTarget(minion);
                        }
                    }
                }
      
            }
            else
            {
                var Minions = MinionManager.GetMinions(Properties.PlayerHero.Position, Orbwalking.GetRealAutoAttackRange(Properties.PlayerHero), MinionTypes.All, MinionTeam.NotAlly);
                foreach (var minion in Minions.Where(minion => Vector3.Distance(ObjectManager.Player.ServerPosition, minion.Position) < Orbwalking.GetRealAutoAttackRange(Properties.PlayerHero) + 50))
                {
                    Properties.PlayerHero.IssueOrder(GameObjectOrder.AttackUnit, minion);
                    break;
                }
            }
        }

        private static void Combo()
        {
            if (Properties.MainMenu.Item("bUseQCombo").GetValue<bool>() && Properties.Champion.Q.IsReady())
            {
                if (Properties.MainMenu.Item("bUseManaManager").GetValue<bool>())
                {
                    if (Properties.PlayerHero.ManaPercent >
                        Properties.MainMenu.Item("sMinManaQ").GetValue<Slider>().Value)
                    {
                        var target = TargetSelector.GetTarget(Properties.Champion.Q.Range,
                            TargetSelector.DamageType.Physical);
                        var predictionPosition = Properties.Champion.Q.GetPrediction(target);
                        if (predictionPosition.Hitchance >= GetHitChance())
                        {
                            if (Properties.MainMenu.Item("bUseQComboReset").GetValue<bool>())
                            {
                                if (Properties.PlayerHero.IsWindingUp || Properties.PlayerHero.IsDashing())
                                    Properties.Champion.Q.Cast(predictionPosition.CastPosition);
                            }
                            else if (!Properties.PlayerHero.IsWindingUp && !Properties.PlayerHero.IsDashing())
                                Properties.Champion.Q.Cast(predictionPosition.CastPosition);
                        }
                    }
                }
            }

            if (!Properties.Champion.E.IsReady()) return;

            if (Properties.MainMenu.Item("bUseManaManager").GetValue<bool>())
                if (Properties.PlayerHero.ManaPercent < Properties.MainMenu.Item("sMinManaE").GetValue<Slider>().Value)
                    return;

            if (!Properties.Time.CheckRendDelay()) // Wait for rend delay
                return;

            if(Properties.MainMenu.Item("bUseECombo").GetValue<bool>())
            AutoEventManager.CheckEnemies();

            if (!Properties.MainMenu.Item("bUseMinionComboWalk").GetValue<bool>()) return;
            OrbWalkMinionsV3();



        }
    




//Cool Q in mid Auto
        //if (Properties.PlayerHero.IsWindingUp || Properties.PlayerHero.IsDashing())
        //Properties.Champion.Q.Cast(predictionPosition.CastPosition);
        
        private static void Mixed()
        {
            if (Properties.MainMenu.Item("bUseQMixed").GetValue<bool>() && Properties.Champion.Q.IsReady())
            {
                if (Properties.MainMenu.Item("bUseManaManager").GetValue<bool>())
                {
                    if (Properties.PlayerHero.ManaPercent >
                        Properties.MainMenu.Item("sMinManaQ").GetValue<Slider>().Value)
                    {
                        var target = TargetSelector.GetTarget(Properties.Champion.Q.Range,
                            TargetSelector.DamageType.Physical);
                        var predictionPosition = Properties.Champion.Q.GetPrediction(target);
                        if (predictionPosition.Hitchance >= GetHitChance())
                            if (predictionPosition.Hitchance >= GetHitChance())
                            {
                                if (Properties.MainMenu.Item("bUseQMixedReset").GetValue<bool>())
                                {
                                    if (Properties.PlayerHero.IsWindingUp || Properties.PlayerHero.IsDashing())
                                        Properties.Champion.Q.Cast(predictionPosition.CastPosition);
                                }
                                else if (!Properties.PlayerHero.IsWindingUp && !Properties.PlayerHero.IsDashing())
                                    Properties.Champion.Q.Cast(predictionPosition.CastPosition);
                            }
                    }
                }
            }
            if (!Properties.MainMenu.Item("bUseEMixed").GetValue<bool>()) return;

            if (Properties.MainMenu.Item("bUseManaManager").GetValue<bool>())
                if (Properties.PlayerHero.ManaPercent < Properties.MainMenu.Item("sMinManaE").GetValue<Slider>().Value) return;

            // ReSharper disable once UnusedVariable
            foreach (var stacks in from target in HeroManager.Enemies where target.IsValid where target.IsValidTarget(Properties.Champion.E.Range) where !DamageCalc.CheckNoDamageBuffs(target) select target.GetBuffCount("kalistaexpungemarker") into stacks where stacks >= Properties.MainMenu.Item("sMixedStacks").GetValue<Slider>().Value select stacks)
            {
                if (!Properties.Time.CheckRendDelay()) // Wait for rend delay
                    continue;
#if DEBUG_MODE
                Console.WriteLine("Using Mixed E:{0}", Properties.Time.TickCount);
#endif
                Properties.Champion.UseRend();
            }
        }

        private static IEnumerable<Obj_AI_Base> GetCollisionMinions(Obj_AI_Base source, Vector3 targetPosition, Spell champSpell)
        {
            var hitAbleObjects = new PredictionInput()
            {
                From = champSpell.From,
                Collision = champSpell.Collision,
                Delay = champSpell.Delay,
                Radius = champSpell.Width,
                Range = champSpell.Range,
                RangeCheckFrom = champSpell.RangeCheckFrom,
                Speed = champSpell.Speed,
                Type = champSpell.Type,
                CollisionObjects =
                                new[]
                                {
                                    CollisionableObjects.Heroes, CollisionableObjects.Minions,
                                    CollisionableObjects.YasuoWall
                                }
            };

            hitAbleObjects.CollisionObjects[0] = CollisionableObjects.Minions;

            return
                Collision.GetCollision(new List<Vector3> { targetPosition }, hitAbleObjects)
                    .OrderBy(obj => obj.Distance(source))
                    .ToList();
        }

        private static void LaneClear()
        {
            if (Properties.MainMenu.Item("bUseQLaneClear").GetValue<bool>())
            {
                if (!Properties.PlayerHero.IsWindingUp && !Properties.PlayerHero.IsDashing())
                {
                    var minions = MinionManager.GetMinions(Properties.PlayerHero.ServerPosition,
                        Properties.Champion.Q.Range);

                    if (Properties.MainMenu.Item("sLaneClearMinionsKilledQ").GetValue<Slider>().Value <= minions.Count)
                    {
                        foreach (var minion in minions.Where(x => x.Health <= Properties.Champion.Q.GetDamage(x)))
                        {
                            var killcount = 0;

                            foreach (
                                var colminion in
                                    GetCollisionMinions(Properties.PlayerHero,
                                        Properties.PlayerHero.ServerPosition.Extend(minion.ServerPosition, Properties.Champion.Q.Range), Properties.Champion.Q))
                            {
                                if (colminion.Health <= Properties.Champion.Q.GetDamage(colminion))
                                    killcount++;
                                else
                                    break;
                            }

                            if (killcount <
                                Properties.MainMenu.Item("sLaneClearMinionsKilledQ").GetValue<Slider>().Value)
                                continue;

                            Properties.Champion.Q.Cast(minion.ServerPosition);
                            break;
                        }
                    }
                }
            }

            if (Properties.MainMenu.Item("bUseELaneClear").GetValue<bool>())
            {
                var count = 0;
                var minions = MinionManager.GetMinions(Properties.PlayerHero.ServerPosition, Properties.Champion.E.Range);
                count += minions.Count(minion => minion.Health <= DamageCalc.GetRendDamage(minion) && minion.IsValid);
                if (Properties.MainMenu.Item("sLaneClearMinionsKilled").GetValue<Slider>().Value <= count)
                {
                    if (Properties.Time.CheckRendDelay())
                    {
                        Properties.Champion.UseRend();
#if DEBUG_MODE
                        Console.WriteLine("Using Lane Clear E:{0}", Properties.Time.TickCount);
#endif
                    }
                }
            }

            if (!Properties.MainMenu.Item("bUseJungleClear").GetValue<KeyBind>().Active) return;
            foreach (var monster in MinionManager.GetMinions(Properties.PlayerHero.ServerPosition,
                Properties.Champion.E.Range,
                MinionTypes.All,
                MinionTeam.Neutral,
                MinionOrderTypes.MaxHealth))
            {
                if (!(DamageCalc.GetRendDamage(monster) > monster.Health)) continue;
                if (!Properties.Time.CheckRendDelay()) return;
                Console.WriteLine("Using Jungle CLear E:{0}", Properties.Time.TickCount);
                Properties.Champion.UseRend();
                return;
            }
        }

        private static void LastHit()
        {
            // Fuck that
        }

        #endregion Private Functions
    }
}
