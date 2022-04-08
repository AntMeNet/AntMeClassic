using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AntMe.Plugin.Fna
{
    internal sealed class Camera
    {
        #region Constants

        private const int DISTANCE_MAX = 12000;
        private const int SCROLLDISTANCE_MAX = 1000;
        private const float CAMERAANGLE_MAX = ((float)Math.PI / 2) - 0.01f;
        private const int DISTANCE_MIN = 100;
        private const float CAMERAANGLE_MIN = 0.1f;

        #endregion

        #region Variables

        private Vector3 viewerCenter;
        private bool moveArea;
        private bool hasFocus;
        private Vector3 cameraDirection;
        private Vector3 cameraUpvector;
        private Vector3 cameraPosition;

        private int mouseX;
        private int mouseY;
        private int mouseWheel = 0;
        private int distanceMax;
        private bool rotateCamera;

        private GameWindow gameWindow;
        private Matrix projectionMatrix;

        #endregion

        #region Construction and init

        /// <summary>
        /// Creates a new instance of camera
        /// </summary>
        /// <param name="renderForm">render-Form</param>
        public Camera(GameWindow gameWindow)
        {
            this.gameWindow = gameWindow;
            this.gameWindow.ClientSizeChanged += gameWindow_ClientSizeChanged;
            gameWindow_ClientSizeChanged(null, null);

            // Reset Camera-position
            viewerCenter = new Vector3(0, 2, 0);
            cameraPosition = new Vector3(0, DISTANCE_MAX, 0);
            cameraUpvector = new Vector3(0, 1, 1);
            cameraDirection =
                new Vector3(((float)Math.PI * 3) / 2, CAMERAANGLE_MAX, DISTANCE_MAX);

            cameraUpvector.Normalize();
        }


        #endregion

        #region Form-Events

        void gameWindow_ClientSizeChanged(object sender, EventArgs e)
        {
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                        MathHelper.PiOver4,
                        (float)gameWindow.ClientBounds.Width / (float)gameWindow.ClientBounds.Height,
                        1, 50000);
        }

        public void Update(GameTime time)
        {
            MouseState mouse = Mouse.GetState();

            // Mouse Wheel
            cameraDirection.Z -= ((float)(mouse.ScrollWheelValue - mouseWheel) / 5f);
            mouseWheel = mouse.ScrollWheelValue;

            // check distance-limits
            if (cameraDirection.Z < DISTANCE_MIN)
            {
                cameraDirection.Z = DISTANCE_MIN;
            }
            else if (cameraDirection.Z > DISTANCE_MAX)
            {
                cameraDirection.Z = DISTANCE_MAX;
            }

            // Mouse buttons
            moveArea = mouse.LeftButton == ButtonState.Pressed;
            rotateCamera = mouse.RightButton == ButtonState.Pressed;

            // calculate deltas
            int deltaX = mouse.X - mouseX;
            int deltaY = mouse.Y - mouseY;

            // calculate movement
            if (moveArea)
            {
                float sinX = (float)Math.Sin(cameraDirection.X);
                float cosX = (float)Math.Cos(cameraDirection.X);
                viewerCenter.X -= sinX * deltaX;
                viewerCenter.X -= cosX * deltaY;
                viewerCenter.Z += cosX * deltaX;
                viewerCenter.Z -= sinX * deltaY;

                cameraPosition.X -= sinX * deltaX;
                cameraPosition.X -= cosX * deltaY;
                cameraPosition.Z += cosX * deltaX;
                cameraPosition.Z -= sinX * deltaY;

                // check scrolling-limits
                if (viewerCenter.X < -SCROLLDISTANCE_MAX)
                {
                    cameraPosition.X = -SCROLLDISTANCE_MAX + (cameraPosition.X - viewerCenter.X);
                    viewerCenter.X = -SCROLLDISTANCE_MAX;
                }
                else if (viewerCenter.X > SCROLLDISTANCE_MAX)
                {
                    cameraPosition.X = SCROLLDISTANCE_MAX + (cameraPosition.X - viewerCenter.X);
                    viewerCenter.X = SCROLLDISTANCE_MAX;
                }

                if (viewerCenter.Z < -SCROLLDISTANCE_MAX)
                {
                    cameraPosition.Z = -SCROLLDISTANCE_MAX + (cameraPosition.Z - viewerCenter.Z);
                    viewerCenter.Z = -SCROLLDISTANCE_MAX;
                }
                else if (viewerCenter.Z > SCROLLDISTANCE_MAX)
                {
                    cameraPosition.Z = SCROLLDISTANCE_MAX + (cameraPosition.Z - viewerCenter.Z);
                    viewerCenter.Z = SCROLLDISTANCE_MAX;
                }
            }

            // calculate rotation
            if (rotateCamera)
            {
                cameraDirection.X += (float)deltaX / 1000;
                cameraDirection.Y += (float)deltaY / 1000;

                // check, rotationlimits
                if (cameraDirection.Y < CAMERAANGLE_MIN)
                {
                    cameraDirection.Y = CAMERAANGLE_MIN;
                }
                else if (cameraDirection.Y > CAMERAANGLE_MAX)
                {
                    cameraDirection.Y = CAMERAANGLE_MAX;
                }
            }

            // save new mouse-position
            mouseX = mouse.X;
            mouseY = mouse.Y;

            cameraUpvector.X = cameraPosition.X - viewerCenter.X;
            cameraUpvector.Z = cameraPosition.Z - viewerCenter.Z;
            cameraUpvector.Y = cameraPosition.Y;
            cameraUpvector.Normalize();
        }

        #endregion

        public Matrix WorldMatrix
        {
            get { return Matrix.CreateWorld(cameraPosition, Vector3.Forward, Vector3.Up); }
        }
        public Matrix ViewMatrix
        {
            get { return Matrix.CreateLookAt(cameraPosition, viewerCenter, cameraUpvector); }
        }

        public Matrix ProjectionMatrix
        {
            get { return projectionMatrix; }
        }

        #region Properties

        /// <summary>
        /// Gets the current Pickray.
        /// </summary>
        public Pickray Pickray
        {
            get
            {
                if (mouseX != -1)
                {

                    Matrix projektionsMatrix = projectionMatrix;
                    Pickray outputRay = new Pickray();
                    // create ray
                    outputRay.Origin = new Vector3(0.0f, 0.0f, 0.0f);
                    outputRay.Direction =
                        new Vector3
                            (
                            (((2.0f * mouseX) / gameWindow.ClientBounds.Width) - 1.0f) / projektionsMatrix.M11,
                            (((-2.0f * mouseY) / gameWindow.ClientBounds.Height) + 1.0f) / projektionsMatrix.M22,
                            -1.0f);
                    
                    // tranform ray to view
                    Matrix viewMatrix = ViewMatrix;
                    viewMatrix = Matrix.Invert(viewMatrix);
                    outputRay.Origin = Vector3.Transform(outputRay.Origin, viewMatrix);
                    outputRay.Direction = Vector3.TransformNormal(outputRay.Direction, viewMatrix);
                    outputRay.Direction.Normalize();


                    return outputRay;
                }
                else
                {
                    // Empty ray, if there is no mouse
                    return new Pickray();
                }
            }
        }
        public Vector3 Unproject(Vector3 source, Matrix projection, Matrix view, Matrix world)
        {
            float MaxDepth = 1.0f, MinDepth = 0.0f;
            Matrix matrix = Matrix.Invert(Matrix.Multiply(Matrix.Multiply(world, view), projection));
            source.X = (((source.X - gameWindow.ClientBounds.X) / ((float)gameWindow.ClientBounds.Width)) * 2f) - 1f;
            source.Y = -((((source.Y - gameWindow.ClientBounds.Y) / ((float)gameWindow.ClientBounds.Height)) * 2f) - 1f);
            source.Z = (source.Z - MinDepth) / (MaxDepth - MinDepth);
            Vector3 vector = Vector3.Transform(source, matrix);
            float a = (((source.X * matrix.M14) + (source.Y * matrix.M24)) + (source.Z * matrix.M34)) + matrix.M44;
            if (!WithinEpsilon(a, 1f))
            {
                vector = (Vector3)(vector / a);
            }
            return vector;
        }
        private static bool WithinEpsilon(float a, float b)
        {
            float num = a - b;
            return ((-1.401298E-45f <= num) && (num <= float.Epsilon));
        }

 

 

        /// <summary>
        /// Gets the current Mouse-Position.
        /// </summary>
        public Point MousePosition
        {
            get
            {
                if (mouseX != -1)
                {
                    return new Point(mouseX, mouseY);
                }
                else
                {
                    return new Point(0, 0);
                }
            }
        }

        /// <summary>
        /// Gets the current Camera-Position.
        /// </summary>
        public Vector3 CameraPosition
        {
            get { return cameraPosition; }
        }

        #endregion

        public void Resize(int playgroundWidth, int playgroundHeight)
        {
            // Maximale Distanz neu ermitteln
            distanceMax = playgroundWidth;

            // Maximalentfernung korrigieren
            if (cameraDirection.Z > distanceMax)
            {
                cameraDirection.Z = distanceMax;
            }

            // Camerapos ermitteln
            float distance = (float)(Math.Cos(cameraDirection.Y) * cameraDirection.Z);
            cameraPosition.Y = (float)(Math.Sin(cameraDirection.Y) * cameraDirection.Z);
            cameraPosition.Z = (float)(Math.Sin(cameraDirection.X) * distance) + viewerCenter.Z;
            cameraPosition.X = (float)(Math.Cos(cameraDirection.X) * distance) + viewerCenter.X;

            // Upvector ermitteln
            cameraUpvector.X = cameraPosition.X - viewerCenter.X;
            cameraUpvector.Z = cameraPosition.Z - viewerCenter.Z;
            cameraUpvector.Y = cameraPosition.Y;
            cameraUpvector.Normalize();
        }
    }
}