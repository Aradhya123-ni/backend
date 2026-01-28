using UnityEngine;
using UnityEngine.UI;

public class SoundToggleButton : MonoBehaviour
{
 
    public Image icon;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;

    void Start()
    {
        UpdateIcon();
    }

    public void ToggleSound()
    {
        SoundManager.Instance.ToggleSound();
        UpdateIcon();
        SoundManager.Instance.PlayClick();
    }

    void UpdateIcon()
    {
        icon.sprite = SoundManager.Instance.IsSoundOn()
            ? soundOnSprite
            : soundOffSprite;
    }
}


