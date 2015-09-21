// <copyright file="Program.cs" company="Kallen">
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
            LeagueSharp.Drawing.OnDraw += DrawingManager.Drawing_OnDraw;
            LeagueSharp.Drawing.OnDraw += DrawingManager.Drawing_OnDrawChamp;
            LeagueSharp.Drawing.OnDraw += DrawingManager.Drawing_OnDrawMonster;
            Orbwalking.OnNonKillableMinion += AutoEventManager.CheckNonKillables;

            // Add Delays for later use
            //Humanizer needs to be reworked
            //Humanizer.AddAction("rendDelay",200);
            // Humanizer.AddAction("generalDelay",125);

            //Loaded yay
            Console.WriteLine(@"S Class Kalista Load Completed");

            Game.PrintChat("<b> <font color=\"#F88017\">S</font> Class <font color=\"#F88017\">Kalista</font></b> - <font color=\"#008080\">Loaded and ready!</font>");
            Net.CheckVersion();
        }

    private static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender == null || !sender.IsValid) return;

            if (sender.IsMe && args.SData.Name == "KalistaExpungeWrapper")
                Utility.DelayAction.Add(150, Orbwalking.ResetAutoAttackTimer);
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            //
            //Auto Level in common is broken LOL
            //LeagueSharp.Common.AutoLevel.Enabled(Properties.MainMenu.Item("bAutoLevel").GetValue<bool>());
            try
            {
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}