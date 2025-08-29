using ValksFireStarter.assets.ModConfig;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace ValksFireStarter;

public class ValksChunkResetModSystem : ModSystem
{
    public override void StartServerSide(ICoreServerAPI api)
    {
        base.StartServerSide(api);
        api.Event.ServerRunPhase(EnumServerRunPhase.WorldReady, () =>
        {
            if (ModConfig.Config.ActivateReset && ResetArchive(api.World.Calendar))
            {
                api.Logger.Event("ValksChunkReset: Starting regeneration");
                int x = ModConfig.Config.XCoordinates;
                int y = ModConfig.Config.ZCoordiantes;

                int radius = ModConfig.Config.Radius;
                for (int dx = -radius; dx <= radius; dx++)
                {
                    for (int dy = -radius; dy <= radius; dy++)
                    {
                        api.WorldManager.DeleteChunkColumn(x + dx, y + dy);
                    }
                }

                api.Logger.Event("ValksChunkReset: Selected Chunks were regenerated");
                ModConfig.Config.LastResetOnDay = (int)api.World.Calendar.ElapsedDays;
                api.StoreModConfig(ModConfig.Config, "ValksChunkReset.json");
            }
        });
    }

    private bool ResetArchive(IGameCalendar calendar)
    {
        if (calendar.ElapsedDays > ModConfig.Config.LastResetOnDay +
            ModConfig.Config.ResetStructureEveryXDays)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}