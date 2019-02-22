using System;

//.SKN reference type ported to C# code
//ToString()-methods are for debugging, ToOBJString()-methods are used to convert (League Of Legends model .SKN) to (WaveFront .OBJ)

namespace SKN_to_OBJ
{
    /// <summary>
    /// .SKN file structure
    /// </summary>
    public class SKN
    {
        /// <summary>
        /// Magic number used in the header of .SKN model files
        /// </summary>
        public int Magic { get; set; }
        /// <summary>
        /// .SKN file version, currently ranging from 1 to 4 (4 being the most recent as of 17/11/2016)
        /// </summary>
        public ushort Version { get; set; }
        /// <summary>
        /// Object count
        /// </summary>
        public ushort NumObjects { get; set; }
        public SkinContent SkinContent { get; set; }

        public SKN(int magic, ushort version, ushort numObjects)
        {
            Magic = magic;
            Version = version;
            NumObjects = numObjects;
        }
    }

    /// <summary>
    /// Custom file type containing the models' materials, indices and vertices.
    /// </summary>
    public class SkinContent
    {
        /// <summary>
        /// Material count
        /// </summary>
        public int MaterialsCount { get; set; }
        /// <summary>
        /// List of materials
        /// </summary>
        public Material[] Materials { get; set; }
        /// <summary>
        /// Irrelevant metadata and total indices/vertices count
        /// </summary>
        public MetaDataContainer MetaDataContainer { get; set; }
        /// <summary>
        /// List of indices
        /// </summary>
        public short[] Indices { get; set; }
        /// <summary>
        /// List of vertices
        /// </summary>
        public Vertex[] Vertices { get; set; }
        /// <summary>
        /// EOF (End Of File) buffer [Only exists if SKN.Version > 2]
        /// </summary>
        public byte[] Eof { get; set; }
    }

    /// <summary>
    /// Custom file type - 3D material
    /// </summary>
    public class Material
    {
        /// <summary>
        /// Material name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// First vertex
        /// </summary>
        public int StartVertex { get; set; }
        /// <summary>
        /// Count of vertices
        /// </summary>
        public int NumVertices { get; set; }
        /// <summary>
        /// First index
        /// </summary>
        public int StartIndex { get; set; }
        /// <summary>
        /// Count of indices
        /// </summary>
        public int NumIndices { get; set; }

        public Material(String name, int startVertex, int numVertices, int startIndex, int numIndices)
        {
            Name = name;
            StartVertex = startVertex;
            NumVertices = numVertices;
            StartIndex = startIndex;
            NumIndices = numIndices;
        }
    }

    /// <summary>
    /// Unknown custom file type - not needed to convert to .OBJ, also seemingly random values
    /// </summary>
    public class MetaDataContainer
    {
        /// <summary>
        /// Unknown identifier, usually defaults to 0 [ONLY EXISTS IF SKN.Version >= 4]
        /// </summary>
        public int Part1 { get; set; }
        /// <summary>
        /// Count of model indices
        /// </summary>
        public int IndicesCount { get; set; }
        /// <summary>
        /// Count of model vertices
        /// </summary>
        public int VerticesCount { get; set; }
        /// <summary>
        /// Metadata, unknown values [ONLY EXISTS IF SKN.Version >= 4]
        /// </summary>
        public byte[] MetaData { get; set; }
    }

    /// <summary>
    /// Custom file type - 3D vertex (x, y, z as 3D space, w as 3D rotation)
    /// </summary>
    public class Vertex
    {

        public Position Position { get; set; }

        public BoneIndex BoneIndex { get; set; }
        public Weight Weight { get; set; }

        public Normals Normals { get; set; }

        public UV UV { get; set; }

        public Vertex(float px, float py, float pz, byte bx, byte by, byte bz, byte bw, float wx, float wy, float wz, float ww, float nx, float ny, float nz, float u, float v)
        {
            Position = new Position(px, py, pz);

            BoneIndex = new BoneIndex(bx, by, bz, bw);

            Weight = new Weight(wx, wy, wz, ww);

            Normals = new Normals(nx, ny, nz);

            UV = new UV(u, v);
        }
    }

    /// <summary>
    /// 3D vertex position (x, y, z)
    /// </summary>
    public struct Position
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Position(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Create a string by concatenating x, y and z  for debug purposes
        /// </summary>
        /// <returns>Concatenated x, y, z floats as string</returns>
        public override string ToString()
        {
            return "{x: " + X.ToStringGB() + ", y: " + Y.ToStringGB() + ", z: " + Z.ToStringGB() + "}";
        }

        /// <summary>
        /// Create a string from x, y and z as they appear in WaveFront .OBJ files
        /// </summary>
        /// <returns>WaveFront .OBJ "v"/vertex parameter line</returns>
        public string ToOBJString()
        {
            return "v " + X.ToStringGB() + " " + Y.ToStringGB() + " " + Z.ToStringGB();
        }
    }

    /// <summary>
    /// Index of parent bone (x, y, z, w)
    /// </summary>
    public struct BoneIndex
    {
        public byte X { get; set; }
        public byte Y { get; set; }
        public byte Z { get; set; }
        public byte W { get; set; }

        public BoneIndex(byte x, byte y, byte z, byte w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public override string ToString()
        {
            return "{x: " + X.ToStringGB() + ", y: " + Y.ToStringGB() + ", z: " + Z.ToStringGB() + ", w: " + W.ToStringGB() + "}";
        }
    }

    /// <summary>
    /// Bone weight (x, y, z, w)
    /// </summary>
    public struct Weight
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }

        public Weight(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public override string ToString()
        {
            return "{x: " + X.ToStringGB() + ", y: " + Y.ToStringGB() + ", z: " + Z.ToStringGB() + ", w: " + W.ToStringGB() + "}";
        }
    }

    /// <summary>
    /// Normals position (x, y, z)
    /// </summary>
    public struct Normals
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Normals(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString()
        {
            return "{x: " + X.ToStringGB() + ", y: " + Y.ToStringGB() + ", z: " + Z.ToStringGB() + "}";
        }

        public string ToOBJString()
        {
            return "vn " + X.ToStringGB() + " " + Y.ToStringGB() + " " + Z.ToStringGB();
        }
    }

    /// <summary>
    /// UV position (u, v)
    /// </summary>
    public struct UV
    {
        public float U { get; set; }
        public float V { get; set; }

        public UV(float u, float v)
        {
            U = u;
            V = v;
        }

        public override string ToString()
        {
            return "{u: " + U.ToStringGB() + ", v: " + V.ToStringGB() + "}";
        }

        public string ToOBJString()
        {
            return "vt " + U.ToStringGB() + " " + V.ToStringGB();
        }
    }

}
