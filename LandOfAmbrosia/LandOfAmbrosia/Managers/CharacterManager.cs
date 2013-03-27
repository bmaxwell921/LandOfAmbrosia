using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LandOfAmbrosia.Characters;

namespace LandOfAmbrosia.Managers
{
    class CharacterManager : DrawableGameComponent
    {
        #region Character Fields
        public Character player1;
        public Character player2;

        public static Vector3 DEFAULT_PLAYER1_START = Vector3.Zero;
        public static Vector3 DEFAULT_PLAYER2_START = Vector3.Zero + new Vector3(5, 0, 0);

        public Model player1Model;
        public Model player2Model;

        public String player1ModelAsset = @"Models/character";
        public String player2ModelAsset = @"";
        #endregion

        public IList<Character> enemies;

        public CharacterManager(Game game)
            : base(game)
        {
            enemies = new List<Character>();
        }

        protected override void LoadContent()
        {
            player1Model = Game.Content.Load<Model>(player1ModelAsset);

            player1 = new UserControlledCharacter(player1Model, DEFAULT_PLAYER1_START);
            //player2Model = Game.Content.Load<Model>(player2ModelAsset);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            //TODO
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            player1.Draw(((Game1)Game).camera);
            base.Draw(gameTime);
        }
    }
}
