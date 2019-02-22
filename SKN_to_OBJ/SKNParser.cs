using System;
using System.IO;

namespace SKN_to_OBJ
{
    public class SKNParser
    {
        public SKN ReadSKN(string path)
        {

            using (BinaryReader br = new BinaryReader(File.OpenRead(path)))
            {
                //skn
                SKN BaseSkin = new SKN(br.ReadInt32(), br.ReadUInt16(), br.ReadUInt16());

                //SkinContent
                BaseSkin.SkinContent = new SkinContent();

                if (BaseSkin.Version > 0)
                {
                    BaseSkin.SkinContent.MaterialsCount = br.ReadInt32();
                    BaseSkin.SkinContent.Materials = new Material[BaseSkin.SkinContent.MaterialsCount];

                    for (int i = 0; i < BaseSkin.SkinContent.MaterialsCount; i++)
                    {
                        Material material = new Material(new string(br.ReadChars(64)), br.ReadInt32(), br.ReadInt32(), br.ReadInt32(), br.ReadInt32());
                        BaseSkin.SkinContent.Materials[i] = material;
                    }

                }

                BaseSkin.SkinContent.MetaDataContainer = new MetaDataContainer();

                //If the SKN version is 3, 2 or 1, exclude Part1 (as it was added with version 4)
                if (BaseSkin.Version >= 4)
                {
                    //UNKS: unknown (Int32)
                    BaseSkin.SkinContent.MetaDataContainer.Part1 = br.ReadInt32();

                }


                //skinContent: indicesCount (Int32)
                BaseSkin.SkinContent.MetaDataContainer.IndicesCount = br.ReadInt32();

                //skinContent: verticesCount (Int32)
                BaseSkin.SkinContent.MetaDataContainer.VerticesCount = br.ReadInt32();

                //If the SKN version is 3, 2 or 1, exclude all MetaData information (as the MetaData was added with version 4)
                if (BaseSkin.Version >= 4)
                {

                    //UNKS: UNKSval (Int16[24])
                    BaseSkin.SkinContent.MetaDataContainer.MetaData = new byte[48];
                    for (int i = 0; i < 48; i++)
                    {
                        BaseSkin.SkinContent.MetaDataContainer.MetaData[i] = br.ReadByte();
                    }

                }

                //skinContent: indices (Int16[skinContent.indicesCount])
                BaseSkin.SkinContent.Indices = new short[BaseSkin.SkinContent.MetaDataContainer.IndicesCount];
                for (int i = 0; i < BaseSkin.SkinContent.MetaDataContainer.IndicesCount; i++)
                {
                    BaseSkin.SkinContent.Indices[i] = br.ReadInt16();
                }

                //skinContent: vertices (Vertex[skinContent.verticesCount])
                BaseSkin.SkinContent.Vertices = new Vertex[BaseSkin.SkinContent.MetaDataContainer.VerticesCount];
                for (int i = 0; i < BaseSkin.SkinContent.MetaDataContainer.VerticesCount; i++)
                {
                    Vertex vertex = new Vertex(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadByte(), br.ReadByte(), br.ReadByte(), br.ReadByte(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
                    BaseSkin.SkinContent.Vertices[i] = vertex;
                }

                if (BaseSkin.Version > 0 && BaseSkin.Version < 4)
                {
                    BaseSkin.SkinContent.Eof = br.ReadBytes(int.MaxValue);
                }

                return BaseSkin;
            }
        }

        //Write a skn object into a Wavefront OBJ file
        public void WriteOBJ(string fileDir, string fileName, SKN sourceSkin)
        {
            Console.WriteLine(String.Format(@"Writing to {0}\{1}.obj ...", fileDir, fileName));

            String output = "";
            string[] vgeom = new string[sourceSkin.SkinContent.Vertices.Length];
            string[] vUV = new string[sourceSkin.SkinContent.Vertices.Length];
            string[] vnormals = new string[sourceSkin.SkinContent.Vertices.Length];
            string[] faces = new string[sourceSkin.SkinContent.Indices.Length];


            for (int i = 0; i < sourceSkin.SkinContent.Vertices.Length; i++)
            {
                vgeom[i] = sourceSkin.SkinContent.Vertices[i].Position.ToOBJString();
                vUV[i] = sourceSkin.SkinContent.Vertices[i].UV.ToOBJString();
                vnormals[i] = sourceSkin.SkinContent.Vertices[i].Normals.ToOBJString();
            }

            for (int i = 0; i < sourceSkin.SkinContent.Indices.Length; i += 3)
            {
                faces[i] = String.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}", sourceSkin.SkinContent.Indices[i] + 1,
                                                                                    sourceSkin.SkinContent.Indices[i + 1] + 1,
                                                                                    sourceSkin.SkinContent.Indices[i + 2] + 1);
            }

            foreach (string str in vgeom)
            {
                output = output + str + "\n";
            }
            foreach (string str in vUV)
            {
                output = output + str + "\n";
            }
            foreach (string str in vnormals)
            {
                output = output + str + "\n";
            }
            foreach (string str in faces)
            {
                if (!String.IsNullOrEmpty(str))
                    output = output + str + "\n";
            }

            File.WriteAllText(String.Format(@"{0}\{1}.obj", fileDir, fileName), output);

            Console.WriteLine("Done.");
        }
    }
}
