using LeagueSharp;
using LeagueSharp.Common;
using S_Class_Kalista.Libs;
using ItemData = LeagueSharp.Common.Data.ItemData;
using Item = LeagueSharp.Common.Items.Item;

namespace S_Class_Kalista
{
    class ItemManager
    {
        struct ItemType
        {
            public readonly Item _Item;
            public readonly char _Type;
            public ItemType(Item item, char type)
            {
                _Item = item;
                _Type = type;
            }

        }
        //A = Activation 
        //D = Damage

        private static ItemType _bilgewaterCutlass = new ItemType(ItemData.Bilgewater_Cutlass.GetItem(),'D');
        private static ItemType _bladeOfTheRuinedKing = new ItemType(ItemData.Blade_of_the_Ruined_King.GetItem(), 'D');
        private static ItemType _youmuusGhostblade = new ItemType(ItemData.Youmuus_Ghostblade.GetItem(), 'A');
      
        //itemMenu.AddItem(new MenuItem("bUseBork", "Use Bork/Cutless").SetValue(true));
        //    itemMenu.AddItem(new MenuItem("bUseYoumuu", "Use Youmuu's").SetValue(true));

        public static void UseItems(Obj_AI_Hero target,bool combo = false)
        {
            if (Properties.MainMenu.Item("bUseBork").GetValue<bool>()) // use bork or cutless
            {
                if (_bladeOfTheRuinedKing._Item.IsReady())
                {
                    if (target.IsValidTarget(_bladeOfTheRuinedKing._Item.Range))
                    {
                        //Target will die
                        if (target.Health < Properties.PlayerHero.GetItemDamage(target, Damage.DamageItems.Botrk) ||
                            Properties.PlayerHero.HealthPercent < 10)
                            _bladeOfTheRuinedKing._Item.Cast(target);
                    }
                }
                else if (_bilgewaterCutlass._Item.IsReady())
                {
                    if (target.IsValidTarget(_bilgewaterCutlass._Item.Range))
                    {
                        //Target will die
                        if (target.Health < Properties.PlayerHero.GetItemDamage(target, Damage.DamageItems.Bilgewater) ||
                            Properties.PlayerHero.HealthPercent < 10)
                            _bilgewaterCutlass._Item.Cast(target);
                    }
                }
            }

            if (Properties.MainMenu.Item("bUseYoumuu").GetValue<bool>()) // use Youmuu's Only in combo and if your close to target
            {
                if (_youmuusGhostblade._Item.IsReady())
                {
                    if (combo)
                    {
                        if (Properties.SkyWalker != null)
                        {
                            if (target.IsValidTarget(SkyWalker.GetRealAutoAttackRange(Properties.PlayerHero) + -50))
                                _youmuusGhostblade._Item.Cast();
                        }
                        else if (Properties.HavenWalker != null)
                        {
                            if (target.IsValidTarget(HWalker.GetRealAutoAttackRange(Properties.PlayerHero) + -50))
                                _youmuusGhostblade._Item.Cast();
                        }
                        else
                        {
                            if (target.IsValidTarget(Orbwalking.GetRealAutoAttackRange(Properties.PlayerHero) + -50))
                                _youmuusGhostblade._Item.Cast();
                        }
                    }
                }

            }
        }
    }
}
