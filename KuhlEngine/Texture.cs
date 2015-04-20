using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace KuhlEngine
{
    class Texture
    {
        private Image mTexture;

        public Texture(string aPath)
        {
            if (File.Exists(aPath))
            {
                mTexture = new Bitmap(16, 16);
            }
            else
            {
                mTexture = Image.FromFile(aPath, true);
            }
        }

        public Boolean Resize(int aWidth, int aHeight)
        {
            try
            {
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Image Image
        {
            get
            {
                return mTexture;
            }
        }
    }
}
