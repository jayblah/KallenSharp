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
            Properties.MainMenu.AddSubMenu(ManaMenu());
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
            mixedMenu.AddItem(new MenuItem("bUseQMixedReset", "Reset AutoAttack With Q").SetValue(false));
            mixedMenu.AddItem(new MenuItem("bUseEMixed", "Auto E on Stacks").SetValue(false));
            mixedMenu.AddItem(new MenuItem("sMixedStacks", "Required E stacks").SetValue(new Slider(4, 2, 15)));

            return mixedMenu;
        }

        private static Menu ComboMenu()
        {
            var mixedMenu = new Menu("Combo Options", "comboMenu");
            mixedMenu.AddItem(new MenuItem("bUseQCombo", "Auto Q").SetValue(false));
            mixedMenu.AddItem(new MenuItem("bUseQComboReset", "Reset AutoAttack With Q").SetValue(false));
            mixedMenu.AddItem(new MenuItem("bUseECombo", "Auto E for kills").SetValue(false));
            mixedMenu.AddItem(new MenuItem("bUseMinionComboWalk", "(Beta) OrbWalk Using Minions(Hard To Expain)").SetValue(false));
            return mixedMenu;
        }

        private static Menu LaneClearMenu()
        {
            var laneClearMenu = new Menu("Lane Clear Options", "laneClearOptions");
            laneClearMenu.AddItem(new MenuItem("bUseELaneClear", "Auto E On Minions Killed").SetValue(true));
            laneClearMenu.AddItem(new MenuItem("sLaneClearMinionsKilled", "Required Minions Killed By E").SetValue(new Slider(3, 2, 10)));

            laneClearMenu.AddItem(new MenuItem("bUseQLaneClear", "Kill Minions With Q").SetValue(false));
            laneClearMenu.AddItem(new MenuItem("sLaneClearMinionsKilledQ", "Required Minions Killed By Q").SetValue(new Slider(3, 2, 10)));

            laneClearMenu.AddItem(new MenuItem("bUseJungleClear", "Jungle Clear").SetValue(new KeyBind('G', KeyBindType.Toggle)));
            return laneClearMenu;
        }

        private static Menu DrawingMenu()
        {
            var drawMenu = new Menu("Drawing Settings", "Drawings");

            drawMenu.AddItem(new MenuItem("bDraw", "Display Drawing").SetValue(true));
            drawMenu.AddItem(new MenuItem("cDrawRendRange", "Draw Rend Range").SetValue(new Circle(true, Color.LightSkyBlue)));

            drawMenu.AddItem(new MenuItem("bDrawOnChamp", "Draw On Enemies").SetValue(true));
            drawMenu.AddItem(new MenuItem("cDrawTextOnChamp", "Display Killable Text On Enemies").SetValue(new Circle(true, Color.Red)));

            drawMenu.AddItem(new MenuItem("cDrawFillOnChamp", "Draw Combo Damage On Champs").SetValue(new Circle(true, Color.DarkGray)));

            drawMenu.AddItem(new MenuItem("bDrawTextOnSelf", "Display Floating Text (self)").SetValue(true));

            drawMenu.AddItem(new MenuItem("cDrawOnMonsters", "Draw Damage On Monsters").SetValue(new Circle(true, Color.LightSlateGray)));
            drawMenu.AddItem(new MenuItem("cFillMonster", "Damage Fill On Monsters").SetValue(new Circle(true, Color.DarkGray)));
            drawMenu.AddItem(new MenuItem("cKillableText", "Display Killable Text On Monsters").SetValue(new Circle(true, Color.Red)));

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
            autoEventsMenu.AddItem(new MenuItem("sAutoEMinionsKilled", "Required Minions Killed From E").SetValue(new Slider(2, 1, 10)));
            autoEventsMenu.AddItem(new MenuItem("bAutoEOnStacksAndMinions", "Auto E When Stacks On Champ And Minions Killed").SetValue(true));
            autoEventsMenu.AddItem(new MenuItem("sUseEOnMinionKilled", "Required Minions Killed From E + Champ Stacks").SetValue(new Slider(3, 1, 10)));
            autoEventsMenu.AddItem(new MenuItem("sUseEOnChampStacks", "Required Stacks On Champion").SetValue(new Slider(1, 1, 10)));
            autoEventsMenu.AddItem(new MenuItem("bUseEOnLeave", "Auto E On Range Leave").SetValue(false));
            autoEventsMenu.AddItem(new MenuItem("sStacksOnLeave", "Required Stacks For Range Leave").SetValue(new Slider(4, 1, 10)));
            autoEventsMenu.AddItem(new MenuItem("bAutoSaveSoul", "Auto Save SoulBound partner").SetValue(true));
            autoEventsMenu.AddItem(new MenuItem("bBST", "Balista? Skalista? Tahmlista?").SetValue(true));
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

        private static Menu ManaMenu()
        {
            var autoEventsMenu = new Menu("Mana Manager", "manaMenu");
            autoEventsMenu.AddItem(new MenuItem("bUseManaManager", "Use Mana Manager").SetValue(true));
            autoEventsMenu.AddItem(new MenuItem("sMinManaQ", "Min% Mana For Q").SetValue(new Slider(15, 10, 40)));
            autoEventsMenu.AddItem(new MenuItem("sMinManaE", "Min% Mana For E").SetValue(new Slider(0, 0, 30)));
            return autoEventsMenu;
        }
    }
}