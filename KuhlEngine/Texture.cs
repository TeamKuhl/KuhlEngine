using System;
using System.Drawing;
using System.IO;

namespace KuhlEngine
{
    public class Texture
    {
        private Image mOriTexture;
        private Image mTexture;
        private Boolean mStretch = true;    //true = Stretch the Image; false = Repeat the Image

        public Image Image { get { return mTexture; } }
        public Boolean Stretch { get { return mStretch; } set { mStretch = value; } }


        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="aPath">The path to the Image</param>
        public Texture(string aPath)
        {
            if (!File.Exists(aPath))
            {
                mOriTexture = new Bitmap(16, 16);
            }
            else
            {
                mOriTexture = Image.FromFile(aPath, true);
            }
        }

        /// <summary>
        /// Just nothing.
        /// </summary>
        public Texture()
        {
            mOriTexture = new Bitmap(16, 16);
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
                    mTexture = new Bitmap(mOriTexture, new Size(aWidth, aHeight));
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
    }
}
