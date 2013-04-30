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
using LandOfAmbrosia.Managers;
using LandOfAmbrosia.Logic;
using System.IO;
using LandOfAmbrosia.Common;

namespace LandOfAmbrosia
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class LandOfAmbrosiaGame : Microsoft.Xna.Framework.Game
    {
        /* This camera needs to be shared between the LevelManager and the CharacterManager:
         * The LevelManager sets it up based on the width and height of the level, but 
         * the CharacterManage knows where the characters are so it will need to move the camera
         * to the left and right. Maybe it would be best to leave this in the Game class since
         * both Managers have access to that
         */
        public CameraComponent camera;

        LevelManager lm;

        SoundManager sm;

        SpriteManager spm;

        MenuManager mm;

        GraphicsDeviceManager graphics;

        public GameState curState;

        public LandOfAmbrosiaGame()
        {
            curState = GameState.START_SCREEN;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 900;

#if !DEBUG
            graphics.IsFullScreen = true;
#endif
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //This will get overridden in the LevelManager constructor
            camera = new CameraComponent(this, Vector3.Zero);

            Components.Add(camera);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            AssetUtil.loadAll(Content);
            mm = new MenuManager(this, new SpriteBatch(GraphicsDevice), false);
            Components.Add(mm);


            sm = new SoundManager(this, Content);
            Components.Add(sm);

            TransitionToState(GameState.START_SCREEN);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.Black);

            //Stops the models from being all jacked up
            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None;
            graphics.GraphicsDevice.RasterizerState = rs;

            //Stops the models from being all jacked up after drawing sprites
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            base.Draw(gameTime);
        }

        public void TransitionToState(GameState newState)
        {
            if (newState == GameState.START_SCREEN)
            {
                EnableDisableDrawable(lm, false);
                EnableDisableDrawable(spm, false);
                EnableDisableComponent(sm, true);
                EnableDisableDrawable(mm, true);
            }
            else if (newState == GameState.PLAYING)
            {
                EnableDisableDrawable(lm, true);
                EnableDisableDrawable(spm, true);
                EnableDisableComponent(sm, true);
                EnableDisableDrawable(mm, false);
            }
            else if (newState == GameState.PAUSE)
            {
                EnableDisableDrawable(lm, false);
                EnableDisableDrawable(spm, false);
                EnableDisableComponent(sm, true);
                EnableDisableDrawable(mm, true);
            }
            else if (newState == GameState.NEXT_LEVEL_WAIT)
            {
                EnableDisableDrawable(lm, true);
                EnableDisableDrawable(spm, true);
                EnableDisableComponent(sm, true);
                EnableDisableDrawable(mm, false);
            }
            else if (curState == GameState.NEXT_LEVEL_GENERATE)
            {
                EnableDisableDrawable(lm, true);
                EnableDisableDrawable(spm, true);
                EnableDisableComponent(sm, true);
                EnableDisableDrawable(mm, false);
            }
            else if (newState == GameState.RESPAWN)
            {
                EnableDisableDrawable(lm, false);
                EnableDisableDrawable(spm, false);
                EnableDisableComponent(sm, true);
                EnableDisableDrawable(mm, true);
            }
            else if (newState == GameState.GAME_OVER)
            {
                EnableDisableDrawable(lm, false);
                EnableDisableDrawable(spm, false);
                EnableDisableComponent(sm, true);
                EnableDisableDrawable(mm, true);
            }
            else if (newState == GameState.VICTORY)
            {
                EnableDisableDrawable(lm, false);
                EnableDisableDrawable(spm, false);
                EnableDisableComponent(sm, true);
                EnableDisableDrawable(mm, true);
            }

            curState = newState;
        }

        public void startGame(int numPlayers)
        {
            lm = new LevelManager(this, numPlayers);
            Components.Add(lm);
            Services.AddService(typeof(LevelManager), lm);

            spm = new SpriteManager(this, new SpriteBatch(GraphicsDevice));
            Components.Add(spm);

            TransitionToState(GameState.PLAYING);
        }

        public void restartGame()
        {
            //Remove them so we can re add them later
            Components.Remove(lm);
            Services.RemoveService(typeof(LevelManager));

            TransitionToState(GameState.START_SCREEN);
        }

        private void EnableDisableDrawable(DrawableGameComponent dgc, bool enabled)
        {
            if (dgc != null)
            {
                dgc.Visible = enabled;
                dgc.Enabled = enabled;
            }
        }
        private void EnableDisableComponent(GameComponent gc, bool enabled)
        {
            if (gc != null)
            {
                gc.Enabled = enabled;
            }
        }
    }
}
