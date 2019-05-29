using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPlane : MonoBehaviour
{
    private Button btn_Start;
    private Button btn_Shop;
    private Button btn_Rank;
    private Button btn_Mute;
    private Button btn_Reset;

    private ManagerVars vars;

    private void Awake()
    {
        vars = ManagerVars.GetManagerVars();
        EventCenter.AddListener<int>(EventDefine.ChangeSkin, ChangeSkin);
        EventCenter.AddListener(EventDefine.ShowMianPlane, Show);
        Init();
    }
    void Start()
    {
        if (GameData.isGameAgain)
        {
            EventCenter.Broadcast(EventDefine.ShowGamePlane);
            gameObject.SetActive(false);
        }
        Sound();
        ChangeSkin(GameManager.Instance.GetCurrenSelectSkin());
    }
    void OnDestroy()
    {
        EventCenter.RemoveListener<int>(EventDefine.ChangeSkin, ChangeSkin);
        EventCenter.RemoveListener(EventDefine.ShowMianPlane, Show);
        
    }

    private void ChangeSkin(int index)
    {
        btn_Shop.transform.GetChild(0).GetComponent<Image>().sprite = vars.skinSprites[index];
    }
    private void Init()
    {
        btn_Start = transform.Find("btn_Start").GetComponent<Button>();
        btn_Start.onClick.AddListener(OnStartClick);
        btn_Shop = transform.Find("btns/btn_Shop").GetComponent<Button>();
        btn_Shop.onClick.AddListener(OnShopClick);
        btn_Rank = transform.Find("btns/btn_Rank").GetComponent<Button>();
        btn_Rank.onClick.AddListener(OnRankClick);
        btn_Mute = transform.Find("btns/btn_Mute").GetComponent<Button>();
        btn_Mute.onClick.AddListener(OnMuteClick);
        btn_Reset = transform.Find("btns/btn_Reset").GetComponent<Button>();
        btn_Reset.onClick.AddListener(OnResetButtonClick);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    

    private void OnStartClick()
    {
        EventCenter.Broadcast(EventDefine.PlayButtonAudio);
        GameManager.Instance.isGameStart = true;
        EventCenter.Broadcast(EventDefine.ShowGamePlane);
        gameObject.SetActive(false);
        
    }

    private void OnShopClick()
    {
        EventCenter.Broadcast(EventDefine.PlayButtonAudio);
        EventCenter.Broadcast(EventDefine.ShowShopPlane);
        gameObject.SetActive(false);
    }

    private void OnRankClick()
    {
        EventCenter.Broadcast(EventDefine.PlayButtonAudio);
        EventCenter.Broadcast(EventDefine.ShowRankPlane);
    }

    private void OnMuteClick()
    {
        EventCenter.Broadcast(EventDefine.PlayButtonAudio);
        GameManager.Instance.SetMute(!GameManager.Instance.GetIsMute());

        Sound();
    }

    private void Sound()
    {
        if (GameManager.Instance.GetIsMute())
        {
            btn_Mute.transform.GetChild(0).GetComponent<Image>().sprite = vars.muteOn;
        }
        else
        {
            btn_Mute.transform.GetChild(0).GetComponent<Image>().sprite = vars.muteClose;
        }
        EventCenter.Broadcast(EventDefine.isMuteOn, GameManager.Instance.GetIsMute());
    }

    private void OnResetButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayButtonAudio);
        EventCenter.Broadcast(EventDefine.ShowResetPlane);
    }
}
