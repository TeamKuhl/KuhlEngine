using System;
using System.Drawing;
using System.IO;

namespace KuhlEngine
{
    /// <summary>
    /// Creates, saves and changes texture images or colors
    /// </summary>
    public class Texture
    {

        #region Declarations

        // save current texture image and original for later resizing
        private Image mOriTexture;
        private Image mTexture;

        // stretch or repeat image to fill size?
        //true = Stretch the Image; false = Repeat the Image
        private Boolean mStretch = true;    


        // propertys, image is read only
        public Image Image { get { return mTexture; } }
        public Boolean Stretch { get { return mStretch; } set { mStretch = value; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor with image as path for texture
        /// </summary>
        /// <param name="aPath">path to the image</param>
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
        /// Constructor with Image object as texture
        /// </summary>
        /// <param name="aImage">Image object</param>
        public Texture(Image aImage)
        {
            mOriTexture = aImage;
            mTexture = mOriTexture;
        }

        /// <summary>
        /// Constructor with RGB colors as texture
        /// </summary>
        /// <param name="aR">Red</param>
        /// <param name="aG">Green</param>
        /// <param name="aB">Blue</param>
        public Texture(int aR, int aG, int aB)
        {
            // call color constructor
            ColorConstructor(aR, aG, aB, 255);
        }

        /// <summary>
        /// Constructor with RGB colors and alpha channel as texture
        /// </summary>
        /// <param name="aR">Red</param>
        /// <param name="aG">Green</param>
        /// <param name="aB">Blue</param>
        /// <param name="aT">Transparency (alpha)</param>
        public Texture(int aR, int aG, int aB, int aT)
        {
            // call color constructor
            ColorConstructor(aR, aG, aB, aT);
        }

        /// <summary>
        /// Constructor helper with RGB colors and alpha channel
        /// </summary>
        /// <param name="aR">Red</param>
        /// <param name="aG">Green</param>
        /// <param name="aB">Blue</param>
        /// <param name="aT">Transparency (alpha)</param>
        private void ColorConstructor(int aR, int aG, int aB, int aT)
        {
            // create bitmap
            mOriTexture = new Bitmap(16, 16);

            // create graphic to draw
            Graphics g = Graphics.FromImage(mOriTexture);

            // load color
            Color clr = Color.FromArgb(aT, aR, aG, aB);

            // create brush
            Brush brush = new System.Drawing.SolidBrush(clr);

            // draw brush on texture
            g.FillRectangle(brush, 0, 0, 16, 16);

            // set texture
            mTexture = mOriTexture;
        }

        /// <summary>
        /// Constructor with empty texture
        /// </summary>
        public Texture()
        {
            // create empty bitmap
            mOriTexture = new Bitmap(16, 16);
            mTexture = mOriTexture;
        }

        #endregion

        #region Edit

        /// <summary>
        /// Flip the image to the x-axis
        /// </summary>
        /// <returns></returns>
        public Boolean FlipX()
        {
            try
            {
                Image tempTexture = mOriTexture;
                tempTexture.RotateFlip(RotateFlipType.RotateNoneFlipX);
                mTexture = tempTexture;
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
        public Boolean FlipY()
        {
            try
            {
                Image tempTexture = mOriTexture;
                tempTexture.RotateFlip(RotateFlipType.RotateNoneFlipY);
                mTexture = tempTexture;
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        ///  Resize an Image to a specified size
        /// </summary>
        /// <param name="aWidth">new width</param>
        /// <param name="aHeight">new height</param>
        /// <returns>success</returns>
        internal Boolean Resize(int aWidth, int aHeight)
        {
            try
            {
                Image tempTexture;

                if (mStretch)
                {
                    // stretch Image
                    tempTexture = new Bitmap(mOriTexture, new Size(aWidth, aHeight));
                    mTexture = tempTexture;
                }
                else
                {
                    // calculate needed x repetions
                    int widthAmount = aWidth / mOriTexture.Width;
                    if ((aWidth % mOriTexture.Width) != 0) widthAmount++;

                    // calculate needed y repetitions
                    int heightAmount = aHeight / mOriTexture.Height;
                    if ((aHeight % mOriTexture.Height) != 0) heightAmount++;

                    // create new texture and graphic
                    Bitmap Texture = new Bitmap(aWidth, aHeight);
                    Graphics g = Graphics.FromImage(Texture);

                    // draw repetitions on image
                    for (int yCount = 0; yCount < heightAmount; yCount++)
                    {
                        for (int xCount = 0; xCount < widthAmount; xCount++)
                        {
                            g.DrawImage(mOriTexture, new Point(mOriTexture.Width * xCount, mOriTexture.Height * yCount));
                        }
                    }

                    // save & destroy
                    mTexture = Texture;
                    g.Dispose();
                }

                // success
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

    }
}
