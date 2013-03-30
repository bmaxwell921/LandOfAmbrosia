using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LandOfAmbrosia.Common
{

    public enum INPUT_TYPE { KEYBOARD, GAMEPAD };
    public enum ATTACK_TYPE { MELEE, RANGE, MAGIC, NONE};

    class Constants
    {
        /*
         * Matrix to fix the differences between the Blender coordinate system and the XNA system 
         */
        public static Matrix blenderToXNA = Matrix.Identity * Matrix.CreateRotationZ(MathHelper.ToRadians(-90));

        #region Level Character Reps
        public static readonly char PLAYER1_CHAR = '1';
        public static readonly char PLAYER2_CHAR = '2';
        public static readonly char EMPTY_CHAR = ' ';
        public static readonly char PLATFORM_CHAR = 'P';
        //TODO enemy reps
        #endregion

        #region Asset Names
        //Tiles
        public static readonly String NULL_MODEL = null;
        public static readonly String PLATFORM = @"Models/smallBlock";


        public static readonly float TILE_WIDTH = 2;
        public static readonly float TILE_HEIGHT = 2;

        //Skybox
        public static readonly String SKYBOX_EFFECT = @"Skybox/effects";
        public static readonly String SKYBOX_MODEL = @"Skybox/skybox2";

        //Characters
        public static readonly String PLAYER1_MODEL = @"Models/swordModel";
        public static readonly String PLAYER2_MODEL = @"Models/TBA";
        #endregion

        //Default Values
        public static readonly Vector3 DEFAULT_PLAYER1_START = new Vector3(TILE_WIDTH, 3 * TILE_HEIGHT, 0);
        public static readonly Vector3 DEFAULT_PLAYER2_START = new Vector3(TILE_WIDTH, TILE_HEIGHT, 0);

        public static readonly int DEFAULT_SEED = 42;

        public static readonly int NUM_TILE_TYPES = 2;

        //Character stuff
        public static readonly int DEFAULT_MAX_HEALTH = 100;
        public static readonly int MAX_SPEED_X = 2;
        public static readonly Matrix scale = Matrix.CreateScale(0.5f);

        //Camera stuff
        //TODO this isn't actually working
        public static readonly int CAMERA_FRAME_WIDTH_BLOCKS = 20;

        /*
         * Since we need to do the weird transformation to go from Blender coordinates to XNA coordinates
         * I think all the translations will be messed up so this is an easy way to fix the translations
         * by passing in the correct vector
         */
        public static Vector3 ConvertToXNAScene(Vector3 regularVector)
        {
            return new Vector3(regularVector.Z, -1 * regularVector.X, regularVector.Y);
        }
    }
}
