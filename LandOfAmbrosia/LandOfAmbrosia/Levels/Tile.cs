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
        public Matrix world = Matrix.Identity;

        //TODO
        public int tileSize;

        /// <summary>
        /// Constructs a new tile at the given location, using the given model as the object drawn
        /// </summary>
        /// <param name="location"></param>
        /// <param name="model"></param>
        public Tile(Vector3 location, Model model)
        {
            this.location = location;
            this.model = model;
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
        public void Draw(Camera c)
        {
            if (IsEmpty()) return;
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

        public bool CollidesWith(ICollidable other)
        {
            throw new NotImplementedException();
        }
    }
}
