using ImGuiNET;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameEngine.GUI
{
    public class GuiSystem
    {
        public ImGuiController controller;
        public GuiSystem() 
        {
            controller = new ImGuiController(1200,675);

        }
        public void WindowResize(int width,int height) 
        {
            controller.WindowResized(width, height);
        }
        public void Update(NativeWindow window,float deltaTime)
        {
            controller.Update(window,deltaTime);
        }
        public void TextInput(char letter) 
        {
            controller.PressChar(letter);
        }
        public void Render()
        {
            /* ImGui.NewFrame();

             ImGui.Begin("Hello");

             ImGui.SetWindowSize(new System.Numerics.Vector2(200,300));
             ImGui.Text("World");
             ImGui.End();

             ImGui.EndFrame();*/

            //ImGui.ShowDemoWindow();
            controller.Render();
        }

    }
}
