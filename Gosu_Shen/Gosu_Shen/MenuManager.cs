using LeagueSharp.Common;


namespace Gosu_Shen
{
    class MenuManager
    {
        #region Public Functions

        public static void LoadMenu()
        {
            if (GlobalProperties.GosuMenu.DisplayName != null) return; // Loaded already
            
            //Else
            GlobalProperties.GosuMenu = GetMenus();
            GlobalProperties.GosuMenu.AddToMainMenu();
        }
        #endregion

        #region Menus

        private static Menu GetMenus()
        {
            var gosuMenu = new Menu(GlobalProperties.MenuName, GlobalProperties.MenuName, true);
            gosuMenu.AddSubMenu(HumanizerMenu());
            gosuMenu.AddSubMenu(HumanizerMenu());
            return gosuMenu;
        }
        private static Menu HumanizerMenu()
        {
            var humanizerMenu = new Menu("Humanizer", "Humanizer");

            humanizerMenu.SubMenu("Humanizer").AddItem(new MenuItem("minDelay", "Minimum delay for actions (ms)").SetValue(new Slider(10, 0, 200)));
            humanizerMenu.SubMenu("Humanizer").AddItem(new MenuItem("maxDelay", "Maximum delay for actions (ms)").SetValue(new Slider(75, 0, 250)));
            humanizerMenu.SubMenu("Humanizer").AddItem(new MenuItem("doHuman", "Humanize").SetValue(true));

            return humanizerMenu;
        }

        private static Menu OrbWalkingMenu()
        {
            var orbWalkingMenu = new Menu("Orbwalking", "Orbwalking");
            var targetSelectorMenu = new Menu("Target Selector", "Target Selector");
            TargetSelector.AddToMenu(targetSelectorMenu);
            orbWalkingMenu.AddSubMenu(targetSelectorMenu);
            return orbWalkingMenu;
        }

        #endregion

    }
}
