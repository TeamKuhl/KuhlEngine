using System;
using System.Drawing;
using System.IO;

namespace KuhlEngine
{
    public class Texture
    {
        private Image mTexture;
        private Boolean mStretch = true;    //true = Stretch the Image; false = Repeat the Image

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="aPath">The path to the Image</param>
        public Texture(string aPath)
        {
            if (!File.Exists(aPath))
            {
                mTexture = new Bitmap(16, 16);
            }
            else
            {
                mTexture = Image.FromFile(aPath, true);
            }
        }

        /// <summary>
        ///     Resize an Image to a specified size
        /// </summary>
        /// <param name="aWidth">new width</param>
        /// <param name="aHeight">new height</param>
        /// <returns>success</returns>
        internal Boolean Resize(int aWidth, int aHeight)
        {
            try
            {
                if (mStretch)
                {
                    //Stretch Image
                    mTexture = new Bitmap(mTexture, new Size(aWidth, aHeight));
                }
                else
                {
                    //Repeat Image
                    int widthAmount = aWidth / mTexture.Width;
                    if ((aWidth % mTexture.Width) != 0) widthAmount++;

                    int heightAmount = aHeight / mTexture.Height;
                    if ((aHeight % mTexture.Height) != 0) heightAmount++;

                    Bitmap Texture = new Bitmap(aWidth, aHeight);
                    Graphics g = Graphics.FromImage(Texture);

                    //Draw new Image
                    for (int yCount = 0; yCount < heightAmount; yCount++)
                    {
                        for (int xCount = 0; xCount < widthAmount; xCount++)
                        {
                            g.DrawImage(mTexture, new Point(mTexture.Width * xCount, mTexture.Height * yCount));
                        }
                    }

                    mTexture = Texture;
                    g.Dispose();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     Returns the Texture as Image
        /// </summary>
        public Image Image
        {
            get
            {
                return mTexture;
            }
        }

        /// <summary>
        ///     Should the Texture be stretched
        /// </summary>
        public Boolean Stretch
        {
            get
            {
                return mStretch;
            }
            set
            {
                mStretch = value;
            }
        }
    }
}
