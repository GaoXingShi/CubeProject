
public class MainController : Singleton<MainController>
{
    public SpawnCubeScript spawnCubeInfo;
    public TriggerCubeScript triggerCubeInfo;
    public SpawnManager spawn;
    public PlayerCubeInfo playerInfo;
    
    private void Start()
    {
        spawn.Init();
    }
}
