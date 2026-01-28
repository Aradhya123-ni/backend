using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public int index;
    //public GAME_MANAGER a;
    public Image symbolImage;
    public Sprite xSprite;
    public Sprite oSprite;
   // private GAME_MANAGER gameManager;
   public static IGameController controller;

    public void OnCellClicked()
    {
        if (controller != null)
        {
            controller.OnCellClicked(index, this);
            SoundManager.Instance.PlayClick();
        }
    }
    public void SetSymbol(int player){
        if(player==1){
            symbolImage.sprite=xSprite;
        
        }
        else{
            symbolImage.sprite=oSprite;
        }
        symbolImage.enabled=true;
        GetComponent<Button>().interactable=false;

    }
    public void ResetCell()
    {
        symbolImage.sprite = null; 
        symbolImage.enabled = false;
        GetComponent<Button>().interactable = true;
    }
    /*

    // Update is called once per frame
    void Update()
    {
        
    }*/
}
