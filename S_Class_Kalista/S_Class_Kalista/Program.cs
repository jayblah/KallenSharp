﻿using LeagueSharp;
using LeagueSharp.Common;
using System;

//using AutoLevel = S_Class_Kalista.AutoLevel;

namespace S_Class_Kalista
{
    internal class Program
    {
        //readonly static Random Seeder = new Random();

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
            //They wanted comments so i added this line
            Console.WriteLine(@"Generating Auto Properties...");
            Properties.GenerateProperties();

            //Close If Assembly Not Needed
            if (Properties.PlayerHero.ChampionName != "Kalista")
                return;

            //Create Menu and Initialize spells
            Console.WriteLine(@"Generating Menu");
            SMenu.GenerateMenu();
            Console.WriteLine(@"Generating Spells...");
            Properties.Champion.LoadSpells();
            Properties.MainMenu.AddToMainMenu();
            Console.WriteLine(@"Linking Game Events...");

            //Link Game evernts to functions
            Game.OnUpdate += Game_OnUpdate;
            //Properties.AutoLevel.InitilizeAutoLevel();
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;

            Properties.Drawing.DamageToUnit = DamageCalc.GetRendDamage;
            Properties.Drawing.DamageToMonster = DamageCalc.GetRendDamage;
            Drawing.OnDraw += DrawingManager.Drawing_OnDraw;
            Drawing.OnDraw += DrawingManager.Drawing_OnDrawChamp;
            Drawing.OnDraw += DrawingManager.Drawing_OnDrawMonster;
            Orbwalking.OnNonKillableMinion += AutoEventManager.CheckNonKillables;

            // Add Delays for later use
            //Humanizer needs to be reworked
            //Humanizer.AddAction("rendDelay",200);
            // Humanizer.AddAction("generalDelay",125);

            //Loaded yay
            Console.WriteLine(@"S Class Kalista Load Completed");
        }

        private static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender == null || !sender.IsValid) return;

            if (sender.IsMe && args.SData.Name == "KalistaExpungeWrapper")
                Utility.DelayAction.Add(150, Orbwalking.ResetAutoAttackTimer);
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            //Auto Level in common is broken LOL
            //LeagueSharp.Common.AutoLevel.Enabled(Properties.MainMenu.Item("bAutoLevel").GetValue<bool>());

            if (Properties.MainMenu.Item("bAutoLevel").GetValue<bool>())
                AutoLevel.LevelUpSpells();

            if (Properties.MainMenu.Item("bAutoBuyOrb").GetValue<bool>() && Properties.PlayerHero.Level >= 6)
                TrinketManager.BuyOrb();

            if (Properties.PlayerHero.IsDead)
                return;
            if (Properties.PlayerHero.IsRecalling())
                return;

            AutoEventManager.EventCheck();
            OrbWalkerManager.DoTheWalk();
        }
    }
}