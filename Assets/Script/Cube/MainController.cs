
public class MainController : Singleton<MainController>
{
    public LoadInfoManager load;
    public SpawnManager spawn;
    public PlayerInfoManager player;

    private void Awake()
    {
        load.LoadJsonFile();
    }

    private void Start()
    {
        spawn.Init();

    }
}
