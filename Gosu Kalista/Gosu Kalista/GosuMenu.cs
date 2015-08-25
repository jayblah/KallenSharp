using LeagueSharp.Common;

namespace Gosu_Kalista
{
    internal class GosuMenu
    {
        private const string MenuName = "Gosu Kalista";

        public static void GenerateMenu()
        {
            Properties.MainMenu = new Menu(MenuName, MenuName, true);
            Properties.MainMenu.AddSubMenu(HumanizerMenu());
            Properties.MainMenu.AddSubMenu(DrawingMenu());
            Properties.MainMenu.AddSubMenu(AutoEvents());
            Properties.MainMenu.AddSubMenu(OrbWalkingMenu());
            Properties.MainMenu.AddSubMenu(MixedMenu());
            Properties.MainMenu.AddSubMenu(ComboMenu());
            Properties.LukeOrbWalker = new Orbwalking.Orbwalker(Properties.MainMenu.SubMenu("Orbwalking"));
        }

        private static Menu HumanizerMenu()
        {
            var humanizerMenu = new Menu("Humanizer", "Humanizer");
            humanizerMenu.SubMenu("Humanizer")
                .AddItem(new MenuItem("minDelay", "Minimum delay for actions (ms)").SetValue(new Slider(10, 0, 200)));
            humanizerMenu.SubMenu("Humanizer")
                .AddItem(new MenuItem("maxDelay", "Maximum delay for actions (ms)").SetValue(new Slider(75, 0, 250)));
            humanizerMenu.SubMenu("Humanizer")
                .AddItem(
                    new MenuItem("minCreepHPOffset", "Minimum HP for a minion to have before CSing Damage >= HP+(%)")
                        .SetValue(new Slider(5, 0, 25)));
            humanizerMenu.SubMenu("Humanizer")
                .AddItem(
                    new MenuItem("maxCreepHPOffset", "Maximum HP for a minion to have before CSing Damage >= HP+(%)")
                        .SetValue(new Slider(15, 0, 25)));
            humanizerMenu.SubMenu("Humanizer").AddItem(new MenuItem("doHuman", "Humanize").SetValue(true));
            return humanizerMenu;
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
          //  drawMenu.SubMenu("Drawings")
            //    .AddItem(new MenuItem("bDrawAutoAttackRange", "Draw Auto Attack Range").SetValue(true));
            drawMenu.SubMenu("Drawings").AddItem(new MenuItem("bDrawText", "Display Floating Text").SetValue(true));
            return drawMenu;
        }

        private static Menu AutoEvents()
        {
            var autoEventsMenu = new Menu("Auto Events", "autoEvents");
            autoEventsMenu.AddItem(new MenuItem("bAutoLevel", "Auto Level Skills").SetValue(true));
            autoEventsMenu.AddItem(new MenuItem("bUseEToKillEpics", "Auto Level Skills").SetValue(true));
            autoEventsMenu.AddItem(new MenuItem("bUseEToAutoKill", "Auto Level Skills").SetValue(true));
            return autoEventsMenu;
        }
    }
}
