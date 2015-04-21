using System;
using System.Threading;
using System.Drawing;
using System.Diagnostics;

namespace KuhlEngine
{
    public class Renderer
    {
        public delegate void RenderHandler(Image aFrame, int aWidth, int aHeight);
        public static RenderHandler newFrame;

        Thread WorkerThread = new Thread(Worker);
        private Map mMap;

        public Boolean initializeMap(int aWidth, int aHeight, Texture aTexture)
        {
            try
            {
                mMap = new Map(aWidth, aHeight, aTexture);
                WorkerThread.Start();
                //if (this.newFrame != null) this.newFrame(aTexture.Image, aTexture.Image.Width, aTexture.Image.Height);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private static void Worker()
        {
            while (true)
            {
                var watch = new Stopwatch();
                watch.Start();
                //Do jobs


                //No more jobs
                //Fire event
                if (newFrame != null) newFrame(null, 0, 0);
                watch.Stop();
                int mSleep = 33 - Convert.ToInt32(watch.ElapsedMilliseconds);
                if (mSleep > 0) Thread.Sleep(mSleep);

            }
        }




    }
}
