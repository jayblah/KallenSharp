﻿// <copyright file="SMenu.cs" company="Kallen">
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
//.GetValue<KeyBind>().Active
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
            mixedMenu.AddItem(new MenuItem("sMixedStacks", "Required E stacks").SetValue(new Slider(4, 2, 15)));

            return mixedMenu;
        }

        private static Menu ComboMenu()
        {
            var mixedMenu = new Menu("Combo Options", "comboMenu");
            mixedMenu.AddItem(new MenuItem("bUseQCombo", "Auto Q").SetValue(false));
            mixedMenu.AddItem(new MenuItem("bUseECombo", "Auto E for kills").SetValue(false));
            return mixedMenu;
        }

        private static Menu LaneClearMenu()
        {
            var laneClearMenu = new Menu("Lane Clear Options", "laneClearOptions");
            laneClearMenu.AddItem(new MenuItem("bUseELaneClear", "Auto E On Minions Killed").SetValue(true));
            laneClearMenu.AddItem(new MenuItem("sLaneClearMinionsKilled", "Required Minions Killed").SetValue(new Slider(3, 2, 10)));
            laneClearMenu.AddItem(new MenuItem("bUseJungleClear", "Jungle Clear").SetValue(new KeyBind('G', KeyBindType.Toggle)));
            return laneClearMenu;
        }

        private static Menu DrawingMenu()
        {
            var drawMenu = new Menu("Drawing Settings", "Drawings");
            drawMenu.SubMenu("Drawings").AddItem(new MenuItem("bDraw", "Display Drawing").SetValue(true));
            drawMenu.SubMenu("Drawings").AddItem(new MenuItem("bDrawRendRange", "Draw Rend Range").SetValue(true));

            drawMenu.SubMenu("Drawings").AddItem(new MenuItem("bDrawOnChamp", "Draw On Enemies").SetValue(true));
            drawMenu.SubMenu("Drawings").AddItem(new MenuItem("bDrawTextOnChamp", "Display Floating Text (on enemies)").SetValue(true));
            drawMenu.SubMenu("Drawings").AddItem(new MenuItem("bDrawFillOnChamp", "Fill Combo Damage On Champs").SetValue(true));
            drawMenu.SubMenu("Drawings").AddItem(new MenuItem("bDrawTextOnSelf", "Display Floating Text (self)").SetValue(true));
            drawMenu.SubMenu("Drawings").AddItem(new MenuItem("bDrawOnMonsters", "Draw Damage On Monsters").SetValue(true));
            drawMenu.SubMenu("Drawings").AddItem(new MenuItem("bFillMonster", "Fill Damage On Monsters").SetValue(true));

            return drawMenu;
        }

        private static Menu AutoEvents()
        {
            var autoEventsMenu = new Menu("Auto Events", "autoEvents");
            autoEventsMenu.AddItem(new MenuItem("bAutoLevel", "Auto Level Skills").SetValue(true));
            autoEventsMenu.AddItem(new MenuItem("bAutoBuyOrb", "Auto Buy Orb at 6").SetValue(true));
            autoEventsMenu.AddItem(new MenuItem("bUseEToKillEpics", "Auto E Epics").SetValue(true));
            autoEventsMenu.AddItem(new MenuItem("bUseEToKillBuffs", "Auto E Buffs").SetValue(true));
            autoEventsMenu.AddItem(new MenuItem("bUseEToAutoKill", "Auto E Kill Enemies").SetValue(true));
            autoEventsMenu.AddItem(new MenuItem("bUseENonKillables", "Auto E NonKillable Minions").SetValue(true));
            autoEventsMenu.AddItem(new MenuItem("bEBeforeDeath", "Auto E Before Death").SetValue(false));
            autoEventsMenu.AddItem(new MenuItem("sEBeforeDeathChamps", "Champions With Stacks").SetValue(new Slider(1, 1, 5)));
            autoEventsMenu.AddItem(new MenuItem("sEBeforeDeathMinStacks", "Min Stacks On Champs").SetValue(new Slider(3, 1, 10)));
            autoEventsMenu.AddItem(new MenuItem("sEBeforeDeathMaxHP", "Max HP% To Activate E Before Death").SetValue(new Slider(10, 1, 30)));
            autoEventsMenu.AddItem(new MenuItem("bUseEToAutoKillMinions", "Auto E Kill Minions").SetValue(true));
            autoEventsMenu.AddItem(new MenuItem("sAutoEMinionsKilled", "Required Minions Killed From E").SetValue(new Slider(2, 2, 10)));
            autoEventsMenu.AddItem(new MenuItem("bAutoEOnStacksAndMinions", "Auto E When Stacks On Champ And Minions Killed").SetValue(true));
            autoEventsMenu.AddItem(new MenuItem("sUseEOnMinionKilled", "Required Minions Killed From E + Champ Stacks").SetValue(new Slider(3, 1, 10)));
            autoEventsMenu.AddItem(new MenuItem("sUseEOnChampStacks", "Required Stacks On Champion").SetValue(new Slider(1, 1, 10)));
            autoEventsMenu.AddItem(new MenuItem("bUseEOnLeave", "Auto E On Range Leave").SetValue(false));
            autoEventsMenu.AddItem(new MenuItem("sStacksOnLeave", "Required Stacks For Range Leave").SetValue(new Slider(4, 1, 10)));
            autoEventsMenu.AddItem(new MenuItem("bAutoSaveSoul", "Auto Save SoulBound partner").SetValue(true));
            autoEventsMenu.AddItem(new MenuItem("bBalista", "Balista?").SetValue(true));
            autoEventsMenu.AddItem(new MenuItem("sSoulBoundPercent", "Soul Bound HP% Remaining").SetValue(new Slider(10, 1, 90)));
            return autoEventsMenu;
        }

        private static Menu MiscMenu()
        {
            var autoEventsMenu = new Menu("Misc Crap", "miscMenu");
            autoEventsMenu.AddItem(new MenuItem("bSentinel", "Sentinel While In Range").SetValue(new KeyBind('T', KeyBindType.Press)));
            autoEventsMenu.AddItem(new MenuItem("bSentinelDragon", "Sentinel Dragon").SetValue(true));
            autoEventsMenu.AddItem(new MenuItem("bSentinelBaron", "Sentinel Baron").SetValue(true));
            autoEventsMenu.AddItem(new MenuItem("slQprediction", "Q Prediction").SetValue(
                            new StringList(new[] { "Very High", "High", "Dashing" })));
            return autoEventsMenu;
        }
    }
}