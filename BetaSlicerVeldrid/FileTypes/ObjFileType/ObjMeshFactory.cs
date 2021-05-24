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

namespace BetaSlicer.FileTypes.ObjFileType
{
    class ObjMeshFactory
    {
        private static string GetMtlFileName(string objFile)
        {
            FileInfo fi = new FileInfo(objFile);
            string fullPathWithoutExtension = Path.GetFileNameWithoutExtension(objFile);

            string mtlFileName = fullPathWithoutExtension + ".mtl";
            return mtlFileName;
        }

        private static string HasMtlFile(string objFileName)
        {
            string mtlFileName = GetMtlFileName(objFileName);
            if (File.Exists(mtlFileName))
            {
                return mtlFileName;
            }

            return null;
        }

        // AssetHelper.GetPath("cat.obj")
        public static IEnumerable<TexturedMesh> GetMeshesFromFile(string fileName)
        {
            ObjParser parser = new ObjParser();
            using (FileStream objStream = File.OpenRead(fileName))
            {
                ObjFile atriumFile = parser.Parse(objStream);
                MtlFile atriumMtls = null;

                bool hasMtl = (HasMtlFile(fileName) != null) ? true : false;

                if (hasMtl)
                {
                    using (FileStream mtlStream = File.OpenRead(AssetHelper.GetPath("Models/SponzaAtrium/sponza.mtl")))
                    {
                        atriumMtls = new MtlParser().Parse(mtlStream);
                    }
                }

                foreach (ObjFile.MeshGroup group in atriumFile.MeshGroups)
                {
                    // NOTE: Models with MTL files are not tested

                    Vector3 scale = new Vector3(1f);
                    ConstructedMeshInfo mesh = atriumFile.GetMesh(group);
                    MaterialDefinition materialDef = null;
                    if (hasMtl)
                        materialDef = atriumMtls.Definitions[mesh.MaterialName];

                    ImageSharpTexture overrideTextureData = null;
                    ImageSharpTexture alphaTexture = null;

                    MaterialPropsAndBuffer materialProps = CommonMaterials.Brick;
                    if (hasMtl)
                    {
                        if (materialDef.DiffuseTexture != null)
                        {
                            string texturePath = AssetHelper.GetPath("Models/SponzaAtrium/" + materialDef.DiffuseTexture);
                            overrideTextureData = LoadTexture(texturePath, true);
                        }
                        if (materialDef.AlphaMap != null)
                        {
                            string texturePath = AssetHelper.GetPath("Models/SponzaAtrium/" + materialDef.AlphaMap);
                            alphaTexture = LoadTexture(texturePath, false);
                        }
                        if (materialDef.Name.Contains("vase"))
                        {
                            materialProps = CommonMaterials.Vase;
                        }
                    }

                    if (group.Name == "sponza_117")
                    {
                        MirrorMesh.Plane = Plane.CreateFromVertices(
                            atriumFile.Positions[group.Faces[0].Vertex0.PositionIndex] * scale.X,
                            atriumFile.Positions[group.Faces[0].Vertex1.PositionIndex] * scale.Y,
                            atriumFile.Positions[group.Faces[0].Vertex2.PositionIndex] * scale.Z);
                        materialProps = CommonMaterials.Reflective;
                    }

                    TexturedMesh texturedMesh = CreateTexturedMesh(mesh, overrideTextureData, alphaTexture, materialProps, Vector3.Zero, Quaternion.Identity, scale, group.Name);
                    yield return texturedMesh;
                }
            }
        }

        private static TexturedMesh CreateTexturedMesh(
                 MeshData meshData,
                 ImageSharpTexture texData,
                 ImageSharpTexture alphaTexData,
                 MaterialPropsAndBuffer materialProps,
                 Vector3 position,
                 Quaternion rotation,
                 Vector3 scale,
                 string name)
        {
            TexturedMesh mesh = new TexturedMesh(name, meshData, texData, alphaTexData, materialProps ?? CommonMaterials.Brick);
            mesh.Transform.Position = position;
            mesh.Transform.Rotation = rotation;
            mesh.Transform.Scale = scale;
            return mesh;
        }

        private static ImageSharpTexture LoadTexture(string texturePath, bool mipmap) // Plz don't call this with the same texturePath and different mipmap values.
        {
            // Maybe implement texture caching again

            //if (!_textures.TryGetValue(texturePath, out ImageSharpTexture tex))
            //{
            //    tex = new ImageSharpTexture(texturePath, mipmap, true);
            //    _textures.Add(texturePath, tex);
            //}

            ImageSharpTexture tex = new ImageSharpTexture(texturePath, mipmap, true);

            return tex;
        }
    }
}
