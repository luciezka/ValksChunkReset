using System;
using Vintagestory.API.Common;
using Vintagestory.Server;

namespace ValksFireStarter.assets.ModConfig;

public class ModConfig : ModSystem
{
    public static ServerConfig Config { get; private set; }
    string cfgFileName = "ValksChunkReset.json";

    public override void StartPre(ICoreAPI api)
    {
        try
        {
            Config = api.LoadModConfig<ServerConfig>(cfgFileName);
            if (Config == null)
            {
                Config = new ServerConfig();
                Mod.Logger.VerboseDebug("Config file not found, creating a new one...");
            }

            api.StoreModConfig(Config, cfgFileName);
        }
        catch (Exception e)
        {
            Mod.Logger.Error("Failed to load config, you probably made a typo: {0}", e);
            Config = new ServerConfig();
        }
    }

    public override void Start(ICoreAPI api)
    {
        api.World.Config.SetInt("Radius", Config.Radius);
        api.World.Config.SetInt("XCoordinates", Config.XCoordinates);
        api.World.Config.SetInt("YCoordiantes", Config.ZCoordiantes);
        api.World.Config.SetInt("ResetStructureEveryXDays", Config.ResetStructureEveryXDays);
        api.World.Config.SetBool("ActivateReset", Config.ActivateReset);
        api.World.Config.SetInt("LastResetOnDay", Config.LastResetOnDay);
    }
}

public class ServerConfig
{
    public static ModConfig Loaded { get; set; } = new ModConfig();
    public int XCoordinates { get; set; } = 0;
    public int ZCoordiantes { get; set; } = 0;
    public int Radius { get; set; } = 7;
    public int ResetStructureEveryXDays = 50;
    public bool ActivateReset = false;
    public int LastResetOnDay;
}