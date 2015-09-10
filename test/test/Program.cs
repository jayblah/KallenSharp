using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Hello{0}:", Net.GetAddress());
            Console.WriteLine(Net.CheckSite().ToString());
   
            Console.ReadLine();
        }
    }

    class Net
    {

        //20-25
        private static string SiteRange = "104.20.2*.154";

        public static IPAddress GetAddress()
        {
            return Dns.GetHostAddresses("www.joduska.me")[0];
        }

        public static bool CheckSite()
        {
            var retAddress = ToInteger(GetAddress());
            Console.WriteLine(retAddress);
            for (var i = 0; i < 10; i++)
            {
                var temp = SiteRange.Replace('*', i.ToString()[0]);
                if (ToInteger(IPAddress.Parse(temp)) == retAddress)
                    return true;
            }
            return false;
        }

        private static int ToInteger(IPAddress ip)
        {
            var bytes = ip.GetAddressBytes();
            return (bytes[0] << 24 | bytes[1] << 16 | bytes[2] << 8 | bytes[3]);
        }

    }

}
