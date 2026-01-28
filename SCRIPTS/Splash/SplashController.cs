using UnityEngine;
using UnityEngine.SceneManagement;

// create new class to store scene names 
public class SplashController : MonoBehaviour
{
    
    public void onOfflineClick(){
        GameSettings.Instance.gamemode=GameMode.Offline;
        
        SceneManager.LoadScene("GRID_SELECTION");
    }
    public void onPassplayClick(){
        GameSettings.Instance.gamemode=GameMode.PassNplay;
        SceneManager.LoadScene("GRID_SELECTION");
    }
    public void onOnlineClick(){
        GameSettings.Instance.gamemode=GameMode.Online;
        SceneManager.LoadScene("GRID_SELECTION");
    }
    public void onCloseClick()
    {
        Application.Quit();

        
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
