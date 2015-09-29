// <copyright file="SoulBound.cs" company="Kallen">
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
using System.Linq;

namespace S_Class_Kalista
{
    internal class SoulBound
    {
        public static void CheckSoulBoundHero()
        {
            if (!Properties.Champion.R.IsReady()) return;

            if (Properties.SoulBoundHero == null)
                Properties.SoulBoundHero =
                    HeroManager.Allies.Find(
                        ally => ally.Buffs.Any(user => user.Caster.IsMe && user.Name.Contains("kalistacoopstrikeally")));

            if (!Properties.Champion.R.IsInRange(Properties.SoulBoundHero) || Properties.SoulBoundHero.IsDead) return;

            var buffName = "";

            switch (Properties.SoulBoundHero.ChampionName)
            {
                case "Blitzcrank":
                    buffName = "rocketgrab2";
                    break;
                case "Skarner":
                    buffName = "skarnerimpale";
                    break;
                case "TahmKench":
                    buffName = "tahmkenchwdevoured";
                    break;
            }


            if (Properties.MainMenu.Item("bBST").GetValue<bool>() && buffName.Length > 0)
            {

                foreach (var target in ObjectManager.Get<Obj_AI_Hero>())
                {

                    if (!target.IsValid) continue;
                    if (!target.IsEnemy) continue;
                    if (target.Distance(ObjectManager.Player) < 2450f) continue;
                    if (target.Buffs == null) continue;
                    if (Properties.SoulBoundHero.Distance(target) < 750) continue;
                    for (var i = 0; i < target.Buffs.Count(); i++)
                    {
                        if (target.Buffs[i].Name.ToLower() != buffName || !target.Buffs[i].IsActive) continue;
                        Properties.Champion.R.Cast();
                    }
                }
            }

            else if (Properties.SoulBoundHero.HealthPercent <
                     Properties.MainMenu.Item("sSoulBoundPercent").GetValue<Slider>().Value &&
                     Properties.SoulBoundHero.CountEnemiesInRange(500) > 0)
                Properties.Champion.R.Cast();
        }
    }
}