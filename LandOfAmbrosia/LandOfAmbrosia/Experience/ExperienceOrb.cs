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

        public int width, height;

        public bool isAlive;

        public int amount;

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
            amount = 25;
            width = 1;
            height = 1;
        }

        public void Draw(CameraComponent c)
        {
            if (!isAlive)
            {
                return;
            }
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
            if (!isAlive)
            {
                return;
            }
            if (timeLeft <= 0)
            {
                this.isAlive = false;
            }
            timeLeft -= gameTime.ElapsedGameTime.Milliseconds;
        }

        private Matrix GetWorld()
        {
            return Matrix.Identity * Matrix.CreateTranslation(Constants.ConvertToXNAScene(position) + Constants.MINION_POSITION_HACK);
        }
    }
}
