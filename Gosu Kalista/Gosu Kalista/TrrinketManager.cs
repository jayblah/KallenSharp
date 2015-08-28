
using System.Net;
using LeagueSharp;
using LeagueSharp.Common;

namespace Gosu_Kalista
{
    class TrrinketManager
    {
        const int Blue = 3363;
        const int Yellow = 3340;
        const int Red = 3341;

        public void BuyBlue()
        {
            if (ObjectManager.Player.InShop()&& !(Items.HasItem(3342) || Items.HasItem(3363)))
            {
                ObjectManager.Player.BuyItem(ItemId.Scrying_Orb_Trinket);
            }
        }
    }
}
