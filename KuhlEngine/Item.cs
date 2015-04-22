using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuhlEngine
{
    public class Item
    {
        // default settings 
        private int mX = 0;
        private int mY = 0;
        private int mWidth = 32;
        private int mHeight = 32;
        private int mLayer = 0;
        private Texture mTexture = new Texture();
        private bool mVisible = false;
       
        // getter and setter
        public int X { get { return mX; } set { mX = value; } }
        public int Y { get { return mY; } set { mY = value; } }
        public int Width { get { return mWidth; } set { mWidth = value; } }
        public int Height { get { return mHeight; } set { mHeight = value; } }
        public int Layer { get { return mLayer; } set { mLayer = value; } }
        public Texture Texture { get { return mTexture; } set { mTexture = value; } }
        public bool Visible { get { return mVisible; } set { mVisible = value; } }
    }
}
