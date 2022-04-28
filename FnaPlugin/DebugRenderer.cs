using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics; // if using XNA with non-reach profile:
// using indexer = System.Int32

// if using XNA with reach profile:
using indexer = System.Int16;

// if using mono game:
// using indexer = System.UInt32


namespace AntMe.Plugin.Fna
{
    internal class DebugRenderer
    {
        BasicEffect effect;
        Camera camera;

        internal DebugRenderer(GraphicsDevice graphicsDevice, Camera camera)
        {
            this.camera = camera;
            this.effect = new BasicEffect(graphicsDevice) { VertexColorEnabled = true };
        }

        public Vector2 WorldToScreen(Vector3 worldPoint, Vector2 screenOffset)
        {
            Vector3 screen = effect.GraphicsDevice.Viewport.Project(worldPoint, camera.ProjectionMatrix, camera.ViewMatrix, Matrix.Identity);
            return new Vector2(screen.X + screenOffset.X, screen.Y + screenOffset.Y);
        }

        public void DrawLine(Vector3 start, Vector3 end, Color color)
        {
            DrawLineStrip(color, false, start, end);
        }

        public void DrawHorizontalCircle(Vector3 center, float radius, int segmentCount, Color color)
        {
            float angleStep = MathHelper.TwoPi / segmentCount;
            Vector3[] positions = new Vector3[segmentCount];

            for(int i = 0; i < segmentCount; i++)
            {
                float angle = i * angleStep;
                positions[i] = center + radius * new Vector3((float)Math.Cos(angle), 0, (float)Math.Sin(angle));
            }

            DrawLineStrip(color, true, positions);
        }

        private void DrawLineStrip(Color color, bool isClosed, params Vector3[] points)
        {
            int count = (isClosed)
                ? points.Length + 1
                : points.Length;


            VertexPositionColor[] vertices = new VertexPositionColor[count];
            indexer[] indices = new indexer[count];

            for (int i = 0; i < count; i++)
            {
                Vector3 pos = points[i % points.Length];
                vertices[i] = new VertexPositionColor(pos, color);
                indices[i] = (indexer)(i % points.Length);
            }

            effect.Projection = camera.ProjectionMatrix;
            effect.View = camera.ViewMatrix;
            effect.CurrentTechnique.Passes[0].Apply();

            effect.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, vertices, 0, count, indices, 0, count - 1);
        }


        internal void Unload()
        {
            if (effect != null)
                effect.Dispose();

            effect = null;
            camera = null;
        }
    }
}
