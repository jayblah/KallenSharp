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

    //    if (!Properties.MainMenu.Item("bDrawOnChamp").GetValue<bool>() || Properties.Drawing.DamageToUnit == null)
    //            return;
          
    //        foreach (var unit in HeroManager.Enemies.Where(h => h.IsValid && h.IsHPBarRendered))
    //        {
    //            var barPos = unit.HPBarPosition;
    //    var damage = DamageCalc.GetRendDamage(unit);
    //    var percentHealthAfterDamage = Math.Max(0, unit.Health - damage) / unit.MaxHealth;
    //    var yPos = barPos.Y + YOffset;
    //    var xPosDamage = barPos.X + XOffset + Width * percentHealthAfterDamage;
    //    var xPosCurrentHp = barPos.X + XOffset + Width * unit.Health / unit.MaxHealth;

    //            if (Properties.MainMenu.Item("bDrawTextOnChamp").GetValue<bool>() && damage > unit.Health)
    //            {
    //                Console.WriteLine("Draw killable text");
    //                RenderText.X = (int)barPos.X + XOffset;
    //                RenderText.Y = (int)barPos.Y + YOffset - 13;
    //                RenderText.text = "Rend Will Kill";
    //                RenderText.OnEndScene();
    //            }


    //LeagueSharp.Drawing.DrawLine(xPosDamage, yPos, xPosDamage, yPos + Height, 1, Color.LightGray);

    //            if (!Properties.MainMenu.Item("bDrawFillOnChamp").GetValue<bool>()) return;

    private static Menu DrawingMenu()
        {
            var drawMenu = new Menu("Drawing Settings", "Drawings");
            drawMenu.SubMenu("Drawings").AddItem(new MenuItem("bDraw", "Display Drawing").SetValue(true));
            drawMenu.SubMenu("Drawings").AddItem(new MenuItem("bDrawRendRange", "Draw Rend Range").SetValue(true));
            //  drawMenu.SubMenu("Drawings")
            //    .AddItem(new MenuItem("bDrawAutoAttackRange", "Draw Auto Attack Range").SetValue(true));
            drawMenu.SubMenu("Drawings").AddItem(new MenuItem("bDrawOnChamp", "Draw On Enemies").SetValue(true));
            drawMenu.SubMenu("Drawings").AddItem(new MenuItem("bDrawTextOnChamp", "Display Floating Text (on enemies)").SetValue(true));
            drawMenu.SubMenu("Drawings").AddItem(new MenuItem("bDrawFillOnChamp", "Fill Combo Damage On Champs").SetValue(true));

            drawMenu.SubMenu("Drawings").AddItem(new MenuItem("bDrawOnMonsters", "Draw Damage On Monsters").SetValue(true));
            //drawMenu.SubMenu("Drawings").AddItem(new MenuItem("bDrawOnCreep", "Display Damage On Creeps").SetValue(true));
            drawMenu.SubMenu("Drawings").AddItem(new MenuItem("bDrawOnEpic", "Draw Damage On Epics").SetValue(true));

            return drawMenu;
        }

        private static Menu AutoEvents()
        {
            var autoEventsMenu = new Menu("Auto Events", "autoEvents");
            autoEventsMenu.AddItem(new MenuItem("bAutoLevel", "Auto Level Skills").SetValue(true));
            autoEventsMenu.AddItem(new MenuItem("bAutoSentinelBaron", "Auto Sentinel Baron").SetValue(true));
            autoEventsMenu.AddItem(new MenuItem("bAutoSentinelDragon", "Auto Sentinel").SetValue(true));
            autoEventsMenu.AddItem(new MenuItem("bUseEToKillEpics", "Auto E Epics").SetValue(true));
            autoEventsMenu.AddItem(new MenuItem("bUseEToKillBuffs", "Auto E Buffs").SetValue(true));
            autoEventsMenu.AddItem(new MenuItem("bUseEToAutoKill", "Auto E Kill Enemies").SetValue(true));
            autoEventsMenu.AddItem(new MenuItem("bUseENonKillables", "Auto E NonKillables").SetValue(true));
            autoEventsMenu.AddItem(new MenuItem("bUseEToAutoKillMinions", "Auto E Kill Minions").SetValue(true));
            autoEventsMenu.AddItem(new MenuItem("sAutoEMinionsKilled", "Required Minions Killed From E").SetValue(new Slider(3, 2, 10)));

            autoEventsMenu.AddItem(new MenuItem("bAutoEOnStacksAndMinions", "Auto E When Stacks On Champ And Minions Killed").SetValue(false));
            autoEventsMenu.AddItem(new MenuItem("sUseEOnMinionKilled", "Required Minions Killed From E").SetValue(new Slider(3, 1, 10)));
            autoEventsMenu.AddItem(new MenuItem("sUseEOnChampStacks", "Required Minions Killed From E").SetValue(new Slider(1, 0, 3)));

            return autoEventsMenu;
        }
    }
}
