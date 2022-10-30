using ConsoleApp4.OpenGL;
using System.Collections.Generic;

namespace ConsoleApp4.Resouces
{
    class Model
    {
        public RawMesh mesh;
        public string name;
        public List<Model> childModels;

        public Model()
        {
        }

        public Model(string name, RawMesh mesh)
        {
            this.name = name;
            this.mesh = mesh;
        }

        public bool isRoot()
        {
            if (childModels.Count != 0)
                return true;
            else return false;
        }
    }
}
