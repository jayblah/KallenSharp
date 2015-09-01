using LeagueSharp.Common;
using System;
using System.Linq;

namespace S_Class_Kalista
{
    internal class OrbWalkerManager
    {
        #region Public Functions

        public static void DoTheWalk()
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

        #endregion Public Functions

        #region Private Functions

        private static HitChance GetHitChance()
        {
            switch (Properties.MainMenu.Item("slQprediction").GetValue<StringList>().SelectedIndex)
            {
                case 0:
                    return HitChance.VeryHigh;

                case 1:
                    return HitChance.High;

                case 2:
                    return HitChance.Dashing;
            }
            return HitChance.VeryHigh;
        }

        private static void Combo()
        {
            if (Properties.MainMenu.Item("bUseQCombo").GetValue<bool>() && Properties.Champion.Q.IsReady())
            {
                var target = TargetSelector.GetTarget(Properties.Champion.Q.Range, TargetSelector.DamageType.Physical);
                var predictionPosition = Properties.Champion.Q.GetPrediction(target);
                if (predictionPosition.Hitchance >= GetHitChance())
                    if (Properties.PlayerHero.IsWindingUp || Properties.PlayerHero.IsDashing())
                        Properties.Champion.Q.Cast(predictionPosition.CastPosition);
            }
            if (!Properties.MainMenu.Item("bUseECombo").GetValue<bool>() || !Properties.Champion.E.IsReady()) return;

            if (!Properties.Time.CheckRendDelay()) // Wait for rend delay
                return;

            AutoEventManager.CheckEnemies();
        }

        private static void Mixed()
        {
            if (Properties.MainMenu.Item("bUseQMixed").GetValue<bool>() && Properties.Champion.Q.IsReady())
            {
                var target = TargetSelector.GetTarget(Properties.Champion.Q.Range, TargetSelector.DamageType.Physical);
                var predictionPosition = Properties.Champion.Q.GetPrediction(target);
                if (predictionPosition.Hitchance >= GetHitChance())
                    if (Properties.PlayerHero.IsWindingUp || Properties.PlayerHero.IsDashing())
                        Properties.Champion.Q.Cast(predictionPosition.CastPosition);
            }
            if (!Properties.MainMenu.Item("bUseEMixed").GetValue<bool>()) return;

            // ReSharper disable once UnusedVariable
            foreach (var stacks in from target in HeroManager.Enemies where target.IsValid where target.IsValidTarget(Properties.Champion.E.Range) where !DamageCalc.CheckNoDamageBuffs(target) select target.GetBuffCount("kalistaexpungemarker") into stacks where stacks >= Properties.MainMenu.Item("sMixedStacks").GetValue<Slider>().Value select stacks)
            {
                if (!Properties.Time.CheckRendDelay()) // Wait for rend delay
                    continue;

                Console.WriteLine("Using Mixed E:{0}", Properties.Time.TickCount);
                Properties.Champion.UseRend();
            }
        }

        private static void LaneClear()
        {
        }

        private static void LastHit()
        {
        }

        #endregion Private Functions
    }
}