using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LandOfAmbrosia.Common
{

    public enum INPUT_TYPE { KEYBOARD, GAMEPAD };
    public enum ATTACK_TYPE { MELEE, RANGE, MAGIC, NONE};
    public enum JUMP_STATE { JUMP, DOUBLE_JUMP, FALLING, NONE };

    class Constants
    {
        #region Level Character Reps
        public static readonly char PLAYER1_CHAR = '1';
        public static readonly char PLAYER2_CHAR = '2';
        public static readonly char EMPTY_CHAR = ' ';
        public static readonly char PLATFORM_CHAR = 'P';
        public static readonly char MINION_CHAR = 'M';
        //TODO enemy reps
        #endregion

        #region Asset Names
        //Tiles
        public static readonly String NULL_MODEL = null;
        public static readonly String PLATFORM = @"Models/platform";

        /*
         * These two have to be up here otherwise the DEFAULT_PLAYER1_START initializes wrong...apparently because
         * c# is too stupid to make sure they've been initialized
         */
        public static readonly float CHARACTER_DEPTH = 1f;

        public static readonly int TILE_SIZE = 2;

        //Skybox
        public static readonly String SKYBOX_EFFECT = @"Skybox/effects";
        public static readonly String SKYBOX_MODEL = @"Skybox/skybox2";

        //Characters
        public static readonly String PLAYER1_MODEL = @"Models/player1";
        public static readonly String PLAYER2_MODEL = @"Models/player2";

        //Minions
        public static readonly String MINION_MODEL = @"Models/minion3";

        //Projectiles
        public static readonly String MAGIC_MODEL = @"Models/magicBig";
        #endregion

        //Default Values
        public static readonly Vector3 DEFAULT_PLAYER1_START = new Vector3(TILE_SIZE  + TILE_SIZE/ 2.0f, TILE_SIZE, CHARACTER_DEPTH);
        public static readonly Vector3 DEFAULT_PLAYER2_START = DEFAULT_PLAYER1_START;// + new Vector3(TILE_SIZE, 0, 0);

        public static readonly int DEFAULT_SEED = 42;

        public static readonly int NUM_TILE_TYPES = 2;

        //Character stuff
        public static readonly int DEFAULT_MAX_HEALTH = 100;
        public static readonly float MAX_SPEED_X = 0.25f;
        public static readonly Matrix scale = Matrix.CreateScale(1);

        public static readonly float CHARACTER_WIDTH = 0.32f;
        public static readonly float CHARACTER_HEIGHT = 2f;

        public static readonly char MAGIC_CHAR = 'm';
        public static readonly Vector3 MINION_POSITION_HACK = new Vector3(1, -1, 0);

        //Level stuff
        public static readonly int DEFAULT_WIDTH = 128;
        public static readonly int DEFAULT_HEIGHT = 16;
        public static readonly int CHUNK_SIZE = 8;

        //Collision Detection
        public static readonly float BUFFER = 0.01f;

        //Camera stuff
        //TODO this isn't actually working
        public static readonly int CAMERA_FRAME_WIDTH_BLOCKS = 5;

        public static readonly float GRAVITY = -0.01f;

        /*
         * Since we need to do the weird transformation to go from Blender coordinates to XNA coordinates
         * I think all the translations will be messed up so this is an easy way to fix the translations
         * by passing in the correct vector
         */
        public static Vector3 ConvertToXNAScene(Vector3 regularVector)
        {
            return new Vector3(regularVector.Z, -1 * regularVector.X, regularVector.Y);
        }

        //Does the opposite of the above method
        public static Vector3 UnconvertFromXNAScene(Vector3 XNAVector)
        {
            return new Vector3(-1 * XNAVector.Y, XNAVector.Z, XNAVector.X);
        }
    }
}
