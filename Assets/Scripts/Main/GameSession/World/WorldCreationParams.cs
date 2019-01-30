public class WorldCreationParams : IWorldCreationParams
{
    public int seed;
    public WorldData worldData;

    public int Seed => seed;
    public WorldData WorldData => worldData;
}
