using System;

namespace SDisplay
{
    class RenderManager
    {
        private static readonly DateTime AssemblyLoadTime = DateTime.Now;
        public static float LastRender { get; set; }
        public static float MonitorRefreshRate { get; set; }

        public static float TickCount
        {
            get
            {
                return (int)DateTime.Now.Subtract(AssemblyLoadTime).TotalMilliseconds;
            }
        }

        public static void Render()
        {
            LastRender = TickCount;
        }
        public static bool CheckRender()
        {
            return !(TickCount - LastRender < 1000 / MonitorRefreshRate);
        }
    }
}
