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

        private readonly float DEFAULT_SPEED = 2;

        public Projectile(Model model, Vector3 position, Character target)
        {
            this.model = model;
            this.position = Constants.ConvertToXNAScene(position);
            this.target = target;
            this.UpdateVelocity();
        }

        private void UpdateVelocity()
        {
            Vector3 normalDirection = Vector3.Normalize(target.position - this.position);
            velocity = normalDirection * DEFAULT_SPEED;
        }

        public virtual void Update(GameTime gameTime)
        {
            UpdateVelocity();

            position += velocity;
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
            return Matrix.CreateScale(0.25f) * Matrix.CreateTranslation(position);
        }
    }
}
