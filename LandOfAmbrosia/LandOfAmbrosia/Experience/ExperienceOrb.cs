using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using LandOfAmbrosia.Common;

namespace LandOfAmbrosia.Experience
{
    class ExperienceOrb
    {

        private readonly int LIFETIME = 10000;
        private int timeLeft;

        public bool isAlive;

        public Model model
        {
            get;
            protected set;
        }

        public Vector3 position;

        public ExperienceOrb(Vector3 position, Model model)
        {
            this.model = model;
            this.position = position;
            timeLeft = LIFETIME;
            isAlive = true;
        }

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
                    be.World = GetWorld() * mesh.ParentBone.Transform;
                }

                mesh.Draw();
            }
        }

        public void Update(GameTime gameTime)
        {
            if (timeLeft <= 0)
            {
                this.isAlive = false;
            }
            timeLeft -= gameTime.ElapsedGameTime.Milliseconds;
        }

        private Matrix GetWorld()
        {
            return Matrix.Identity * Matrix.CreateTranslation(Constants.ConvertToXNAScene(position));
        }
    }
}
