using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandOfAmbrosia.Characters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LandOfAmbrosia.Common;
using LandOfAmbrosia.Levels;

namespace LandOfAmbrosia.Weapons
{
    class Projectile
    {
        public Model model
        {
            get;
            protected set;
        }

        public Vector3 velocity;
        public Vector3 position;

        public Level currentLevel;
        
        //This vector is UNCONVERTED
        public Vector3 targetPosition;
        public Character source;
        public bool timeToDie;

        public float width, height;

        public Projectile(Level l, Model model, Vector3 position, Character source, Vector3 targetPosition)
        {
            currentLevel = l;
            width = Constants.MAGIC_WIDTH;
            height = Constants.MAGIC_HEIGHT;
            this.model = model;
            //No conversion necessary because source.position is already in XNA coords
            this.source = source;
            this.position = Constants.ConvertToXNAScene(position);
            this.targetPosition = targetPosition;
            timeToDie = false;
            this.UpdateVelocity();
        }

        //Lovingly stolen from www.cse.scu.edu. I would put the whole address, but it downloads as a ppt so I don't really know how to get it...
        private void UpdateVelocity()
        {
            //2 blocks
            float radius = 4;
            float maxSpeed = 0.5f;

            Vector3 myPos = Constants.UnconvertFromXNAScene(this.position);

            //If the target field is null, then just update using the target Position

            Vector3 desiredVel = targetPosition - myPos;
            if (desiredVel.Length() < radius)
            {
                desiredVel /= 1.5f;
                if (desiredVel.Length() > maxSpeed)
                {
                    desiredVel /= desiredVel.Length();
                }
            }
            else
            {
                desiredVel /= desiredVel.Length();
            }
            velocity = desiredVel;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (!ReadyToDie())
            {
                UpdateTarget();
                UpdateVelocity();
                Vector3 tempPos = Constants.UnconvertFromXNAScene(position);
                tempPos += velocity;
                position = Constants.ConvertToXNAScene(tempPos);
                CheckKill();
            }
        }

        /// <summary>
        /// Let subtypes override this to track to enemies
        /// </summary>
        protected virtual void UpdateTarget()
        {
        }

        protected virtual void CheckKill()
        {
            //Velocity goes to 0 when we get close to the guy, so if the velocity is really small let's just say it's good enough
            if (velocity.Length() <= 0.01)
            {
                timeToDie = true;
            }

            //Check if we hit the source's enemies in the level
            IList<Character> sourceEnemies = source is UserControlledCharacter ? currentLevel.enemies : currentLevel.players;
            BoundingBox sourceBound = new BoundingBox(new Vector3(getX(), getY(), Constants.CHARACTER_DEPTH), 
                new Vector3(getX() + width, getY() + height, Constants.CHARACTER_DEPTH));
            foreach (Character enemy in sourceEnemies)
            {
                BoundingBox enemyBound = new BoundingBox(new Vector3(enemy.getX(), enemy.getY(), Constants.CHARACTER_DEPTH), 
                    new Vector3(enemy.getX() + enemy.width, enemy.getY() + enemy.height, Constants.CHARACTER_DEPTH));
                if (sourceBound.Intersects(enemyBound))
                {
                    timeToDie = true;
                    float damage = source.stats.getStatCurrentVal(Constants.ATTACK_KEY) - enemy.stats.getStatCurrentVal(Constants.DEFENCE_KEY);
                    enemy.stats.changeCurrentStat(Constants.HEALTH_KEY, -damage);
                    Console.WriteLine("Doing " + damage + " points of damage!");
                    return;
                }
            }
        }

        private float getX()
        {
            return Constants.UnconvertFromXNAScene(position).X;
        }

        private float getY()
        {
            return Constants.UnconvertFromXNAScene(position).Y;
        }

        public bool ReadyToDie()
        {
            return timeToDie;
        }

        public virtual void Draw(CameraComponent c)
        {
            if (!ReadyToDie())
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
                        be.World = GetWorld() * mesh.ParentBone.Transform;
                    }

                    mesh.Draw();
                }
            }
        }

        public virtual Matrix GetWorld()
        {
            Vector3 hackedPos = Constants.ConvertToXNAScene(Constants.UnconvertFromXNAScene(position) + Constants.MINION_POSITION_HACK);
            return Matrix.CreateTranslation(hackedPos);
        }
    }
}
