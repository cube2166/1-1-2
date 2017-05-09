using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace _2048Demo
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        List<Texture2D> TextureList;

        public const int window_Height = 800;
        public const int window_Width = 800;
        Board myboard;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = window_Width;
            graphics.PreferredBackBufferHeight = window_Height;
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
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            TextureList = new List<Texture2D>();

            for (int ii = 0; ii < 2; ii++)
            {
                Texture2D temp = Content.Load<Texture2D>(string.Format("bg{0}", ii + 1));
                TextureList.Add(temp);
                temp = Content.Load<Texture2D>(string.Format("block_{0}", ii+1));
                TextureList.Add(temp);
            }

            int baseValue = 0;
            while (true)
            {
                Texture2D temp = Content.Load<Texture2D>(string.Format("block_{0}", baseValue));
                TextureList.Add(temp);
                if (baseValue == 0) baseValue = 3;
                else baseValue *= 2;
                
                if (baseValue > 3072) break;
            }
            myboard = new Board(TextureList, 4);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            myboard.Update(elapsedTime);

            //if (gameOver == false)
            //{
            //    totalTime += elapsedTime;
            //    aliveTime += elapsedTime;
            //    if (totalTime > totalLaunchpad * 5)
            //    {
            //        Launchpad temp = new Launchpad(texture_blank, circle_center, circle_radius, 10, prepareShow, rand.Next());
            //        temp.showHandler += prepareShow;
            //        ObjectCollect.Add(temp);
            //        totalLaunchpad++;
            //    }
            //    for (int ii = 0; ii < ObjectCollect.Count; ii++)
            //    {
            //        ObjectCollect[ii].Update(elapsedTime);
            //    }
            //    if (player.OnCheck() == true)
            //    {
            //        gameOver = true;
            //    }
            //}

            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.A) || keyboard.IsKeyDown(Keys.Left))
            {
                myboard.Move(KeyArrow.LEFT);
            }
            else if (keyboard.IsKeyDown(Keys.D) || keyboard.IsKeyDown(Keys.Right))
            {
                myboard.Move(KeyArrow.RIGHT);
            }
            if (keyboard.IsKeyDown(Keys.W) || keyboard.IsKeyDown(Keys.Up))
            {
                myboard.Move(KeyArrow.UP);
            }
            else if (keyboard.IsKeyDown(Keys.S) || keyboard.IsKeyDown(Keys.Down))
            {
                myboard.Move(KeyArrow.DOWN);
            }

            //if (keyboard.IsKeyDown(Keys.R) && gameOver)
            //{
            //    gameOver = false;
            //    StartGame();
            //}

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            myboard.Show(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
