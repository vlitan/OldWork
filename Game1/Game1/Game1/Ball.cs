using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Game1
{
    public class Ball
    {
        public float X_Speed;
        public float Y_Speed;
        public Rectangle Rect, OldRect;
        public float Angle=0.1f;
        public float SpinAngle = 0.1f;
        public bool Way_of_spin;

        public Ball(Point pozition, Point size)
        {
            Random r = new Random();
            Y_Speed = r.Next(-2, 2);
            if (Y_Speed <= 0)
                Y_Speed = -3.5f;
            else
                Y_Speed = 3.5f;
            X_Speed = r.Next(-4, 4);
            X_Speed = (X_Speed == 0) ? 4 : X_Speed;
            Rect = new Rectangle(pozition.X, pozition.Y, size.X, size.Y);
        }

        public void Update(int lim)
        {
            Acc(lim);
            Dec(lim);
            OldRect = Rect;
            Rect.X += (int)X_Speed;
            Rect.Y += (int)Y_Speed;
            Spin();
        }

        public void BoostSpeed(float Booster)
        {
            X_Speed *= Booster;
            Y_Speed *= Booster;
        }

        public void BoostRight()
        {
            X_Speed = Math.Abs(X_Speed);
            BoostSpeed(1.03f);
        }

        public void BoostLeft()
        {
            X_Speed = -Math.Abs(X_Speed);
            BoostSpeed(1.03f);
        }

        private void Acc(int lim)
        {
            if (VectorSpeed(X_Speed, Y_Speed) < lim)
            {
                BoostSpeed(1.003f);
            }
        }

        private void Dec(int lim)
        {
            if (VectorSpeed(X_Speed, Y_Speed) >= lim)
            {
                BoostSpeed(1/(1.3f));
            }

        }

        private float VectorSpeed(float XS, float YS)
        { 
            return (float) Math.Sqrt(XS*XS+YS*YS);
        }

        public void InvertWall()
        {
            X_Speed *= -1;
        }
        public void InvertBar()
        {
            Y_Speed *= -1;
        }

        private void Spin()
        {
            Angle += (Way_of_spin) ? SpinAngle : -SpinAngle;
        }

        public void ReverseSpin()
        {
            Way_of_spin = !Way_of_spin;
        }
    }
}
