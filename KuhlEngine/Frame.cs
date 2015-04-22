using System.Collections.Generic;
using System.Linq;
using System.Drawing;

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
        public Frame(int aWidth, int aHeight, Texture aBackground, Dictionary<string, Item> aItems)
        {
            // draw background
            aBackground.Resize(aWidth, aHeight);
            mFrame = aBackground.Image;

            // get graphic from image
            Graphics drawGraphic = Graphics.FromImage(mFrame);

            // sort layers
            var sortedItems = from pair in aItems orderby pair.Value.Layer ascending select pair;

            // draw items
            foreach (KeyValuePair<string, Item> Keypair in sortedItems)
            {
                // get texture
                Texture texture = Keypair.Value.Texture;

                // draw texture on frame graphic
                drawGraphic.DrawImage(texture.Image, new Point(Keypair.Value.X, Keypair.Value.Y));
            }

            // dispose graphic
            drawGraphic.Dispose();
        }

        #endregion

    }


}
