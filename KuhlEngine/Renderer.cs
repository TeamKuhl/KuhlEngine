using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace KuhlEngine
{
    class Renderer
    {
        public delegate void RenderHandler(Image aFrame, int aWidth, int aHeight);
        public RenderHandler newFrame;

        private Map mMap;

        /// <summary>
        ///     Initialize a new Map
        /// </summary>
        /// <returns></returns>
        public Boolean initializeMap(int aWidth, int aHeight)
        {
            try
            {
                mMap = new Map(aWidth, aHeight);

                if (this.newFrame != null) this.newFrame(null, 0, 0);

                return true;
            }
            catch
            {
                return false;
            }
        }



        
    }
}
