// <copyright file="OrbWalkerManager.cs" company="Kallen">
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

        private static void Combo()
        {
            if (Properties.MainMenu.Item("bUseQCombo").GetValue<bool>() && Properties.Champion.Q.IsReady())
            {
                var target = TargetSelector.GetTarget(Properties.Champion.Q.Range, TargetSelector.DamageType.Physical);
                var predictionPosition = Properties.Champion.Q.GetPrediction(target);
                if (predictionPosition.Hitchance >= GetHitChance())
                    if (!Properties.PlayerHero.IsWindingUp && !Properties.PlayerHero.IsDashing())
                        Properties.Champion.Q.Cast(predictionPosition.CastPosition);
            }
            if (!Properties.MainMenu.Item("bUseECombo").GetValue<bool>() || !Properties.Champion.E.IsReady()) return;

            if (!Properties.Time.CheckRendDelay()) // Wait for rend delay
                return;

            AutoEventManager.CheckEnemies();
        }

        //Cool Q in mid Auto
        //if (Properties.PlayerHero.IsWindingUp || Properties.PlayerHero.IsDashing())
        //Properties.Champion.Q.Cast(predictionPosition.CastPosition);

        private static void Mixed()
        {
            if (Properties.MainMenu.Item("bUseQMixed").GetValue<bool>() && Properties.Champion.Q.IsReady())
            {
                var target = TargetSelector.GetTarget(Properties.Champion.Q.Range, TargetSelector.DamageType.Physical);
                var predictionPosition = Properties.Champion.Q.GetPrediction(target);
                if (predictionPosition.Hitchance >= GetHitChance())
                    if (!Properties.PlayerHero.IsWindingUp && !Properties.PlayerHero.IsDashing())
                        Properties.Champion.Q.Cast(predictionPosition.CastPosition);
            }
            if (!Properties.MainMenu.Item("bUseEMixed").GetValue<bool>()) return;

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
                //var minions = MinionManager.GetMinions(Properties.PlayerHero.ServerPosition,
                //    Properties.Champion.Q.Range);

                //var count = minions.Count(minion => minion.Health <= Properties.Champion.Q.GetDamage(minion) && minion.IsValid);

                //if (Properties.MainMenu.Item("sLaneClearMinionsKilledQ").GetValue<Slider>().Value <= count)
                //{
                //    foreach (var minion in minions)
                //    {
                //        if (!minion.IsValid) continue;
                //        if (minion.Health > Properties.Champion.Q.GetDamage(minion)) continue;
                //        var killcount = GetCollisionMinions(ObjectManager.Player, ObjectManager.Player.ServerPosition.Extend(minion.ServerPosition, Properties.Champion.Q.Range), Properties.Champion.Q).Count(collisionMinion => collisionMinion.Health < Properties.Champion.Q.GetDamage(collisionMinion));
                //        if (killcount < Properties.MainMenu.Item("sLaneClearMinionsKilledQ").GetValue<Slider>().Value) continue;

                //        Properties.Champion.Q.Cast(minion.ServerPosition);
                //        break;
                //    }
                //}
            }

            if (Properties.MainMenu.Item("bUseELaneClear").GetValue<bool>())
            {
                var count = 0;
                var minions = MinionManager.GetMinions(Properties.PlayerHero.ServerPosition, Properties.Champion.E.Range);
                count += minions.Count(minion => minion.Health <= DamageCalc.GetRendDamage(minion) && minion.IsValid);
                if (Properties.MainMenu.Item("sLaneClearMinionsKilled").GetValue<Slider>().Value > count) return;
                if (!Properties.Time.CheckRendDelay()) return;
#if DEBUG_MODE
                        Console.WriteLine("Using Lane Clear E:{0}", Properties.Time.TickCount);
#endif
                Properties.Champion.UseRend();
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
#if DEBUG_MODE
                Console.WriteLine("Using Jungle CLear E:{0}", Properties.Time.TickCount);
#endif
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