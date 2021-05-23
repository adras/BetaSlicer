using BetaSlicer.FileTypes.ObjFileType;
using BetaSlicer.NeoDemo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using Veldrid.ImageSharp;
using Veldrid.NeoDemo;
using Veldrid.NeoDemo.Objects;
using Veldrid.Utilities;

namespace BetaSlicer.GUI
{
    class Something
    {
        public static void AddSponzaAtriumObjects(Scene scene, string fileName)
        {
            foreach(TexturedMesh mesh in ObjMeshFactory.GetMeshesFromFile(fileName))
            {
                scene.AddRenderable(mesh);
            }
        }
    }
}
