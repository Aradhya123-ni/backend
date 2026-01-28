using UnityEngine;
 using UnityEngine.SceneManagement;

public enum GameMode
{
    Offline,
    PassNplay,
    Online
}

// you can turn this into a data container
public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance;
    public GameMode gamemode;
    public int grid_size;
    
    /// <summary>
    /// this should not be here
    /// </summary>
    public int Get_result;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
