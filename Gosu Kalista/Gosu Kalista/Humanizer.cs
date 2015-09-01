//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace Gosu_Kalista
//{
//    class Humanizer
//    {

//        private struct Action
//        {
//            public string Name { get; set; }
//            public float Delay { get; set; }
//            public float LastTick { get; set; }
//        }

//        private static DateTime _assemblyLoadTime = DateTime.Now;
//        private static readonly List<Action> ActionDelayList = new List<Action>();

//        public static float TickCount
//        {
//            get
//            {
//                return (int)DateTime.Now.Subtract(_assemblyLoadTime).TotalMilliseconds;
//            }
//        }

//        public static void AddAction(string actionName, float delayMs)
//        {
//            if (ActionDelayList.Any(a => a.Name == actionName)) return; // Id is in list already
//            var nAction = new Action {Name = actionName, Delay = delayMs};
//            ActionDelayList.Add(nAction);
//        }


//        public static void ChangeDelay(string actionName, float nDelay)
//        {
//            var cAction = ActionDelayList.Find(action => action.Name == actionName);
//            if (cAction.Name == null) return;
//            cAction.Delay = nDelay + TickCount;
//        }

//        public static bool CheckDelay(string actionName)
//        {
//            var cAction = ActionDelayList.Find(action => action.Name == actionName);
//            if (cAction.Name == null) return false;
            
//            Console.WriteLine("Current Last Tick{0} Current Tick{1}" ,cAction.LastTick,TickCount);
//            if (!(TickCount - cAction.LastTick >= cAction.Delay)) return false;
//            float delay = cAction.Delay;
//            ActionDelayList.Remove(cAction);
//            AddAction(actionName, delay);
//            return true;
//        }
//    }
//}
