using System;
using System.Collections.Generic;
using System.Linq;
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

        private static void OnLoad(EventArgs args)
        {
            var ping = new System.Net.NetworkInformation.Ping();

            var result = ping.Send("joduska.me");

            if (result.Status != System.Net.NetworkInformation.IPStatus.Success)
                return;

            Console.WriteLine("yay");
            //Console.WriteLine("Hello{0}:", Net.GetAddress());
           // Console.WriteLine("Hello{0}:",Net.GetAddress());
        }
    }
}
