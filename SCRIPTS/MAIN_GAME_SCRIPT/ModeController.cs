using UnityEngine;
using TMPro;

public class ModeController : MonoBehaviour
{
    public GameManager gamemanager;
    public OnlineManager onlinemanager;
    public TMP_Text onlineTurnText;
    public TMP_Text offlineTurnText;

    void Start()
    {
        
        gamemanager.gameObject.SetActive(false);
        onlinemanager.gameObject.SetActive(false);

        onlineTurnText.gameObject.SetActive(false);
        offlineTurnText.gameObject.SetActive(false);
        

        if (GameSettings.Instance.gamemode == GameMode.Online)
        {
            onlinemanager.gameObject.SetActive(true);
            onlineTurnText.gameObject.SetActive(true);
        //
            onlinemanager.StartOnline();   //  ONLY PLACE SERVER CONNECTS
        }
        else
        {
            gamemanager.gameObject.SetActive(true);
            offlineTurnText.gameObject.SetActive(true);
        }
    }
    public void OnBackClicked()
    {
        switch (GameSettings.Instance.gamemode)
        {
            case GameMode.Online:
                onlinemanager.QuitOnlineGame();
                break;

            case GameMode.Offline:
                gamemanager.QuitOfflineGame(); 
                break;

            default:
                Debug.Log("No active game mode");
                break;
        }
    }
}

