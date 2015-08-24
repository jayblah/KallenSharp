using LeagueSharp;
using LeagueSharp.Common;

namespace Gosu_Shen
{
    class GlobalProperties
    {
        private const string _menuName = "Gosu Shen";
        private const string _champName = "Shen";
        private static readonly Obj_AI_Hero PlayerHero = ObjectManager.Player;
        public static Menu GosuMenu { get; set; }
        public static string MenuName { get { return _menuName; }}
        public static string ChampName { get { return _champName; } }

        public static Obj_AI_Hero GetHero
        {
            get { return PlayerHero; }
        }
    }
}
