using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AntMe.Plugin.Fna
{
    // A cube mesh for the skybox
    public class CubeMesh
    {
        const int NUMBER_OF_VERTICES = 8;
        const int NUMBER_OF_INDICES = 36;

        VertexBuffer vertices;
        IndexBuffer indices;
        GraphicsDevice graphics;

        
        public CubeMesh(GraphicsDevice graphics, Vector3 size)
        {
            this.graphics = graphics;

            Vector3[] cubeVertices = new Vector3[NUMBER_OF_VERTICES];

            cubeVertices[0] = new Vector3(-size.X, -size.Y, -size.Z);
            cubeVertices[1] = new Vector3(-size.X, -size.Y, +size.Z);
            cubeVertices[2] = new Vector3(+size.X, -size.Y, +size.Z);
            cubeVertices[3] = new Vector3(+size.X, -size.Y, -size.Z);
            cubeVertices[4] = new Vector3(-size.X, +size.Y, -size.Z);
            cubeVertices[5] = new Vector3(-size.X, +size.Y, +size.Z);
            cubeVertices[6] = new Vector3(+size.X, +size.Y, +size.Z);
            cubeVertices[7] = new Vector3(+size.X, +size.Y, -size.Z);

            VertexDeclaration VertexPositionDeclaration = new VertexDeclaration(
                new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0)
            );

            vertices = new VertexBuffer(graphics, VertexPositionDeclaration, NUMBER_OF_VERTICES, BufferUsage.WriteOnly);
            vertices.SetData<Vector3>(cubeVertices);


            UInt16[] cubeIndices = new UInt16[NUMBER_OF_INDICES];

            //bottom face
            cubeIndices[0] = 0;
            cubeIndices[1] = 2;
            cubeIndices[2] = 3;
            cubeIndices[3] = 0;
            cubeIndices[4] = 1;
            cubeIndices[5] = 2;

            //top face
            cubeIndices[6] = 4;
            cubeIndices[7] = 6;
            cubeIndices[8] = 5;
            cubeIndices[9] = 4;
            cubeIndices[10] = 7;
            cubeIndices[11] = 6;

            //front face
            cubeIndices[12] = 5;
            cubeIndices[13] = 2;
            cubeIndices[14] = 1;
            cubeIndices[15] = 5;
            cubeIndices[16] = 6;
            cubeIndices[17] = 2;

            //back face
            cubeIndices[18] = 0;
            cubeIndices[19] = 7;
            cubeIndices[20] = 4;
            cubeIndices[21] = 0;
            cubeIndices[22] = 3;
            cubeIndices[23] = 7;

            //left face
            cubeIndices[24] = 0;
            cubeIndices[25] = 4;
            cubeIndices[26] = 1;
            cubeIndices[27] = 1;
            cubeIndices[28] = 4;
            cubeIndices[29] = 5;

            //right face
            cubeIndices[30] = 2;
            cubeIndices[31] = 6;
            cubeIndices[32] = 3;
            cubeIndices[33] = 3;
            cubeIndices[34] = 6;
            cubeIndices[35] = 7;

            indices = new IndexBuffer(graphics, IndexElementSize.SixteenBits, NUMBER_OF_INDICES, BufferUsage.WriteOnly);
            indices.SetData<UInt16>(cubeIndices);

        }


        public void Draw()
        {
            graphics.SetVertexBuffer(vertices);
            graphics.Indices = indices;


            graphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, NUMBER_OF_VERTICES, 0, NUMBER_OF_INDICES / 3);

        }
    }
}
