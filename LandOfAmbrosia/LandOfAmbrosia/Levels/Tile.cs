using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LandOfAmbrosia.Characters;

namespace LandOfAmbrosia.Levels
{
    class Tile : ICollidable
    {
        public Vector3 location;
        public Model model;
        public Matrix world;

        private Matrix scale = Matrix.CreateScale(0.75f);

        //TODO
        public int tileSize;

        /// <summary>
        /// Constructs a new tile at the given location, using the given model as the object drawn
        /// </summary>
        /// <param name="location"></param>
        /// <param name="model"></param>
        public Tile(Model model, Vector3 location)
        {
            this.location = location;
            this.model = model;
            this.world = Matrix.CreateTranslation(location);
        }

        /// <summary>
        /// Method to check whether this tile contains any object at all, or if it is just empty
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return model == null;
        }

        /// <summary>
        /// Draws this tile using the given the camera
        /// </summary>
        /// <param name="c"></param>
        public void Draw(CameraComponent c)
        {
            if (IsEmpty()) return;
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect be in mesh.Effects)
                {
                    be.EnableDefaultLighting();
                    be.Projection = c.ProjectionMatrix;
                    be.View = c.ViewMatrix;
                    be.World = scale * world * mesh.ParentBone.Transform;
                }

                mesh.Draw();
            }
        }

        public bool CollidesWith(ICollidable other)
        {
            throw new NotImplementedException();
        }
    }
}
