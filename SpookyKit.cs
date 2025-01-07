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
    public override string TestedVersionRange => "[0.44.1]";
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
            // Lists the Colours used and any settings such as their default color
            Colors = new ColorSettings(ColorScheme.SimpleWood, ColorScheme.MutedPastels) { Defaults = new int[] { 1, 16 }, PreviewName = "SpookyKit.BasicDiningTable" },
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
            Colors = new ColorSettings(ColorScheme.MutedPastels, ColorScheme.MutedPastels, ColorScheme.MutedPastels, ColorScheme.Plants){ Defaults = new int[] { 6, 5, 7, 1 }, PreviewName = "SpookyKit.PackOPumpkins" },
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
            Colors = new ColorSettings(ColorScheme.Grays){Defaults = new int[] { 0 } },
            DefaultRotation = MLEM.Maths.Direction2.Right
        });
        FurnitureType.Register(new FurnitureType.TypeSettings("SpookyKit.SpookyGarland", new Point(1, 1), ObjectCategory.WallHanging | ObjectCategory.NonColliding, 50, new ColorScheme[] { ColorScheme.White, ColorScheme.MutedPastels, ColorScheme.MutedPastels, ColorScheme.MutedPastels })
        {
            Icon = this.Icon,
            Tab = (FurnitureTool.Tab.Decoration),
            Colors = new ColorSettings(ColorScheme.White, ColorScheme.MutedPastels, ColorScheme.MutedPastels, ColorScheme.MutedPastels){Defaults = new int[] { 0, 6, 16, 8 }, PreviewName = "SpookyKit.SpookyGarland" },
            DefaultRotation = MLEM.Maths.Direction2.Right
        });
                FurnitureType.Register(new FurnitureType.TypeSettings("SpookyKit.SmallShroom", new Point(1, 1), ObjectCategory.SmallObject | ObjectCategory.NonColliding, 50, new ColorScheme[] { ColorScheme.MutedPastels, ColorScheme.MutedPastels, ColorScheme.White })
        {
            Icon = this.Icon,
            Tab = (FurnitureTool.Tab.Outside),
            Colors = new ColorSettings(ColorScheme.MutedPastels, ColorScheme.MutedPastels, ColorScheme.White){ Defaults = new int[] { 3, 15, 0 }, PreviewName = "SpookyKit.SmallShroom" },
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
            Colors = new ColorSettings(ColorScheme.SimpleWood, ColorScheme.SimpleWood){Defaults = new int[] { 1, 2 }, PreviewName = "SpookyKit.MarketCounter" },
            ConstructedType = typeof(CornerFurniture.Counter),
            ObjectSpots = ObjectSpot.CounterSpots(false).ToArray()
        });
        FurnitureType.Register(new FurnitureType.TypeSettings("SpookyKit.CafeAwning", new Point(1, 1), ObjectCategory.WallHanging | ObjectCategory.NonColliding, 50, new ColorScheme[] { ColorScheme.MutedPastels, ColorScheme.MutedPastels })
        {
            Icon = this.Icon,
            Tab = (FurnitureTool.Tab.Decoration),
            Colors = new ColorSettings(ColorScheme.MutedPastels, ColorScheme.MutedPastels){Defaults = new int[] { 14, 12 }, PreviewName = "SpookyKit.CafeAwning" },
            DefaultRotation = MLEM.Maths.Direction2.Right
        });
        FurnitureType.Register(new FurnitureType.TypeSettings("SpookyKit.Barrel", new Point(1, 1), ObjectCategory.SmallObject, 50, new ColorScheme[] { ColorScheme.SimpleWood, ColorScheme.Grays } )
        {
            Icon = this.Icon,
            Tab = (FurnitureTool.Tab.Outside),
            Colors = new ColorSettings(ColorScheme.SimpleWood, ColorScheme.Grays){ Defaults = new int[] { 1, 1 }, PreviewName = "SpookyKit.Barrel" },
        });
        FurnitureType.Register(new FurnitureType.TypeSettings("SpookyKit.StackBarrels", new Point(1, 1), ObjectCategory.SmallObject, 50, new ColorScheme[] { ColorScheme.SimpleWood, ColorScheme.Grays } )
        {
            Icon = this.Icon,
            Tab = (FurnitureTool.Tab.Outside),
            Colors = new ColorSettings(ColorScheme.SimpleWood, ColorScheme.Grays){ Defaults = new int[] { 1, 1 }, PreviewName = "SpookyKit.StackBarrels" },
        });
        FurnitureType.Register(new FurnitureType.TypeSettings("SpookyKit.BatBookcase", new Point(1, 1), ObjectCategory.Bookshelf, 50, new ColorScheme[] { ColorScheme.SimpleWood, ColorScheme.Grays })
        {
            Icon = this.Icon,
            Tab = (FurnitureTool.Tab.Office),
            Colors = new ColorSettings(ColorScheme.SimpleWood, ColorScheme.Grays){ Defaults = new int[] { 7, 1 }, PreviewName = "SpookyKit.BatBookcase" },
            DefaultRotation = MLEM.Maths.Direction2.Right,
        });
        FurnitureType.Register(new FurnitureType.TypeSettings("SpookyKit.CrookedTree", new Point(1, 1), ObjectCategory.SmallObject, 50, new ColorScheme[] { ColorScheme.SimpleWood, ColorScheme.Plants })
        {
            Icon = this.Icon,
            Tab = (FurnitureTool.Tab.Outside),
            Colors = new ColorSettings(new ColorScheme[] { ColorScheme.SimpleWood, ColorScheme.Plants }){ Defaults = new int[] { 2, 2 }, PreviewName = "SpookyKit.CrookedTree" },
            DefaultRotation = MLEM.Maths.Direction2.Up,
        });
        FurnitureType.Register(new FurnitureType.TypeSettings("SpookyKit.OldOak", new Point(1, 1), ObjectCategory.SmallObject, 50, new ColorScheme[] { ColorScheme.SimpleWood, ColorScheme.Plants })
        {
            Icon = this.Icon,
            Tab = (FurnitureTool.Tab.Outside),
            Colors = new ColorSettings(new ColorScheme[] { ColorScheme.SimpleWood, ColorScheme.Plants }){ Defaults = new int[] { 2, 2 }, PreviewName = "SpookyKit.OldOak" },
            DefaultRotation = MLEM.Maths.Direction2.Up,
        });
/*         FurnitureType.Register(new FurnitureType.TypeSettings("SpookyKit.Grave0", new Point(1, 1), ObjectCategory.SmallObject, 50, new ColorScheme[] { ColorScheme.Grays })
        {
            Icon = this.Icon,
            Tab = (FurnitureTool.Tab.Decoration),
            Colors = new ColorSettings(ColorScheme.Grays){ Defaults = new int[] { 0 } },
        }); */
            FurnitureType.Register(new FurnitureType.TypeSettings("SpookyKit.Grave0", new Point(1, 1), ObjectCategory.Lamp, 50, new ColorScheme[] { ColorScheme.Grays })
        {
            Icon = this.Icon,
            Tab = (FurnitureTool.Tab.Lighting),
            Colors = new ColorSettings(ColorScheme.Grays){ Defaults = new int[] { 0 } },
        });
            FurnitureType.Register(new FurnitureType.TypeSettings("SpookyKit.Candlestick", new Point(1, 1), ObjectCategory.Lamp | ObjectCategory.SmallObject, 50, new ColorScheme[] { ColorScheme.White })
        {
            Icon = this.Icon,
            Tab = (FurnitureTool.Tab.Lighting),
            Colors = new ColorSettings(ColorScheme.White){ Defaults = new int[] { 0 } },
            LightSettings = new LightFurniture.Settings
            {
                CreateLights = f => [
                    new Light(f.Map, f.Position, f.Floor, Light.CircleTexture, new Vector2(6, 8), new Color(255, 181, 112)) {
                        VisualWorldOffset = new Vector2(0.5F),
                        Scale = 0.5f,
                    }
                ],
                IsAutomatic = false,
                IsElectrical = true,
                Flickers = true
            }
        });
        // -------------------------------------------- Standard Roof

                FurnitureType.Register(new FurnitureType.TypeSettings("SpookyKit.Roof", new Point(1, 1), ObjectCategory.SmallObject, 50, new ColorScheme[] { ColorScheme.Grays })
        {
            Icon = this.Icon,
            Tab = (FurnitureTool.Tab.Outside),
            Colors = new ColorSettings(new ColorScheme[] { ColorScheme.Grays }){ Defaults = new int[] { 0 }, PreviewName = "SpookyKit.Roof" },
            DefaultRotation = MLEM.Maths.Direction2.Up,
        });
        FurnitureType.Register(new FurnitureType.TypeSettings("SpookyKit.RoofCorner", new Point(1, 1), ObjectCategory.SmallObject, 50, new ColorScheme[] { ColorScheme.Grays, ColorScheme.ColoredMetal })
        {
            Icon = this.Icon,
            Tab = (FurnitureTool.Tab.Outside),
            Colors = new ColorSettings(new ColorScheme[] { ColorScheme.Grays, ColorScheme.ColoredMetal }){ Defaults = new int[] { 0, 0 }, PreviewName = "SpookyKit.RoofCorner" },
            DefaultRotation = MLEM.Maths.Direction2.Up,
        });
        FurnitureType.Register(new FurnitureType.TypeSettings("SpookyKit.RoofCap", new Point(1, 1), ObjectCategory.SmallObject, 50, new ColorScheme[] { ColorScheme.Grays, ColorScheme.ColoredMetal })
        {
            Icon = this.Icon,
            Tab = (FurnitureTool.Tab.Outside),
            Colors = new ColorSettings(new ColorScheme[] { ColorScheme.Grays, ColorScheme.ColoredMetal }){ Defaults = new int[] { 0, 0 }, PreviewName = "SpookyKit.RoofCap" },
            DefaultRotation = MLEM.Maths.Direction2.Up,
        });
        FurnitureType.Register(new FurnitureType.TypeSettings("SpookyKit.RoofInner", new Point(1, 1), ObjectCategory.SmallObject, 50, new ColorScheme[] { ColorScheme.Grays, ColorScheme.ColoredMetal })
        {
            Icon = this.Icon,
            Tab = (FurnitureTool.Tab.Outside),
            Colors = new ColorSettings(new ColorScheme[] { ColorScheme.Grays, ColorScheme.ColoredMetal }){ Defaults = new int[] { 0, 0 }, PreviewName = "SpookyKit.RoofInner" },
            DefaultRotation = MLEM.Maths.Direction2.Up,
        });

        // -------------------------------------------- Tiled Roof

                FurnitureType.Register(new FurnitureType.TypeSettings("SpookyKit.TiledRoof", new Point(1, 1), ObjectCategory.SmallObject, 50, new ColorScheme[] { ColorScheme.Grays })
        {
            Icon = this.Icon,
            Tab = (FurnitureTool.Tab.Outside),
            Colors = new ColorSettings(new ColorScheme[] { ColorScheme.Grays }){ Defaults = new int[] { 0 }, PreviewName = "SpookyKit.TiledRoof" },
            DefaultRotation = MLEM.Maths.Direction2.Up,
        });
        FurnitureType.Register(new FurnitureType.TypeSettings("SpookyKit.TiledRoofCorner", new Point(1, 1), ObjectCategory.SmallObject, 50, new ColorScheme[] { ColorScheme.Grays, ColorScheme.ColoredMetal })
        {
            Icon = this.Icon,
            Tab = (FurnitureTool.Tab.Outside),
            Colors = new ColorSettings(new ColorScheme[] { ColorScheme.Grays, ColorScheme.ColoredMetal }){ Defaults = new int[] { 0, 0 }, PreviewName = "SpookyKit.TiledRoofCorner" },
            DefaultRotation = MLEM.Maths.Direction2.Up,
        });
        FurnitureType.Register(new FurnitureType.TypeSettings("SpookyKit.TiledRoofCap", new Point(1, 1), ObjectCategory.SmallObject, 50, new ColorScheme[] { ColorScheme.Grays, ColorScheme.ColoredMetal })
        {
            Icon = this.Icon,
            Tab = (FurnitureTool.Tab.Outside),
            Colors = new ColorSettings(new ColorScheme[] { ColorScheme.Grays, ColorScheme.ColoredMetal }){ Defaults = new int[] { 0, 0 }, PreviewName = "SpookyKit.TiledRoofCap" },
            DefaultRotation = MLEM.Maths.Direction2.Up,
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
        yield return "Roof";
        yield return "RoofCorner";
        yield return "RoofCap";
        yield return "RoofInner";
        yield return "TiledRoof";
        yield return "TiledRoofCorner";
        yield return "TiledRoofCap";
        yield return "Candlestick";
    }
}