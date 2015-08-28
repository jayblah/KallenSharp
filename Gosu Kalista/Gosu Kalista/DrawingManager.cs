﻿using System;
using System.Linq;
using System.Runtime.InteropServices;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

namespace Gosu_Kalista
{
    internal class DrawingManager
    {
        #region Variable Declaration

        static Render.Text renderText = new Render.Text(0, 0, "", 13, SharpDX.Color.Red, "monospace");
        #endregion

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

            if (!Properties.MainMenu.Item("bDrawOnEpic").GetValue<bool>()) return;

            foreach (var minion in ObjectManager.Get<Obj_AI_Minion>())
            {
                if (minion.Team != GameObjectTeam.Neutral || !minion.IsValidTarget() || !minion.IsHPBarRendered)
                    continue;

                var hpBarPosition = minion.HPBarPosition;
                var maxHealth = minion.MaxHealth;
                var rendDamage = DamageCalc.GetRendDamage(minion);
                var percentHealth = rendDamage/maxHealth;

                var barWidth = 75;
                var xOffset = 54;
                var yOffset = 19;
                var yOffset2 = 4;

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
                }
                Drawing.DrawLine(new Vector2(hpBarPosition.X + xOffset + (barWidth*percentHealth), hpBarPosition.Y + yOffset), new Vector2(hpBarPosition.X + xOffset + (barWidth*percentHealth), hpBarPosition.Y + yOffset + yOffset2), 4, Color.LightGray);
                if (!(rendDamage > minion.Health)) continue;

                Drawing.DrawText(hpBarPosition.X + xOffset, hpBarPosition.Y, Color.Red, "Killable");
            }
        }


        public static void Drawing_OnDrawChamp(EventArgs args)
        {
            if (!Properties.MainMenu.Item("bDrawOnChamp").GetValue<bool>() || Properties.Drawing.DamageToUnit == null)
                return;
            // For every enemis in E range
            foreach (var unit in HeroManager.Enemies.Where(unit => unit.IsValid && unit.IsHPBarRendered && Properties.Champion.E.IsInRange(unit)))
            {
                const int xOffset = 10;
                const int yOffset = 20;
                const int width = 103;
                const int height = 8;

                var barPos = unit.HPBarPosition;
                var damage = DamageCalc.GetRendDamage(unit);
                var percentHealthAfterDamage = Math.Max(0, unit.Health - damage)/unit.MaxHealth;
                var yPos = barPos.Y + yOffset;
                var xPosDamage = barPos.X + xOffset + width*percentHealthAfterDamage;
                var xPosCurrentHp = barPos.X + xOffset + width*unit.Health/unit.MaxHealth;

                if (Properties.MainMenu.Item("bDrawTextOnChamp").GetValue<bool>() && damage > unit.Health)
                {
                    renderText.X = (int) barPos.X + xOffset;
                    renderText.Y = (int) barPos.Y + yOffset - 13;
                    renderText.text = "Rend Will Kill";
                    renderText.OnEndScene();
                }


                Drawing.DrawLine(xPosDamage, yPos, xPosDamage, yPos + height, 1, Color.LightGray);

                if (!Properties.MainMenu.Item("bDrawFillOnChamp").GetValue<bool>()) return;

                var differenceInHp = xPosCurrentHp - xPosDamage;
                var pos1 = barPos.X + 9 + (107*percentHealthAfterDamage);

                for (var i = 0; i < differenceInHp; i++)
                {
                    Drawing.DrawLine(pos1 + i, yPos, pos1 + i, yPos + height, 1, Color.DarkGray);
                }
            }
        }
    }
}
