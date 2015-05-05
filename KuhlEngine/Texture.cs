using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing.Drawing2D;

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
        /// Constructor with Text
        /// </summary>
        public Texture(String text, Font font, Color textColor)
        {
            //first, create a dummy bitmap just to get a graphics object
            Image img = new Bitmap(1, 1);
            Graphics drawing = Graphics.FromImage(img);
            drawing.InterpolationMode = Renderer.mInterpolationMode;
            drawing.SmoothingMode = Renderer.mSmoothingMode;
            drawing.CompositingQuality = Renderer.mCompositingQuality;
            drawing.PixelOffsetMode = Renderer.mPixelOffsetMode;

            //measure the string to see how big the image needs to be
            SizeF textSize = drawing.MeasureString(text, font);

            //free up the dummy image and old graphics object
            img.Dispose();
            drawing.Dispose();

            //create a new image of the right size
            img = new Bitmap((int)textSize.Width, (int)textSize.Height);

            drawing = Graphics.FromImage(img);

            //paint the background
            drawing.Clear(Color.Transparent);

            //create a brush for the text
            Brush textBrush = new SolidBrush(textColor);

            drawing.DrawString(text, font, textBrush, 0, 0);

            textBrush.Dispose();
            drawing.Dispose();

            mOriTexture = img;
            mTexture = mOriTexture;
        }

        /// <summary>
        /// Constructor with image as path for texture
        /// </summary>
        /// <param name="aPath">path to the image</param>
        public Texture(string aPath)
        {
            if (!File.Exists(aPath))
            {
                mOriTexture = new Bitmap(16, 16);

                // create graphic to draw
                Graphics g = Graphics.FromImage(mOriTexture);
                g.InterpolationMode = Renderer.mInterpolationMode;
                g.SmoothingMode = Renderer.mSmoothingMode;
                g.CompositingQuality = Renderer.mCompositingQuality;
                g.PixelOffsetMode = Renderer.mPixelOffsetMode;

                // load color
                Color clr = Color.FromArgb(255, 255, 0, 220);

                // create brush
                Brush brush = new System.Drawing.SolidBrush(clr);

                // draw black image
                g.Clear(Color.Black);

                // draw brush on texture
                g.FillRectangle(brush, 0, 0, 8, 8);
                g.FillRectangle(brush, 8, 8, 16, 16);

                g.Dispose();
                // set texture
                mTexture = mOriTexture;
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
            g.InterpolationMode = Renderer.mInterpolationMode;
            g.SmoothingMode = Renderer.mSmoothingMode;
            g.CompositingQuality = Renderer.mCompositingQuality;
            g.PixelOffsetMode = Renderer.mPixelOffsetMode;

            // load color
            Color clr = Color.FromArgb(aT, aR, aG, aB);

            // create brush
            Brush brush = new System.Drawing.SolidBrush(clr);

            // draw brush on texture
            g.FillRectangle(brush, 0, 0, 16, 16);

            g.Dispose();

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
        /// Change the opacity of the Texture
        /// </summary>
        /// <param name="aOpacity"></param>
        /// <returns></returns>
        public Boolean SetOpacity(float aOpacity)
        {
            try
            {
                //create a Bitmap the size of the image provided  
                Bitmap bmp = new Bitmap(mTexture.Width, mTexture.Height);

                //create a graphics object from the image  
                Graphics gfx = Graphics.FromImage(bmp);
                gfx.InterpolationMode = Renderer.mInterpolationMode;
                gfx.SmoothingMode = Renderer.mSmoothingMode;
                gfx.CompositingQuality = Renderer.mCompositingQuality;
                gfx.PixelOffsetMode = Renderer.mPixelOffsetMode;


                //create a color matrix object  
                ColorMatrix matrix = new ColorMatrix();

                //set the opacity 
                matrix.Matrix33 = aOpacity;

                //create image attributes  
                ImageAttributes attributes = new ImageAttributes();

                //set the color(opacity) of the image  
                attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                //now draw the image  
                gfx.DrawImage(mTexture, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, mTexture.Width, mTexture.Height, GraphicsUnit.Pixel, attributes);

                gfx.Dispose();

                mTexture = bmp;
                return true;
            }
            catch
            {
                return false;
            }
        }

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
                    g.InterpolationMode = Renderer.mInterpolationMode;
                    g.SmoothingMode = Renderer.mSmoothingMode;
                    g.CompositingQuality = Renderer.mCompositingQuality;
                    g.PixelOffsetMode = Renderer.mPixelOffsetMode;

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
