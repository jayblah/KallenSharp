using LeagueSharp.Common;


namespace Gosu_Kalista
{
    class OrbWalkerManager
    {
        public static void OrbWalk()
        {
            switch (Properties.LukeOrbWalker.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Combo:
                    Combo();
                    break;

                case Orbwalking.OrbwalkingMode.Mixed:
                    Mixed();
                    break;

                case Orbwalking.OrbwalkingMode.LaneClear:
                    LaneClear();
                    break;

                case Orbwalking.OrbwalkingMode.LastHit:
                    LastHit();
                    break;
            }
        }
        private static void Combo()
        {
            
        }
        private static void Mixed()
        {

        }
        private static void LaneClear()
        {

        }
        private static void LastHit()
        {

        }
    }
}
