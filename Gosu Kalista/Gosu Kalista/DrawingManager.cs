using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

namespace Gosu_Kalista
{
    internal class DrawingManager
    {
        #region Variable Declaration

        private const int XOffset = 10;
        private const int YOffset = 20;
        private const int Width = 103;
        private const int Height = 8;
        private static readonly Render.Text RenderText = new Render.Text(0, 0, "", 13, SharpDX.Color.Red, "monospace");

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

            //if (Properties.MainMenu.Item("bDrawAutoAttackRange").GetValue<bool>())
            //    Render.Circle.DrawCircle(Properties.PlayerHero.Position, Properties.PlayerHero.AttackRange,
            //        Color.DarkRed, 2);
        }

        //drawMenu.SubMenu("Drawings").AddItem(new MenuItem("bDrawOnMonsters", "Draw Damage On Monsters").SetValue(true));
        //    drawMenu.SubMenu("Drawings").AddItem(new MenuItem("bDrawOnCreep", "Display Damage On Creeps").SetValue(true));
        //    drawMenu.SubMenu("Drawings").AddItem(new MenuItem("bDrawOnEpics", "Draw Damage On Epics").SetValue(true));
        //    drawMenu.SubMenu("Drawings").AddItem(new MenuItem("bDrawTextOnChamp", "Display Floating Text (on enemies)").SetValue(true));

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

            foreach (var minion in ObjectManager.Get<Obj_AI_Minion>().Where( m => m.Team == GameObjectTeam.Neutral && m.IsValidTarget() && m.IsHPBarRendered))
            {
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

                Drawing.DrawLine(new Vector2(hpBarPosition.X + xOffset + (barWidth * percentHealth), hpBarPosition.Y + yOffset), new Vector2(hpBarPosition.X + xOffset + (barWidth * percentHealth), hpBarPosition.Y + yOffset+yOffset2), 1, Color.LightGray);
                if(rendDamage > minion.Health)
                    Drawing.DrawText(hpBarPosition.X + xOffset, hpBarPosition.Y, Color.Red, "Killable");
            }
        }


        public static void Drawing_OnDrawChamp(EventArgs args)
        {
           
            if (!Properties.MainMenu.Item("bDrawOnChamp").GetValue<bool>() || Properties.Drawing.DamageToUnit == null)
                return;
            // For every enemis in E range
            foreach (var unit in HeroManager.Enemies.Where(h => h.IsValid && h.IsHPBarRendered && Properties.Champion.E.IsInRange(h)))
            {
                var barPos = unit.HPBarPosition;
                var damage = DamageCalc.GetRendDamage(unit);
                var percentHealthAfterDamage = Math.Max(0, unit.Health - damage) / unit.MaxHealth;
                var yPos = barPos.Y + YOffset;
                var xPosDamage = barPos.X + XOffset + Width * percentHealthAfterDamage;
                var xPosCurrentHp = barPos.X + XOffset + Width * unit.Health / unit.MaxHealth;

                if (Properties.MainMenu.Item("bDrawTextOnChamp").GetValue<bool>() && damage > unit.Health)
                {
                    RenderText.X = (int)barPos.X + XOffset;
                    RenderText.Y = (int)barPos.Y + YOffset - 13;
                    RenderText.text = "Rend Will Kill";
                    RenderText.OnEndScene();
                }

                
                LeagueSharp.Drawing.DrawLine(xPosDamage, yPos, xPosDamage, yPos + Height, 1, Color.LightGray);

                if (!Properties.MainMenu.Item("bDrawFillOnChamp").GetValue<bool>()) return;

                var differenceInHp = xPosCurrentHp - xPosDamage;
                var pos1 = barPos.X + 9 + (107 * percentHealthAfterDamage);

                for (var i = 0; i < differenceInHp; i++)
                {
                    LeagueSharp.Drawing.DrawLine(pos1 + i, yPos, pos1 + i, yPos + Height, 1, Color.DarkGray);
                }

            }

        }
    }
}
