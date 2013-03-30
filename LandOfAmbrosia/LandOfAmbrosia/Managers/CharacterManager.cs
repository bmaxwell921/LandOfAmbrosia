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
            player1 = new UserControlledCharacter(AssetUtil.GetPlayerModel(Constants.PLAYER1_CHAR), Constants.ConvertToXNAScene(Constants.DEFAULT_PLAYER1_START));
            //player2Model = Game.Content.Load<Model>(AssetUtil.GetPlayerModel(Constants.PLAYER1_CHAR), Constants.DEFAULT_PLAYER1_START);
        }

        public override void Update(GameTime gameTime)
        {
            //TODO
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            player1.Draw(((LandOfAmbrosiaGame)Game).camera);
            base.Draw(gameTime);
        }
    }
}
