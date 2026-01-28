using UnityEngine;
using TMPro;
using System.Collections.Generic;
//using  GameSettings.Instance.Get_result="Player O  wins!"
using UnityEngine.SceneManagement;

// make new class for bot 

// data flow should be linear 
// change name of class , follow convention
public class GameManager : MonoBehaviour,IGameController
{
    ///public static GAME_MANAGER Instance;
    //public GameMode gamemode;
    public bool botMode=false;
    public int grid_size;
    public TMP_Text turnText;
    public int []board;
    public int curr_player=1;  // 1==X && 2===O
    bool gameOver=false;
    bool move=true;
    void OnEnable()
    {
        Cell.controller = this;
    }

    void OnDisable()
    {
        if ((object)Cell.controller == this)
            Cell.controller = null;
    }

    

    void Start()
    {
        grid_size=GameSettings.Instance.grid_size;
        board=new int[grid_size*grid_size];
        curr_player=1;
        if(GameSettings.Instance.gamemode==GameMode.Offline)botMode=true;
        UpdateTurnText();
    }
    public void OnCellClicked(int index, Cell cell)
    {
       if(move) MakeMove(index, cell);
    }



    public void MakeMove(int index,Cell cell){
        move=false;
        if(gameOver)return;
        if(board[index]!=0)return;
        board[index]=curr_player;
        cell.SetSymbol(curr_player);
        if(check_win( index,curr_player)){
            gameOver=true;
             GameSettings.Instance.Get_result=(curr_player==1)? 1:2;
            //turnText.text=(curr_player==1)?"Player X  WINS!":"Player Y WINS!";
            //Debug.Log("Player"+curr_player+"win");
            SceneManager.LoadScene("Final_result");
            return ;
        }
        if(check_draw()){
            gameOver=true;
             GameSettings.Instance.Get_result=0;
            //turnText.text="Draw!";
            //Debug.Log("Draw");
             SceneManager.LoadScene("Final_result");
            return ;

        }
        curr_player=(curr_player==1)?2:1;
        UpdateTurnText();
       if(botMode) Invoke("bot_move",2f);
    }
    public void bot_move(){
        move=true;
        SoundManager.Instance.PlayClick();
        int bot_index=Give_bot_index();
        board[bot_index]=curr_player;
        Cell[] cells = FindObjectsByType<Cell>(FindObjectsSortMode.None);
        for(int i=0;i<cells.Length;i++){
            if(cells[i].index == bot_index){
                cells[i].SetSymbol(curr_player);
                break;
            }
        }
        if(check_win(bot_index,curr_player)){
            gameOver=true;
            //turnText.text="Player O  wins!";
            GameSettings.Instance.Get_result=2;
             SceneManager.LoadScene("Final_result");
            return;

        }
        if(check_draw()){
            gameOver=true;
             GameSettings.Instance.Get_result=0;
              SceneManager.LoadScene("Final_result");
            //turnText.text="Draw!";
            return;

        }
        curr_player=(curr_player==1)?2:1;
        UpdateTurnText();

    }
    int Give_bot_index(){
        for(int i=0;i<board.Length;i++){
            if(board[i]!=0)continue;
            board[i]=2;
            if(check_win(i,2)){
                board[i]=0;
                return i;
                
            }
            board[i]=0;
        }
        for(int i=0;i<board.Length;i++){
            if(board[i]!=0)continue;
            board[i]=1;
            if(check_win(i,1)){
                board[i]=0;
                return i;
                
            }
            board[i]=0;
        }
        List<int> empty_cell=new List<int>(); 
        for(int i = 0; i < board.Length; i++) {
            if(board[i]==0)empty_cell.Add(i);
            
        }
        return empty_cell[Random.Range(0,empty_cell.Count)];
    }
    void UpdateTurnText(){
        if(curr_player==1)turnText.text="Player X's Turn";
        else{
            turnText.text="Player O's Turn";
        }
    }
    
    bool check_win(int index,int curr_player){
        /*int row=0;
        int col=0;
        //if(index==0)row=col=0;
        if(index<grid_size)col=index;
        else {
            row=index/grid_size;
        }
        if(index!=0)col=index%grid_size;*/
        int row=index/grid_size;
        int col=index%grid_size;

        // ROW CHECK
        int cnt=0;
        for(int i=0;i<grid_size;i++){
            if(board[(row*grid_size)+i]==curr_player)cnt++;
        
        }
        if(cnt==grid_size)return true;
        // col check
        cnt=0;
        for(int i=0;i<grid_size;i++){
            if(board[(i*grid_size)+col]==curr_player)cnt++;
            
        }
        if(cnt==grid_size)return true;
        cnt=0;
        // for normal diagonal;
        if(row==col){
            

            for(int i=0;i<grid_size;i++){
                if(board[((i)*grid_size)+(i)]==curr_player)cnt++;
                


            }
            if(cnt== grid_size)return true;

        }
        /// for cross diagonal;
        cnt=0;
        if(row+col==grid_size-1) {
            
            int b=grid_size-1;
            for(int i=0;i<grid_size;i++){
                if(board[((i)*grid_size)+(b-i)]==curr_player)cnt++;
                

            }
            if(cnt==grid_size)return true ;
            
        }
        return false;
        


    }
    bool check_draw(){
        for(int i=0;i<board.Length;i++){
            if(board[i]==0)return false;
        }
        return true;
    }
    public void QuitOfflineGame()
    {
        // 3--- for X quit 
        // 4----for O quit
        GameSettings.Instance.Get_result=(curr_player==1)?3:4;
        SceneManager.LoadScene("Final_result");

        

    }

    

    
    void Update()
    {
        
    }
    
    /*public void makemove(int index,Cell cell){
        if(board[index]!=0)return;
        if(curr_player==1){
            cell.symbolImage(1);
            board[index]=1;
            //curr_player=2;
        }
        else {
            //cell.symbolImage=oSprite;
            cell.symbolImage(2);
            board[index]=2;
            //curr_player=1;
        }
        if(checkwin(ind)){
            TMP_Text.text=(curr_player==1)?"Player X win!":"Player o win";

        }
        if(check_draw){
            TMP_Text.text="DRAW!";

        }
        curr_player=

        

    }*/
    
    
    
    
    
    
    
    
}
