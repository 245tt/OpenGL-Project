using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4.OpenGL.lights
{
    public struct SpotLight :IGLLight
    {
        public Vector3 ambient;
        public Vector3 diffuse;
        public Vector3 specular;
        public Vector3 position;
        public Vector3 direction;
        public float fallOff;

        //in degrees
        public float cutOff;
        public float outerCutOff;
    }
}
