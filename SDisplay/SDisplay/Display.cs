using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

namespace SDisplay
{
    public class Display
    {
        private static List<NamedCircles> CirclesToRender = new List<NamedCircles>();

        struct NamedCircles
        {
            public string Name;
            public Render.Circle Circle;
        }

        public static void AddCircle(Vector3 origin, float radios, Color circleColor, int thinkness,string name)
        {
            if (name.Length < 0) return;
            if (CirclesToRender.Any(a => a.Name == name)) return; // Id is in list already
            var named = new NamedCircles
            {
                Circle = new Render.Circle(origin, radios, circleColor, thinkness),
                Name = name
            };
        }

        public static void RenderCycle(List<Render.Circle> circles)
        {
            foreach (var circ in CirclesToRender)
            {
                if (!RenderManager.CheckRender()) return;
                    if (circ.Name == null || circ.Name.Length < 0)  return;
                Render.Circle.DrawCircle(circ.Circle.Position, circ.Circle.Radius, circ.Circle.Color, circ.Circle.Width);


            }
        }
    }
}
