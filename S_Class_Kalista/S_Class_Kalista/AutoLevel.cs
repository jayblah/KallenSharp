using LeagueSharp;

namespace S_Class_Kalista
{
    class AutoLevel
    {
        #region Structures
        struct Abilitys // So you can refeer to spell to level by slot rather than 1,2,3,4
        {
            public const int Q = 1;
            public const int W = 2;
            public const int E = 3;
            public const int R = 4;
        }
        #endregion
        #region Variable Declaration

        private static readonly int[] AbilitySequence ={
            Abilitys.W,Abilitys.E,Abilitys.Q,Abilitys.E,
            Abilitys.E,Abilitys.R,Abilitys.E,Abilitys.Q,
            Abilitys.E,Abilitys.Q,Abilitys.R,Abilitys.Q,
            Abilitys.Q,Abilitys.W,Abilitys.W,Abilitys.R,
            Abilitys.W,Abilitys.W
        };

        private static int QOff = 0;
        private static int WOff = 0;
        private static int EOff = 0;
        private static int ROff = 0;
        #endregion
        #region Public Functions
        public static void LevelUpSpells()
        {
            var qL = Properties.PlayerHero.Spellbook.GetSpell(Properties.Champion.Q.Slot).Level + QOff;
            var wL = Properties.PlayerHero.Spellbook.GetSpell(Properties.Champion.W.Slot).Level + WOff;
            var eL = Properties.PlayerHero.Spellbook.GetSpell(Properties.Champion.E.Slot).Level + EOff;
            var rL = Properties.PlayerHero.Spellbook.GetSpell(Properties.Champion.R.Slot).Level + ROff;

            if (qL + wL + eL + rL >= Properties.PlayerHero.Level) return;

            int[] level = { 0, 0, 0, 0 };

            for (var i = 0; i < Properties.PlayerHero.Level; i++)
            {
                level[AbilitySequence[i] - 1] = level[AbilitySequence[i] - 1] + 1;
            }

            if (qL < level[0]) Properties.PlayerHero.Spellbook.LevelSpell(SpellSlot.Q);
            if (wL < level[1]) Properties.PlayerHero.Spellbook.LevelSpell(SpellSlot.W);
            if (eL < level[2]) Properties.PlayerHero.Spellbook.LevelSpell(SpellSlot.E);
            if (rL < level[3]) Properties.PlayerHero.Spellbook.LevelSpell(SpellSlot.R);
        }
        #endregion
    }
}

