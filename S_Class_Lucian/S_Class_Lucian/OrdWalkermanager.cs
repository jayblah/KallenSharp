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
//   Assembly to be use with LeagueSharp for champion Lucian
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using System.Collections.Generic;
using System.Linq;
using Collision = LeagueSharp.Common.Collision;

namespace S_Class_Lucian
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

            if (!Properties.Time.CheckLastDelay()) return;

            var target = TargetSelector.GetTarget(Properties.Champion.E.Range + Properties.Champion.Q.Range, TargetSelector.DamageType.Physical);
            if (Properties.Champion.PassiveReady())
                if (Properties.LukeOrbWalker.InAutoAttackRange(target)) return; // Wait for orbwalker to lose passive


            if (Properties.Champion.Q.IsReady())
            {
                var predictionPosition = Properties.Champion.Q.GetPrediction(target);
                if (predictionPosition.Hitchance >= GetHitChance())
                {
                    if (target.IsValidTarget(Properties.Champion.Q.Range))
                    {
                        Properties.Champion.Q.Cast(predictionPosition.CastPosition);
                        Properties.PlayerHero.IssueOrder(GameObjectOrder.AutoAttack, target);
                        Properties.Champion.UseTick();
                        return;
                    }
                }
            }


            if (Properties.Champion.W.IsReady())
            {
                var predictionPosition = Properties.Champion.W.GetPrediction(target);
                if (predictionPosition.Hitchance >= GetHitChance())
                {
                    if (target.IsValidTarget(Properties.Champion.W.Range))
                    {
                        Properties.Champion.W.Cast(predictionPosition.CastPosition);
                        Properties.PlayerHero.IssueOrder(GameObjectOrder.AutoAttack, target);
                        Properties.Champion.UseTick();
                        return;
                    }
                }
            }

            if (Properties.Champion.E.IsReady())
            {
                if (
                    target.IsValidTarget(Properties.Champion.E.Range + Properties.PlayerHero.AttackRange +
                                         Properties.PlayerHero.BoundingRadius))
                {
                    Properties.Champion.E.Cast(target);
                    Properties.PlayerHero.IssueOrder(GameObjectOrder.AutoAttack, target);
                    Properties.Champion.UseTick();
                    return;

                }
            }
        }

        private static void Mixed()
        {
           

        }

        private static void LaneClear()
        {

        }

        private static void LastHit()
        {
            // Fuck that
        }

        #endregion Private Functions
    }
}