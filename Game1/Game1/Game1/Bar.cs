using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Game1
{
    class Bar
    {
        public bool Left;
        public bool Right;
        private float Speed_Add = 13;
        public Rectangle Rect;

       public Bar(Point pozition, Point size)
        {
            Random r = new Random();
            Rect = new Rectangle(pozition.X, pozition.Y, size.X, size.Y);
            Stop();
        }

        public void Update()
        {
            if (Right)
                Rect.X += (int)Speed_Add;
            else if(Left)
                Rect.X -= (int)Speed_Add;
        }

        public void Stop()
        {
            Left = false;
            Right = false;
        }

        public void ToLeft()
        {
                Left = true;
                Right = false;
        }

        public void ToRight()
        {
                Left = false;
                Right = true;
         
        }

    }
}
