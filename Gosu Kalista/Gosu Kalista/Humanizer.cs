﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Gosu_Kalista
{
    class Humanizer
    {

        private struct Action
        {
            public string Name { get; set; }
            public float Delay { get; set; }
            public float LastTick { get; set; }
        }

        private static readonly List<Action> ActionDelayList = new List<Action>();

        public static void AddAction(string actionName, float delayMs)
        {
            if (ActionDelayList.Any(a => a.Name == actionName)) return; // Id is in list already

            var nAction = new Action {Name = actionName, Delay = delayMs};
            ActionDelayList.Add(nAction);
        }

        //public static void DeleteAction(string actionName)
        //{
        //    if (ActionDelayList.All(a => a.Name != actionName)) return; // ID is not in list
        //    ActionDelayList.Remove(ActionDelayList.First(a => a.Name == actionName));
        //}

        public static void ChangeDelay(string actionName, float nDelay)
        {
            var cAction = ActionDelayList.Find(action => action.Name == actionName);
            if (cAction.Name == null) return;
            cAction.Delay = nDelay;
        }

        public static bool CheckDelay(string actionName)
        {
            var cAction = ActionDelayList.Find(action => action.Name == actionName);
            if (cAction.Name == null) return false;

            if (!(Environment.TickCount - cAction.LastTick >= cAction.Delay)) return false;

            cAction.LastTick = Environment.TickCount;
            return true;
        }
    }
}
