using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;

namespace S_Class_Jinx
{
    class Properties
    {
        public static void GenerateProperties()
        {
            PlayerHero = ObjectManager.Player;
        }

        public static Orbwalking.Orbwalker LukeOrbWalker { get; set; }
        public static Menu MainMenu { get; set; }
        public static Obj_AI_Hero PlayerHero { get; set; }

        internal class Champion
        {
            public static Spell Q { get; set; }
            public static Spell W { get; set; }
            public static Spell E { get; set; }
            public static Spell R { get; set; }


            public static void LoadSpells()
            {
                //Loads range and shit
                Q = new Spell(SpellSlot.Q);
                W = new Spell(SpellSlot.W, 1450);
                E = new Spell(SpellSlot.E, 900);
                R = new Spell(SpellSlot.R, 2150f);

                W.SetSkillshot(0.6f,60f,3300f,true,SkillshotType.SkillshotLine);
                E.SetSkillshot(0.7f, 120f, 1750f, false, SkillshotType.SkillshotCircle);
            }

        }
    }
}

