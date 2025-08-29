using ValksFireStarter.assets.ModConfig;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace ValksFireStarter;

public class ValksChunkResetModSystem : ModSystem
{ 
    ModConfig fromDisk;
    string cfgFileName = "ValksChunkReset.json";
    
    public override void StartPre(ICoreAPI api)
    {
        try
        {
            if ((fromDisk = api.LoadModConfig<ModConfig>(cfgFileName)) == null)
            { api.StoreModConfig(ModConfig.Loaded, cfgFileName); }
            else
            { ModConfig.Loaded = fromDisk; }
        }
        catch
        { api.StoreModConfig(ModConfig.Loaded, cfgFileName); }
        base.StartPre(api);
    }
    
    
    public override void StartServerSide(ICoreServerAPI api)
    {
        base.StartServerSide(api);
            api.Event.ServerRunPhase(EnumServerRunPhase.WorldReady, () =>
                {
            if (ModConfig.Loaded.activateReset && resetArchive(api.World.Calendar))
            {
                api.Logger.Event("ValksChunkReset: Starting regeneration");
                int x = ModConfig.Loaded.xCoordinates;
                int y = ModConfig.Loaded.yCoordiantes;

                int radius =  ModConfig.Loaded.radius;
                for (int dx = -radius; dx <= radius; dx++)
                {
                    for (int dy = -radius; dy <= radius; dy++)
                    {
                        api.WorldManager.DeleteChunkColumn(x + dx, y + dy); 
                    }
                }
                api.Logger.Event("ValksChunkReset: Selected Chunks were regenerated");
                ModConfig.Loaded.lastResetOnDay = (int)api.World.Calendar.ElapsedDays;
                api.StoreModConfig(ModConfig.Loaded,cfgFileName);
                
            }
        });
    }
    
    public bool resetArchive(IGameCalendar calendar)
    {
        if (calendar.ElapsedDays > ModConfig.Loaded.lastResetOnDay +
            ModConfig.Loaded.resetStructureEveryXDays)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
}