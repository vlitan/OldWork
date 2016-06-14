using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Reflection;

namespace Game1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Texture2D T_Ball;
        private Texture2D T_Bar;
        private Texture2D T_BackGround;
        private List<Color> ColorList;
        private Ball Ball1;
        private Bar Bar1;
        private Bar Bar2;
        private int R = 1;
        private int w = 250, h = 50,r,rr;
        private int Score1=0, Score2 = 0;
        private SpriteFont Font;
        private Rectangle Score1_Rect, Score2_Rect;
        private List<Ball> BallList;
        private int SpeedLim = 10;
        private int k = 0;// nr de pase per tura

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            ColorList = new List<Color>();
            ColorList = ColorStructToList();
            BallList = new List<Ball>();
            

            Ball1 = new Ball(new Point(this.Window.ClientBounds.Width / 2, this.Window.ClientBounds.Height / 2), new Point(r,rr));
            
            Bar1 = new Bar(new Point(this.Window.ClientBounds.Width / 2-w/2, this.Window.ClientBounds.Height / 10-h), new Point(w, h));
            Bar2 = new Bar(new Point(this.Window.ClientBounds.Width / 2-w/2, this.Window.ClientBounds.Height*9 / 10), new Point(w, h));
            Score1_Rect = new Rectangle(20,this.Window.ClientBounds.Height * 3 / 8, 70, 30);
            Score2_Rect = new Rectangle(20,this.Window.ClientBounds.Height *5/8, 70, 30);    
        }

        public static List<Color> ColorStructToList()
        {
            return typeof(Color).GetProperties(BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public)
                                .Select(c => (Color)c.GetValue(null, null))
                                .ToList();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            T_Ball = Content.Load<Texture2D>("Ball");
            T_Bar = Content.Load<Texture2D>("Bar");
            T_BackGround = Content.Load<Texture2D>("Back");
            Font = Content.Load<SpriteFont>("Score");

            r=T_Ball.Height/10;
            rr = T_Ball.Width / 10;
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            
            //input

            KeyboardState state = Keyboard.GetState();
             MoveBars(state);

            //colision
            BallsColide(ref Ball1);

             Bar1.Update();
             Bar2.Update();
            if((k>SpeedLim*2)&&(SpeedLim<=15))
                SpeedLim++;
            Ball1.Update(SpeedLim);  

            base.Update(gameTime);
        }

        public void BallsColide(ref Ball Ball1)
        {
            Random rand = new Random();

            if ((Ball1.Rect.Intersects(Bar1.Rect)) || (Ball1.Rect.Intersects(Bar2.Rect)))
            {
                k++;
                Ball1.ReverseSpin();
                if ((Bar1.Left) || (Bar2.Left))
                    Ball1.BoostLeft();

                if ((Bar1.Right) || (Bar2.Right))
                    Ball1.BoostRight();

                Ball1.InvertBar();
            }
            if ((Ball1.Rect.X <= 1) || (Ball1.Rect.X >= this.Window.ClientBounds.Width - 1 - r))
            {
                Ball1.InvertWall();
                Ball1.ReverseSpin();
            }
            if (Ball1.Rect.Y <= 1)
            {
                Score2++;
                R = rand.Next(ColorList.Count - 1);
                k = 0;
                Ball1 = new Ball(new Point(this.Window.ClientBounds.Width / 2, this.Window.ClientBounds.Height / 2), new Point(r, r));
            }
            if (Ball1.Rect.Y >= this.Window.ClientBounds.Height - 1)
            {
                Score1++;
                R = rand.Next(ColorList.Count - 1);
                k = 0;
                Ball1 = new Ball(new Point(this.Window.ClientBounds.Width / 2, this.Window.ClientBounds.Height / 2), new Point(r, r));
            }
        }

        public void MoveBars(KeyboardState state)
        {
            if (state.IsKeyDown(Keys.Left))
            {
                Bar1.ToLeft();
            }
            else if (state.IsKeyDown(Keys.Right))
            {
                Bar1.ToRight();
            }
            else
                Bar1.Stop();

            if (state.IsKeyDown(Keys.A))
            {
                Bar2.ToLeft();
            }
            else if (state.IsKeyDown(Keys.D))
            {
                Bar2.ToRight();
            }
            else
                Bar2.Stop();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //to do: spining
            /*Vector2 origin = new Vector2(Ball1.Rect.Width / 2, Ball1.Rect.Height);
            Vector2 location = new Vector2(Ball1.Rect.X, Ball1.Rect.Y);
            Rectangle SourceRectangle = new Rectangle(0, 0, T_Ball.Width, T_Ball.Height);*/
            Vector2 Score1_Vector= new Vector2(Score1_Rect.X, Score1_Rect.Y);
            Vector2 Score2_Vector= new Vector2(Score2_Rect.X, Score2_Rect.Y);

            spriteBatch.Begin();

            
            spriteBatch.Draw(T_BackGround, new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height), ColorList[R]);
            spriteBatch.Draw(T_Bar, Bar1.Rect, Color.White);
            spriteBatch.Draw(T_Bar, Bar2.Rect, Color.White);

            spriteBatch.DrawString(Font, "" + Score1, Score1_Vector, Color.White);
            spriteBatch.DrawString(Font, "" + Score2, Score2_Vector, Color.White);
            //spriteBatch.Draw(T_Ball,location, SourceRectangle, Color.White, Ball1.Angle, origin, 0.1f, SpriteEffects.None, 1);
            spriteBatch.Draw(T_Ball, Ball1.Rect, Color.White);
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
