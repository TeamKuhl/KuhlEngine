using System;
using System.Threading;
using System.Drawing;
using System.Diagnostics;
using System.Collections.Generic;

namespace KuhlEngine
{
    /// <summary>
    /// Main engine class, runs main process, manages items, renders frames
    /// </summary>
    public class Renderer
    {
        #region Declarations

        // itemcontainer
        private Dictionary<string, Item> mItems = new Dictionary<string, Item>();
        private readonly object mSyncLock = new object();

        #endregion

        #region Settings

        // settings & default values
        private int mFPS = 30;
        private int mWidth = 300;
        private int mHeight = 300;
        private Texture mBackground = new Texture();

        /// <summary>
        /// Maximum Frames Per Second: If there is time left, the engine will wait the remaining 1/FPS seconds until next frame
        /// </summary>
        public int FPS { get { return mFPS; } set { if (value > 0) mFPS = value; } }

        /// <summary>
        /// Width of the rendered frames
        /// </summary>
        public int Width { get { return mWidth; } set { mWidth = value; } }

        /// <summary>
        /// Height of the rendered frames
        /// </summary>
        public int Height { get { return mHeight; } set { mHeight = value; } }

        /// <summary>
        /// A texture object for the background, will be empty/transparent if not set
        /// </summary>
        public Texture Background { get { return mBackground; } set { mBackground = value; } }

        #endregion

        #region Start & Run

        /// <summary>
        /// Starts the engine
        /// </summary>
        public void Start()
        {
            // create new thread and start it
            Thread WorkerThread = new Thread(Worker);
            WorkerThread.Start();
        }

        /// <summary>
        /// Engine thread worker: Contains the main rendering loop, fires the rendering event and waits to match FPS
        /// </summary>
        private void Worker()
        {
            // main rendering loop
            while (true)
            {
                // create stopwatch to measure time needed for rendering
                var watch = new Stopwatch();
                watch.Start();

                // move item dictionary to a temporary dictionary to avoid problems with changes while rendering
                //Dictionary<string, Item> tempItems = new Dictionary<string, Item>();
                //foreach (KeyValuePair<string, Item> keyPair in mItems)
                //{
                //    tempItems[keyPair.Key] = keyPair.Value;
                //}

                Dictionary<string, Item> tempItems = copyDict(mItems);

                // create frame (main rendering)
                Frame frame = new Frame(mWidth, mHeight, mBackground, tempItems);

                // fire event
                if (Event.NewFrame != null) Event.NewFrame(frame.Image);

                // take time and wait for next frame
                watch.Stop();
                int mSleep = 1000 / mFPS - Convert.ToInt32(watch.ElapsedMilliseconds);
                if (mSleep > 0) Thread.Sleep(mSleep);

            }
        }

        private Dictionary<string, Item> copyDict(Dictionary<string, Item> source)
        {

            lock (mSyncLock)
            {
                Dictionary<string, Item> target = new Dictionary<string, Item>();
                foreach (KeyValuePair<string, Item> keyPair in source)
                {
                    target[keyPair.Key] = keyPair.Value;
                }
                return target;
            }

        }

        #endregion

        #region Items

        /// <summary>
        /// Creates a default item and return its uuid
        /// </summary>
        /// <returns>Item uuid</returns>
        public Item CreateItem()
        {
            lock (mSyncLock)
            {
                // generate uuid
                string uuid;
                do
                {
                    uuid = Guid.NewGuid().ToString();
                } while (mItems.ContainsKey(uuid));

                // create item
                mItems.Add(uuid, new Item(uuid));

                return GetItem(uuid);
            }
        }

        /// <summary>
        /// Deletes an item
        /// </summary>
        /// <param name="aUuid"></param>
        /// <returns></returns>
        public bool DeleteItem(string aUuid)
        {
            // find item
            if (mItems.ContainsKey(aUuid))
            {
                mItems.Remove(aUuid);

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get item by uuid
        /// </summary>
        /// <param name="aUuid">Item uuid</param>
        /// <returns>Item object</returns>
        public Item GetItem(string aUuid)
        {
            // find item
            if (mItems.ContainsKey(aUuid))
            {
                return mItems[aUuid].getCopy();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Set an item
        /// </summary>
        /// <param name="aUuid">Item uuid</param>
        /// <param name="aItem">Item object</param>
        public bool SetItem(Item aItem)
        {
            lock (mSyncLock)
            {
                // find item
                if (mItems.ContainsKey(aItem.Uuid))
                {
                    // resize texture and save
                    if (checkCollisions(aItem, CollisionType.Undefined))
                    {
                        aItem.resizeTexture();
                        mItems[aItem.Uuid] = aItem;
                        return true;
                    }
                    else return false;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Set the position of an item
        /// </summary>
        /// <param name="aUuid">Item uuid</param>
        /// <param name="aX">X coordinate</param>
        /// <param name="aY">Y coordinate</param>
        public bool SetItemPosition(string aUuid, int aX, int aY)
        {
            // find item
            if (mItems.ContainsKey(aUuid))
            {
                Item item = mItems[aUuid].getCopy();
                item.X = aX;
                item.Y = aY;
                if (checkCollisions(item, CollisionType.Move))
                {
                    // set position
                    mItems[aUuid].X = aX;
                    mItems[aUuid].Y = aY;
                    return true;
                }
                else return false;
            }
            else
            {
                return false;
            }
        }

        public bool MoveItem(string aUuid, int aDirection, int aLength)
        {
            // find item
            if (mItems.ContainsKey(aUuid))
            {
                Item item = mItems[aUuid].getCopy();
                item.X = (int) (aLength * Math.Cos(aDirection));
                item.Y = (int)(aLength * Math.Sin(aDirection));
                if (checkCollisions(item, CollisionType.Move))
                {
                    // set position
                    mItems[aUuid].X = (int)(aLength * Math.Cos(aDirection));
                    mItems[aUuid].Y = (int)(aLength * Math.Sin(aDirection));
                    return true;
                }
                else return false;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Set the texture of an item
        /// </summary>
        /// <param name="aUuid">Item uuid</param>
        /// <param name="aTexture">Texture object</param>
        public bool SetItemTexture(string aUuid, Texture aTexture)
        {
            if (mItems.ContainsKey(aUuid))
            {
                mItems[aUuid].Texture = aTexture;
                mItems[aUuid].resizeTexture();
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
        /// <param name="aUuid">Item uuid</param>
        /// <param name="aWidth">Item width</param>
        /// <param name="aHeight">Item height</param>
        public bool SetItemSize(string aUuid, int aWidth, int aHeight)
        {
            // find item
            if (mItems.ContainsKey(aUuid))
            {
                Item item = mItems[aUuid];
                item.Width = aWidth;
                item.Height = aHeight;

                if (checkCollisions(item, CollisionType.Resize))
                {
                    // set size
                    mItems[aUuid].Width = aWidth;
                    mItems[aUuid].Height = aHeight;

                    // resize texture to fit size
                    mItems[aUuid].resizeTexture();

                    return true;
                }
                else return false;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Set the enabled of an item
        /// </summary>
        /// <param name="aUuid">Item uuid</param>
        /// <param name="aVisibility">Item enalbed</param>
        public bool SetItemEnabled(string aUuid, bool aEnabled)
        {
            // find item
            if (mItems.ContainsKey(aUuid))
            {
                Item item = mItems[aUuid];
                item.Enabled = aEnabled;

                if (!aEnabled || checkCollisions(item, CollisionType.Show))
                {
                    // set visibility
                    mItems[aUuid].Enabled = aEnabled;
                    return true;
                }
                else return false;

            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Set the layer of an item
        /// </summary>
        /// <param name="aUuid">Item uuid</param>
        /// <param name="aLayer">Item layer number</param>
        /// <returns></returns>
        public bool SetItemLayer(string aUuid, int aLayer)
        {
            // find item
            if (mItems.ContainsKey(aUuid))
            {
                // set layer
                mItems[aUuid].Layer = aLayer;
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Collisions

        private bool checkCollisions(Item aItem, int aType)
        {
            // create dictionary with collision items
            Dictionary<string, Item> collisionItems = new Dictionary<string, Item>();

            // fill dictionary
            foreach (KeyValuePair<string, Item> Keypair in copyDict(mItems))
            {
                if (Keypair.Value.CheckCollision && Keypair.Value.Enabled)
                {
                    collisionItems[Keypair.Key] = Keypair.Value;
                }
            }

            return Physics.testForCollision(aItem, collisionItems, aType);
        }

        #endregion

    }
}
