using LeagueSharp;

namespace S_Class_Kalista
{
    internal class SWalker
    {
        static SWalker()
        {
        }

        internal class BeforeAttackEventArgs
        {
            public static bool DisableNextAttack;
            private bool _process = true;
            public AttackableUnit Target;
            public Obj_AI_Base Unit = ObjectManager.Player;

            public bool Process
            {
                get { return _process; }
                set
                {
                    DisableNextAttack = !value;
                    _process = value;
                }
            }
        }

        private struct Events
        {
            public delegate void AfterAttack(AttackableUnit unit, AttackableUnit target);

            public delegate void BeforeAttack(BeforeAttackEventArgs args);

            public delegate void OnAttack(AttackableUnit unit, AttackableUnit target);

            public delegate void OnNonKillableMinion(AttackableUnit minion);

            public delegate void OnTargetChange(AttackableUnit oldTarget, AttackableUnit newTarget);
        }

        public struct OrbWalker
        {
            private enum Mode
            {
                LastHit,
                Mixed,
                Clear,
                Combo,
                Null
            }

            public float Delay;
        }
    }
}