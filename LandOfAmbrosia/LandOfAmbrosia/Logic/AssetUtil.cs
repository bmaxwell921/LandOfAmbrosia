using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using LandOfAmbrosia.Common;

namespace LandOfAmbrosia.Logic
{
    class AssetUtil
    {
        //Map of character representation to model for the tiles. Used when reading levels from a file
        private static IDictionary<char, Model> tileModels = new Dictionary<char, Model>();

        //Map of character representation to model for the characters
        private static IDictionary<char, Model> playerModels = new Dictionary<char, Model>();

        //Map of character representation to model for the enmies
        private static IDictionary<char, Model> enemyModels = new Dictionary<char, Model>();

        //Map of character representation to model for projectiles
        private static IDictionary<char, Model> projectileModels = new Dictionary<char, Model>();

        //The actual cube for the skybox
        public static Model skyboxModel
        {
            get
            {
                return _skyboxModel;
            }
            protected set
            {
                _skyboxModel = value;
            }
        }
        private static Model _skyboxModel;

        //The textures for the skybox
        public static Texture2D[] skyboxTextures
        {
            get
            {
                return _textures;
            }
            protected set
            {
                _textures = value;
            }
        }
        private static Texture2D[] _textures;

        /// <summary>
        /// Method called by the game at the very beginning to load 
        /// the assets
        /// </summary>
        /// <param name="content"></param>
        public static void loadAll(ContentManager content)
        {
            loadTileModels(content);
            loadPlayerModels(content);
            loadEnemyModels(content);
            loadProjectileModels(content);
            loadSkyboxAssets(content);
        }

        //Loads the models for the tiles and fills in the tileModels map
        private static void loadTileModels(ContentManager content)
        {
            //Is there a better way to do this with the empty tile?
            tileModels.Add(Constants.EMPTY_CHAR, null);
            tileModels.Add(Constants.PLATFORM_CHAR, content.Load<Model>(Constants.PLATFORM));
        }

        //Loads the models for the players and fills in the playerModels map
        private static void loadPlayerModels(ContentManager content)
        {
            playerModels.Add(Constants.PLAYER1_CHAR, content.Load<Model>(Constants.PLAYER1_MODEL));
            playerModels.Add(Constants.PLAYER2_CHAR, content.Load<Model>(Constants.PLAYER2_MODEL));
        }

        private static void loadEnemyModels(ContentManager content)
        {
            enemyModels.Add(Constants.MINION_CHAR, content.Load<Model>(Constants.MINION_MODEL));
        }

        private static void loadProjectileModels(ContentManager content)
        {
            projectileModels.Add(Constants.MAGIC_CHAR, content.Load<Model>(Constants.MAGIC_MODEL));
        }

        //Loads the assets for the skybox, including the effect and model
        private static void loadSkyboxAssets(ContentManager content)
        {
            Effect skyboxEffect = content.Load<Effect>(Constants.SKYBOX_EFFECT);
            skyboxModel = content.Load<Model>(Constants.SKYBOX_MODEL);
            skyboxTextures = new Texture2D[skyboxModel.Meshes.Count];
            int i = 0;
            foreach (ModelMesh mesh in skyboxModel.Meshes)
                foreach (BasicEffect currentEffect in mesh.Effects)
                    skyboxTextures[i++] = currentEffect.Texture;

            foreach (ModelMesh mesh in skyboxModel.Meshes)
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    meshPart.Effect = skyboxEffect.Clone();
        }

        public static Model GetTileModel(char tile)
        {
            if (tileModels.ContainsKey(tile))
            {
                return tileModels[tile];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Method to get the model associated with the given character for players
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static Model GetPlayerModel(char player)
        {
            if (playerModels.ContainsKey(player)) 
            {
                return playerModels[player];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Method to get the model associated with the given character for enemies
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static Model GetEnemyModel(char enemy)
        {
            if (enemyModels.ContainsKey(enemy))
            {
                return enemyModels[enemy];
            }
            else
            {
                return null;
            }
        }

        public static Model GetProjectileModel(char proj)
        {
            if (projectileModels.ContainsKey(proj))
            {
                return projectileModels[proj];
            }
            return null;
        }
    }
}
