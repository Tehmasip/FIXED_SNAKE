using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    public Sprite ONPIC;
    public Sprite OFFPIC;

    public Sprite ONMPIC;
    public Sprite OFFMPIC;

    public Image SFXImage;
    public Image BGSImage;

    public static SettingsScript instance;
    // Start is called before the first frame update
    void Awake()
    {
        
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        if (GameConstants.GetContant("SFX") == 0) 
        {
            SFXImage.sprite = ONPIC;
        }
        else
        {
            SFXImage.sprite = OFFPIC;
        }

        if (GameConstants.GetContant("BGS") == 0)
        {
            BGSImage.sprite = ONMPIC;
        }
        else
        {
            BGSImage.sprite = OFFMPIC;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SwitchSFX()
    {
        if (GameConstants.GetContant("SFX") == 0)
        {

            GameConstants.SetContant("SFX", 1);
            AudioManager.instance.OffSound();
            SFXImage.sprite = OFFPIC;
        }
        else
        {
            GameConstants.SetContant("SFX", 0);

            AudioManager.instance.ONSound();


            SFXImage.sprite = ONPIC;
        }

        AudioManager.instance.Play("ButtonClick");
    }

    public void SwitchBGS()
    {
        if (GameConstants.GetContant("BGS") == 0)
        {
            GameConstants.SetContant("BGS", 1);
            AudioManager.instance.OffBGSound();
            BGSImage.sprite = OFFMPIC;
        }
        else
        {
            if (AudioManager.instance.CheckPlay("MenuBG"))
            {
                AudioManager.instance.Stop("GamePlayBG");
                AudioManager.instance.Play("MenuBG");
            }
            else if (AudioManager.instance.CheckPlay("GamePlayBG"))
            {
                AudioManager.instance.Stop("MenuBG");
                AudioManager.instance.Play("GamePlayBG");
            }

            GameConstants.SetContant("BGS", 0);
            AudioManager.instance.ONBGSound();
            BGSImage.sprite = ONMPIC;
        }
        AudioManager.instance.Play("ButtonClick");
    }

    public void Back()
    {
       // Instantiate(ToyScreensManager.Instance.MainMenu);

        AudioManager.instance.Play("ButtonClick");
        Destroy(this.gameObject);
    }
}
