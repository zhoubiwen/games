using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ShopPlane : MonoBehaviour
{
    private ManagerVars vars;

    private int selectIndex;

    private Transform parent;
    private Text txt_name;
    private Text txt_Price;
    private Text txt_diamond;
    private Text txt_tips;
    private Button btn_back;
    private Button btn_Select;
    private Button btn_Buy;

    void Awake()
    {
        EventCenter.AddListener(EventDefine.ShowShopPlane, Show);
        vars = ManagerVars.GetManagerVars();
        parent = transform.Find("ScrollRect/Parent");
        txt_name = transform.Find("txt_name").GetComponent<Text>();
        txt_Price = transform.Find("btn_Buy/txt_Price").GetComponent<Text>();
        txt_diamond = transform.Find("Diamond/txt_diamond").GetComponent<Text>();
        txt_tips = transform.Find("txt_tips").GetComponent<Text>();
        btn_back = transform.Find("btn_back").GetComponent<Button>();
        btn_Select = transform.Find("btn_Select").GetComponent<Button>();
        btn_Buy = transform.Find("btn_Buy").GetComponent<Button>();
        btn_back.onClick.AddListener(OnBackButtonClick);
        btn_Buy.onClick.AddListener(OnBuyButtonClick);
        btn_Select.onClick.AddListener(OnSelectedButtonClick);
    }
    void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowShopPlane, Show);
    }

    void Start()
    {
        Init();
        gameObject.SetActive(false);
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void OnBackButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayButtonAudio);
        EventCenter.Broadcast(EventDefine.ShowMianPlane);
        gameObject.SetActive(false);
    }

    private void OnBuyButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayButtonAudio);
        int price = int.Parse(txt_Price.text.ToString());
        if (GameManager.Instance.GetAllDiamond() < price)
        {
            StartCoroutine(ShowTips());
            return;
        }
        
        GameManager.Instance.UpdataDiamond(-price);
        GameManager.Instance.SetSkinUnlock(selectIndex);
        parent.GetChild(selectIndex).GetChild(0).GetComponent<Image>().color = Color.white;

    }

    private void OnSelectedButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayButtonAudio);
        EventCenter.Broadcast(EventDefine.ChangeSkin, selectIndex);
        GameManager.Instance.SetSkinSelectedIndex(selectIndex);
        EventCenter.Broadcast(EventDefine.ShowMianPlane);
        gameObject.SetActive(false);
    }

    IEnumerator ShowTips()
    {
        txt_tips.transform.localPosition = new Vector3(0, -150, 0);
        txt_tips.gameObject.SetActive(true);
        txt_tips.transform.DOLocalMoveY(-80, 0.4f);
        yield return new WaitForSeconds(1f);
        txt_tips.gameObject.SetActive(false);
    }

    private void Init()
    {
        parent.GetComponent<RectTransform>().sizeDelta = new Vector2((vars.skinSprites.Count + 2) * 160, 270);
        for (int i = 0; i < vars.skinSprites.Count; i++)
        {
            
            GameObject go = Instantiate(vars.SkinChooseItem, parent);
            //未解锁
            if (GameManager.Instance.GetSkinUnlock(i) == false)
            {
                go.GetComponentInChildren<Image>().color = Color.gray;
              
            }
            else
            {
                go.GetComponentInChildren<Image>().color = Color.white;
             
            }
            go.GetComponentInChildren<Image>().sprite = vars.skinSprites[i];
            go.transform.localPosition = new Vector3((i + 1) * 160, 0, 0);
        }
        parent.transform.localPosition = new Vector3(GameManager.Instance.GetCurrenSelectSkin() * -160, 0, 0);
    }

    void Update()
    {
        selectIndex = (int)Mathf.Round(parent.localPosition.x / -160.0f);
        if (Input.GetMouseButtonUp(0))
        {
            parent.transform.DOLocalMoveX(selectIndex * -160, 0.2f);
        }

        SetItemSize(selectIndex);
        RefrashName(selectIndex);
    }

    private void SetItemSize(int index)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            if (index == i)
            {
                parent.GetChild(i).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(160, 160);
            }
            else
            {
                parent.GetChild(i).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(80, 80);
            }
        }
    }

    private void RefrashName(int index)
    {
        txt_name.text = vars.skinName[index];
        txt_diamond.text = GameManager.Instance.GetAllDiamond().ToString();
        //未解锁
        if (GameManager.Instance.GetSkinUnlock(index) == false)
        {
            btn_Select.gameObject.SetActive(false);
            btn_Buy.gameObject.SetActive(true);
            txt_Price.text = vars.skinPrice[index].ToString();
        }
        else
        {
            btn_Select.gameObject.SetActive(true);
            btn_Buy.gameObject.SetActive(false);
        }
    }

}
