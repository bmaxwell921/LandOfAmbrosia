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
        public Character target;

        public Projectile(Model model, Vector3 position, Character target)
        {
            this.model = model;
            this.position = Constants.ConvertToXNAScene(position);
            this.target = target;
            this.UpdateVelocity();
        }

        //Lovingly stolen from www.cse.scu.edu. I would put the whole address, but it downloads as a ppt so I don't really know how to get it...
        private void UpdateVelocity()
        {
            //2 blocks
            float radius = 4;
            float maxSpeed = 0.5f;
            //Hack it into the right spot
            Vector3 targetPos = (target is Minion) ? Constants.UnconvertFromXNAScene(target.position) + new Vector3(1, -1, 0) : Constants.UnconvertFromXNAScene(target.position);
            Vector3 myPos = Constants.UnconvertFromXNAScene(this.position);

            Vector3 desiredVel = targetPos - myPos;
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
            UpdateVelocity();
            Vector3 tempPos = Constants.UnconvertFromXNAScene(position);
            tempPos += velocity;
            position = Constants.ConvertToXNAScene(tempPos);
        }

        public virtual void Draw(CameraComponent c)
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

        public virtual Matrix GetWorld()
        {
            return Matrix.CreateTranslation(position);
        }
    }
}
