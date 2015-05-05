using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace KuhlEngine
{
    /// <summary>
    /// Renders one frame with the given background and the given items
    /// </summary>
    internal class Frame
    {

        #region Declarations

        // final drawing image, only readable
        private Image mFrame;

        private readonly object mSyncLock = new object();

        public Image Image { get { return mFrame; } }

        #endregion

        #region Constructor & Drawer

        /// <summary>
        /// Frame constructor, renders frame immediately, you can get it with Frame.Image
        /// </summary>
        /// <param name="aWidth">Width of the frame</param>
        /// <param name="aHeight">Height of the frame</param>
        /// <param name="aBackground">Background texture</param>
        /// <param name="aItems">Dictionary with items to draw. Note: this dictionary should be a temporary copy of the original dictionary.</param>
        public Frame(int aWidth, int aHeight, Texture aBackground, Item[] aItems, int aP1x, int aP1y, int aP2x, int aP2y)
        {
            // draw background
            mFrame = new Bitmap(aBackground.Image);

            // get graphic from image
            Graphics drawGraphic = Graphics.FromImage(mFrame);
            drawGraphic.InterpolationMode = Renderer.mInterpolationMode;
            drawGraphic.SmoothingMode = Renderer.mSmoothingMode;
            drawGraphic.CompositingQuality = Renderer.mCompositingQuality;
            drawGraphic.PixelOffsetMode = Renderer.mPixelOffsetMode;

            Parallel.For(0, aItems.Count(), i =>
            {
                lock (mSyncLock)
                {
                    if (aItems[i].Enabled)
                    {
                        // get texture
                        Texture texture = aItems[i].Texture;

                        try
                        {
                            // draw full texture on frame graphic
                            drawGraphic.DrawImage(texture.Image, new Point(aItems[i].X, aItems[i].Y));
                        }
                        catch { }
                        texture = null;
                    }
                }

            });

            drawGraphic.Dispose();

            //Camera
            Image resizedFrame = new Bitmap(aP2x - aP1x, aP2y - aP1y);
            Graphics resizeGraphic = Graphics.FromImage(resizedFrame);
            resizeGraphic.InterpolationMode = Renderer.mInterpolationMode;
            resizeGraphic.SmoothingMode = Renderer.mSmoothingMode;
            resizeGraphic.CompositingQuality = Renderer.mCompositingQuality;
            resizeGraphic.PixelOffsetMode = Renderer.mPixelOffsetMode;
            resizeGraphic.DrawImage(mFrame, new Point(aP1x * (-1), aP1x * (-1)));

            mFrame = new Bitmap(resizedFrame, new Size(aWidth, aHeight));

            resizedFrame = null;
            resizeGraphic.Dispose();
        }

        #endregion

    }


}
