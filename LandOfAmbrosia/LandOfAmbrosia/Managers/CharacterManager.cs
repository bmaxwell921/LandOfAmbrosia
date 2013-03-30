using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LandOfAmbrosia.Characters;
using LandOfAmbrosia.Common;
using LandOfAmbrosia.Logic;

namespace LandOfAmbrosia.Managers
{
    class CharacterManager : DrawableGameComponent
    {
        #region Character Fields
        public Character player1;
        public Character player2;
        #endregion

        public IList<Character> enemies;

        public CharacterManager(Game game)
            : base(game)
        {
            enemies = new List<Character>();

            //The internal implementation of the characters will handle changing the coordinate system
            player1 = new UserControlledCharacter(Constants.PLAYER1_CHAR, AssetUtil.GetPlayerModel(Constants.PLAYER1_CHAR), Constants.DEFAULT_PLAYER1_START);
            player2 = new UserControlledCharacter(Constants.PLAYER2_CHAR, AssetUtil.GetPlayerModel(Constants.PLAYER1_CHAR), Constants.DEFAULT_PLAYER2_START);
        }

        public override void Update(GameTime gameTime)
        {
            this.UpdateCharacters();
            base.Update(gameTime);
        }

        private void UpdateCharacters()
        {
            player1.Update();
            //player2.Update();
        }

        private void CheckCharacterGroundCollision()
        {

        }

        public override void Draw(GameTime gameTime)
        {
            player1.Draw(((LandOfAmbrosiaGame)Game).camera);
            //player2.Draw(((LandOfAmbrosiaGame)Game).camera);
            base.Draw(gameTime);
        }
    }
}
