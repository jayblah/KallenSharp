using LeagueSharp.Common;

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
            Properties.MainMenu.AddSubMenu(MiscMenu());
            Properties.LukeOrbWalker = new Orbwalking.Orbwalker(Properties.MainMenu.SubMenu("Orbwalking"));
        }

        //private static Menu HumanizerMenu()
        //{
        //    var humanizerMenu = new Menu("Humanizer", "Humanizer Not Implimented");
        //    humanizerMenu.SubMenu("Humanizer")
        //        .AddItem(new MenuItem("minDelay", "Minimum delay for actions (ms)").SetValue(new Slider(10, 0, 200)));
        //    humanizerMenu.SubMenu("Humanizer")
        //        .AddItem(new MenuItem("maxDelay", "Maximum delay for actions (ms)").SetValue(new Slider(75, 0, 250)));
        //    humanizerMenu.SubMenu("Humanizer")
        //        .AddItem(
        //            new MenuItem("minCreepHPOffset", "Minimum HP for a minion to have before CSing Damage >= HP+(%)")
        //                .SetValue(new Slider(5, 0, 25)));
        //    humanizerMenu.SubMenu("Humanizer")
        //        .AddItem(
        //            new MenuItem("maxCreepHPOffset", "Maximum HP for a minion to have before CSing Damage >= HP+(%)")
        //                .SetValue(new Slider(15, 0, 25)));
        //    humanizerMenu.SubMenu("Humanizer").AddItem(new MenuItem("doHuman", "Humanize").SetValue(true));
        //    return humanizerMenu;
        //}

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
            mixedMenu.AddItem(new MenuItem("bUseQMixed", "Auto Q").SetValue(false));
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

    private static Menu DrawingMenu()
        {
            var drawMenu = new Menu("Drawing Settings", "Drawings");
            drawMenu.SubMenu("Drawings").AddItem(new MenuItem("bDraw", "Display Drawing").SetValue(true));
            drawMenu.SubMenu("Drawings").AddItem(new MenuItem("bDrawRendRange", "Draw Rend Range").SetValue(true));

            drawMenu.SubMenu("Drawings").AddItem(new MenuItem("bDrawOnChamp", "Draw On Enemies").SetValue(true));
            drawMenu.SubMenu("Drawings").AddItem(new MenuItem("bDrawTextOnChamp", "Display Floating Text (on enemies)").SetValue(true));
            drawMenu.SubMenu("Drawings").AddItem(new MenuItem("bDrawFillOnChamp", "Fill Combo Damage On Champs").SetValue(true));

            drawMenu.SubMenu("Drawings").AddItem(new MenuItem("bDrawOnMonsters", "Draw Damage On Monsters").SetValue(true));
            drawMenu.SubMenu("Drawings").AddItem(new MenuItem("bDrawOnEpic", "Draw Damage On Epics").SetValue(true));
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
            autoEventsMenu.AddItem(new MenuItem("bUseEToAutoKillMinions", "Auto E Kill Minions").SetValue(true));
            autoEventsMenu.AddItem(new MenuItem("sAutoEMinionsKilled", "Required Minions Killed From E").SetValue(new Slider(2, 2, 10)));
            autoEventsMenu.AddItem(new MenuItem("bAutoEOnStacksAndMinions", "Auto E When Stacks On Champ And Minions Killed").SetValue(false));
            autoEventsMenu.AddItem(new MenuItem("sUseEOnMinionKilled", "Required Minions Killed From E + Champ Stacks").SetValue(new Slider(3, 1, 10)));
            autoEventsMenu.AddItem(new MenuItem("sUseEOnChampStacks", "Required Stacks On Champion").SetValue(new Slider(1, 1, 10)));
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
