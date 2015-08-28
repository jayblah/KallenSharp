using System;
using LeagueSharp;
using LeagueSharp.Common;

namespace Gosu_Kalista
{
    internal class Program
    {
        readonly static Random Seeder = new Random();

        private static void Main(string[] args)
        {
            if (args == null) throw new ArgumentNullException("args");
            // So you can test if it in VS wihout crahses
#if !DEBUG
            CustomEvents.Game.OnGameLoad += OnLoad;
#endif
        }

        // ReSharper disable once UnusedParameter.Local
        // ReSharper disable once UnusedMember.Local
        private static void OnLoad(EventArgs args)
        {
            Console.WriteLine(@"Generating Auto Properties...");
            Properties.GenerateProperties();

            if (Properties.PlayerHero.ChampionName != "Kalista")
                return;

            Console.WriteLine(@"Generating Menu");
            GosuMenu.GenerateMenu();
            Console.WriteLine(@"Generating Spells...");
            Properties.Champion.LoadSpells();
            Properties.MainMenu.AddToMainMenu();
            Console.WriteLine(@"Linking Game Events...");

            Game.OnUpdate += Game_OnUpdate;
            //Properties.AutoLevel.InitilizeAutoLevel();
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;

            Properties.Drawing.DamageToUnit = DamageCalc.GetRendDamage;
            Properties.Drawing.DamageToMonster = DamageCalc.GetRendDamage;
            Drawing.OnDraw += DrawingManager.Drawing_OnDraw;
            Drawing.OnDraw += DrawingManager.Drawing_OnDrawChamp;
            Drawing.OnDraw += DrawingManager.Drawing_OnDrawMonster;
            Orbwalking.OnNonKillableMinion += OrbWalkerManager.CheckNonKillables;
            Humanizer.AddAction("rendDelay",100);
            Humanizer.AddAction("generalDelay",125);

            Console.WriteLine(@"Gosu Kalista Load Completed");
        }

        private static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {

            if (sender == null || !sender.IsValid) return;
                // Reset auto attack after 125 MS
            if (sender.IsMe && args.SData.Name == "KalistaExpungeWrapper")
                Utility.DelayAction.Add(150, Orbwalking.ResetAutoAttackTimer);
            
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            //Auto Level in common is broken LOL

            //LeagueSharp.Common.AutoLevel.Enabled(Properties.MainMenu.Item("bAutoLevel").GetValue<bool>());

            AutoLevel.LevelUpSpells();

            if (Properties.PlayerHero.IsDead)
                return;
            if (Properties.PlayerHero.IsRecalling())
                return;

            OrbWalkerManager.EventCheck();
           OrbWalkerManager.DoTheWalk();

        }
    }
}
