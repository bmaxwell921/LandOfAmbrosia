using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LandOfAmbrosia.Characters;
using LandOfAmbrosia.Common;

namespace LandOfAmbrosia.Levels
{
    class Tile : ICollidable
    {
        public Vector3 location;
        public Model model;

        /// <summary>
        /// Constructs a new tile at the given location, using the given model as the object drawn
        /// </summary>
        /// <param name="location"></param>
        /// <param name="model"></param>
        public Tile(Model model, Vector3 location)
        {
            //Location is the actual location in 3D space
            this.location = Constants.ConvertToXNAScene(new Vector3(location.X * Constants.TILE_WIDTH, location.Y * Constants.TILE_HEIGHT, 0));
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
                    be.World = GetWorld() * mesh.ParentBone.Transform;
                }

                mesh.Draw();
            }
        }

        public Matrix GetWorld()
        {
            return Matrix.Identity * Matrix.CreateTranslation(location);
        }

        public bool CollidesWith(ICollidable other)
        {
            throw new NotImplementedException();
        }
    }
}
