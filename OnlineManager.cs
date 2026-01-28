using UnityEngine;
using NativeWebSocket;
using TMPro;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.SceneManagement;
using System.Dynamic;

//using System.Diagnostics;
//using System.Reflection.PortableExecutable;
//using System.Diagnostics;

//using System.Text.Encodings.Web;
//using System.Diagnostics;
//using System.Diagnostics;

public class OnlineManager : MonoBehaviour, IGameController
{
    [Header("Scene References")]
    //private bool opponentLeft = false;
    private bool gameOver = false;


    public  UnityEngine.UI.Button quitButton;

    public BoardManager boardManager;
   //private bool ignoreNextBoardUpdate = false;

     public UnityEngine.UI.Button replayButton;

    public GameObject ResultSplash;
    public GameObject ReplayConfirmation;
    public TMP_Text turnText_online;
    public TMP_Text turnText_Result;

    private WebSocket ws;
    private bool started = false;

    private int myPlayer;        // 1 = X, 2 = O
    private int currentPlayer;   // 0 = unknown, 1 = X, 2 = O (SERVER authoritative)

    // Queue to execute server messages on Unity main thread
    private Queue<Action> mainThreadActions = new Queue<Action>();

    // ================= UNITY FLOW =================

    public void StartOnline()
    {
        if (started) return;
        started = true;

        
        Cell.controller = this;
        ResultSplash.SetActive(false);
        ReplayConfirmation.SetActive(false);

        // Initial UI state
        currentPlayer = 1;
        turnText_online.text = "Connecting...";

        ConnectToServer();
    }

    async void ConnectToServer()
    {
        ws = new WebSocket("ws://192.168.43.252:8080/ws"); // change IP if needed

        ws.OnOpen += () =>
        {
            Debug.Log("WebSocket Connected");
            ws.SendText("{\"type\":\"find_match\",\"data\":{}}");
            //turnText_online.text="Connected , Looking for opponent";
        };

        ws.OnMessage += (bytes) =>
        {
            string msg = Encoding.UTF8.GetString(bytes);

            lock (mainThreadActions)
            {
                mainThreadActions.Enqueue(() =>
                {
                    HandleServerMessage(msg);
                });
            }
        };

        ws.OnError += (e) =>
        {
            Debug.LogError("WebSocket Error: " + e);
        };

        ws.OnClose += (e) =>
        {
            Debug.Log("WebSocket Closed");
            //lock (mainThreadActions)
       /*{
            mainThreadActions.Enqueue(() =>
            {
            HandleOpponentLeft_TEMP();
             });
       }*/
        };

        await ws.Connect();
    }
   /*void HandleOpponentLeft_TEMP()
{
    if (opponentLeft) return; // prevent double trigger
    opponentLeft = true;

    ResultSplash.SetActive(true);
    ReplayConfirmation.SetActive(false);

    turnText_online.text = "";
    turnText_Result.text = "Opponent left the game";

    Cell.controller = null;
}*/

    void Update()
    {
        if (ws != null)
            ws.DispatchMessageQueue();

        lock (mainThreadActions)
        {
            while (mainThreadActions.Count > 0)
            {
                mainThreadActions.Dequeue().Invoke();
            }
        }
    }

    // ================= SERVER MESSAGE HANDLING =================

    void HandleServerMessage(string json)
    {
        Debug.Log("SERVER: " + json);

        if (json.Contains("\"match_found\""))
        {
            
            myPlayer = json.Contains("\"X\"") ? 1 : 2;
           // turnText_online.text = "Opponent Found" ;
            turnText_online.text = (myPlayer==currentPlayer)?"U will play as X":"U will play as O";
            Invoke("UpdateTurnText",2);
            //UpdateTurnText();
            

        }
        else if (json.Contains("\"board_update\""))
        {
            /*if (ignoreNextBoardUpdate)
            {
                ignoreNextBoardUpdate = false;
                return;
            }*/
            ApplyBoard(json);
            UpdateTurnText();
        }
        else if (json.Contains("\"replay_invite\""))
        {
            //ReplayConfirmation.SetActive(true);
            ReplayConfirmation.SetActive(true);
            
        }
        else if (json.Contains("\"replay_denied\""))
        {
            turnText_online.text="";
            HandleReplayDenied();
            
        }
        else if (json.Contains("\"replay_start\""))
        {
           // isResettingForReplay = true;

            Debug.Log("its playing");
            //ignoreNextBoardUpdate=true;
            //Cell.controller = this;
            ReplayConfirmation.SetActive(false);
            ResultSplash.SetActive(false);
            boardManager.ResetBoardUI();
            currentPlayer = 1;
            UpdateTurnText();
        }
        else if (json.Contains("\"opponent_left\"")){
            Debug.Log("fine");

            HandleOpponentLeft();
            
        }
        else if (json.Contains("\"game_over\""))
        {  
            quitButton.enabled=false;
            gameOver=true;
            ResultSplash.SetActive(true);
            turnText_online.text="";
            if (json.Contains("\"winner\":\"draw\""))
            {
                turnText_Result.text="MATCH DRAW !! ";
            }
            else if (json.Contains("\"winner\":\"X\""))
            {
                turnText_Result.text=(myPlayer==1)?" YOU WON !! ":" YOU LOST !! ";
            }
            else if (json.Contains("\"winner\":\"O\""))
            {
                turnText_Result.text=(myPlayer==2)?" YOU WON !! ":" YOU LOST !! ";
            }

            
        }
    }
  


    // ================= BOARD + TURN =================
void HandleOpponentLeft()
    {
        ReplayConfirmation.SetActive(false);
        ResultSplash.SetActive(true);
        turnText_online.text = "";
        if(!gameOver)turnText_Result.text = "Opponent left the game,You won ";
        else turnText_Result.text = "Opponent left the game";
        Cell.controller = null;
        replayButton.interactable = false;


        //Invoke(nameof(ScenceChange), 4f);
    }
void ScenceChange()
    {
        SceneManager.LoadScene("First_splash_page");
    }
void HandleReplayDenied()
{
    
    ReplayConfirmation.SetActive(false);

    
    ResultSplash.SetActive(true);

    replayButton.interactable = false;

    turnText_Result.text = "Opponent declined replay,back to home";
    SceneManager.LoadScene("First_splash_page");

  
}

    void ApplyBoard(string json)
{
    int start = json.IndexOf('[');
    int end = json.IndexOf(']');

    if (start == -1 || end == -1)
        return;

    string boardData = json.Substring(start + 1, end - start - 1);
    string[] cells = boardData.Split(',');

    for (int i = 0; i < cells.Length; i++)
    {
        string value = cells[i].Replace("\"", "").Trim();

        if (value == "X")
            UpdateCell(i, 1);
        else if (value == "O")
            UpdateCell(i, 2);
        // empty = do nothing
    }
    currentPlayer=(currentPlayer==1)?2:1;
}

    void UpdateCell(int index, int player)
    {
        Debug.Log("running");
        Cell cell = boardManager.GetCell(index);
        if (cell != null)
            cell.SetSymbol(player);
    }

    // ================= TURN UI =================

    void UpdateTurnText()
    {
        /*if (currentPlayer == 0)
        {
            turnText_online.text = "Waiting for opponent...";
            return;
        }*/

        if (currentPlayer == myPlayer)
        {
            if (currentPlayer == 1)
            {
                turnText_online.text = " Your Turn (PLAY X)";
                
            }else 
                turnText_online.text = " Your Turn (PLAY O)";
        }
            
        else
            turnText_online.text = "Opponent's Turn";
    }

    // ================= SEND MOVE =================

    public void OnCellClicked(int index, Cell cell)
    {
        if (ws == null || ws.State != WebSocketState.Open)////////////////how??????
            return;

        // Block input if not your turn
        if (currentPlayer != myPlayer)
            return;

        string json = "{\"type\":\"move\",\"data\":{\"index\":" + index + "}}";
        ws.SendText(json);
    }
    public void OnReplayClicked () {
        SoundManager.Instance.PlayClick();
        //replayButton.interactable = false;
        if (ws == null || ws.State != WebSocketState.Open)
            return;
        /*ResultSplash.SetActive(false);
        //ReplayButton.interactable = false;
        turnText_online.text="Wating for  replay confirmation";
        ws.SendText("{\"type\":\"replay_request\"}");*/
        turnText_online.text = "Waiting for opponent  replay confirmation ";
        ws.SendText("{\"type\":\"replay_request\"}");
        
        
    }
    public void OnYesClicked()
    { 
        SoundManager.Instance.PlayClick();
        //replayButton.interactable = false;
        if (ws == null || ws.State != WebSocketState.Open)
            return;
        ReplayConfirmation.SetActive(false);
        //turnText_online.text="Confirmed Replay";
        ws.SendText("{\"type\":\"replay_accepted\"}");
        
    }
    public void OnNOClicked()
    {  
        SoundManager.Instance.PlayClick();
        replayButton.enabled=false;
        
        if (ws == null || ws.State != WebSocketState.Open)
            return;
        ReplayConfirmation.SetActive(false);

        ws.SendText("{\"type\":\"replay_denied\"}");
        
    }

    public async void QuitOnlineGame()
{
    SoundManager.Instance.PlayClick();

    // In case connectiion not established 
    if (ws == null)
    {
        SceneManager.LoadScene("First_splash_page");
        return;
    }

    
    if (!gameOver && ws.State == WebSocketState.Open)
    {
        ws.SendText("{\"type\":\"quit_game\"}");

        
        await System.Threading.Tasks.Task.Delay(100);
    }

    // Close socket
    if (ws.State == WebSocketState.Open)
        await ws.Close();

    SceneManager.LoadScene("First_splash_page");
}

    public async void OnHomeClicked()
{
    SoundManager.Instance.PlayClick();

   
    //turnText_online.text = "Opponent left";
    if (replayButton != null)
        replayButton.interactable = false;

   
    if (ws != null )
    {
        Debug.Log("working fine");
        //ws.SendText("{\"type\":\"leave_room\"}");
       // await System.Threading.Tasks.Task.Delay(100);
        await ws.Close();
    }
        Invoke(nameof(Load),1f);
    //SceneManager.LoadScene("First_splash_page");
}
public void Load()
    {
        SceneManager.LoadScene("First_splash_page");
    }

    async void OnApplicationQuit()
    {
        if (ws != null)
            await ws.Close();
    }
}
