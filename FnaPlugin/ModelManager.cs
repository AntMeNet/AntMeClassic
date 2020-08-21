namespace AntMe.Plugin.Fna
{
    /// <summary>
    /// class, to manage and render all model-resources.
    /// </summary>
    //internal sealed class ModellManager
    //{
    //    #region Constants

    //    private const int ROWHEIGHT = 13; // Distance between rows in statistic-box
    //    private const int MARKERTRANSPARENCY = 16; // alpha-value for marker
    //    private const float FONTSIZE = 9.0f; // font-size for text in the statistic-box
    //    private const string FONTFAMILY = "Airal"; // font-family for text in the statistic-box

    //    #endregion

    //    #region private Variables

    //    private readonly Dictionary<int, Material> antMaterial;
    //    private readonly Material fruitMaterial;
    //    private readonly ColorFinder colorFinder;
    //    private readonly Material bugMaterial;
    //    private readonly Line line;
    //    private readonly Dictionary<int, Material> markerMaterials;

    //    private readonly Font fontNormal;
    //    private readonly Font fontBold;
    //    private readonly Material selectionMaterial;
    //    private readonly Material playgroundMaterial;
    //    private readonly Device renderDevice;
    //    private readonly Material sugarMaterial;
    //    private Mesh antHillMesh;
    //    private Mesh antMesh;
    //    private Mesh fruitMesh;
    //    private Mesh bugMesh;
    //    private Mesh collisionBox;
    //    private Mesh markerMesh;

    //    private int playgroundWidth;
    //    private int playgroundHeight;
    //    private Mesh playgroundMesh;
    //    private Mesh sugarMesh;
    //    private Mesh sugarCubeMesh;

    //    #endregion

    //    #region Construction and init

    //    public ModellManager(Device renderDevice)
    //    {
    //        this.renderDevice = renderDevice;

    //        antMaterial = new Dictionary<int, Material>();
    //        markerMaterials = new Dictionary<int, Material>();
    //        colorFinder = new ColorFinder();

    //        playgroundMaterial = new Material();
    //        playgroundMaterial.Ambient = Color.FromArgb(114, 114, 73);
    //        playgroundMaterial.Emissive = Color.FromArgb(90, 90, 58);
    //        playgroundMaterial.Specular = Color.FromArgb(114, 114, 73);
    //        colorFinder.BelegeFarbe(new Farbe(114, 114, 73));

    //        sugarMaterial = new Material();
    //        sugarMaterial.Emissive = Color.FromArgb(200, 200, 200);
    //        sugarMaterial.Specular = Color.FromArgb(255, 255, 255);
    //        colorFinder.BelegeFarbe(new Farbe(200, 200, 200));

    //        bugMaterial = new Material();
    //        bugMaterial.Emissive = Color.DarkBlue;
    //        bugMaterial.Specular = Color.FromArgb(0, 0, 150);
    //        colorFinder.BelegeFarbe(new Farbe(Color.DarkBlue.R, Color.DarkBlue.G, Color.DarkBlue.B));

    //        fruitMaterial = new Material();
    //        fruitMaterial.Emissive = Color.Green;
    //        fruitMaterial.Specular = Color.FromArgb(0, 255, 0);
    //        colorFinder.BelegeFarbe(new Farbe(Color.Green.R, Color.Green.G, Color.Green.B));

    //        selectionMaterial = new Material();
    //        selectionMaterial.Emissive = Color.FromArgb(120, 0, 0);
    //        selectionMaterial.Specular = Color.Red;

    //        fontNormal = new Font(renderDevice, new System.Drawing.Font(FONTFAMILY, FONTSIZE, FontStyle.Regular));
    //        fontBold = new Font(renderDevice, new System.Drawing.Font(FONTFAMILY, FONTSIZE, FontStyle.Bold));
    //        line = new Line(renderDevice);

    //        createResources();
    //    }

    //    #endregion

    //    #region Resource-Management

    //    /// <summary>
    //    /// Initializes all resources after a device reset.
    //    /// </summary>
    //    public void DeviceReset()
    //    {
    //        Dispose();
    //        createResources();
    //    }

    //    /// <summary>
    //    /// Creates all needed resources.
    //    /// </summary>
    //    private void createResources()
    //    {
    //        collisionBox = Mesh.Box(renderDevice, 30.0f, 30.0f, 30.0f);
    //        playgroundMesh = Mesh.Box(renderDevice, 1.0f, 1.0f, 1.0f);
    //        sugarCubeMesh = Mesh.Box(renderDevice, 2.0f, 1.5f, 2.0f);
    //        markerMesh = Mesh.Sphere(renderDevice, 1.0f, 10, 10);
    //        antMesh = Mesh.FromStream(new MemoryStream(Models.ant), MeshFlags.DoNotClip, renderDevice);
    //        bugMesh = Mesh.FromStream(new MemoryStream(Models.bug), MeshFlags.DoNotClip, renderDevice);
    //        sugarMesh =
    //            Mesh.FromStream(new MemoryStream(Models.sugar), MeshFlags.DoNotClip, renderDevice);
    //        antHillMesh =
    //            Mesh.FromStream(new MemoryStream(Models.anthill), MeshFlags.DoNotClip, renderDevice);
    //        fruitMesh = Mesh.FromStream(new MemoryStream(Models.apple), MeshFlags.DoNotClip, renderDevice);
    //    }

    //    /// <summary>
    //    /// Disposes all resources.
    //    /// </summary>
    //    public void Dispose()
    //    {
    //        playgroundMesh.Dispose();
    //        sugarCubeMesh.Dispose();
    //        antMesh.Dispose();
    //        bugMesh.Dispose();
    //        sugarMesh.Dispose();
    //        antHillMesh.Dispose();
    //        collisionBox.Dispose();
    //    }

    //    /// <summary>
    //    /// Prepares all needed Colony-Materials
    //    /// </summary>
    //    /// <param name="colony">Colony-ID</param>
    //    public void PrepareColony(int colony)
    //    {
    //        if (!antMaterial.ContainsKey(colony))
    //        {
    //            // choose another color
    //            Farbe color;
    //            switch (colony)
    //            {
    //                case 0:
    //                    color = new Farbe(0, 0, 0);
    //                    break;
    //                case 1:
    //                    color = new Farbe(255, 0, 0);
    //                    break;
    //                case 2:
    //                    color = new Farbe(0, 0, 255);
    //                    break;
    //                case 3:
    //                    color = new Farbe(255, 255, 0);
    //                    break;
    //                default:
    //                    color = colorFinder.ErzeugeFarbe();
    //                    break;
    //            }
    //            colorFinder.BelegeFarbe(color);

    //            // Material for ants and flag
    //            Material material = new Material();
    //            material.Emissive = Color.FromArgb(color.Rot, color.Grün, color.Blau);
    //            material.Specular = Color.FromArgb(200, 200, 200);
    //            antMaterial.Add(colony, material);

    //            // Material for markers
    //            material = new Material();
    //            material.Ambient = Color.FromArgb(MARKERTRANSPARENCY, color.Rot, color.Grün, color.Blau);
    //            material.Diffuse = Color.FromArgb(MARKERTRANSPARENCY, color.Rot, color.Grün, color.Blau);
    //            material.Emissive = Color.FromArgb(MARKERTRANSPARENCY, color.Rot, color.Grün, color.Blau);
    //            material.Specular = Color.FromArgb(MARKERTRANSPARENCY, color.Rot, color.Grün, color.Blau);
    //            markerMaterials.Add(colony, material);
    //        }
    //    }

    //    #endregion

    //    #region Render-functions

    //    /// <summary>
    //    /// Sets the size of playground.
    //    /// </summary>
    //    /// <param name="width">Width of playground</param>
    //    /// <param name="height">Height of playground</param>
    //    public void SetPlaygroundSize(int width, int height)
    //    {
    //        playgroundWidth = width / 2;
    //        playgroundHeight = height / 2;
    //    }

    //    /// <summary>
    //    /// Renders playground.
    //    /// </summary>
    //    public void RenderPlayground()
    //    {
    //        Matrix matrix = Matrix.Scaling(playgroundWidth * 2, 1.0f, playgroundHeight * 2);
    //        matrix.M42 = -0.5f;
    //        renderDevice.Transform.World = matrix;
    //        renderDevice.Material = playgroundMaterial;
    //        playgroundMesh.DrawSubset(0);
    //    }

    //    /// <summary>
    //    /// Renders a bug.
    //    /// </summary>
    //    /// <param name="state"><see cref="BugState"/> for additional information</param>
    //    /// <param name="pickray">current PickRay</param>
    //    /// <param name="selected">true, if bug is selected</param>
    //    /// <returns>distance from viewer to item, if <see cref="Pickray"/> hits</returns>
    //    public float RenderBug(BugState state, Pickray pickray, bool selected)
    //    {
    //        Matrix matrix = Matrix.Identity;
    //        matrix.RotateY((float)(state.Direction * Math.PI) / 180);
    //        matrix.M41 = (state.PositionX) - playgroundWidth;
    //        matrix.M43 = (-state.PositionY) + playgroundHeight;
    //        renderDevice.Material = (selected ? selectionMaterial : bugMaterial);
    //        renderDevice.Transform.World = matrix;
    //        bugMesh.DrawSubset(0);

    //        // Check for pickray-collision
    //        matrix.Invert();
    //        pickray.Origin.TransformCoordinate(matrix);
    //        pickray.Direction.TransformNormal(matrix);
    //        if (collisionBox.Intersect(pickray.Origin, pickray.Direction))
    //        {
    //            return
    //                Vector3.Subtract
    //                    (
    //                    pickray.Origin,
    //                    new Vector3((state.PositionX) - playgroundWidth, 0, (-state.PositionY) + playgroundHeight)).
    //                    Length();
    //        }

    //        return 0.0f;
    //    }

    //    /// <summary>
    //    /// Renders an ant.
    //    /// </summary>
    //    /// <param name="colony">ID of Colony</param>
    //    /// <param name="state">State</param>
    //    /// <param name="pickray"><see cref="Pickray"/></param>
    //    /// <param name="selected">true, if ant is selected</param>
    //    /// <returns>distance from viewer to item, if <see cref="Pickray"/> hits</returns>
    //    public float RenderAnt(int colony, AntState state, Pickray pickray, bool selected)
    //    {
    //        Matrix matrix = Matrix.Identity;
    //        matrix.RotateY((float)(state.Direction * Math.PI) / 180);
    //        matrix.M41 = state.PositionX - playgroundWidth;
    //        matrix.M43 = -state.PositionY + playgroundHeight;
    //        renderDevice.Material = (selected ? selectionMaterial : antMaterial[colony]);
    //        renderDevice.Transform.World = matrix;
    //        antMesh.DrawSubset(0);

    //        // Draw sugar-block, if ant has sugar loaded
    //        if (state.LoadType == LoadType.Sugar)
    //        {
    //            matrix.M42 = 3.5f;
    //            renderDevice.Material = sugarMaterial;
    //            renderDevice.Transform.World = matrix;
    //            sugarCubeMesh.DrawSubset(0);
    //        }

    //        if (selected && state.TargetPositionX != 0)
    //        {
    //            renderDevice.Transform.World = Matrix.Identity;
    //            renderDevice.Material = sugarMaterial;

    //            CustomVertex.PositionColored[] verts = new CustomVertex.PositionColored[2];
    //            verts[0].X = (float)state.PositionX - playgroundWidth;
    //            verts[0].Z = (float)-state.PositionY + playgroundHeight;
    //            verts[0].Y = 2;

    //            verts[1].X = (float)state.TargetPositionX - playgroundWidth;
    //            verts[1].Z = (float)-state.TargetPositionY + playgroundHeight;
    //            verts[1].Y = 2;

    //            renderDevice.VertexFormat = CustomVertex.PositionColored.Format;
    //            renderDevice.DrawUserPrimitives(PrimitiveType.LineList, 1, verts);
    //        }

    //        // Check for pickray-collision
    //        matrix.M42 = 0.0f;
    //        matrix.Invert();
    //        pickray.Origin.TransformCoordinate(matrix);
    //        pickray.Direction.TransformNormal(matrix);
    //        if (collisionBox.Intersect(pickray.Origin, pickray.Direction))
    //        {
    //            return
    //                Vector3.Subtract
    //                    (
    //                    pickray.Origin,
    //                    new Vector3(state.PositionX - playgroundWidth, 0, -state.PositionY + playgroundHeight)).Length
    //                    ();
    //        }

    //        return 0.0f;
    //    }

    //    /// <summary>
    //    /// Renders sugar.
    //    /// </summary>
    //    /// <param name="state"><see cref="SugarState"/></param>
    //    /// <param name="pickray"><see cref="Pickray"/></param>
    //    /// <param name="selected">true, if selected</param>
    //    /// <returns>distance from viewer to item, if <see cref="Pickray"/> hits</returns>
    //    public float RenderSugar(SugarState state, Pickray pickray, bool selected)
    //    {
    //        Matrix matrix =
    //            Matrix.Translation(state.PositionX - playgroundWidth, 0, -state.PositionY + playgroundHeight);
    //        matrix.M11 = matrix.M22 = matrix.M33 = state.Radius / 50.0f;
    //        renderDevice.Material = (selected ? selectionMaterial : sugarMaterial);
    //        renderDevice.Transform.World = matrix;
    //        sugarMesh.DrawSubset(0);

    //        // Check for pickray-collision
    //        matrix.M42 = 0.0f;
    //        matrix.Invert();
    //        pickray.Origin.TransformCoordinate(matrix);
    //        pickray.Direction.TransformNormal(matrix);
    //        if (collisionBox.Intersect(pickray.Origin, pickray.Direction))
    //        {
    //            return
    //                Vector3.Subtract
    //                    (
    //                    pickray.Origin,
    //                    new Vector3(state.PositionX - playgroundWidth, 0, -state.PositionY + playgroundHeight)).Length
    //                    ();
    //        }
    //        return 0.0f;
    //    }

    //    /// <summary>
    //    /// Renders Fruit.
    //    /// </summary>
    //    /// <param name="state"><see cref="FruitState"/></param>
    //    /// <param name="pickray"><see cref="Pickray"/></param>
    //    /// <param name="selected">true, if selected</param>
    //    /// <returns>distance from viewer to item, if <see cref="Pickray"/> hits</returns>
    //    public float RenderFruit(FruitState state, Pickray pickray, bool selected)
    //    {
    //        Matrix matrix =
    //            Matrix.Translation(state.PositionX - playgroundWidth, 0, -state.PositionY + playgroundHeight);
    //        matrix.M11 = state.Radius / 4.5f;
    //        matrix.M22 = state.Radius / 4.5f;
    //        matrix.M33 = state.Radius / 4.5f;
    //        renderDevice.Material = (selected ? selectionMaterial : fruitMaterial);
    //        renderDevice.Transform.World = matrix;
    //        fruitMesh.DrawSubset(0);

    //        // Check for pickray-collision
    //        matrix.M42 = 0.0f;
    //        matrix.Invert();
    //        pickray.Origin.TransformCoordinate(matrix);
    //        pickray.Direction.TransformNormal(matrix);
    //        if (collisionBox.Intersect(pickray.Origin, pickray.Direction))
    //        {
    //            return
    //                Vector3.Subtract
    //                    (
    //                    pickray.Origin,
    //                    new Vector3(state.PositionX - playgroundWidth, 0, -state.PositionY + playgroundHeight)).Length();
    //        }
    //        return 0.0f;
    //    }

    //    /// <summary>
    //    /// Render Anthill.
    //    /// </summary>
    //    /// <param name="colony">Colony-ID</param>
    //    /// <param name="state"><see cref="AnthillState"/></param>
    //    /// <param name="pickray"><see cref="Pickray"/></param>
    //    /// <param name="selected">true, if selected</param>
    //    /// <returns>distance from viewer to item, if <see cref="Pickray"/> hits</returns>
    //    public float RenderAnthill(int colony, AnthillState state, Pickray pickray, bool selected)
    //    {
    //        Matrix matrix =
    //            Matrix.Translation(state.PositionX - playgroundWidth, 0, -state.PositionY + playgroundHeight);
    //        renderDevice.Material = (selected ? selectionMaterial : antMaterial[colony]);
    //        renderDevice.Transform.World = matrix;
    //        antHillMesh.DrawSubset(0);

    //        // Check for pickray-collision
    //        matrix.M42 = 0.0f;
    //        matrix.Invert();
    //        pickray.Origin.TransformCoordinate(matrix);
    //        pickray.Direction.TransformNormal(matrix);
    //        if (collisionBox.Intersect(pickray.Origin, pickray.Direction))
    //        {
    //            return
    //                Vector3.Subtract
    //                    (
    //                    pickray.Origin,
    //                    new Vector3(state.PositionX - playgroundWidth, 0, -state.PositionY + playgroundHeight)).Length();
    //        }
    //        return 0.0f;
    //    }

    //    /// <summary>
    //    /// Render Marker.
    //    /// </summary>
    //    /// <param name="colony">Colony-ID</param>
    //    /// <param name="state"><see cref="MarkerState"/></param>
    //    public void RenderMarker(int colony, MarkerState state)
    //    {
    //        Matrix matrix =
    //            Matrix.Translation
    //                (state.PositionX - playgroundWidth, 0, -state.PositionY + playgroundHeight);
    //        matrix.M11 = state.Radius;
    //        matrix.M22 = state.Radius;
    //        matrix.M33 = state.Radius;

    //        // Enable transperency
    //        renderDevice.RenderState.AlphaBlendEnable = true;
    //        renderDevice.RenderState.ZBufferWriteEnable = false;
    //        renderDevice.RenderState.SourceBlend = Blend.SourceAlpha;
    //        renderDevice.RenderState.DestinationBlend = Blend.InvSourceAlpha;

    //        renderDevice.Material = markerMaterials[colony];
    //        renderDevice.Transform.World = matrix;
    //        markerMesh.DrawSubset(0);

    //        // Disable transparency
    //        renderDevice.RenderState.AlphaBlendEnable = false;
    //        renderDevice.RenderState.ZBufferWriteEnable = true;
    //    }

    //    /// <summary>
    //    /// Renders the statistic-box.
    //    /// </summary>
    //    /// <param name="state"><see cref="SimulationState"/></param>
    //    public void RenderInfobox(SimulationState state)
    //    {
    //        int player = 0;
    //        for (int i = 0; i < state.TeamStates.Count; i++)
    //        {
    //            player += state.TeamStates[i].ColonyStates.Count;
    //        }

    //        int height = ROWHEIGHT * player + 60;
    //        int position = (height / 2) + 10;
    //        line.Width = height;
    //        line.Begin();
    //        line.Draw
    //            (
    //            new Vector2[] { new Vector2(10, position), new Vector2(500, position) },
    //            Color.FromArgb(100, Color.White).ToArgb());
    //        line.End();

    //        fontNormal.DrawText(null, Resource.InfoboxColumnColony2, 20, ROWHEIGHT + 20, Color.Black);

    //        fontNormal.DrawText(null, Resource.InfoboxColumnCollectedFood1, 200, 20, Color.Green);
    //        fontNormal.DrawText(null, Resource.InfoboxColumnCollectedFood2, 200, ROWHEIGHT + 20, Color.Green);

    //        fontNormal.DrawText(null, Resource.InfoboxColumnKilledAnts1, 290, 20, Color.Red);
    //        fontNormal.DrawText(null, Resource.InfoboxColumnKilledAnts2, 290, ROWHEIGHT + 20, Color.Red);

    //        fontNormal.DrawText(null, Resource.InfoboxColumnKilledBugs1, 370, 20, Color.Blue);
    //        fontNormal.DrawText(null, Resource.InfoboxColumnKilledBugs2, 370, ROWHEIGHT + 20, Color.Blue);

    //        fontNormal.DrawText(null, Resource.InfoboxColumnPoints2, 440, ROWHEIGHT + 20, Color.Black);

    //        int count = 0;

    //        for (int i = 0; i < state.TeamStates.Count; i++)
    //        {
    //            for (int j = 0; j < state.TeamStates[i].ColonyStates.Count; j++)
    //            {
    //                ColonyState colony = state.TeamStates[i].ColonyStates[j];
    //                int killedAnts = colony.StarvedAnts + colony.EatenAnts + colony.BeatenAnts;

    //                fontBold.DrawText
    //                    (null, colony.ColonyName, 20, count * ROWHEIGHT + 55, antMaterial[count].Emissive);
    //                fontNormal.DrawText
    //                    (null, colony.CollectedFood.ToString(), 200, count * ROWHEIGHT + 55, Color.Green);
    //                fontNormal.DrawText
    //                    (null, killedAnts.ToString(), 290, count * ROWHEIGHT + 55, Color.Red);
    //                fontNormal.DrawText
    //                    (null, colony.KilledBugs.ToString(), 370, count * ROWHEIGHT + 55, Color.Blue);
    //                fontBold.DrawText
    //                    (null, colony.Points.ToString(), 440, count * ROWHEIGHT + 55, Color.Black);
    //                count++;
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// Draws the information-box for hovered items.
    //    /// </summary>
    //    /// <param name="position">Position of Information-Box</param>
    //    /// <param name="line1">Content of line 1</param>
    //    /// <param name="line2">Content of line 2</param>
    //    public void RenderInfoTag(Point position, String line1, String line2)
    //    {
    //        int height = 2 * ROWHEIGHT + 10;
    //        int positionY = (height / 2) + position.Y + 5;

    //        line.Width = height;
    //        line.Begin();
    //        line.Draw
    //            (
    //            new Vector2[] { new Vector2(position.X, positionY), new Vector2(position.X + 200, positionY) },
    //            Color.FromArgb(100, Color.White).ToArgb());
    //        line.End();

    //        fontNormal.DrawText(null, line1, position.X + 10, position.Y + 10, Color.Black);
    //        fontNormal.DrawText(null, line2, position.X + 10, position.Y + ROWHEIGHT + 10, Color.Black);
    //    }

    //    #endregion
    //}
}