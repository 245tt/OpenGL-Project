using OpenTK.Mathematics;
using System;

namespace ConsoleApp4.OpenGL
{
    public class GLCamera
    {
        public Vector3 Position;
        public Vector3 Rotation; //pitch //yaw //roll
        public Vector3 lookDir;
        float fov;
        public GLCamera(float fov)
        {
            this.fov = MathHelper.DegreesToRadians(fov);
            Position = Vector3.Zero;
            Rotation = Vector3.Zero;
            Rotation.Y = -MathHelper.PiOver2; // without this camera would be rotated 90degrees;
        }

        public Matrix4 GetViewMatrix()
        {
            Rotation.X = Math.Clamp(Rotation.X, -89.99f, 89.99f);
            lookDir.X = (float)Math.Cos(MathHelper.DegreesToRadians(Rotation.X)) * (float)Math.Cos(MathHelper.DegreesToRadians(Rotation.Y));
            lookDir.Y = (float)Math.Sin(MathHelper.DegreesToRadians(Rotation.X));
            lookDir.Z = (float)Math.Cos(MathHelper.DegreesToRadians(Rotation.X)) * (float)Math.Sin(MathHelper.DegreesToRadians(Rotation.Y));
            lookDir.Normalize();

            return Matrix4.LookAt(Position, Position + lookDir, Vector3.UnitY);
        }

        public Matrix4 GetProjectionMatrix()
        {
            return Matrix4.CreatePerspectiveFieldOfView(fov, 16 / 9f, 0.01f, 100f);
        }
    }
}
