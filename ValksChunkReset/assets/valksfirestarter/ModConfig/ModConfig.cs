namespace ValksFireStarter.assets.ModConfig;

public class ModConfig
{
    public static ModConfig Loaded { get; set; } = new ModConfig();
    public int xCoordinates  { get; set; } = 0;
    public int yCoordiantes  { get; set; } = 0;
    public int radius  { get; set; } = 0;
    public int resetStructureEveryXDays  = 1;
    public bool activateReset = false;
    public int lastResetOnDay;
        
}