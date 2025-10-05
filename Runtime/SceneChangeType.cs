namespace Calluna.SceneManagement
{
    public enum SceneChangeType
    {
        Load = 1 << 0,
        LoadAdditive = 1 << 1,
        Unload = 1 << 2
    }
}