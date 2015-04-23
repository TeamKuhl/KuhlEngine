
namespace KuhlEngine
{
    /// <summary>
    /// Item object to save all propertys needed for the rendered items
    /// </summary>
    public class Item
    {

        #region Settings

        // position
        private int mX = 0;
        private int mY = 0;

        // size
        private int mWidth = 32;
        private int mHeight = 32;

        // layer 
        private int mLayer = 0;

        // texture
        private Texture mTexture = new Texture();

        // visibility
        private bool mVisible = false;

        // collision
        private bool mCheckCollision = true;

        #endregion

        #region Getter & Setter

        /// <summary>
        /// Horizontal item position, X coordinate 
        /// </summary>
        public int X { get { return mX; } set { mX = value; } }

        /// <summary>
        /// Vertical item position, Y coordinate
        /// </summary>
        public int Y { get { return mY; } set { mY = value; } }

        /// <summary>
        /// Item width, texture will be resized to this width
        /// </summary>
        public int Width { get { return mWidth; } set { mWidth = value; } }

        /// <summary>
        /// Item height, texture will be resized to this height
        /// </summary>
        public int Height { get { return mHeight; } set { mHeight = value; } }

        /// <summary>
        /// Relative position, higher layer will be drawed above lower layer
        /// </summary>
        public int Layer { get { return mLayer; } set { mLayer = value; } }

        /// <summary>
        /// Item texture, will be resized to width and height of item
        /// </summary>
        public Texture Texture { get { return mTexture; } set { mTexture = value; } }

        /// <summary>
        /// Item visibility, by default set to invisible to change item settings before starting to draw this item.
        /// You have to set this to true for every item.
        /// </summary>
        public bool Visible { get { return mVisible; } set { mVisible = value; } }

        /// <summary>
        /// If true the engine will check for collisions
        /// </summary>
        public bool CheckCollision { get { return mCheckCollision; } set { mCheckCollision = value; } }
        #endregion

        #region Functions

        /// <summary>
        /// Resize the texture to image size, must be executed after every change of Texture, Width or Height.
        /// </summary>
        public void resizeTexture()
        {
            mTexture.Resize(mWidth, mHeight);
        }

        #endregion

    }
}
