using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandOfAmbrosia.Characters;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using LandOfAmbrosia.Weapons;
using LandOfAmbrosia.Common;

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
        #endregion

        #region Movement
        public Vector3 velocity;
        public Vector3 position;
        public bool onGround;
        #endregion

        public float width, height;

        #region Weapons
        public Weapon meleeWeapon;
        public Weapon rangeWeapon;
        #endregion

        #region Health
        //TODO: Box this up into a stat object
        public int health;
        public int maxHealth;
        #endregion

        public Character(Model model, Vector3 speed, Vector3 position, Weapon meleeWeapon, Weapon rangeWeapon, int maxHealth)
        {
            this.model = model;
            this.velocity = speed;
            this.position = Constants.ConvertToXNAScene(position);
            this.meleeWeapon = meleeWeapon;
            this.rangeWeapon = rangeWeapon;
            this.maxHealth = maxHealth;
            this.onGround = false;
            this.health = this.maxHealth;

            this.width = model.Meshes[0].BoundingSphere.Radius * 2;
            this.height = model.Meshes[0].BoundingSphere.Radius * 2;
        }

        /// <summary>
        /// Method to update the character from it's 
        /// old position to it's new position.
        /// 
        /// I think this will actually just update the animation...cause the LevelManager does the position updating
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
                    be.World =  GetWorld() * mesh.ParentBone.Transform;
                }

                mesh.Draw();
            }
        }

        public virtual Matrix GetWorld()
        {
            return Constants.scale * Matrix.CreateTranslation(position);
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

        public virtual bool isFlying()
        {
            return false;
        }

        /// <summary>
        /// Correctly sets the x velocity for the character by converting to the xna coordinates
        /// </summary>
        /// <param name="x"></param>
        public void setVelocityX(float x)
        {
            Vector3 oldCorrectVel = Constants.UnconvertFromXNAScene(velocity);
            oldCorrectVel.X = x;
            velocity = Constants.ConvertToXNAScene(oldCorrectVel);
        }

        /// <summary>
        /// Gets the x component of the velocity in xna coordinates
        /// </summary>
        /// <returns></returns>
        public float getVelocityX()
        {
            return Constants.UnconvertFromXNAScene(velocity).X;
        }

        /// <summary>
        /// Correctly sets the y velocity for the character by converting to the xna coordinates
        /// </summary>
        /// <param name="y"></param>
        public void setVelocityY(float y)
        {
            Vector3 oldCorrectVel = Constants.UnconvertFromXNAScene(velocity);
            oldCorrectVel.Y = y;
            velocity = Constants.ConvertToXNAScene(oldCorrectVel);
        }

        /// <summary>
        /// Gets the y component of the velocity in xna coordinates
        /// </summary>
        /// <returns></returns>
        public float getVelocityY()
        {
            return Constants.UnconvertFromXNAScene(velocity).Y;
        }

        public float getX()
        {
            return Constants.UnconvertFromXNAScene(position).X;
        }

        public void setX(float x)
        {
            Vector3 oldCorrectPos = Constants.UnconvertFromXNAScene(position);
            oldCorrectPos.X = x;
            position = Constants.ConvertToXNAScene(oldCorrectPos);
        }

        public float getY()
        {
            return Constants.UnconvertFromXNAScene(position).Y;
        }

        public void setY(float y)
        {
            Vector3 oldCorrectPos = Constants.UnconvertFromXNAScene(position);
            oldCorrectPos.Y = y;
            position = Constants.ConvertToXNAScene(oldCorrectPos);
        }

        public void collideHorizontal()
        {
            setVelocityX(-getVelocityX());
        }

        public void collideVertical()
        {
            setVelocityY(0);
        }
    }
}
