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
    public enum ChunkType { FLOOR, STAIRS, FLOATING_PLATFORMS_SAFE, FLOATING_PLATFORMS_NOT_SAFE, MOUNTAIN_LEFT, MOUNTAIN_RIGHT, JAGGIES_LEFT, JAGGIES_RIGHT, TALL_GROUND, EMPTY }
    public enum GameState { START_SCREEN, PLAYING, PAUSE, NEXT_LEVEL_WAIT, NEXT_LEVEL_GENERATE, RESPAWN, GAME_OVER, VICTORY };

    //I don't like this, but this enum is being used for both the ai state and the decision tree traversal
    public enum AI_STATE { NO, YES, 
        WAIT, NEW_MOVE, CONTINUE_MOVE, CALC_PATH, FOLLOW_PATH, ATTACK };

    class Constants
    {
        //Level Reader character reps
        public static readonly char PLAYER1_CHAR = '1';
        public static readonly char PLAYER2_CHAR = '2';
        public static readonly char EMPTY_CHAR = ' ';

        public static readonly char GREEN_PLATFORM = 'G';
        public static readonly char BLUE_PLATFORM = 'B';
        public static readonly char RED_PLATFORM = 'R';

        public static readonly char EXPERIENCE = 'E';

        public static readonly char MINION_CHAR = 'M';

        public static readonly float CHARACTER_DEPTH = 1f;
        public static readonly int TILE_SIZE = 2;

        #region Asset Names
        //Tiles
        public static readonly string NULL_MODEL = null;
        public static readonly string GREEN_PLATFORM_ASSET = @"Models/greenPlatform";
        public static readonly string BLUE_PLATFORM_ASSET = @"Models/bluePlatform";
        public static readonly string RED_PLATFORM_ASSET = @"Models/redPlatform";

        //Skybox
        public static readonly string SKYBOX_EFFECT = @"Skybox/effects";
        public static readonly string SKYBOX_MODEL = @"Skybox/skybox2";

        //Characters
        public static readonly string PLAYER1_MODEL = @"Models/player1";
        public static readonly string PLAYER2_MODEL = @"Models/player2";

        //Minions
        public static readonly string MINION_MODEL = @"Models/minion3";

        public static readonly string EXPERIENCE_ASSET = @"Models/experience";

        //Projectiles
        public static readonly string MAGIC_MODEL = @"Models/magicBig";
        #endregion

        //Default Values
        public static readonly Vector3 DEFAULT_PLAYER1_START = new Vector3(TILE_SIZE  + TILE_SIZE/ 2.0f, TILE_SIZE, CHARACTER_DEPTH);
        public static readonly Vector3 DEFAULT_PLAYER2_START = DEFAULT_PLAYER1_START + new Vector3(TILE_SIZE, 0, 0);

        public static readonly int DEFAULT_SEED = 42;

        public static readonly int NUM_TILE_TYPES = 2;

        //Character stuff
        public static readonly int DEFAULT_MAX_HEALTH = 100;
        public static readonly float MAX_SPEED_X = 0.25f;
        public static readonly float AI_MAX_SPEED_X = 0.15f;
        public static readonly Matrix scale = Matrix.CreateScale(1);

        public static readonly float CHARACTER_WIDTH = 0.32f;
        public static readonly float CHARACTER_HEIGHT = 2f;

        public static readonly char MAGIC_CHAR = 'm';
        public static readonly Vector3 MINION_POSITION_HACK = new Vector3(1, -1, 0);
        public static readonly int DEFAULT_MINION_HEALTH = 50;

        public static readonly int MINION_SIGHT = 10;
        public static readonly int MINION_ATTACK_RANGE = 5;

        //Level stuff
        public static readonly int DEFAULT_WIDTH = 128;
        public static readonly int DEFAULT_HEIGHT = 16;
        public static readonly int CHUNK_SIZE = 8;

        //Collision Detection
        public static readonly float BUFFER = 0.01f;

        //Magic
        public static readonly float MAGIC_WIDTH = 0.5f;
        public static readonly float MAGIC_HEIGHT = 0.05f;

        //Stat keys
        public static readonly string HEALTH_KEY = "Health";
        public static readonly string ATTACK_KEY = "Attack";
        public static readonly string DEFENCE_KEY = "Defence";
        public static readonly string EXPERIENCE_KEY = "Experience";
        public static readonly string LEVEL_NUMBER_KEY = "Level";

        //Decision Tree Types
        public static readonly int MINION_TREE = 0;
        public static readonly int BOSS_TREE = 0;

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


        public static bool InRange(Character c, CameraComponent cam)
        {
            return Math.Abs(c.getX() - cam.Position.X) < Constants.TILE_SIZE * 30;
        }
    }
}
