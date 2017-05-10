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
        private GraphicsDeviceManager _c_Graphics;
        private SpriteBatch _c_SpriteBatch;
        private List<Texture2D> _l_TextureList;

        public const int p_WindowHeight = 800;
        public const int p_WindowWidth = 800;
        private Board _c_MyBoard;

        public Game1()
        {
            _c_Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _c_Graphics.PreferredBackBufferWidth = p_WindowWidth;
            _c_Graphics.PreferredBackBufferHeight = p_WindowHeight;
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
            _c_SpriteBatch = new SpriteBatch(GraphicsDevice);

            _l_TextureList = new List<Texture2D>();

            for (int ii = 0; ii < 2; ii++)
            {
                Texture2D temp = Content.Load<Texture2D>(string.Format("bg{0}", ii + 1));
                _l_TextureList.Add(temp);
                temp = Content.Load<Texture2D>(string.Format("block_{0}", ii+1));
                _l_TextureList.Add(temp);
            }

            int baseValue = 0;
            while (true)
            {
                Texture2D temp = Content.Load<Texture2D>(string.Format("block_{0}", baseValue));
                _l_TextureList.Add(temp);
                if (baseValue == 0) baseValue = 3;
                else baseValue *= 2;
                
                if (baseValue > 3072) break;
            }
            _c_MyBoard = new Board(_l_TextureList, 4);

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

            _c_MyBoard.f_Update(elapsedTime);

            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.A) || keyboard.IsKeyDown(Keys.Left))
            {
                _c_MyBoard.f_Move(KeyArrow.LEFT);
            }
            else if (keyboard.IsKeyDown(Keys.D) || keyboard.IsKeyDown(Keys.Right))
            {
                _c_MyBoard.f_Move(KeyArrow.RIGHT);
            }
            if (keyboard.IsKeyDown(Keys.W) || keyboard.IsKeyDown(Keys.Up))
            {
                _c_MyBoard.f_Move(KeyArrow.UP);
            }
            else if (keyboard.IsKeyDown(Keys.S) || keyboard.IsKeyDown(Keys.Down))
            {
                _c_MyBoard.f_Move(KeyArrow.DOWN);
            }

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
            _c_SpriteBatch.Begin();
            _c_MyBoard.f_Show(_c_SpriteBatch);

            _c_SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
