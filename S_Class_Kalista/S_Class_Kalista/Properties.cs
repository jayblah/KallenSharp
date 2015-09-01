﻿using LeagueSharp;
using LeagueSharp.Common;
using System;

namespace S_Class_Kalista
{
    internal class Properties
    {
        public static void GenerateProperties()
        {
            PlayerHero = ObjectManager.Player;
        }

        #region Auto Properties

        public static Orbwalking.Orbwalker LukeOrbWalker { get; set; }
        public static Menu MainMenu { get; set; }
        public static Obj_AI_Hero PlayerHero { get; set; }
        public static Obj_AI_Hero SoulBoundHero { get; set; }

        #endregion Auto Properties

        internal class Time
        {
            private static readonly DateTime AssemblyLoadTime = DateTime.Now;
            public static float LastRendTick { get; set; }
            public static float LastNonKillable { get; set; }
            public static float TickCount
            {
                get
                {
                    return (int)DateTime.Now.Subtract(AssemblyLoadTime).TotalMilliseconds;
                }
            }

            public static bool CheckRendDelay()
            {
                return !(TickCount - LastRendTick < 750);
            }

            public static bool CheckNonKillable()
            {
                return !(TickCount - LastNonKillable < 2000);
            }

        }

        internal class Drawing
        {
            private static DamageToUnitDelegate _damageToUnit, _damageToMonster;

            public delegate float DamageToUnitDelegate(Obj_AI_Hero hero);

            public static DamageToUnitDelegate DamageToUnit
            {
                get { return _damageToUnit; }

                set
                {
                    if (_damageToUnit == null)
                    {
                        LeagueSharp.Drawing.OnDraw += DrawingManager.Drawing_OnDrawChamp;
                    }
                    _damageToUnit = value;
                }
            }

            public static DamageToUnitDelegate DamageToMonster
            {
                get { return _damageToMonster; }

                set
                {
                    if (_damageToMonster == null)
                    {
                        LeagueSharp.Drawing.OnDraw += DrawingManager.Drawing_OnDrawMonster;
                    }
                    _damageToMonster = value;
                }
            }
        }

        //internal class AutoLevel
        //{
        //    public static LeagueSharp.Common.AutoLevel LevelManager { get; set; }

        //    private struct Abilitys // So you can refeer to spell to level by slot rather than 1,2,3,4
        //    {
        //        public const int Q = 1;
        //        public const int W = 2;
        //        public const int E = 3;
        //        public const int R = 4;
        //    }

        //    private static readonly int[] AbilitySequence =
        //    {
        //        Abilitys.W, Abilitys.E, Abilitys.Q, Abilitys.E,
        //        Abilitys.E, Abilitys.R, Abilitys.E, Abilitys.Q,
        //        Abilitys.E, Abilitys.Q, Abilitys.R, Abilitys.Q,
        //        Abilitys.Q, Abilitys.W, Abilitys.W, Abilitys.R,
        //        Abilitys.W, Abilitys.W
        //    };

        //    public static void InitilizeAutoLevel()
        //    {
        //        LevelManager = new LeagueSharp.Common.AutoLevel(AbilitySequence);
        //    }
        //}

        internal class Champion
        {
            public static Spell Q { get; set; }
            public static Spell W { get; set; }
            public static Spell E { get; set; }
            public static Spell R { get; set; }
            public Obj_AI_Hero SoulBound { get; set; }
            //public static Spell D { get; set; }
            //public static Spell F { get; set; }

            public static void UseRend()
            {
                E.Cast();
                Console.WriteLine("Last Rend Tick:{0} Current Tick{1}", Time.LastRendTick, Time.TickCount);
                Time.LastRendTick = Time.TickCount;
            }

            public static void UseNonKillableRend()
            {
                E.Cast();
                Console.WriteLine("Last Nonkillable Tick:{0} Current Tick{1}", Time.LastNonKillable, Time.TickCount);
                Time.LastNonKillable = Time.TickCount;
            }
            public static void LoadSpells()
            {
                //Loads range and shit
                Q = new Spell(SpellSlot.Q, 1150);
                W = new Spell(SpellSlot.W, 5200);
                E = new Spell(SpellSlot.E, 950);
                R = new Spell(SpellSlot.R, 1200);

                Q.SetSkillshot(0.25f, 40f, 1200f, true, SkillshotType.SkillshotLine);
                R.SetSkillshot(0.50f, 1500f, float.MaxValue, false, SkillshotType.SkillshotCircle);
            }
        }
    }
}