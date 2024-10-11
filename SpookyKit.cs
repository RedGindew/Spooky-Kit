using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ExtremelySimpleLogger;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Data;
using MLEM.Data.Content;
using MLEM.Textures;
using MLEM.Ui;
using MLEM.Ui.Elements;
using TinyLife;
using TinyLife.Actions;
using TinyLife.Emotions;
using TinyLife.Mods;
using TinyLife.Objects;
using TinyLife.Utilities;
using TinyLife.World;
using TinyLife.Tools;
using Action = TinyLife.Actions.Action;
using MLEM.Misc;
using TinyLife.Skills;

namespace SpookyKit;

public class SpookyKit : Mod
{

    // the logger that we can use to log info about this mod
    public static Logger Logger { get; private set; }

    // visual data about this mod
    public override string Name => "Spooky Kit";
    public override string Description => "Celebrate the Spooky Season! - v1.2";
    public override string IssueTrackerUrl => "https://x.com/RedGindew";
    public override string TestedVersionRange => "[0.43.11]";
    private Dictionary<Point, TextureRegion> uiTextures;
    public override TextureRegion Icon => this.uiTextures[new Point(0, 0)];


    public override void Initialize(Logger logger, RawContentManager content, RuntimeTexturePacker texturePacker, ModInfo info)
    {
        SpookyKit.Logger = logger;
        texturePacker.Add(new UniformTextureAtlas(content.Load<Texture2D>("UITex"), 8, 8), r => this.uiTextures = r, 1, true);
    }

    public override void AddGameContent(GameImpl game, ModInfo info)
    {
        // Basic Object
        FurnitureType.Register(new FurnitureType.TypeSettings("SpookyKit.BasicDiningTable", new Point(1, 1), ObjectCategory.Table, 150, new ColorScheme[] { ColorScheme.SimpleWood, ColorScheme.MutedPastels })
        {
            // Sets the Mod's icon to appear on the object in the Build Menu
            Icon = this.Icon,
            // Sets it's catagory in the Build Menu
            Tab = (FurnitureTool.Tab.DiningRoom),
            // Set's the colours available and then it's defaults
            Colors = new ColorSettings(ColorScheme.SimpleWood, ColorScheme.MutedPastels) { Defaults = new int[] { 1, 16 } },
            // Creates points on the object for clutter
            // TableSlots adds chair slots by default
            // New Point means the number of tiles it uses.
            ObjectSpots = ObjectSpot.TableSpots(new Point(1, 1)).ToArray()
        });
        FurnitureType.Register(new FurnitureType.TypeSettings("SpookyKit.SmallPumpkin", new Point(1, 1), ObjectCategory.SmallObject, 50, new ColorScheme[] { ColorScheme.MutedPastels, ColorScheme.Plants })
        {
            Icon = this.Icon,
            Tab = (FurnitureTool.Tab.Outside),
            Colors = new ColorSettings(ColorScheme.MutedPastels, ColorScheme.Plants){ Defaults = new int[] { 6, 7 } },
        });
        FurnitureType.Register(new FurnitureType.TypeSettings("SpookyKit.PackOPumpkins", new Point(1, 1), ObjectCategory.SmallObject, 80, new ColorScheme[] { ColorScheme.MutedPastels, ColorScheme.MutedPastels, ColorScheme.MutedPastels, ColorScheme.Plants })
        {
            Icon = this.Icon,
            Tab = (FurnitureTool.Tab.Outside),
            Colors = new ColorSettings(ColorScheme.MutedPastels, ColorScheme.MutedPastels, ColorScheme.MutedPastels, ColorScheme.Plants){ Defaults = new int[] { 6, 5, 7, 1 } },
        });
        FurnitureType.Register(new FurnitureType.TypeSettings("SpookyKit.FallLeaves", new Point(1, 1), ObjectCategory.SmallObject, 20, new ColorScheme[] { ColorScheme.Plants })
        {
            Icon = this.Icon,
            Tab = (FurnitureTool.Tab.Outside),
            Colors = new ColorSettings(ColorScheme.Plants){ Defaults = new int[] { 6 } },
        });
        FurnitureType.Register(new FurnitureType.TypeSettings("SpookyKit.Cobwebs", new Point(1, 1), ObjectCategory.WallHanging | ObjectCategory.NonColliding, 50, ColorScheme.Grays)
        {
            Icon = this.Icon,
            Tab = (FurnitureTool.Tab.Decoration),
            Colors = new ColorSettings(ColorScheme.Grays){Defaults = new int[] { 0 }},
            DefaultRotation = MLEM.Maths.Direction2.Right
        });
        FurnitureType.Register(new FurnitureType.TypeSettings("SpookyKit.SpookyGarland", new Point(1, 1), ObjectCategory.WallHanging | ObjectCategory.NonColliding, 50, new ColorScheme[] { ColorScheme.White, ColorScheme.MutedPastels, ColorScheme.MutedPastels, ColorScheme.MutedPastels })
        {
            Icon = this.Icon,
            Tab = (FurnitureTool.Tab.Decoration),
            Colors = new ColorSettings(ColorScheme.White, ColorScheme.MutedPastels, ColorScheme.MutedPastels, ColorScheme.MutedPastels){Defaults = new int[] { 0, 6, 16, 8 }},
            DefaultRotation = MLEM.Maths.Direction2.Right
        });
                FurnitureType.Register(new FurnitureType.TypeSettings("SpookyKit.SmallShroom", new Point(1, 1), ObjectCategory.SmallObject | ObjectCategory.NonColliding, 50, new ColorScheme[] { ColorScheme.MutedPastels, ColorScheme.MutedPastels, ColorScheme.White })
        {
            Icon = this.Icon,
            Tab = (FurnitureTool.Tab.Outside),
            Colors = new ColorSettings(ColorScheme.MutedPastels, ColorScheme.MutedPastels, ColorScheme.White){ Defaults = new int[] { 3, 15, 0 } },
        });
        FurnitureType.Register(new FurnitureType.TypeSettings("SpookyKit.MangledRoots", new Point(1, 1), ObjectCategory.SmallObject | ObjectCategory.NonColliding, 50, new ColorScheme[] { ColorScheme.SimpleWood })
        {
            Icon = this.Icon,
            Tab = (FurnitureTool.Tab.Outside),
            Colors = new ColorSettings(ColorScheme.SimpleWood){ Defaults = new int[] { 3 } },
        });
        FurnitureType.Register(new FurnitureType.TypeSettings("SpookyKit.MarketCounter", new Point(1, 1), ObjectCategory.Counter, 40, new ColorScheme[] { ColorScheme.SimpleWood, ColorScheme.SimpleWood })
        {
            Icon = this.Icon,
            Tab = (FurnitureTool.Tab.Kitchen),
            DefaultRotation = MLEM.Maths.Direction2.Right,
            Colors = new ColorSettings(ColorScheme.SimpleWood, ColorScheme.SimpleWood){Defaults = new int[] { 1, 2 }},
            ConstructedType = typeof(CornerFurniture.Counter),
            ObjectSpots = ObjectSpot.CounterSpots(false).ToArray()
        });
        FurnitureType.Register(new FurnitureType.TypeSettings("SpookyKit.CafeAwning", new Point(1, 1), ObjectCategory.WallHanging | ObjectCategory.NonColliding, 50, new ColorScheme[] { ColorScheme.MutedPastels, ColorScheme.MutedPastels })
        {
            Icon = this.Icon,
            Tab = (FurnitureTool.Tab.Decoration),
            Colors = new ColorSettings(ColorScheme.MutedPastels, ColorScheme.MutedPastels){Defaults = new int[] { 14, 12 }},
            DefaultRotation = MLEM.Maths.Direction2.Right
        });
        FurnitureType.Register(new FurnitureType.TypeSettings("SpookyKit.Barrel", new Point(1, 1), ObjectCategory.SmallObject, 50, new ColorScheme[] { ColorScheme.SimpleWood, ColorScheme.Grays } )
        {
            Icon = this.Icon,
            Tab = (FurnitureTool.Tab.Outside),
            Colors = new ColorSettings(ColorScheme.SimpleWood, ColorScheme.Grays){ Defaults = new int[] { 1, 1 } },
        });
        FurnitureType.Register(new FurnitureType.TypeSettings("SpookyKit.StackBarrels", new Point(1, 1), ObjectCategory.SmallObject, 50, new ColorScheme[] { ColorScheme.SimpleWood, ColorScheme.Grays } )
        {
            Icon = this.Icon,
            Tab = (FurnitureTool.Tab.Outside),
            Colors = new ColorSettings(ColorScheme.SimpleWood, ColorScheme.Grays){ Defaults = new int[] { 1, 1 } },
        });
        FurnitureType.Register(new FurnitureType.TypeSettings("SpookyKit.BatBookcase", new Point(1, 1), ObjectCategory.Bookshelf, 50, new ColorScheme[] { ColorScheme.SimpleWood, ColorScheme.Grays })
        {
            Icon = this.Icon,
            Tab = (FurnitureTool.Tab.Office),
            Colors = new ColorSettings(ColorScheme.SimpleWood, ColorScheme.Grays){ Defaults = new int[] { 7, 1 } },
            DefaultRotation = MLEM.Maths.Direction2.Right,
        });
        FurnitureType.Register(new FurnitureType.TypeSettings("SpookyKit.CrookedTree", new Point(1, 1), ObjectCategory.SmallObject, 50, new ColorScheme[] { ColorScheme.SimpleWood, ColorScheme.Plants })
        {
            Icon = this.Icon,
            Tab = (FurnitureTool.Tab.Outside),
            Colors = new ColorSettings(new ColorScheme[] { ColorScheme.SimpleWood, ColorScheme.Plants }){ Defaults = new int[] { 2, 2 } },
            DefaultRotation = MLEM.Maths.Direction2.Up,
        });
        FurnitureType.Register(new FurnitureType.TypeSettings("SpookyKit.OldOak", new Point(1, 1), ObjectCategory.SmallObject, 50, new ColorScheme[] { ColorScheme.SimpleWood, ColorScheme.Plants })
        {
            Icon = this.Icon,
            Tab = (FurnitureTool.Tab.Outside),
            Colors = new ColorSettings(new ColorScheme[] { ColorScheme.SimpleWood, ColorScheme.Plants }){ Defaults = new int[] { 2, 2 } },
            DefaultRotation = MLEM.Maths.Direction2.Up,
        });
        FurnitureType.Register(new FurnitureType.TypeSettings("SpookyKit.Grave0", new Point(1, 1), ObjectCategory.SmallObject, 50, new ColorScheme[] { ColorScheme.Grays })
        {
            Icon = this.Icon,
            Tab = (FurnitureTool.Tab.Decoration),
            Colors = new ColorSettings(ColorScheme.Grays){ Defaults = new int[] { 0 } },
        });
    }

    public override IEnumerable<string> GetCustomFurnitureTextures(ModInfo info)
    {
        yield return "BasicDiningTable";
        yield return "SmallPumpkin";
        yield return "PackOPumpkins";
        yield return "FallLeaves";
        yield return "SpookyGarland";
        yield return "Cobwebs";
        yield return "SmallShroom";
        yield return "MangledRoots";
        yield return "MarketCounter";
        yield return "CafeAwning";
        yield return "Barrel";
        yield return "BatBookcase";
        yield return "StackBarrels";
        yield return "CrookedTree";
        yield return "OldOak";
        yield return "Grave0";
    }
}