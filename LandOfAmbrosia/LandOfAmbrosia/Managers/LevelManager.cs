using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandOfAmbrosia.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LandOfAmbrosia.Managers
{
    class LevelManager : DrawableGameComponent
    {
        #region Level Fields
        private Level currentLevel;
        private String[] tileModelAssets = { null, @"Models/coloredPlatform"};
        //Empty, Ground
        private const int NUM_TILE_TYPES = 2;
        #endregion

        private const int DEFAULT_SEED = 42;


        public Random gen;

        /// <summary>
        /// Constructs a new LevelManager with the default Level
        /// </summary>
        public LevelManager(Game game) :
            this(game, DEFAULT_SEED)
        {
            
        }

        public LevelManager(Game game, int seed): base(game)
        {
            currentLevel = new Level();
            gen = new Random(seed);
        }

        protected override void LoadContent()
        {
            Texture2D[] textures;
            Model m = this.loadSkybox(out textures);
            Skybox skybox = new Skybox(m, textures);
            currentLevel.skybox = skybox;

            Model[] tileModels = this.LoadTileModels();
            this.GenerateLevel(tileModels);
            base.LoadContent();
        }

        private Model loadSkybox(out Texture2D[] textures)
        {
            Effect effect = Game.Content.Load<Effect>(@"Skybox/effects"); ;
            Model newModel = Game.Content.Load<Model>(@"Skybox/skybox2");
            textures = new Texture2D[newModel.Meshes.Count];
            int i = 0;
            foreach (ModelMesh mesh in newModel.Meshes)
                foreach (BasicEffect currentEffect in mesh.Effects)
                    textures[i++] = currentEffect.Texture;

            foreach (ModelMesh mesh in newModel.Meshes)
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    meshPart.Effect = effect.Clone();

            return newModel;
        }

        private Model[] LoadTileModels()
        {
            Model[] tileModels = new Model[NUM_TILE_TYPES];
            for (int i = 0; i < NUM_TILE_TYPES; ++i)
            {
                tileModels[i] = tileModelAssets[i] == null ? null : Game.Content.Load<Model>(tileModelAssets[i]);
            }
            return tileModels;
        }

        /// <summary>
        /// Called by the LevelManager to randomly initialize the level. I think this should actually be in the LevelManager...
        /// </summary>
        /// <param name="possibleModels"></param>
        public void GenerateLevel(Model[] possibleModels)
        {
            //int numNonEmptySpaces = gen.Next(currentLevel.width * currentLevel.height / 2);

            //for (int i = 0; i < numNonEmptySpaces; ++i)
            //{
            //    //addLoc tells where to put the tile in the Level array, worldLoc tells where we need to draw the tile in the scene
            //    Vector3 addLoc = this.GetRandomTileLocation();
            //    //Vector3 worldLoc = this.AddLocToWorldLoc(addLoc);
            //    Model addModel = this.GetRandomModel(possibleModels);
            //    currentLevel.SetTile((int)addLoc.X, (int)addLoc.Y, new Tile(addModel, addLoc));
            //}

            this.FillFloor(possibleModels);
        }

        private Vector3 GetRandomTileLocation()
        {
            //Multiply the generated value by the width and height to put them in the correct location
            return new Vector3(gen.Next(currentLevel.width), gen.Next(currentLevel.height), 0);
        }

        //private Vector3 AddLocToWorldLoc(Vector3 addLoc)
        //{
        //    //return new Vector3(TILE_WIDTH * addLoc.X, TILE_HEIGHT * addLoc.Y, addLoc.Z);
        //}

        private Model GetRandomModel(Model[] possibleModels)
        {
            return possibleModels[(int)gen.Next(possibleModels.Count())];
        }

        private void FillFloor(Model[] possibleModels)
        {
            for (int i = 0; i < currentLevel.width; ++i)
            {
                /*
                 * Spot 1 is the basic platform. The y value is 0 because positive y goes up the page
                 */
                currentLevel.SetTile(i, 0, new Tile(possibleModels[1], new Vector3(i, 0, 0)));
            }
        }

        /// <summary>
        /// Constructor that creates a level by reading in the level from a file
        /// </summary>
        /// <param name="game"></param>
        /// <param name="levelFileLoc"></param>
        public LevelManager(Game game, String levelFileLoc) : 
            base(game)
        {
            throw new NotImplementedException();
        }

        public override void Draw(GameTime gameTime)
        {
            currentLevel.Draw(((Game1)Game).camera, Game.GraphicsDevice);

            base.Draw(gameTime);
        }
    }
}
