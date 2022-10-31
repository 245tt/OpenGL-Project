using ConsoleApp4.OpenGL;
using ConsoleApp4.OpenGL.lights;
using ConsoleApp4.Resouces.models;
using GameEngine.OpenGL;
using GameEngine.OpenGL.lights;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
namespace ConsoleApp4
{
    public sealed class Core
    {
        public static Stopwatch stopwatch { get; private set; }
        public double time;
        public static double deltaTime;
        public static GLWindow window { get; private set; }
        public static KeyboardState keyboardState { get; set; }
        public static MouseState mouseState { get; set; }
        public static bool IsActive = true;
        private Core() { }

        public void Init()
        {
            //initializing components
            window = new GLWindow();
            stopwatch = new Stopwatch();
            stopwatch.Start();

            Run();
        }

        private static Core instance;

        public static Core GetInstance()
        {
            if (instance == null)
            {
                instance = new Core();
            }
            return instance;
        }

        private void Run()
        {

            GLCamera cam = new GLCamera(90f);
            cam.Position.Y = 0.2f;

            RawMesh mesh = new RawMesh()
            {
                vertPos = window.verticesdata,
                vertTex = texCoords,
                vertNorm = normals,

                triangles = window.triangles,
                scale = new Vector3(6f, 0.1f, 6f),
                material = new Material
                {
                    specular = new Vector3(1.0f),
                    diffuse = new Vector3(1.0f),
                    ambient = new Vector3(0.3f),
                    shininess = 8f,
                },
            };



            PointLight pointLight = new PointLight()
            {
                position = new Vector3(0, 1f, 3f),
                ambient = new Vector3(0.0f),
                diffuse = Vector3.One,
                specular = Vector3.One,
                fallOff = 1.0f,
            };

            PointLight pointLight2 = new PointLight()
            {
                position = new Vector3(2f, 1f, 2),
                ambient = new Vector3(0.0f),
                diffuse = new Vector3(0.0f, 1.0f, 0.4f),
                specular = new Vector3(0.0f, 1.0f, 0.4f),
                fallOff = 1.0f,
            };

            SpotLight spotLight = new SpotLight()
            {
                position = new Vector3(1f, 2f, -2f),
                direction = new Vector3(0, -1f, 0.4f),
                ambient = new Vector3(0.3f),
                diffuse = new Vector3(1.0f, 0.4f, 0.4f),
                specular = Vector3.One,
                fallOff = 1.0f,
                cutOff = (float)Math.Cos(MathHelper.DegreesToRadians(30f)),
                outerCutOff = (float)Math.Cos(MathHelper.DegreesToRadians(50f)),
            };

            DirectionalLight directionalLight = new DirectionalLight()
            {
                direction = new Vector3(0f, 1f, 0.5f),
                ambient = new Vector3(0.0f),
                diffuse = new Vector3(0.0f, 0.4f, 0.4f),
                specular = new Vector3(0.0f, 0.4f, 0.4f),

            };
            LightList lights = new LightList();
            lights.pointLights = new PointLight[] { pointLight, pointLight2 };
            lights.spotLights = new SpotLight[] { spotLight };
            lights.directionalLights = new DirectionalLight[] { };// {directionalLight };



           

            while (IsActive)
            {
                time = stopwatch.Elapsed.TotalMilliseconds;

                //render
                window.renderQueue.Enqueue(mesh);
                //renderer.renderQueue.Enqueue(light);

                window.Update(cam,lights);

                //update
                mesh.rot.Y += 0.1f;
                mesh.pos.X = (float)Math.Sin(time/1000f);
                //mesh.pos.Y += 0.001f;

                Console.WriteLine(mouseState.Position);

                Vector3 front = new Vector3((float)Math.Cos(MathHelper.DegreesToRadians(cam.Rotation.Y)), 0, (float)Math.Sin(MathHelper.DegreesToRadians(cam.Rotation.Y))) * 0.01f;
                Vector3 right = Vector3.Cross(front, Vector3.UnitY);
                if (keyboardState.IsKeyDown(Keys.W))
                {
                    //cam.Position.X += (float)Math.Cos(MathHelper.DegreesToRadians( cam.Rotation.Y)) * 0.01f;
                    //cam.Position.Z += (float)Math.Sin(MathHelper.DegreesToRadians(cam.Rotation.Y)) * 0.01f;
                    cam.Position += front;

                }
                if (keyboardState.IsKeyDown(Keys.S))
                {
                    //cam.Position.X -= (float)Math.Cos(MathHelper.DegreesToRadians(cam.Rotation.Y)) * 0.01f;
                    //cam.Position.Z -= (float)Math.Sin(MathHelper.DegreesToRadians(cam.Rotation.Y)) * 0.01f;
                    cam.Position -= front;
                }

                if (keyboardState.IsKeyDown(Keys.D))
                {
                    //cam.Position.X += (float)Math.Sin(MathHelper.DegreesToRadians(cam.Rotation.Y)) * 0.01f;
                    //cam.Position.Z += (float)Math.Cos(MathHelper.DegreesToRadians(cam.Rotation.Y)) * 0.01f;
                    cam.Position += right;
                }
                if (keyboardState.IsKeyDown(Keys.A))
                {
                    //cam.Position.X -= (float)Math.Sin(MathHelper.DegreesToRadians(cam.Rotation.Y)) * 0.01f;
                    //cam.Position.Z -= (float)Math.Cos(MathHelper.DegreesToRadians(cam.Rotation.Y)) * 0.01f;
                    cam.Position -= right;
                }

                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    //cam.Position.X -= (float)Math.Sin(MathHelper.DegreesToRadians(cam.Rotation.Y)) * 0.01f;
                    //cam.Position.Z -= (float)Math.Cos(MathHelper.DegreesToRadians(cam.Rotation.Y)) * 0.01f;
                    cam.Position.Y += 0.01f;
                }
                if (keyboardState.IsKeyDown(Keys.LeftControl))
                {
                    //cam.Position.X -= (float)Math.Sin(MathHelper.DegreesToRadians(cam.Rotation.Y)) * 0.01f;
                    //cam.Position.Z -= (float)Math.Cos(MathHelper.DegreesToRadians(cam.Rotation.Y)) * 0.01f;
                    cam.Position.Y -= 0.01f;
                }


                cam.Rotation.Y += mouseState.Delta.X;
                cam.Rotation.X -= mouseState.Delta.Y;


                if (keyboardState.IsKeyDown(Keys.Escape))
                {
                    IsActive = false;
                }

                double newTime = stopwatch.Elapsed.TotalMilliseconds - time;
                deltaTime = newTime;
                double newfps = 1d / newTime * 1000;
                window.window.Title = "FPS: " + newfps;
            }
        }


        public float[] texCoords = new float[]
        {
            0.0f,1.0f,
            0.0f,0.0f,
            1.0f,1.0f,
            1.0f,0.0f,

            0.0f,1.0f,
            0.0f,0.0f,
            1.0f,1.0f,
            1.0f,0.0f,

            0.0f,1.0f,
            0.0f,0.0f,
            1.0f,1.0f,
            1.0f,0.0f,

            0.0f,1.0f,
            0.0f,0.0f,
            1.0f,1.0f,
            1.0f,0.0f,

            0.0f,1.0f,
            0.0f,0.0f,
            1.0f,1.0f,
            1.0f,0.0f,

            0.0f,1.0f,
            0.0f,0.0f,
            1.0f,1.0f,
            .1f,0.0f,
        };
        public float[] normals = new float[]
        {
            //bottom
            0.0f,-1.0f,0.0f,
            0.0f,-1.0f,0.0f,
            0.0f,-1.0f,0.0f,
            0.0f,-1.0f,0.0f,

            //top
            0.0f,1.0f,0.0f,
            0.0f,1.0f,0.0f,
            0.0f,1.0f,0.0f,
            0.0f,1.0f,0.0f,

            //back
            0.0f,0.0f,1.0f,
            0.0f,0.0f,1.0f,
            0.0f,0.0f,1.0f,
            0.0f,0.0f,1.0f,

            //front
            0.0f,0.0f,-1.0f,
            0.0f,0.0f,-1.0f,
            0.0f,0.0f,-1.0f,
            0.0f,0.0f,-1.0f,

            //left
           1.0f,0.0f,0.0f,
           1.0f,0.0f,0.0f,
           1.0f,0.0f,0.0f,
           1.0f,0.0f,0.0f,
          
             //right
           -1.0f,0.0f,0.0f,
           -1.0f,0.0f,0.0f,
           -1.0f,0.0f,0.0f,
           -1.0f,0.0f,0.0f,
        };
    }
}
