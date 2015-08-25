using System;
using System.Linq;
using LeagueSharp.Common;
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

            if (Properties.MainMenu.Item("bDrawAutoAttackRange").GetValue<bool>())
                Render.Circle.DrawCircle(Properties.PlayerHero.Position, Properties.PlayerHero.AttackRange,
                    Color.DarkRed, 2);
        }

        public static void Drawing_OnDrawChamp(EventArgs args)
        {
            if (!Properties.Drawing.EnableDrawingDamage || Properties.Drawing.DamageToUnit == null)
                return;

            foreach (var unit in HeroManager.Enemies.Where(h => h.IsValid && h.IsHPBarRendered))
            {
                var barPos = unit.HPBarPosition;
                var damage = DamageCalc.GetRendDamage(unit);
                var percentHealthAfterDamage = Math.Max(0, unit.Health - damage) / unit.MaxHealth;
                var yPos = barPos.Y + YOffset;
                var xPosDamage = barPos.X + XOffset + Width * percentHealthAfterDamage;
                var xPosCurrentHp = barPos.X + XOffset + Width * unit.Health / unit.MaxHealth;

                if (Properties.MainMenu.Item("bDrawText").GetValue<bool>() && damage > unit.Health)
                {
                    Console.WriteLine("Draw killable text");
                    RenderText.X = (int)barPos.X + XOffset;
                    RenderText.Y = (int)barPos.Y + YOffset - 13;
                    RenderText.text = "Rend Will Kill";
                    RenderText.OnEndScene();
                }

                
                LeagueSharp.Drawing.DrawLine(xPosDamage, yPos, xPosDamage, yPos + Height, 1, Color.LightGray);

                if (!Properties.EnableFillDamage) return;
                Console.WriteLine("Fill damage");
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
