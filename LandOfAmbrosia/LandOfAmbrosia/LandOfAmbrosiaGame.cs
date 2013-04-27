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

namespace LandOfAmbrosia
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class LandOfAmbrosiaGame : Microsoft.Xna.Framework.Game
    {
        public string levelLoc = @"G:\Documents\GitRepos\LandOfAmbrosia\LandOfAmbrosia\LandOfAmbrosia\PreLoadedLevels\OneChunkLevel.txt";

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

        GraphicsDeviceManager graphics;

        public LandOfAmbrosiaGame()
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
            //This will get overridden in the LevelManager constructor
            camera = new CameraComponent(this, Vector3.Zero);

            //TODO take off this part and the camera won't be able to move unless I make it move
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

            lm = new LevelManager(this, 1);
            //lm = new LevelManager(this, true);
            Components.Add(lm);
            Services.AddService(typeof(LevelManager), lm);

            //sm = new SoundManager(this, Content);
            //Components.Add(sm);

            spm = new SpriteManager(this, new SpriteBatch(GraphicsDevice), Content);
            Components.Add(spm);
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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //Stops the models from being all jacked up
            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None;
            graphics.GraphicsDevice.RasterizerState = rs;

            //Stops the models from being all jacked up after drawing sprites
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            base.Draw(gameTime);
        }
    }
}
