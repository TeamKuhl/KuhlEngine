using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace KuhlEngine
{
    public class Renderer
    {
        public delegate void RenderHandler(Image aFrame, int aWidth, int aHeight);
        public RenderHandler newFrame;

        private Map mMap;

        public Boolean initializeMap(int aWidth, int aHeight, Texture aTexture)
        {
            try
            {
                mMap = new Map(aWidth, aHeight, aTexture);

                if (this.newFrame != null) this.newFrame(aTexture.Image, aTexture.Image.Width, aTexture.Image.Height);

                return true;
            }
            catch
            {
                return false;
            }
        }



        
    }
}
