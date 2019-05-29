using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class ResetPlane : MonoBehaviour
{
    private Image bg;
    private GameObject plane;
    private Button btn_Yes;
    private Button btn_No;

    void Awake()
    {
        EventCenter.AddListener(EventDefine.ShowResetPlane, Show);
        bg = transform.GetComponent<Image>();
        plane = transform.Find("Plane").gameObject;
        btn_No = transform.Find("Plane/btn_No").GetComponent<Button>();
        btn_Yes = transform.Find("Plane/btn_Yes").GetComponent<Button>();

        btn_Yes.onClick.AddListener(OnYesButtonClick);
        btn_No.onClick.AddListener(OnNoButtonClick);

        bg.color = new Color(bg.color.r, bg.color.b, bg.color.g, 0);
        plane.transform.localScale = Vector3.zero;
        gameObject.SetActive(false);
    }
    void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowResetPlane, Show);
    }
    private void Show()
    {
        gameObject.SetActive(true);
        bg.DOColor(new Color(bg.color.r, bg.color.b, bg.color.g, 0.3f), 0.3f);
        plane.transform.DOScale(Vector3.one, 0.3f);
    }

    private void OnNoButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayButtonAudio);
        bg.DOColor(new Color(bg.color.r, bg.color.b, bg.color.g, 0.3f), 0);
        plane.transform.DOScale(Vector3.zero, 0.3f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
    private void OnYesButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayButtonAudio);
        GameManager.Instance.ResetData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
