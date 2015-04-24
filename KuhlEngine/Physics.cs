using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuhlEngine
{
    class Physics
    {

        public static bool testForCollision(Item aActiveItem, Dictionary<string, Item> aAllItems, int aType)
        {
            foreach (KeyValuePair<string, Item> Keypair in aAllItems)
            {
                if (aActiveItem.Uuid != Keypair.Value.Uuid)
                {
                    Item item1 = aActiveItem;
                    Item item2 = Keypair.Value;

                    Boolean xCollision = false;
                    Boolean yCollision = true;

                    // check for x-axis collision
                    if (item2.X + item2.Width <= item1.X || item1.X + item1.Width <= item2.X) xCollision = false;
                    else xCollision = true;

                    // check for y-axis collision
                    if (item1.Y >= item2.Y + item2.Height || item1.Y + item1.Height <= item2.Y) yCollision = false;
                    else yCollision = true;

                    if (xCollision && yCollision)
                    {
                        CollisionEventArgs e = new CollisionEventArgs();
                        e.ActiveItem = item1;
                        e.PassiveItem = item2;
                        e.Type = aType;

                        // fire collision event
                        if (Event.Collision != null) Event.Collision(e);

                        return false;
                    }
                }
            }

            return true;
        }

    }

    public class CollisionType
    {
        private static int mMove = 1;
        private static int mResize = 2;
        private static int mShow = 3;
        private static int mUndefined = 666;

        public static int Move { get { return mMove; } }
        public static int Resize { get { return mResize; } }
        public static int Show { get { return mShow; } }
        public static int Undefined { get { return mUndefined; } }

    }
}
