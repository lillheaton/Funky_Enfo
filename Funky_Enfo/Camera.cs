using Microsoft.Xna.Framework;

namespace FunkyEnfo
{
    public class Camera
    {
        private float zoom;
        public float Zoom { get { return zoom; } set { zoom = MathHelper.Clamp(value, 0.5f, 1.2f); } }
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }

        public Camera()
        {
            Zoom = 1;
            Position = Vector2.Zero;
            Rotation = 0;
            Origin = Vector2.Zero;
            Position = Vector2.Zero;
        }

        public void Move(Vector2 direction)
        {
            // Revert the direction so it feels like 0,0 is still in the top left corner
            Position += -direction;
        }

        public Matrix GetViewMatrix()
        {
            var translationMatrix = Matrix.CreateTranslation(new Vector3(Position.X, Position.Y, 0));
            var rotationMatrix = Matrix.CreateRotationZ(Rotation);
            var scaleMatrix = Matrix.CreateScale(new Vector3(Zoom, Zoom, 1));
            var originMatrix = Matrix.CreateTranslation(new Vector3(Origin.X, Origin.Y, 0));

            return translationMatrix * rotationMatrix * scaleMatrix * originMatrix;
        }

        public Vector2 ScreenToWorld(Vector2 vec)
        {
            return Vector2.Transform(vec, Matrix.Invert(GetViewMatrix()));
        }
    }
}
