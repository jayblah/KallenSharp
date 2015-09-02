﻿// <copyright file="DrawingManager.cs" company="Kallen">
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
using System.Runtime.InteropServices;
using Color = System.Drawing.Color;

namespace S_Class_Kalista
{
    internal class DrawingManager
    {
        #region Public Functions

        public static void Drawing_OnDraw(EventArgs args)
        {
            if (Properties.PlayerHero.IsDead)
                return;
            //If User does not want drawing
            if (!Properties.MainMenu.Item("bDraw").GetValue<bool>())
                return;

            if (!Properties.PlayerHero.Position.IsOnScreen())
                return;

            if (Properties.MainMenu.Item("bDrawRendRange").GetValue<bool>() && Properties.Champion.E.Level > 0)
                Render.Circle.DrawCircle(Properties.PlayerHero.Position, Properties.Champion.E.Range,
                    Color.DarkSlateBlue, 2);
        }

        public static void Drawing_OnDrawMonster(EventArgs args)
        {
            if (!Properties.MainMenu.Item("bDrawOnMonsters").GetValue<bool>() ||
                Properties.Drawing.DamageToMonster == null)
                return;

            //if (Properties.MainMenu.Item("bDrawOnCreep").GetValue<bool>())
            //{
            //    //For every minion in range of E with E stacks on it
            //    foreach (
            //        var creep in
            //            MinionManager.GetMinions(Properties.PlayerHero.ServerPosition, Properties.Champion.E.Range)
            //                .Where(creep => (creep.GetBuff("kalistaexpungemarker").Count > 0)
            //                ))
            //    {
            //    }
            //}

            foreach (var minion in ObjectManager.Get<Obj_AI_Minion>())
            {
                if (minion.Team != GameObjectTeam.Neutral || !minion.IsValidTarget() || !minion.IsHPBarRendered)
                    continue;

                var rendDamage = DamageCalc.GetRendDamage(minion);

                // Monster bar widths and offsets from ElSmite
                var barWidth = 0;
                var xOffset = 0;
                var yOffset = 0;
                var yOffset2 = 0;
                var display = true;
                switch (minion.CharData.BaseSkinName)
                {
                    case "SRU_Red":
                    case "SRU_Blue":
                    case "SRU_Dragon":
                        barWidth = 145;
                        xOffset = 3;
                        yOffset = 18;
                        yOffset2 = 10;
                        break;

                    case "SRU_Baron":
                        barWidth = 194;
                        xOffset = -22;
                        yOffset = 13;
                        yOffset2 = 16;
                        break;

                    case "Sru_Crab":
                        barWidth = 61;
                        xOffset = 45;
                        yOffset = 34;
                        yOffset2 = 3;
                        break;

                    case "SRU_Krug":
                        barWidth = 81;
                        xOffset = 58;
                        yOffset = 18;
                        yOffset2 = 4;
                        break;

                    case "SRU_Gromp":
                        barWidth = 87;
                        xOffset = 62;
                        yOffset = 18;
                        yOffset2 = 4;
                        break;
                    case "SRU_Murkwolf":
                        barWidth = 75;
                        xOffset = 54;
                        yOffset = 19;
                        yOffset2 = 4;
                        break;
                    case "SRU_Razorbeak":
                        barWidth = 75;
                        xOffset = 54;
                        yOffset = 18;
                        yOffset2 = 4;
                        break;
                    default:
                        display = false;
                        break;
                }
                if(!display)continue;
                var barPos = minion.HPBarPosition;
                var percentHealthAfterDamage = Math.Max(0, minion.Health - rendDamage) / minion.MaxHealth;
                var yPos = barPos.Y + yOffset;
                var xPosDamage = barPos.X + xOffset + barWidth * percentHealthAfterDamage;
                var xPosCurrentHp = barPos.X + xOffset + barWidth * minion.Health / minion.MaxHealth;

                if (Properties.MainMenu.Item("bFillMonster").GetValue<bool>())
                {
                    var differenceInHp = xPosCurrentHp - xPosDamage;
                    var pos1 = barPos.X + xOffset;

                    for (var i = 0; i < differenceInHp; i++)
                    {
                        Drawing.DrawLine(pos1 + i, yPos, pos1 + i, yPos + yOffset2, 1, Color.DarkGray);
                    }
                }
                else Drawing.DrawLine(xPosDamage, yPos, xPosDamage, yPos + yOffset2, 1, Color.LightGray);

                if (!(rendDamage > minion.Health)) continue;

                Drawing.DrawText(minion.HPBarPosition.X + xOffset, minion.HPBarPosition.Y, Color.Red, "Killable");
            }
        }

        private static Color GetColor(bool b)
        {
            return b ? Color.White : Color.SlateGray;
        }

        public static void Drawing_OnDrawChamp(EventArgs args)
        {
            if (!Properties.MainMenu.Item("bDrawOnChamp").GetValue<bool>() || Properties.Drawing.DamageToUnit == null)
                return;

            var playerPos = Drawing.WorldToScreen(Properties.PlayerHero.Position);
            var jungleBool = Properties.MainMenu.Item("bUseJungleClear").GetValue<KeyBind>().Active ? "True" : "False";
            var jungleClear = string.Format("Jungle Clear:{0}", jungleBool);
            var vColor = GetColor(Properties.MainMenu.Item("bUseJungleClear").GetValue<KeyBind>().Active);
            Drawing.DrawText(playerPos.X - Drawing.GetTextExtent(jungleClear).Width + 50, playerPos.Y - Drawing.GetTextExtent(jungleClear).Height + 30, vColor, jungleClear);

            // For every enemis in E range
            foreach (var unit in HeroManager.Enemies.Where(unit => unit.IsValid && unit.IsHPBarRendered && Properties.Champion.E.IsInRange(unit)))
            {
                const int xOffset = 10;
                const int yOffset = 20;
                const int width = 103;
                const int height = 8;

                var barPos = unit.HPBarPosition;
                var damage = DamageCalc.GetRendDamage(unit);
                var percentHealthAfterDamage = Math.Max(0, unit.Health - damage) / unit.MaxHealth;
                var yPos = barPos.Y + yOffset;
                var xPosDamage = barPos.X + xOffset + width * percentHealthAfterDamage;
                var xPosCurrentHp = barPos.X + xOffset + width * unit.Health / unit.MaxHealth;

                if (Properties.MainMenu.Item("bDrawTextOnChamp").GetValue<bool>() && damage > unit.Health)
                    Drawing.DrawText(barPos.X + xOffset, barPos.Y + yOffset - 13, Color.Red, "Killable");

                Drawing.DrawLine(xPosDamage, yPos, xPosDamage, yPos + height, 1, Color.LightGray);

                if (!Properties.MainMenu.Item("bDrawFillOnChamp").GetValue<bool>()) return;

                var differenceInHp = xPosCurrentHp - xPosDamage;
                var pos1 = barPos.X + 9 + (107 * percentHealthAfterDamage);

                for (var i = 0; i < differenceInHp; i++)
                {
                    Drawing.DrawLine(pos1 + i, yPos, pos1 + i, yPos + height, 1, Color.DarkGray);
                }
            }
        }

        #endregion Public Functions
    }
}