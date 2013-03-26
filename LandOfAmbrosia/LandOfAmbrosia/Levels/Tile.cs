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

        public Tile(Vector3 location, Model model)
        {
            this.location = location;
            this.model = model;
        }

        public void Draw(Camera c)
        {
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
