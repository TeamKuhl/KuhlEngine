using System;
using System.Threading;
using System.Drawing;
using System.Diagnostics;
using System.Collections.Generic;

namespace KuhlEngine
{
    public class Renderer
    {
        #region Declarations

        // Renderevent
        public delegate void RenderHandler(Image aFrame);
        public static RenderHandler newFrame;

        // itemcontainer
        private Dictionary<string, Item> mItems = new Dictionary<string, Item>();

        #endregion

        #region Settings

        // settings
        private int mFPS = 30;
        private int mWidth = 300;
        private int mHeight = 300;
        private Texture mBackground = new Texture();

        // settings setter
        public int FPS { get { return mFPS; } set { mFPS = value; } }
        public int Width { get { return mWidth; } set { mWidth = value; } }
        public int Height { get { return mHeight; } set { mHeight = value; } }
        public Texture Background { get { return mBackground; } set { mBackground = value; } }

        #endregion

        #region Start & Run

        /// <summary>
        /// Start the engine
        /// </summary>
        /// <returns></returns>
        public Boolean Start()
        {
            Thread WorkerThread = new Thread(Worker);
            WorkerThread.Start();

            return true;
        }

        /// <summary>
        /// Engine thread worker
        /// </summary>
        private void Worker()
        {
            while (true)
            {
                var watch = new Stopwatch();
                watch.Start();
                //Do jobs
                Dictionary<string, Item> tempItems = new Dictionary<string, Item>();
                foreach (KeyValuePair<string, Item> keyPair in mItems)
                {
                    tempItems[keyPair.Key] = keyPair.Value;
                }


                Frame frame = new Frame(mWidth, mHeight, mBackground, tempItems);

                //No more jobs
                //Fire event (kill forms)
                if (newFrame != null) newFrame(frame.Image);

                // take time and wait for next frame
                watch.Stop();
                int mSleep = 1000 / mFPS - Convert.ToInt32(watch.ElapsedMilliseconds);
                if (mSleep > 0) Thread.Sleep(mSleep);

            }
        }

        #endregion

        #region Items

        /// <summary>
        /// Creates a default item and return its uuid
        /// </summary>
        /// <returns>Item uuid</returns>
        public string CreateItem()
        {
            // generate uuid
            string uuid;
            do
            {
                uuid = Guid.NewGuid().ToString();
            } while (mItems.ContainsKey(uuid));

            // create item
            mItems.Add(uuid, new Item());

            return uuid;
        }

        /// <summary>
        /// Get item by uuid
        /// </summary>
        /// <param name="aUuid"></param>
        /// <returns></returns>
        public Item GetItem(string aUuid)
        {
            if (mItems.ContainsKey(aUuid))
            {
                return mItems[aUuid];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Set an item
        /// </summary>
        /// <param name="aUuid"></param>
        /// <param name="aItem"></param>
        /// <returns></returns>
        public bool SetItem(string aUuid, Item aItem)
        {
            if (mItems.ContainsKey(aUuid))
            {
                mItems[aUuid] = aItem;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Set the position of an item
        /// </summary>
        /// <param name="aUuid"></param>
        /// <param name="aX"></param>
        /// <param name="aY"></param>
        /// <returns></returns>
        public bool SetItemPosition(string aUuid, int aX, int aY)
        {
            if (mItems.ContainsKey(aUuid))
            {
                mItems[aUuid].X = aX;
                mItems[aUuid].Y = aY;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Set the texture of an item
        /// </summary>
        /// <param name="aUuid"></param>
        /// <param name="aTexture"></param>
        /// <returns></returns>
        public bool SetItemTexture(string aUuid, Texture aTexture)
        {
            if (mItems.ContainsKey(aUuid))
            {
                mItems[aUuid].Texture = aTexture;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Set the size of an item
        /// </summary>
        /// <param name="aUuid"></param>
        /// <param name="aWidth"></param>
        /// <param name="aHeight"></param>
        /// <returns></returns>
        public bool SetItemSize(string aUuid, int aWidth, int aHeight)
        {
            if (mItems.ContainsKey(aUuid))
            {
                mItems[aUuid].Width = aWidth;
                mItems[aUuid].Height = aHeight;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Set the visibility of an item
        /// </summary>
        /// <param name="aUuid"></param>
        /// <param name="aVisibility"></param>
        /// <returns></returns>
        public bool SetItemVisibility(string aUuid, bool aVisibility)
        {
            if (mItems.ContainsKey(aUuid))
            {
                mItems[aUuid].Visible = aVisibility;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Set the layer of an item
        /// </summary>
        /// <param name="aUuid"></param>
        /// <param name="aLayer"></param>
        /// <returns></returns>
        public bool SetItemLayer(string aUuid, int aLayer)
        {
            if (mItems.ContainsKey(aUuid))
            {
                mItems[aUuid].Layer = aLayer;
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

    }
}
