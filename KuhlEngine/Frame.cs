using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Collections.Generic;

namespace KuhlEngine
{
    public class Frame
    {
        private Image mFrame;

        public Image Image { get { return mFrame; } }

        public Frame(int aWidth, int aHeight, Texture aBackground, Dictionary<string, Item> aItems)
        {
            // Draw background
            aBackground.Resize(aWidth, aHeight);
            mFrame = aBackground.Image;

            Graphics g = Graphics.FromImage(mFrame);

            //Draw items
            foreach (KeyValuePair<string, Item> Keypair in aItems)
            {
                Texture texture = Keypair.Value.Texture;
                texture.Resize(Keypair.Value.Width, Keypair.Value.Height);
                g.DrawImage(texture.Image, new Point(Keypair.Value.X, Keypair.Value.Y));
            }

            g.Dispose();
        }

    }
}
