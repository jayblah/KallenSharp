using System;
using LeagueSharp.Common;

namespace Gosu_Shen
{
    class Program
    {
        static void Main(string[] args)
        {
            #if !DEBUG
            CustomEvents.Game.OnGameLoad += OnLoad;

            #endif
        }

        private static void OnLoad(EventArgs args)
        {
            if (GlobalProperties.ChampName != GlobalProperties.GetHero.ChampionName) return;
            //Else
            MenuManager.LoadMenu();
        }

    }
}
