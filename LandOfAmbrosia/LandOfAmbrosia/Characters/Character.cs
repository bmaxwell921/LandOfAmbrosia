using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandOfAmbrosia.Characters;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using LandOfAmbrosia.Weapons;

namespace LandOfAmbrosia
{
    abstract class Character : ICharacter, ICollidable
    {
        #region Drawing Fields
        public Model model
        {
            get;
            protected set;
        }
        public Matrix world;
        #endregion

        #region Movement
        public Vector3 speed;
        public Vector3 position;
        #endregion

        #region Weapons
        public Weapon meleeWeapon;
        public Weapon rangeWeapon;
        #endregion

        #region Health
        public int health;
        public int maxHealth;
        #endregion

        /// <summary>
        /// Method to update the character from it's 
        /// old position to it's new position.
        /// </summary>
        public virtual void Update()
        {
        }

        /// <summary>
        /// Method to draw the character on the screen 
        /// at its location
        /// </summary>
        /// <param name="c"></param>
        public void Draw(Camera c)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect be in mesh.Effects)
                {
                    be.EnableDefaultLighting();
                    be.Projection = c.projection;
                    be.View = c.view;
                    be.World = world * mesh.ParentBone.Transform;
                }

                mesh.Draw();
            }
        }

        /// <summary>
        /// Method to check if one character
        /// collides with another ICollidable object
        /// </summary>
        /// <param name="other">The other item being checked</param>
        /// <returns>True if the two items collide, false otherwise</returns>
        public bool CollidesWith(ICollidable other)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// A method to execute a meleeAttack done
        /// by this character
        /// </summary>
        public virtual void meleeAttack()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// A method to execute a rangeAttack by the character.
        /// </summary>
        /// <returns>Either an Arrow or a MagicSpell object depending on what type of 
        /// RangeWeapon the character has</returns>
        public virtual Projectile rangeAttack()
        {
            throw new NotImplementedException();
        }
    }
}
