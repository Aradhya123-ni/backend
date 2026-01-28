using UnityEngine;
using UnityEngine.UI;
using TMPro;
 using UnityEngine.SceneManagement;

public class Final_result : MonoBehaviour
{   
    public Image Symbol;
    public Sprite xSprite;
    public Sprite oSprite;
    public  TMP_Text turn;
    public int check;
    // Start is called once be\ fore the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(GameSettings.Instance.Get_result==0)turn.text="Draw!";
        if(GameSettings.Instance.Get_result==1)turn.text="Player X Wins!!";
        if(GameSettings.Instance.Get_result==2)turn.text="Player O wins!";
        if(GameSettings.Instance.Get_result==3)turn.text=" Player X left ,Player O wins!";
        if(GameSettings.Instance.Get_result==4)turn.text=" Player O left ,Player X wins!";

        //turn.text=GameSettings.Instance.Get_result;
        check=GameSettings.Instance.Get_result;
        Symbol.enabled=true; 
        if(check == 1){
            Symbol.sprite=xSprite;

        }
        if(check == 2){
            Symbol.sprite=oSprite;

        }
        if(check==0){
            Symbol.enabled=false;
        }
        if (check == 3)
        {
          Symbol.sprite=oSprite;
  
        }
         if (check == 4)
        {
          Symbol.sprite=xSprite;
  
        }
        
        
    }
    public void  OnClickReplay(){
         SoundManager.Instance.PlayClick();
        SceneManager.LoadScene("MainGame_arena");
       
        

    }
    public void  OnClickHome(){
        SoundManager.Instance.PlayClick();

         SceneManager.LoadScene("First_splash_page");
         
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
