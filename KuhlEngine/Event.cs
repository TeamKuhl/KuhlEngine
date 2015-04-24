using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace KuhlEngine
{
    public static class Event
    {
        // Render
        public delegate void RenderHandler(Image aFrame);
        public static RenderHandler NewFrame;

        // Collision
        public delegate void CollisionHandler(CollisionEventArgs e);
        public static CollisionHandler Collision;
    }

    public class CollisionEventArgs
    {
        public Item ActiveItem { get; internal set; }
        public Item PassiveItem { get; internal set; }
        public int Type { get; internal set; }

        public bool Cancel { get; set; }
    }
}
