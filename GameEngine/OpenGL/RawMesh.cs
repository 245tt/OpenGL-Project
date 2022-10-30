using ConsoleApp4.Resouces.models;
using OpenTK.Mathematics;

namespace ConsoleApp4.OpenGL
{
    public struct RawMesh
    {
        public float[] vertPos; //3 floats
        public float[] vertColor; //3
        public float[] vertNorm; //3
        public float[] vertTex; //2
        public float[] vertTex2; //2
        public float[] vertTex3; //2
        public float[] vertTan; //3

        public uint[] triangles;

        public Material material;

        public Vector3 pos;
        public Vector3 rot;
        public Vector3 scale;

    }
}
