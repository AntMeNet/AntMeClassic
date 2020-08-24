using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AntMe.SharedComponents.States;
using AntMe.SharedComponents.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AntMe.Plugin.Fna
{
    internal sealed class RenderWindow : Game
    {
        private const int ROWHEIGHT = 13; // Distance between rows in statistic-box
        private const int MARKERTRANSPARENCY = 16; // alpha-value for marker
        private const float VIEWRANGE_MAX = 50000.0f;
        private const float VIEWRANGE_MIN = 1.0f;

        static readonly Vector3 LIGHT_0_DIRECTION = Vector3.Normalize(new Vector3(1, -3, 1));
        static readonly Vector3 LIGHT_1_DIRECTION = Vector3.Normalize(new Vector3(0, 1, -2));
        static readonly Vector3 LIGHT_2_DIRECTION = Vector3.Normalize(new Vector3(-2, 1, 0));

        KeyboardState previousKeyboardState;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Camera camera;

        Texture2D textFieldBackground;
        Texture2D groundTexture; // free texture from: http://www.textureking.com
        Texture2D borderTexture; // free texture from: http://www.textureking.com
        TextureCube skyTex;
        Effect skyEffect;
        CubeMesh skyMesh;

        NineSlicedTexture infoBox;

        Model bug;
        Model ant;
        Model anthill;
        Model apple;
        Model sugar;
        Model marker;
        Model box;

        SpriteFont hudFont;
        BasicEffect effect;
        Vector3[] playerColors;

        int playgroundWidth;
        int playgroundHeight;

        VertexPositionNormalTexture[] plainVertices;

        Dictionary<int, DebugMessage> debugMessages = new Dictionary<int, DebugMessage>();

        bool showDebugInfo = false;
        DebugRenderer debugRenderer;

        RasterizerState defaultRasterizerState, markerRasterizerState;
        DepthStencilState defaultDepthStencilState, markerDepthStencilState;

        public SimulationState CurrentState { get; set; }

        public RenderWindow()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Window.Title = "AntMe! - " + Strings.PluginName;
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {
            camera = new Camera(Window);
            previousKeyboardState = Keyboard.GetState();

            if (!string.Equals(Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName, "de", StringComparison.InvariantCultureIgnoreCase))
            {
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en");
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            debugRenderer = new DebugRenderer(GraphicsDevice, camera);

            defaultRasterizerState = GraphicsDevice.RasterizerState;
            markerRasterizerState = new RasterizerState()
            {
                CullMode = CullMode.CullCounterClockwiseFace,
                FillMode = FillMode.Solid,
                DepthBias = 0f,
                SlopeScaleDepthBias = 1f,
            };

            defaultDepthStencilState = GraphicsDevice.DepthStencilState;
            markerDepthStencilState = new DepthStencilState() { DepthBufferEnable = false };

            // default effect
            effect = new BasicEffect(GraphicsDevice);
            effect.LightingEnabled = true;
            effect.DirectionalLight0.Enabled = false;
            effect.DirectionalLight1.Enabled = false;
            effect.DirectionalLight2.Enabled = false;
            effect.World = Matrix.Identity;
            effect.View = Matrix.CreateLookAt(new Vector3(0, 0, -2), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 4f / 3f, 1f, 50000f);

            // Textures
            groundTexture = Content.Load<Texture2D>("dirt");
            borderTexture = Content.Load<Texture2D>("borderTex");

            textFieldBackground = Content.Load<Texture2D>("textbox");
            infoBox = new NineSlicedTexture(textFieldBackground, new Rectangle(10, 10, 12, 12));

            // Sky
            skyTex = Content.Load<TextureCube>("sky");
            skyEffect = Content.Load<Effect>("SkyEffect");
            skyEffect.Parameters["tex"].SetValue(skyTex);
            VertexDeclaration skyDeclaration = new VertexDeclaration(new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0));
            skyMesh = new CubeMesh(GraphicsDevice, Vector3.One);

            // Common Meshes
            bug = Content.Load<Model>("bug");
            ant = Content.Load<Model>("ant");
            anthill = Content.Load<Model>("anthill");
            apple = Content.Load<Model>("apple");
            sugar = Content.Load<Model>("sugar");
            marker = Content.Load<Model>("sphere");
            box = Content.Load<Model>("box");

            hudFont = Content.Load<SpriteFont>("HudFont");


            // Create Plane Mesh
            plainVertices = new VertexPositionNormalTexture[]
            {
                new VertexPositionNormalTexture(new Vector3(-0.5f, 0, -0.5f),new Vector3(0, 1, 0), new Vector2(0, 0)),
                new VertexPositionNormalTexture(new Vector3(+0.5f, 0, -0.5f),new Vector3(0, 1, 0), new Vector2(1, 0)),
                new VertexPositionNormalTexture(new Vector3(+0.5f, 0, +0.5f),new Vector3(0, 1, 0), new Vector2(1, 1)),

                new VertexPositionNormalTexture(new Vector3(-0.5f, 0, -0.5f),new Vector3(0, 1, 0), new Vector2(0, 0)),
                new VertexPositionNormalTexture(new Vector3(+0.5f, 0, +0.5f),new Vector3(0, 1, 0), new Vector2(1, 1)),
                new VertexPositionNormalTexture(new Vector3(-0.5f, 0, +0.5f),new Vector3(0, 1, 0), new Vector2(0, 1)),
            };


            // Create Player Color
            playerColors = new Vector3[]
            {
                new Vector3(0f, 0f, 0f),
                new Vector3(1f, 0f, 0f),
                new Vector3(0f, 0f, 1f),
                new Vector3(0f, 1f, 0f),
                new Vector3(0f, 1f, 1f),
                new Vector3(1f, 1f, 0f),
                new Vector3(1f, 0f, 1f),
                new Vector3(1f, 1f, 1f),
            };
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            debugRenderer.Unload();
            Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (CurrentState != null)
            {
                playgroundWidth = CurrentState.PlaygroundWidth / 2;
                playgroundHeight = CurrentState.PlaygroundHeight / 2;
                camera.Resize(CurrentState.PlaygroundWidth, CurrentState.PlaygroundHeight);
            }

            var ks = Keyboard.GetState();
            if (ks.IsKeyUp(Keys.D) && previousKeyboardState.IsKeyDown(Keys.D))
            {
                showDebugInfo = !showDebugInfo;
            }
            previousKeyboardState = ks;


            camera.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            SimulationState state = CurrentState;
            if (state == null)
                return;

            DrawSky();

            effect.CurrentTechnique.Passes[0].Apply();
            effect.Projection = camera.ProjectionMatrix;
            effect.View = camera.ViewMatrix;

            DrawPlayground();

            Selection selectedItem = new Selection();
            Pickray pickray = camera.Pickray;
            Point mousePosition = camera.MousePosition;

            // Selektionsinfos zurücksetzen
            selectedItem.SelectionType = SelectionType.Nothing;
            selectedItem.Item = null;
            float distanceToSelectedItem = VIEWRANGE_MAX * VIEWRANGE_MAX;


            // Draw Bugs
            float distance;
            foreach (var bug in state.BugStates)
            {
                if ((distance = DrawBug(bug, pickray, false)) > 0)
                {
                    if (distance < distanceToSelectedItem)
                    {
                        distanceToSelectedItem = distance;
                        selectedItem.Item = bug;
                        selectedItem.SelectionType = SelectionType.Bug;
                    }
                }
            }

            // Draw Sugar
            foreach (var sugar in state.SugarStates)
            {
                if ((distance = DrawSugar(sugar, pickray, false)) > 0)
                {
                    if (distance < distanceToSelectedItem)
                    {
                        distanceToSelectedItem = distance;
                        selectedItem.Item = sugar;
                        selectedItem.SelectionType = SelectionType.Sugar;
                    }
                }
            }

            // Draw Fruit
            foreach (var fruit in state.FruitStates)
            {
                if ((distance = DrawFruit(fruit, pickray, false)) > 0)
                {
                    if (distance < distanceToSelectedItem)
                    {
                        distanceToSelectedItem = distance;
                        selectedItem.Item = fruit;
                        selectedItem.SelectionType = SelectionType.Fruit;
                    }
                }
            }

            // Draw Colony Base
            foreach (var colony in state.ColonyStates)
            {
                // Draw AntHills
                foreach (var anthill in colony.AnthillStates)
                {
                    if ((distance = DrawAnthill(colony.Id, anthill, pickray, false)) > 0)
                    {
                        if (distance < distanceToSelectedItem)
                        {
                            distanceToSelectedItem = distance;
                            selectedItem.Item = anthill;
                            selectedItem.SelectionType = SelectionType.Anthill;
                            selectedItem.AdditionalInfo = CurrentState.ColonyStates[anthill.ColonyId - 1].ColonyName;
                        }
                    }
                }

                // Draw Ants
                foreach (var ant in colony.AntStates)
                {
                    // Debug Messages aktualisieren
                    if (!string.IsNullOrEmpty(ant.DebugMessage))
                    {
                        DebugMessage msg;
                        if (debugMessages.ContainsKey(ant.Id))
                            msg = debugMessages[ant.Id];
                        else
                        {
                            msg = new DebugMessage();
                            debugMessages.Add(ant.Id, msg);
                        }

                        msg.CreateRound = state.CurrentRound;
                        msg.Message = ant.DebugMessage;
                    }

                    // Draw
                    if ((distance = DrawAnt(colony.Id, ant, pickray, false)) > 0)
                    {
                        if (distance < distanceToSelectedItem)
                        {
                            distanceToSelectedItem = distance;
                            selectedItem.Item = ant;
                            selectedItem.SelectionType = SelectionType.Ant;
                            selectedItem.AdditionalInfo = CurrentState.ColonyStates[ant.ColonyId - 1].ColonyName;
                        }
                    }
                }

                // Remove old Messages
                foreach (var key in debugMessages.Keys.ToArray())
                {
                    DebugMessage msg = debugMessages[key];
                    if (state.CurrentRound - msg.CreateRound > DebugMessage.ROUNDS_TO_LIFE)
                        debugMessages.Remove(key);
                }
            }

            // Draw Marker
            foreach (var colony in state.ColonyStates)
            {
                foreach (var marker in colony.MarkerStates)
                {
                    DrawMarker(colony.Id, marker);

                }
            }

            // render all sprites in one SpriteBatch.Begin()-End() cycle to save performance
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);



            // Draw debug ant-thoughts
            if (showDebugInfo)
            {
                foreach (var colony in CurrentState.ColonyStates)
                {
                    foreach (var ant in colony.AntStates)
                    {
                        // Draw actual debug text
                        if (debugMessages.ContainsKey(ant.Id))
                        {
                            DebugMessage msg = debugMessages[ant.Id];
                            Vector3 pos = new Vector3(ant.PositionX - playgroundWidth, 4, -ant.PositionY + playgroundHeight);
                            Vector2 screenPos = debugRenderer.WorldToScreen(pos, new Vector2(0, -20));
                            Color boxCol = new Color(0.5f * playerColors[ant.ColonyId - 1]);
                            boxCol.A = 128;
                            DrawTextBox(msg.Message, screenPos, boxCol, Color.White);
                        }
                    }
                }
            }

            // Draw Infobox
            DrawInfobox(state);

            // Draw Info-Tag at selected item
            if (selectedItem.SelectionType != SelectionType.Nothing)
            {
                string line1;
                string line2;
                switch (selectedItem.SelectionType)
                {
                    case SelectionType.Ant:

                        AntState ameise = (AntState)selectedItem.Item;
                        string antName = NameHelper.GetFemaleName(ameise.Id);
                        line1 = string.Format(Strings.HovertextAntLine1, antName, selectedItem.AdditionalInfo);
                        line2 = string.Format(Strings.HovertextAntLine2, ameise.Vitality);
                        break;
                    case SelectionType.Anthill:
                        line1 = Strings.HovertextAnthillLine1;
                        line2 = string.Format(Strings.HovertextAnthillLine2, selectedItem.AdditionalInfo);
                        break;
                    case SelectionType.Bug:
                        BugState bugState = (BugState)selectedItem.Item;
                        string bugName = NameHelper.GetMaleName(bugState.Id);
                        line1 = string.Format(Strings.HovertextBugLine1, bugName);
                        line2 = string.Format(Strings.HovertextBugLine2, bugState.Vitality);
                        break;
                    case SelectionType.Fruit:
                        FruitState fruitState = (FruitState)selectedItem.Item;
                        line1 = Strings.HovertextFruitLine1;
                        line2 = string.Format(Strings.HovertextFruitLine2, fruitState.Amount);
                        break;
                    case SelectionType.Sugar:
                        SugarState sugar = (SugarState)selectedItem.Item;
                        line1 = Strings.HovertextSugarLine1;
                        line2 = string.Format(Strings.HovertextSugarLine2, sugar.Amount);
                        break;
                    default:
                        line1 = String.Empty;
                        line2 = String.Empty;
                        break;
                }

                // Text an Mausposition ausgeben
                if (line1 != String.Empty || line2 != String.Empty)
                {
                    DrawInfoTag(mousePosition, line1, line2);
                }
            }


            spriteBatch.End();

            base.Draw(gameTime);
        }

        #region Render Methode

        private void DrawSky()
        {
            skyEffect.Parameters["WVP"].SetValue(camera.WorldMatrix * camera.ViewMatrix * camera.ProjectionMatrix);
            skyEffect.CurrentTechnique.Passes[0].Apply();

            skyMesh.Draw();
        }

        private void DrawPlayground()
        {
            Matrix matrix = Matrix.CreateScale(playgroundWidth * 2, 1.0f, playgroundHeight * 2);
            effect.World = matrix;
            effect.EmissiveColor = new Vector3(0.6f, 0.52f, 0.43f);//new Vector3(0.45f, 0.45f, 0.29f); // 114, 114, 73

            effect.TextureEnabled = true;
            effect.Texture = groundTexture;

            //effect.LightingEnabled = true;
            //effect.DirectionalLight0.Enabled = true;
            //effect.DirectionalLight0.Direction = LIGHT_0_DIRECTION;

            effect.CurrentTechnique.Passes[0].Apply();
            GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, plainVertices, 0, 2);


            // Draw Border
            const int BORDER_WIDTH = 200;
            const int BORDER_POS_Y = -BORDER_WIDTH / 2 + 10;

            for (int i = 0; i < 4; i++)
            {
                Vector3 scale, pos;
                float rotation;
                switch (i)
                {
                    case 0:
                        scale = new Vector3(BORDER_WIDTH, BORDER_WIDTH, 2 * playgroundWidth + BORDER_WIDTH);
                        pos = new Vector3(BORDER_WIDTH / 2, BORDER_POS_Y, playgroundHeight + BORDER_WIDTH / 2);
                        rotation = MathHelper.PiOver2;
                        break;
                    case 1:
                        scale = new Vector3(BORDER_WIDTH, BORDER_WIDTH, 2 * playgroundWidth + BORDER_WIDTH);
                        pos = new Vector3(-BORDER_WIDTH / 2, BORDER_POS_Y, -playgroundHeight - BORDER_WIDTH / 2);
                        rotation = MathHelper.PiOver2;
                        break;
                    case 2:
                        scale = new Vector3(BORDER_WIDTH, BORDER_WIDTH, 2 * playgroundHeight + BORDER_WIDTH);
                        pos = new Vector3(playgroundWidth + BORDER_WIDTH / 2, BORDER_POS_Y, -BORDER_WIDTH / 2);
                        rotation = 0;
                        break;
                    case 3:
                        scale = new Vector3(BORDER_WIDTH, BORDER_WIDTH, 2 * playgroundHeight + BORDER_WIDTH);
                        pos = new Vector3(-playgroundWidth - BORDER_WIDTH / 2, BORDER_POS_Y, BORDER_WIDTH / 2);
                        rotation = 0;
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }

                Matrix world = Matrix.CreateScale(scale) * Matrix.CreateRotationY(rotation) * Matrix.CreateTranslation(pos);

                foreach (var mesh in box.Meshes)
                {
                    foreach (BasicEffect eff in mesh.Effects)
                    {

                        eff.World = world;
                        eff.View = camera.ViewMatrix;
                        eff.Projection = camera.ProjectionMatrix;

                        eff.DiffuseColor = Color.White.ToVector3();

                        eff.TextureEnabled = true;
                        eff.Texture = borderTexture;

                        eff.LightingEnabled = true;

                        eff.DirectionalLight0.Enabled = true;
                        eff.DirectionalLight0.DiffuseColor = new Vector3(1, 1, 1);
                        eff.DirectionalLight0.Direction = LIGHT_0_DIRECTION;

                        eff.DirectionalLight1.Enabled = true;
                        eff.DirectionalLight1.DiffuseColor = new Vector3(1, 1, 1);
                        eff.DirectionalLight1.Direction = LIGHT_1_DIRECTION;

                        eff.DirectionalLight2.Enabled = true;
                        eff.DirectionalLight2.DiffuseColor = new Vector3(1, 1, 1);
                        eff.DirectionalLight2.Direction = LIGHT_2_DIRECTION;
                    }
                    mesh.Draw();
                }

            }

        }

        public float DrawBug(BugState state, Pickray pickray, bool selected)
        {
            Matrix matrix = Matrix.CreateRotationY((float)(state.Direction * Math.PI) / 180);
            matrix.M41 = (state.PositionX) - playgroundWidth;
            matrix.M43 = (-state.PositionY) + playgroundHeight;
            foreach (var mesh in bug.Meshes)
            {
                foreach (BasicEffect eff in mesh.Effects)
                {
                    eff.World = matrix;
                    eff.View = camera.ViewMatrix;
                    eff.Projection = camera.ProjectionMatrix;


                    if (mesh.Name == "Sphere02" || mesh.Name == "Sphere03")
                    {
                        eff.LightingEnabled = false;

                        // change the bugs eye color depending on its vitality 
                        eff.EmissiveColor = Vector3.Lerp(new Vector3(1, 0, 0), new Vector3(0f, 0.6f, 1f), state.Vitality / 1000f);

                    }
                    else if (mesh.Name == "Sphere01")
                    {
                        eff.LightingEnabled = true;
                        eff.DiffuseColor = new Vector3(0.1f, 0.1f, 0.1f);
                        eff.EmissiveColor = new Vector3(0f, 0f, 0f);

                        eff.PreferPerPixelLighting = true;
                        eff.DirectionalLight0.Enabled = true;
                        eff.DirectionalLight0.Direction = LIGHT_0_DIRECTION;
                        eff.DirectionalLight0.DiffuseColor = new Vector3(1, 1, 1);
                        eff.DirectionalLight0.SpecularColor = new Vector3(0.6f, 1f, 1f);//new Vector3(0.7f, 0.3f, 0f);//

                        eff.DirectionalLight1.Enabled = false;
                        eff.DirectionalLight2.Enabled = false;
                    }
                    else
                    {
                        eff.LightingEnabled = false;
                    }
                }
                mesh.Draw();
            }
            BoundingSphere sphere = getBoundingSphere(bug, matrix);


            float? distance = sphere.Intersects(new Ray(pickray.Origin, pickray.Direction));
            if (distance != null)
                return distance.Value;

            return 0.0f;
        }
        private BoundingSphere getBoundingSphere(Model model, Matrix world)
        {
            BoundingSphere sphere = new BoundingSphere();
            foreach (ModelMesh mesh in model.Meshes)
            {
                if (sphere.Radius == 0)
                    sphere = mesh.BoundingSphere;
                else
                    sphere = BoundingSphere.
                             CreateMerged(sphere, mesh.BoundingSphere);
            }
            return sphere.Transform(world);
        }

        public float DrawAnt(int colony, AntState state, Pickray pickray, bool selected)
        {
            Matrix matrix = Matrix.CreateRotationY((float)(state.Direction * Math.PI) / 180);
            Vector3 position = new Vector3(state.PositionX - playgroundWidth, 0, -state.PositionY + playgroundHeight);

            matrix.M41 = position.X;
            matrix.M43 = position.Z;

            foreach (var mesh in ant.Meshes)
            {
                foreach (BasicEffect eff in mesh.Effects)
                {
                    eff.World = matrix;
                    eff.View = camera.ViewMatrix;
                    eff.Projection = camera.ProjectionMatrix;

                    eff.LightingEnabled = true;
                    if (mesh.Name == "Sphere01" || mesh.Name == "Sphere02")
                    {
                        // eff.EmissiveColor = playerColors[colony];
                        eff.DiffuseColor = playerColors[colony - 1];
                    }
                    else
                    {
                        // eff.EmissiveColor = new Vector3(0, 0, 0);
                        eff.DiffuseColor = 0.2f * playerColors[colony - 1];//new Vector3(0.2f, 0.2f, 0.2f);
                    }

                    //eff.SpecularColor = new Vector3(0.5f, 0.5f, 0.5f) + playerColors[colony];

                    eff.DirectionalLight0.Enabled = true;
                    eff.DirectionalLight0.Direction = LIGHT_0_DIRECTION;
                    eff.DirectionalLight0.DiffuseColor = new Vector3(1, 1, 1);
                    eff.DirectionalLight0.SpecularColor = new Vector3(1f, 1f, 1f);// + playerColors[colony];

                    eff.DirectionalLight1.Enabled = true;
                    eff.DirectionalLight1.DiffuseColor = new Vector3(1, 1, 1);
                    eff.DirectionalLight1.Direction = LIGHT_1_DIRECTION;

                    eff.DirectionalLight2.Enabled = true;
                    eff.DirectionalLight2.DiffuseColor = new Vector3(1, 1, 1);
                    eff.DirectionalLight2.Direction = LIGHT_2_DIRECTION;
                }
                mesh.Draw();
            }

            // Draw sugar-block, if ant has sugar loaded
            if (state.LoadType == LoadType.Sugar)
            {
                Matrix sugarWorld =
                      Matrix.CreateScale(2)
                    * Matrix.CreateTranslation(new Vector3(-1.5f, 4, 0))
                    * matrix;

                foreach (var mesh in box.Meshes)
                {
                    foreach (BasicEffect eff in mesh.Effects)
                    {
                        eff.World = sugarWorld;
                        eff.View = camera.ViewMatrix;
                        eff.Projection = camera.ProjectionMatrix;


                        eff.TextureEnabled = false;
                        eff.DiffuseColor = Color.White.ToVector3();
                    }
                    mesh.Draw();
                }

            }


            // Debug info
            if (showDebugInfo)
            {
                Color color = new Color(playerColors[state.ColonyId - 1]);
                Vector3 antPosition = position + Vector3.Up;

                float sightRadius = state.ViewRange;
                debugRenderer.DrawHorizontalCircle(antPosition, sightRadius, 16, color);

                if (state.TargetType != TargetType.None)
                {
                    Vector3 targetPos = new Vector3(state.TargetPositionX - playgroundWidth, 1, -state.TargetPositionY + playgroundHeight);
                    debugRenderer.DrawLine(antPosition, targetPos, color);
                }
            }

            float? distance = getBoundingSphere(ant, matrix).Intersects(new Ray(pickray.Origin, pickray.Direction));
            if (distance.HasValue)
                return distance.Value;


            return 0.0f;
        }

        public float DrawSugar(SugarState state, Pickray pickray, bool selected)
        {
            Matrix matrix = Matrix.CreateTranslation(state.PositionX - playgroundWidth,
                0, -state.PositionY + playgroundHeight);
            matrix.M11 = matrix.M22 = matrix.M33 = state.Radius / 50.0f;
            foreach (var mesh in sugar.Meshes)
            {
                foreach (BasicEffect eff in mesh.Effects)
                {
                    eff.World = matrix;
                    eff.View = camera.ViewMatrix;
                    eff.Projection = camera.ProjectionMatrix;

                    eff.LightingEnabled = true;
                    eff.DiffuseColor = new Vector3(0.85f, 0.85f, 0.75f);
                    eff.EmissiveColor = new Vector3(0.3f, 0.3f, 0.25f);

                    eff.DirectionalLight0.Enabled = true;
                    eff.DirectionalLight0.DiffuseColor = new Vector3(1, 1, 1);
                    eff.DirectionalLight0.Direction = LIGHT_0_DIRECTION;

                    eff.DirectionalLight1.Enabled = true;
                    eff.DirectionalLight1.DiffuseColor = new Vector3(1, 1, 1);
                    eff.DirectionalLight1.Direction = LIGHT_1_DIRECTION;

                    eff.DirectionalLight2.Enabled = true;
                    eff.DirectionalLight2.DiffuseColor = new Vector3(1, 1, 1);
                    eff.DirectionalLight2.Direction = LIGHT_2_DIRECTION;
                }
                mesh.Draw();
            }

            float? distance = getBoundingSphere(sugar, matrix).Intersects(new Ray(pickray.Origin, pickray.Direction));
            if (distance.HasValue)
                return distance.Value;

            return 0.0f;
        }

        public float DrawFruit(FruitState state, Pickray pickray, bool selected)
        {
            Matrix matrix = Matrix.CreateTranslation(state.PositionX - playgroundWidth, 0, -state.PositionY + playgroundHeight);
            matrix.M11 = state.Radius / 4.5f;
            matrix.M22 = state.Radius / 4.5f;
            matrix.M33 = state.Radius / 4.5f;
            foreach (var mesh in apple.Meshes)
            {
                foreach (BasicEffect eff in mesh.Effects)
                {
                    eff.World = matrix;
                    eff.View = camera.ViewMatrix;
                    eff.Projection = camera.ProjectionMatrix;

                    eff.LightingEnabled = true;
                    eff.DiffuseColor = new Vector3(0.6f, 0.7f, 0.1f);
                    eff.EmissiveColor = new Vector3(0.1f, 0.3f, 0f);

                    eff.DirectionalLight0.Enabled = true;
                    eff.DirectionalLight0.Direction = LIGHT_0_DIRECTION;
                    eff.DirectionalLight0.DiffuseColor = new Vector3(0.7f, 0.4f, 0f);
                    eff.DirectionalLight0.SpecularColor = new Vector3(0.1f, 0.5f, 0f);

                    eff.DirectionalLight1.Enabled = true;
                    eff.DirectionalLight1.Direction = LIGHT_1_DIRECTION;
                    eff.DirectionalLight1.DiffuseColor = new Vector3(0.3f, 0.1f, 0f);

                    eff.DirectionalLight2.Enabled = false;
                    eff.DirectionalLight2.Direction = LIGHT_2_DIRECTION;
                    eff.DirectionalLight2.DiffuseColor = new Vector3(0.3f, 0.4f, 0f);
                    eff.DirectionalLight2.SpecularColor = new Vector3(0.1f, 0.5f, 0f);

                }
                mesh.Draw();
            }

            //// Check for pickray-collision

            float? distance = getBoundingSphere(apple, matrix).Intersects(new Ray(pickray.Origin, pickray.Direction));
            if (distance.HasValue)
                return distance.Value;

            return 0.0f;
        }

        public float DrawAnthill(int colony, AnthillState state, Pickray pickray, bool selected)
        {
            Matrix matrix = Matrix.CreateTranslation(state.PositionX - playgroundWidth, 0, -state.PositionY + playgroundHeight);
            BoundingSphere collisionBox = new BoundingSphere(
                new Vector3(state.PositionX - playgroundWidth, 0, -state.PositionY + playgroundHeight),
                state.Radius);

            // Cone
            var mesh = anthill.Meshes[0];
            foreach (BasicEffect eff in mesh.Effects)
            {
                eff.World = matrix;
                eff.View = camera.ViewMatrix;
                eff.Projection = camera.ProjectionMatrix;

                eff.LightingEnabled = true;
                eff.EmissiveColor = new Vector3(0.1f, 0.05f, 0f);

                eff.DirectionalLight0.Enabled = true;
                eff.DirectionalLight0.Direction = LIGHT_0_DIRECTION;
                eff.DirectionalLight0.DiffuseColor = new Vector3(0.4f, 0.4f, 0.4f);

                eff.DirectionalLight1.Enabled = true;
                eff.DirectionalLight1.Direction = LIGHT_1_DIRECTION;
                eff.DirectionalLight1.DiffuseColor = new Vector3(0.1f, 0.1f, 0.2f);

                eff.DirectionalLight2.Enabled = true;
                eff.DirectionalLight2.Direction = LIGHT_2_DIRECTION;
                eff.DirectionalLight2.DiffuseColor = new Vector3(0.1f, 0.1f, 0.2f);
            }
            mesh.Draw();

            // Bar
            mesh = anthill.Meshes[1];
            foreach (BasicEffect eff in mesh.Effects)
            {
                eff.World = matrix;
                eff.View = camera.ViewMatrix;
                eff.Projection = camera.ProjectionMatrix;

                eff.LightingEnabled = true;
                eff.EmissiveColor = new Vector3(0.5f, 0.5f, 0.5f);

                eff.DirectionalLight0.Enabled = true;
                eff.DirectionalLight0.Direction = LIGHT_0_DIRECTION;

                eff.DirectionalLight1.Enabled = true;
                eff.DirectionalLight1.Direction = LIGHT_1_DIRECTION;

                eff.DirectionalLight2.Enabled = true;
                eff.DirectionalLight2.Direction = LIGHT_2_DIRECTION;
            }
            mesh.Draw();

            // Flag
            mesh = anthill.Meshes[2];
            foreach (BasicEffect eff in mesh.Effects)
            {
                eff.World = matrix;
                eff.View = camera.ViewMatrix;
                eff.Projection = camera.ProjectionMatrix;

                eff.LightingEnabled = true;
                eff.EmissiveColor = playerColors[colony - 1];

                eff.DirectionalLight0.Enabled = true;
                eff.DirectionalLight0.Direction = LIGHT_0_DIRECTION;

                eff.DirectionalLight1.Enabled = true;
                eff.DirectionalLight1.Direction = LIGHT_1_DIRECTION;

                eff.DirectionalLight2.Enabled = true;
                eff.DirectionalLight2.Direction = LIGHT_2_DIRECTION;
            }
            mesh.Draw();


            // Check for pickray-collision
            float? distance = collisionBox.Intersects(new Ray(pickray.Origin, pickray.Direction));
            if (distance.HasValue)
            {
                return distance.Value;
            }
            return 0.0f;
        }

        public void DrawMarker(int colony, MarkerState state)
        {

            GraphicsDevice.RasterizerState = markerRasterizerState;
            GraphicsDevice.DepthStencilState = markerDepthStencilState;

            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            //GraphicsDevice.BlendState = new BlendState()
            //{
            //    AlphaBlendFunction = BlendFunction.Add,
            //    ColorBlendFunction = BlendFunction.Add,

            //    AlphaSourceBlend = Blend.SourceAlpha,
            //    ColorSourceBlend = Blend.SourceColor,

            //    AlphaDestinationBlend = Blend.InverseSourceAlpha,
            //    ColorDestinationBlend = Blend.InverseSourceColor,
            //};
            Vector3 pos = new Vector3(state.PositionX - playgroundWidth, 0, -state.PositionY + playgroundHeight);

            Matrix matrix = Matrix.CreateTranslation(pos);

            matrix.M11 = matrix.M22 = matrix.M33 = state.Radius;

            foreach (var mesh in marker.Meshes)
            {
                foreach (BasicEffect eff in mesh.Effects)
                {
                    eff.World = matrix;
                    eff.View = camera.ViewMatrix;
                    eff.Projection = camera.ProjectionMatrix;

                    eff.LightingEnabled = false;
                    eff.Alpha = 0.05f;
                    eff.EmissiveColor = 0.33f * playerColors[state.ColonyId - 1];
                    eff.DiffuseColor = 0.33f * playerColors[state.ColonyId - 1];
                }
                mesh.Draw();
            }

            GraphicsDevice.RasterizerState = defaultRasterizerState;
            GraphicsDevice.DepthStencilState = defaultDepthStencilState;

            // draw debug
            if (showDebugInfo)
            {
                debugRenderer.DrawHorizontalCircle(pos, state.Radius, 16, new Color(0.33f * playerColors[state.ColonyId - 1]));
            }
        }

        public void DrawInfobox(SimulationState state)
        {
            int height = ROWHEIGHT * state.ColonyStates.Count + 60;
            int position = (height / 2) + 10;


            // Box and Header
            //spriteBatch.Draw(textFieldBackground, new Rectangle(10, 10, 500, height), new Color(0.5f, 0.5f, 0.5f, 0.5f));
            infoBox.Draw(spriteBatch, new Rectangle(10, 10, 525, height), new Color(1f, 1f, 1f, 0.7f));
            //InfoBox.Draw(spriteBatch, new Rectangle(15, 50, 510, height - 45), new Color(0.5f, 0.5f, 0.5f, 0.5f));
            infoBox.Draw(spriteBatch, new Rectangle(175, 15, 80, height - 10), new Color(0.3f, 0.9f, 0.3f, 0.3f));
            infoBox.Draw(spriteBatch, new Rectangle(265, 15, 80, height - 10), new Color(0.9f, 0.3f, 0.3f, 0.3f));
            infoBox.Draw(spriteBatch, new Rectangle(355, 15, 80, height - 10), new Color(0.3f, 0.3f, 0.9f, 0.3f));
            infoBox.Draw(spriteBatch, new Rectangle(445, 15, 80, height - 10), new Color(1f, 1f, 1f, 0.3f));

            spriteBatch.DrawString(hudFont, Strings.InfoboxColumnColony2, new Vector2(20, ROWHEIGHT + 20), Color.Black);
            spriteBatch.DrawString(hudFont, Strings.InfoboxColumnCollectedFood1, new Vector2(180, 20), Color.Black); //Green);
            spriteBatch.DrawString(hudFont, Strings.InfoboxColumnCollectedFood2, new Vector2(180, ROWHEIGHT + 20), Color.Black); //Green);
            spriteBatch.DrawString(hudFont, Strings.InfoboxColumnKilledAnts1, new Vector2(270, 20), Color.Black); //Red);
            spriteBatch.DrawString(hudFont, Strings.InfoboxColumnKilledAnts2, new Vector2(270, ROWHEIGHT + 20), Color.Black); //Red);
            spriteBatch.DrawString(hudFont, Strings.InfoboxColumnKilledBugs1, new Vector2(360, 20), Color.Black); //Blue);
            spriteBatch.DrawString(hudFont, Strings.InfoboxColumnKilledBugs2, new Vector2(360, ROWHEIGHT + 20), Color.Black); //Blue);
            spriteBatch.DrawString(hudFont, Strings.InfoboxColumnPoints2, new Vector2(450, /*ROWHEIGHT +*/ 20), Color.Black);

            //int count = 0;
            for (int i = 0; i < state.ColonyStates.Count; i++)
            {
                Color color = new Color(0.5f * playerColors[i]);
                ColonyState colony = state.ColonyStates[i];
                int killedAnts = colony.StarvedAnts + colony.EatenAnts + colony.BeatenAnts;
                int posY = i * ROWHEIGHT + 55;

                spriteBatch.DrawString(hudFont, colony.ColonyName, new Vector2(20, i * ROWHEIGHT + 55), color);

                // DrawTextRightAligned(colony.ColonyName, 20, posY, color);
                DrawTextRightAligned(colony.CollectedFood.ToString(), 250, posY, color);
                DrawTextRightAligned(killedAnts.ToString(), 340, posY, color);
                DrawTextRightAligned(colony.KilledBugs.ToString(), 430, posY, color);
                DrawTextRightAligned(colony.Points.ToString(), 520, posY, color);

            }


        }

        private void DrawTextRightAligned(string text, int posX, int posY, Color color)
        {
            int width = (int)hudFont.MeasureString(text).X;
            spriteBatch.DrawString(hudFont, text, new Vector2(posX - width, posY), color);
        }

        public void DrawInfoTag(Point position, String line1, String line2)
        {
            int height = 2 * ROWHEIGHT + 10;
            int posY = position.Y - height - 5;

            int widthLine1 = (int)hudFont.MeasureString(line1).X;
            int widthLine2 = (int)hudFont.MeasureString(line2).X;
            int width = (int)Math.Max(widthLine1, widthLine2) + 20;
            int posX = position.X - width / 2;


            infoBox.Draw(spriteBatch, new Rectangle(posX, posY, width, height), new Color(1f, 1f, 1f, 0.8f));

            spriteBatch.DrawString(hudFont, line1, new Vector2(posX + 10, posY + 5), Color.Black);
            spriteBatch.DrawString(hudFont, line2, new Vector2(posX + 10, posY + ROWHEIGHT + 5), Color.Black);


        }

        private void DrawTextBox(string text, Vector2 pos, Color boxColor, Color textColor)
        {
            Vector2 textSize = hudFont.MeasureString(text);
            pos = new Vector2((int)(pos.X - 0.5f * textSize.X), (int)(pos.Y - 0.5f * textSize.Y));

            Rectangle rect = new Rectangle((int)pos.X - 10, (int)pos.Y - 10, (int)textSize.X + 20, (int)textSize.Y + 20);

            infoBox.Draw(spriteBatch, rect, boxColor);
            spriteBatch.DrawString(hudFont, text, pos, textColor);

        }

        #endregion
    }
}
