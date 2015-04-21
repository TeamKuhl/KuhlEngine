using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuhlEngine
{
    public class Map
    {
        public Map(int aWidth, int aHeight, Texture aTexture)
        {
            aTexture.Resize(aWidth, aHeight);
        }
    }
}
