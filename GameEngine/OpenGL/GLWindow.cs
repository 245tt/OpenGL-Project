using ConsoleApp4.OpenGL.lights;
using GameEngine.GUI;
using GameEngine.OpenGL;
using GameEngine.OpenGL.lights;
using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp4.OpenGL
{
    public class GLWindow
    {
        public NativeWindow window;
        public float[] verticesdata = new float[]
        {
            //bottom
            -0.5f,-0.5f,-0.5f,
            -0.5f,-0.5f, 0.5f,
             0.5f,-0.5f,-0.5f,
             0.5f,-0.5f, 0.5f,  

            //top
            -0.5f, 0.5f, 0.5f,
            -0.5f, 0.5f,-0.5f,
             0.5f, 0.5f, 0.5f,
             0.5f, 0.5f,-0.5f,

            //back
            -0.5f,-0.5f, 0.5f,
            -0.5f, 0.5f, 0.5f,
             0.5f,-0.5f, 0.5f,
             0.5f, 0.5f, 0.5f,

            //front
            -0.5f, 0.5f, -0.5f,
            -0.5f,-0.5f, -0.5f,
             0.5f, 0.5f, -0.5f,
             0.5f,-0.5f, -0.5f,

             //left
             0.5f, 0.5f, -0.5f,
             0.5f,-0.5f, -0.5f,
             0.5f, 0.5f, 0.5f,
             0.5f,-0.5f, 0.5f,

             //right
            -0.5f, 0.5f, -0.5f,
            -0.5f,-0.5f, -0.5f,
            -0.5f, 0.5f, 0.5f,
            -0.5f,-0.5f, 0.5f,






        };

        public uint[] triangles = new uint[]
        {
            //bottom
            0,2,1,
            1,2,3,

            //top
            4,6,5,
            5,6,7,

            //back
            8,10,9,
            9,10,11,

            //front
            12,14,13,
            13,14,15,

            //left
            16,18,17,
            17,18,19,

            //right
            20,21,22,
            21,23,22,

        };
        Vector3 lightPos = new Vector3(1.0f, 0.2f, 2f);


        int vbo, ebo, vao;
        GLTexture texture1;
        GLShader shader;
        public Queue<RawMesh> renderQueue;

        GuiSystem guiSystem;
        public GLWindow()
        {
            renderQueue = new Queue<RawMesh>();
            NativeWindowSettings setting = new NativeWindowSettings()
            {
                Title = "Window",
                Size = new Vector2i(1200, 675),
            };
            window = new NativeWindow(setting);
            //window.WindowState = OpenTK.Windowing.Common.WindowState.Fullscreen; window.Size = new Vector2i(1920,1080); GL.Viewport(0,0,1920,1080);
           // window.CursorState = OpenTK.Windowing.Common.CursorState.Grabbed;
            window.Resize += Window_Resize;
            window.Closing += Window_Closing;
            window.TextInput += Window_TextInput;
           // GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.DepthTest);

            guiSystem = new GuiSystem();

            //setting main buffers
            vbo = GL.GenBuffer();
            ebo = GL.GenBuffer();
            vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);

            shader = new GLShader(@"C:\Users\a\Documents\GitHub\OpenGL-Project\GameEngine\OpenGL\shader\vertex.vert",
                @"C:\Users\a\Documents\GitHub\OpenGL-Project\GameEngine\OpenGL\shader\fragment.frag");

            texture1 = new GLTexture("C:/Users/a/source/repos/ConsoleApp4/ConsoleApp4/OpenGL/Textures/test.png");
            texture1.Use(TextureUnit.Texture0);
        }

        private void Window_TextInput(OpenTK.Windowing.Common.TextInputEventArgs obj)
        {
            guiSystem.TextInput((char)obj.Unicode);
        }

        private void Window_Closing(System.ComponentModel.CancelEventArgs obj)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.DeleteBuffer(vbo);
            GL.DeleteBuffer(ebo);
            GL.DeleteVertexArray(vao);
            GL.DeleteProgram(shader.Handle);
            GL.DeleteTexture(texture1.Handle);
            Core.IsActive = false;
        }

        public void Update(GLCamera camera,LightList lights)
        {
            window.ProcessEvents();
            Core.keyboardState = window.KeyboardState; //pass keyboard state to Core
            Core.mouseState = window.MouseState;
            //clear screen
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(0.5f, 0.5f, 0.8f, 1.0f);
           

            Draw(camera,lights);
            DrawGUI();
            renderQueue.Clear();
            //show next frame
            window.Context.SwapBuffers();
           
        }


        private void Window_Resize(OpenTK.Windowing.Common.ResizeEventArgs e)
        {
            GL.Viewport(0, 0, window.Size.X, window.Size.Y);
            guiSystem.WindowResize(window.Size.X, window.Size.Y);
        }

        void DrawGUI() 
        {
            GL.Disable(EnableCap.DepthTest);

            guiSystem.controller.Update(window,(float)Core.deltaTime);
            //guiSystem.Update(window, (float)Core.deltaTime);

            //ImGui.ShowDemoWindow();

            guiSystem.Render();

            ImGuiController.CheckGLError("End of frame");

            GL.Enable(EnableCap.DepthTest);
        }

        void Draw(GLCamera camera,LightList lights)
        {
            foreach (RawMesh mesh in renderQueue)
            {
                List<float> vertexData = new List<float>();
                for (int i = 0; i < mesh.vertPos.Length / 3; i++)
                {
                    vertexData.Add(mesh.vertPos[3 * i]);//X
                    vertexData.Add(mesh.vertPos[3 * i + 1]);//Y
                    vertexData.Add(mesh.vertPos[3 * i + 2]);//Z

                    vertexData.Add(mesh.vertTex[2 * i]);//U
                    vertexData.Add(mesh.vertTex[2 * i + 1]);//V

                    vertexData.Add(mesh.vertNorm[3 * i]);//X
                    vertexData.Add(mesh.vertNorm[3 * i + 1]);//Y
                    vertexData.Add(mesh.vertNorm[3 * i + 2]);//Z
                }

                //GL.BindVertexArray(vao);
                //GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
                //GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);

                GL.BufferData(BufferTarget.ArrayBuffer, vertexData.Count * sizeof(float), vertexData.ToArray(), BufferUsageHint.StaticDraw);
                GL.BufferData(BufferTarget.ElementArrayBuffer, triangles.Length * sizeof(uint), triangles, BufferUsageHint.StaticDraw);

               
                shader.Use();
                texture1.Use(TextureUnit.Texture0);

                //light properties
                int lightPosLoc = GL.GetUniformLocation(shader.Handle, "lightPos");
                GL.Uniform3(lightPosLoc, lightPos);

                //int lightColLoc = GL.GetUniformLocation(shader.Handle, "lightColor");
                //GL.Uniform3(lightColLoc, new Vector3(1.0f,1.0f,1.0f));

                int viewPosLoc = GL.GetUniformLocation(shader.Handle, "viewPos");
                GL.Uniform3(viewPosLoc, camera.Position);

                Matrix4 model = Matrix4.Identity;
                model *= Matrix4.CreateScale(mesh.scale);
                model *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(mesh.rot.X));
                model *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(mesh.rot.Y));
                model *= Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(mesh.rot.Z));
                model *= Matrix4.CreateTranslation(mesh.pos);

                //material properties
                shader.SetVector3("material.ambient",mesh.material.ambient);
                shader.SetVector3("material.diffuse", mesh.material.diffuse);
                shader.SetVector3("material.specular", mesh.material.specular);
                shader.SetFloat("material.shininess", mesh.material.shininess);

                //dir light
                for (int i = 0; i < lights.directionalLights.Length; i++)
                {
                    shader.SetVector3("dirLights.direction", lights.directionalLights.ElementAt(i).direction);
                    shader.SetVector3("dirLights.ambient", lights.directionalLights.ElementAt(i).ambient);
                    shader.SetVector3("dirLights.diffuse", lights.directionalLights.ElementAt(i).diffuse);
                    shader.SetVector3("dirLights.specular", lights.directionalLights.ElementAt(i).specular);
                }
                //point lights
                for (int i = 0; i < lights.pointLights.Length;i++) 
                {
                    shader.SetVector3("pointLights[" + i + "].position", lights.pointLights.ElementAt(i).position);
                    shader.SetVector3("pointLights[" + i + "].ambient", lights.pointLights.ElementAt(i).ambient);
                    shader.SetVector3("pointLights[" + i + "].diffuse", lights.pointLights.ElementAt(i).diffuse);
                    shader.SetVector3("pointLights[" + i + "].specular", lights.pointLights.ElementAt(i).specular);
                    shader.SetFloat("pointLights[" + i + "].fallOff", lights.pointLights.ElementAt(i).fallOff);
                }

                //spot lights
                for (int i = 0; i < lights.spotLights.Length; i++)
                {
                    shader.SetVector3("spotLights[" + i + "].position", lights.spotLights.ElementAt(i).position);
                    shader.SetVector3("spotLights[" + i + "].ambient", lights.spotLights.ElementAt(i).ambient);
                    shader.SetVector3("spotLights[" + i + "].diffuse", lights.spotLights.ElementAt(i).diffuse);
                    shader.SetVector3("spotLights[" + i + "].specular", lights.spotLights.ElementAt(i).specular);
                    shader.SetVector3("spotLights[" + i + "].direction", lights.spotLights.ElementAt(i).direction);
                    shader.SetFloat("spotLights[" + i + "].fallOff", lights.spotLights.ElementAt(i).fallOff);
                    shader.SetFloat("spotLights[" + i + "].cutOff", lights.spotLights.ElementAt(i).cutOff);
                    shader.SetFloat("spotLights[" + i + "].outerCutOff", lights.spotLights.ElementAt(i).outerCutOff);

                }

                //mvp matrices
                shader.SetMatrix4("model", model);
                shader.SetMatrix4("view", camera.GetViewMatrix());
                shader.SetMatrix4("projection", camera.GetProjectionMatrix());


                //vertex pos
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
                GL.EnableVertexAttribArray(0);

                //texture Coords
                GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
                GL.EnableVertexAttribArray(1);

                //normals
                GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 5 * sizeof(float));
                GL.EnableVertexAttribArray(2);


                shader.Use();

                //GL.DrawArrays(PrimitiveType.Triangles,0,3);
                GL.DrawElements(BeginMode.Triangles, triangles.Length, DrawElementsType.UnsignedInt, 0);
                
            }
        }
    }
}
