// <copyright file="SMenu.cs" company="Kallen">
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
using LeagueSharp.Common;
using Color = System.Drawing.Color;

namespace S_Class_Kalista
{
    internal class SMenu
    {
        private const string MenuName = "S Class Kalista";

        public static void GenerateMenu()
        {
            Properties.MainMenu = new Menu(MenuName, MenuName, true);
            Properties.MainMenu.AddSubMenu(new Menu("Created by Kallen aka 0x0539", "ddsfhjsjhdfjhdsfjhdfsjhdf"));
            //Properties.MainMenu.AddSubMenu(HumanizerMenu());
            Properties.MainMenu.AddSubMenu(DrawingMenu());
            Properties.MainMenu.AddSubMenu(AutoEvents());
            Properties.MainMenu.AddSubMenu(OrbWalkingMenu());
            Properties.MainMenu.AddSubMenu(MixedMenu());
            Properties.MainMenu.AddSubMenu(ComboMenu());
            Properties.MainMenu.AddSubMenu(LaneClearMenu());
            Properties.MainMenu.AddSubMenu(MiscMenu());
            Properties.LukeOrbWalker = new Orbwalking.Orbwalker(Properties.MainMenu.SubMenu("Orbwalking"));
        }

        private static Menu OrbWalkingMenu()
        {
            var orbWalkingMenu = new Menu("Orbwalking", "lukeWalker");
            var targetSelectorMenu = new Menu("Target Selector", "tSelect");
            TargetSelector.AddToMenu(targetSelectorMenu);
            orbWalkingMenu.AddSubMenu(targetSelectorMenu);
            return orbWalkingMenu;
        }

        private static Menu MixedMenu()
        {
            var mixedMenu = new Menu("Mixed Options", "mixedOptions");
            mixedMenu.AddItem(new MenuItem("bUseQMixed", "Auto Q").SetValue(true));
            mixedMenu.AddItem(new MenuItem("bUseEMixed", "Auto E on Stacks").SetValue(false));
            mixedMenu.AddItem(new MenuItem("sMixedStacks", "Required E Stacks").SetValue(new Slider(4, 2, 15)));

            return mixedMenu;
        }

        private static Menu ComboMenu()
        {
            var mixedMenu = new Menu("Combo Options", "comboMenu");
            mixedMenu.AddItem(new MenuItem("bUseQCombo", "Auto Q").SetValue(false));
            mixedMenu.AddItem(new MenuItem("bUseECombo", "Auto E to Kill").SetValue(false));
            return mixedMenu;
        }

        private static Menu LaneClearMenu()
        {
            var laneClearMenu = new Menu("Lane Clear Options", "laneClearOptions");
            laneClearMenu.AddItem(new MenuItem("bUseELaneClear", "Auto E on Minions Killed").SetValue(true));
            laneClearMenu.AddItem(new MenuItem("sLaneClearMinionsKilled", "Min. Minions to Use E").SetValue(new Slider(3, 2, 10)));

            laneClearMenu.AddItem(new MenuItem("bUseQLaneClear", "Kill Minions with Q").SetValue(false));
            laneClearMenu.AddItem(new MenuItem("sLaneClearMinionsKilledQ", "Min. Minions to Use Q").SetValue(new Slider(3, 2, 10)));

            laneClearMenu.AddItem(new MenuItem("bUseJungleClear", "Jungle Clear").SetValue(new KeyBind('G', KeyBindType.Toggle)));
            return laneClearMenu;
        }

        private static Menu DrawingMenu()
        {
            var drawMenu = new Menu("Drawing Settings", "Drawings");

            drawMenu.AddItem(new MenuItem("bDraw", "Display Drawing").SetValue(true));
            drawMenu.AddItem(new MenuItem("cDrawRendRange", "Draw E Range").SetValue(new Circle(true, Color.LightSkyBlue)));

            drawMenu.AddItem(new MenuItem("bDrawOnChamp", "Draw on Enemies").SetValue(true));
            drawMenu.AddItem(new MenuItem("cDrawTextOnChamp", "Display Killable Text on Enemies").SetValue(new Circle(true, Color.Red)));

            drawMenu.AddItem(new MenuItem("cDrawFillOnChamp", "Draw Combo Damage on Enemies").SetValue(new Circle(true, Color.DarkGray)));

            drawMenu.AddItem(new MenuItem("bDrawTextOnSelf", "Display Floating Text (Self)").SetValue(true));

            drawMenu.AddItem(new MenuItem("cDrawOnMonsters", "Draw Damage on Monsters").SetValue(new Circle(true, Color.LightSlateGray)));
            drawMenu.AddItem(new MenuItem("cFillMonster", "Damage Fill on Monsters").SetValue(new Circle(true, Color.DarkGray)));
            drawMenu.AddItem(new MenuItem("cKillableText", "Display Killable Text on Monsters").SetValue(new Circle(true, Color.Red)));

            return drawMenu;
        }

        private static Menu AutoEvents()
        {
            var autoEventsMenu = new Menu("Auto Events", "autoEvents");
            autoEventsMenu.AddItem(new MenuItem("bAutoLevel", "Auto-level Skills").SetValue(true));
            autoEventsMenu.AddItem(new MenuItem("bAutoBuyOrb", "Auto-buy Orb at 6").SetValue(true));
            autoEventsMenu.AddItem(new MenuItem("bUseEToKillEpics", "Auto E Epic Camps").SetValue(true));
            autoEventsMenu.AddItem(new MenuItem("bUseEToKillBuffs", "Auto E Buff Camps").SetValue(true));
            autoEventsMenu.AddItem(new MenuItem("bUseEToAutoKill", "Auto E to Kill Enemies").SetValue(true));
            autoEventsMenu.AddItem(new MenuItem("bUseENonKillables", "Auto E Non-killable Minions").SetValue(true));
            autoEventsMenu.AddItem(new MenuItem("bEBeforeDeath", "Auto E before Death").SetValue(false));
            autoEventsMenu.AddItem(new MenuItem("sEBeforeDeathChamps", "-- Enemies with Stacks").SetValue(new Slider(1, 1, 5)));
            autoEventsMenu.AddItem(new MenuItem("sEBeforeDeathMinStacks", "-- Min Stacks on Enemies to E").SetValue(new Slider(3, 1, 10)));
            autoEventsMenu.AddItem(new MenuItem("sEBeforeDeathMaxHP", "Max HP% To Activate E Before Death").SetValue(new Slider(10, 1, 30)));
            autoEventsMenu.AddItem(new MenuItem("bUseEToAutoKillMinions", "Auto E to Kill Minions").SetValue(true));
            autoEventsMenu.AddItem(new MenuItem("sAutoEMinionsKilled", "-- Min. Minions Killed by E").SetValue(new Slider(2, 2, 10)));
            autoEventsMenu.AddItem(new MenuItem("bAutoEOnStacksAndMinions", "Auto E When Stacks on Enemies and Minions Can Kill").SetValue(true));
            autoEventsMenu.AddItem(new MenuItem("sUseEOnMinionKilled", "Min. Minions Killed by E & Enemy Stacks").SetValue(new Slider(3, 1, 10)));
            autoEventsMenu.AddItem(new MenuItem("sUseEOnChampStacks", "-- Required Stacks on Enemy").SetValue(new Slider(1, 1, 10)));
            autoEventsMenu.AddItem(new MenuItem("bUseEOnLeave", "Auto E if Leaving Rend Range").SetValue(false));
            autoEventsMenu.AddItem(new MenuItem("sStacksOnLeave", "-- Min. Stacks to Rend").SetValue(new Slider(4, 1, 10)));
            autoEventsMenu.AddItem(new MenuItem("bAutoSaveSoul", "Auto-save Soulbound Partner").SetValue(true));
            autoEventsMenu.AddItem(new MenuItem("bBalista", "Use Balista-Combo (Blitzcrank + R)").SetValue(true));
            autoEventsMenu.AddItem(new MenuItem("sSoulBoundPercent", "-- Min. Ally HP % to Save with R").SetValue(new Slider(10, 1, 90)));
            return autoEventsMenu;
        }

        private static Menu MiscMenu()
        {
            var autoEventsMenu = new Menu("Miscellaneous", "miscMenu");
            autoEventsMenu.AddItem(new MenuItem("bSentinel", "Sentinel While in Range").SetValue(new KeyBind('T', KeyBindType.Press)));
            autoEventsMenu.AddItem(new MenuItem("bSentinelDragon", "Sentinel to Dragon").SetValue(true));
            autoEventsMenu.AddItem(new MenuItem("bSentinelBaron", "Sentinel to Baron").SetValue(true));
            autoEventsMenu.AddItem(new MenuItem("slQprediction", "Q Prediction").SetValue(
                            new StringList(new[] { "Very High", "High", "Dashing" })));
            return autoEventsMenu;
        }
    }
}
