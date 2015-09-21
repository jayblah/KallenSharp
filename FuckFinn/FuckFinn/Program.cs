using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp.Common;

namespace FuckFinn
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += OnLoad;
        }
        [PermissionSet(SecurityAction.Assert, Unrestricted = true)]
        private static void OnLoad(EventArgs args)
        {
            var ping = new System.Net.NetworkInformation.Ping();
           if(Net.CheckSite())
                Console.WriteLine("L# IP {0}:", Net.GetAddress());
        }
    }
}
