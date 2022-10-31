using ConsoleApp4.OpenGL.lights;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameEngine.OpenGL.lights
{
    public struct LightList
    {
        public DirectionalLight[] directionalLights;
        public SpotLight[] spotLights;
        public PointLight[] pointLights;
    }
}
