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
        //Just the indices into the array
        public Vector3 location;
        public Model model;
        public int width
        {
            get
            {
                return Constants.TILE_SIZE;
            }
        }

        public int height
        {
            get
            {
                return Constants.TILE_SIZE;
            }
        }

        /// <summary>
        /// Constructs a new tile at the given location, using the given model as the object drawn
        /// </summary>
        /// <param name="location">The actual location in XNA 3D space</param>
        /// <param name="model"></param>
        public Tile(Model model, Vector3 location)
        {
            //Location is the actual location in 3D space
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
            Vector3 realLoc = Constants.ConvertToXNAScene(new Vector3(location.X, location.Y, 0));
            return Matrix.Identity * Matrix.CreateTranslation(realLoc);
        }

        public bool CollidesWith(ICollidable other)
        {
            throw new NotImplementedException();
        }

        public float getX()
        {
            //return Constants.UnconvertFromXNAScene(location).X;
            return location.X;
        }

        public float getY()
        {
            //return Constants.UnconvertFromXNAScene(location).Y;
            return location.Y;
        }
    }
}
