using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandOfAmbrosia.Characters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LandOfAmbrosia.Common;

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
        //public Character target;
        
        //This vector is UNCONVERTED
        public Vector3 targetPosition;
        public Character source;
        public bool timeToDie;

        public float width, height;
        /// <summary>
        /// TargetPosition comes in as UNCONVERTED vector
        /// </summary>
        /// <param name="model"></param>
        /// <param name="position"></param>
        /// <param name="targetPosition"></param>
        //public Projectile(Model model, Vector3 position, Vector3 targetPosition)
        //{
        //    width = Constants.MAGIC_WIDTH;
        //    height = Constants.MAGIC_HEIGHT;
        //    this.model = model;
        //    this.position = Constants.ConvertToXNAScene(position);
        //    this.targetPosition = targetPosition;
        //    timeToDie = false;
        //    this.UpdateVelocity();
        //}

        public Projectile(Model model, Vector3 position, Character source, Vector3 targetPosition)
        {
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
            //Hack it into the right spot
            //Vector3 targetPos = (target is Minion) ? Constants.UnconvertFromXNAScene(target.position) + new Vector3(1, -1, 0) : Constants.UnconvertFromXNAScene(target.position);
            Vector3 myPos = Constants.UnconvertFromXNAScene(this.position);

            //Vector3 desiredVel = targetPos - myPos;
            Vector3 desiredVel = targetPosition - myPos;
            if (desiredVel.Length() < radius)
            {
                desiredVel /= 2;
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
            //Velocity goes to 0 when we  get close to the guy, so if the velocity is really small let's just say it's good enough
            if (velocity.Length() <= 0.01)
            {
                timeToDie = true;
            }
        }

        public bool ReadyToDie()
        {
            //return target == null;
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
            return Matrix.CreateTranslation(position);
        }
    }
}
