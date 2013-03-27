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

        public Matrix blenderToXNAFix = Matrix.Identity * Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateRotationY(MathHelper.ToRadians(0)) 
            * Matrix.CreateRotationZ(MathHelper.ToRadians(0));

        public Matrix scale = Matrix.CreateScale(0.5f);
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

        public Character(Model model, Vector3 speed, Vector3 position, Weapon meleeWeapon, Weapon rangeWeapon, int maxHealth)
        {
            this.model = model;
            this.speed = speed;
            this.position = position;
            this.meleeWeapon = meleeWeapon;
            this.rangeWeapon = rangeWeapon;
            this.maxHealth = maxHealth;

            this.health = this.maxHealth;
            this.world = Matrix.CreateTranslation(this.position);
        }

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
        public void Draw(CameraComponent c)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect be in mesh.Effects)
                {
                    be.EnableDefaultLighting();
                    be.Projection = c.ProjectionMatrix;
                    be.View = c.ViewMatrix;
                    be.World = blenderToXNAFix * scale * world * mesh.ParentBone.Transform;
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
