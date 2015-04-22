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
            mTexture = mOriTexture;
        }

        /// <summary>
        /// Image constructor
        /// </summary>
        /// <param name="aImage"></param>
        public Texture(Image aImage)
        {
            mOriTexture = aImage;
            mTexture = mOriTexture;
        }

        /// <summary>
        /// Color constructor
        /// </summary>
        /// <param name="aImage"></param>
        public Texture(int aR, int aG, int aB)
        {
            mOriTexture = new Bitmap(16, 16);
            Graphics g = Graphics.FromImage(mOriTexture);
            Color clr = Color.FromArgb(aR, aG, aB);
            Brush brush = new System.Drawing.SolidBrush(clr);
            g.FillRectangle(brush, 0, 0, 20, 20);
            mTexture = mOriTexture;
        }

        /// <summary>
        /// Just nothing.
        /// </summary>
        public Texture()
        {
            mOriTexture = new Bitmap(16, 16);
            mTexture = mOriTexture;
        }

        /// <summary>
        /// Flip the image to the x-axis
        /// </summary>
        /// <returns></returns>
        internal Boolean FlipX()
        {
            try
            {
                mTexture = mOriTexture;
                mTexture.RotateFlip(RotateFlipType.RotateNoneFlipX);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Flip the image to the y-axis
        /// </summary>
        /// <returns></returns>
        internal Boolean FlipY()
        {
            try
            {
                mTexture = mOriTexture;
                mTexture.RotateFlip(RotateFlipType.RotateNoneFlipY);
                return true;
            }
            catch
            {
                return false;
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
                    mTexture = new Bitmap(mOriTexture, new Size(aWidth, aHeight));
                }
                else
                {
                    //Set image
                    mTexture = mOriTexture;
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
