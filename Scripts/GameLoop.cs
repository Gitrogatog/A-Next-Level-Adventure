namespace MyECS;

using System;
using CustomTilemap;
using Godot;
using MoonTools.ECS;
using MyECS.Components;
using MyECS.Prefabs;
using MyECS.Relations;

public partial class GameLoop : Node2D
{
    public static Node rootNode;
    public World World { get; } = new World();
    SystemGroup onReadyGroup = new SystemGroup();
    SystemGroup updateGroup = new SystemGroup();
    SystemGroup physicsUpdateGroup = new SystemGroup();
    SystemGroup postCollisionsGroup = new SystemGroup();
    SystemGroup tileDrawGroup = new SystemGroup();
    SystemGroup uiDrawGroup = new SystemGroup();
    Tilemap tilemap;

    // Godot nodes
    Godot.TileMap tilemapNode;
    DrawCallback tileCanvasNode;
    RichTextLabel dialogNode;
    RichTextLabel npcLevelNode;
    RichTextLabel nameNode;
    RichTextLabel playerLevelNode;
    Camera2D camera;
    SubViewport viewport;
    TextureRect gameTexture;
    TimeSpan lastProcessSpan;
    // [Export] public EntityTextureResource textureResource;
    // [Export] public DialogStorageResource dialogResource;
    [Export] public NPCDictionaryResource npcResource;
    [Export] EntityTextureResource entityTextures;
    [Export] PackedScene explosionScene;
    [Export] AudioStream[] explosionSFX;
    // [Export] AudioStream voiceSFX;
    // [Export] AudioStream[] milestoneSounds
    // sound to play on player win the game
    // music playing
    [Export] public Vector2I roomSize;

    [Export] public Color hurtColor1;
    [Export] public Color hurtColor2;
    [Export] public int exploreOuterLevel;
    [Export] public int fastModeLevel;


    // [Export] public Vector2I playerTile;
    // DialogStorage[] dialogs;
    // int[] powers;
    // Texture2D[][] textures;
    NPCData npcData;

    // [Export] private CharacterBody2D playerPrefab;

    public override void _Ready()
    {
        // GD.Print("Hello world!");
        rootNode = this;
        GD.Randomize();

        // load nodes
        viewport = GetNode<SubViewport>("GameViewport");
        tilemapNode = GetNode<TileMap>("GameViewport/TileMap");
        tileCanvasNode = GetNode<DrawCallback>("GameViewport/Drawing");

        dialogNode = GetNode<RichTextLabel>("Display/NPCUI/DialogBox");
        npcLevelNode = GetNode<RichTextLabel>("Display/NPCUI/Level");
        nameNode = GetNode<RichTextLabel>("Display/NPCUI/Name");
        playerLevelNode = GetNode<RichTextLabel>("Display/PlayerUI/Level");

        camera = GetNode<Camera2D>("GameViewport/Camera2D");
        gameTexture = GetNode<TextureRect>("Display/GameTexture");

        CanvasItem extraDialogIndicator = GetNode<CanvasItem>("Display/NPCUI/ExtraDialog");
        AnimatedSprite2D extraDialogSprite = GetNode<AnimatedSprite2D>("Display/NPCUI/ExtraDialog/Anim");
        extraDialogIndicator.Visible = false;

        Node explosionParent = GetNode<Node>("Display/ExplosionParent");
        Node audioParent = GetNode<Node>("AudioParent");

        Control alert = GetNode<Control>("Display/InfoUI/Alert");
        RichTextLabel alertText = GetNode<RichTextLabel>("Display/InfoUI/Alert/TextBox");
        Control fastMode = GetNode<Control>("Display/Inventory/FastMode");
        BaseButton fastButton = GetNode<BaseButton>("Display/Inventory/FastMode/CheckButton");
        Control lockHub = GetNode<Control>("Display/Inventory/Locks");
        RichTextLabel lockCount = GetNode<RichTextLabel>("Display/Inventory/Locks/NumBox");

        alert.Visible = false;
        lockHub.Visible = false;
        // fastMode.Visible = false;
        // fastButton.Disabled = true;

        Godot.Collections.Array<Node> nodes = tilemapNode.GetChildren();
        foreach (Node child in nodes)
        {
            if (child is Node2D node2D)
            {
                node2D.Visible = false;
            }
        }

        // load resources
        // load from npcResource
        npcData = npcResource.Load();
        entityTextures.LoadTextures();

        // TileEntityPrefabs.npcPowers = npcData.Powers;

        // textureResource.LoadTextures();
        // dialogStorage = dialogResource.LoadDialog();

        // Drawing.TileSize = tilemapNode.TileSet.TileSize;
        Vector2I tileSize = tilemapNode.TileSet.TileSize;
        // Rect2I usedTiles = tilemapNode.GetUsedRect();
        Vector2I tileOffset = tilemapNode.GetUsedRect().Position;

        Vector2I roomSizeInPixels = roomSize * tileSize;
        viewport.Size = roomSizeInPixels;
        gameTexture.Size = roomSizeInPixels;

        TilemapReader tilemapReader = new TilemapReader(tilemapNode, World, npcData.NameToID);
        tilemap = tilemapReader.ReadTilemap();
        // tilemap.LogTiles();

        AudioStreamPlayer[] explosionPlayers = UIUtils.CreateAudioPlayers(explosionSFX, audioParent, "SFX");
        // AudioStreamPlayer[] voicePlayers = UIUtils.CreateAudioPlayers(explosionSFX, audioParent, "Voice");
        AudioStreamPlayer voicePlayer = GetNode<AudioStreamPlayer>("AudioParent/VoicePlayer");
        AudioStreamPlayer musicPlayer = GetNode<AudioStreamPlayer>("AudioParent/MusicPlayer");
        VolumeSlider musicSlider = GetNode<VolumeSlider>("Display/InfoUI/Sliders/MusicSlider");

        SystemGroup postPlayerMoveGroup = new SystemGroup();
        postPlayerMoveGroup
            // movement and responses

            .Add(new MoveCharacterSystem(World, tilemap))
            .Add(new CameraFollowSystem(World, roomSize, tileSize.X, camera))

            .Add(new DestroyWallSystem(World, tilemap, tilemapNode))

            // update UI
            .Add(new ClearUISystem(World, new RichTextLabel[] { dialogNode, npcLevelNode, nameNode }))
            .Add(new ShowDialogSystem(World, dialogNode, npcData.Dialogs))
            .Add(new ShowDefeatUISystem(World, dialogNode, nameNode))
            .Add(new NPCLevelUISystem(World, npcLevelNode))
            .Add(new NPCNameUISystem(World, nameNode))
            .Add(new ShowExtraDialogIndicatorSystem(World, npcData.Dialogs, extraDialogIndicator, extraDialogSprite))
            .Add(new BonkWallDialogSystem(World, dialogNode, nameNode))

            .Add(new PlayerLevelUISystem(World, playerLevelNode))


            .Add(new EnableWallBustingSystem(World))
            .Add(new DeathExplosionSystem(World, gameTexture, explosionParent, 10, 3, explosionScene, explosionPlayers))


            .Add(new ShowFastModeSystem(World, fastModeLevel, fastMode, fastButton))
            .Add(new ShowLockSystem(World, lockHub, lockCount))

            // cleanup


            .Add(new MovementCleanupSystem(World))
            .Add(new InteractCleanupSystem(World))


            .Add(new BlockTileSystem(World, tilemap))// updates blocked tiles for NEXT FRAME (run now so marked for destruction entities dont disrupt)

            ;


        onReadyGroup
            .Add(new FinishLoadNPCSystem(World, npcData))
            .Add(new BlockTileSystem(World, tilemap))
            ;

        updateGroup
            .Add(new TimerSystem(World))
            .Add(new DestroyMarkedTimerSystem(World))


            .Add(new PlayerMovement(World))
            .Add(new RunPostMoveSystem(World, postPlayerMoveGroup))

            .Add(new PlayAudioOnDeathSystem(World, voicePlayer, 0.6f))
            .Add(new PlayAudioOnDestroyWallSystem(World, musicPlayer, 1f))
            .Add(new AlertOnGameEndSystem(World, "You Win!\nTry walking into walls!!!"))
            .Add(new AlertOnReachLevelSystem(World, exploreOuterLevel, "Explore the outer lands!"))
            .Add(new AlertOnReachLevelSystem(World, fastModeLevel, "Unlocked fast mode!"))
            .Add(new ShowAlertSystem(World, alert, alertText))
            .Add(new CreateDeadNPCSystem(World, 0.1f, 0.5f, hurtColor1, hurtColor2))

            .Add(new DependentSystem<SpriteModder>(World))


            .Add(new FlashColorsSystem(World))

            .Add(new DestroyOnDestroyedWallSystem(World))
            .Add(new DestroyEndOfFrameSystem(World))
            .Add(new KillNPCSystem(World))

            ;



        tileDrawGroup
            .Add(new RenderTileEntitiesSystem(World, tileCanvasNode, tileSize, tileOffset));
        tileCanvasNode.OnDraw += TileDraw;

        tilemapReader.ReadEntityLayer(tilemap);

        TimeSpan span = new TimeSpan(0);
        onReadyGroup.Run(span);
        // UIUtils.CheckTextBox(dialogNode, npcData.Dialogs);
        dialogNode.Text = "Arrow Keys or WASD to move.";

        // GD.Print($"upper 0: {UppercaseStorage.GetUpper("")}");
        // GD.Print($"upper 1: {UppercaseStorage.GetUpper("a")}");
        // GD.Print($"upper 2: {UppercaseStorage.GetUpper("ha")}");
        // GD.Print($"upper 3: {UppercaseStorage.GetUpper("wall")}");
        // GD.Print($"upper num: {UppercaseStorage.GetUpper("0aa")}");
        // GD.Print($"exists: {exists}");

    }
    public override void _Process(double delta)
    {
        // GD.Print("running process step");
        lastProcessSpan = DeltaToTimeSpan(delta);
        updateGroup.Run(lastProcessSpan);
        World.FinishUpdate();
    }
    public override void _PhysicsProcess(double delta)
    {
        // GD.Print("running physics step");
        TimeSpan span = DeltaToTimeSpan(delta);
        physicsUpdateGroup.Run(span);
        // CallDeferred(MethodName._PostCollisionsProcess, delta);
    }

    public void _PostCollisionsProcess(double delta)
    {
        // GD.Print("running post collision step");
        TimeSpan span = DeltaToTimeSpan(delta);
        postCollisionsGroup.Run(span);


    }
    public override void _Draw()
    {
        // drawGroup.Run(lastProcessSpan);

    }

    public void TileDraw()
    {
        tileDrawGroup.Run(lastProcessSpan);
    }

    void testingdraw()
    {
        // DrawTextureRect(playerTexture, new Rect2(Hacky.playerPos, Drawing.TileSize), false);
    }

    private static TimeSpan DeltaToTimeSpan(double delta)
    {
        long ticks = (long)(delta * 10000000);
        TimeSpan span = new TimeSpan(ticks);
        return span;
    }

}


