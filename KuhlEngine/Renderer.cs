using System;
using System.Threading;
using System.Drawing;
using System.Diagnostics;

namespace KuhlEngine
{
    public class Renderer
    {
        // Renderevent
        public delegate void RenderHandler(Image aFrame);
        public static RenderHandler newFrame;

        // settings
        private int mFPS = 30;
        private int mWidth = 300;
        private int mHeight = 300;
        private Texture mBackground = new Texture();

        public int FPS { get { return mFPS; } set { mFPS = value; } }
        public int Width { get { return mWidth; } set { mWidth = value; } }
        public int Height { get { return mHeight; } set { mHeight = value; } }
        public Texture Background { get { return mBackground; } set { mBackground = value; } }

        public Boolean Start()
        {
            Thread WorkerThread = new Thread(Worker);
            WorkerThread.Start();

            return true;
        }

        private void Worker()
        {
            while (true)
            {
                var watch = new Stopwatch();
                watch.Start();
                //Do jobs

                Frame frame = new Frame(mWidth, mHeight, mBackground);


                //No more jobs
                //Fire event
                if (newFrame != null) newFrame(frame.Image);
                watch.Stop();
                int mSleep = 1000 / mFPS - Convert.ToInt32(watch.ElapsedMilliseconds);
                if (mSleep > 0) Thread.Sleep(mSleep);

            }
        }
    }
}
