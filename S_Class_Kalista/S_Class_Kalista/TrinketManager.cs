using LeagueSharp;
using LeagueSharp.Common;

namespace S_Class_Kalista
{
    class TrinketManager
    {
        public static void BuyOrb()
        {
            if (!ObjectManager.Player.InShop() ||
                Items.HasItem(ItemId.Scrying_Orb_Trinket.ToString()) ||
                Items.HasItem(ItemId.Farsight_Orb_Trinket.ToString())) return;
            ObjectManager.Player.BuyItem(ItemId.Scrying_Orb_Trinket);
        }
    }
}
