using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace KuhlEngine
{
    public class Frame
    {
        private Image mFrame;

        public Image Image { get { return mFrame; } }

        public Frame(int aWidth, int aHeight, Texture aBackground)
        {
            aBackground.Resize(aWidth, aHeight);
            mFrame = aBackground.Image;
        }

    }
}
