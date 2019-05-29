using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    public SpriteRenderer[] spriteRenderers;

    public GameObject obstacle;

    private float fallTime;
    private bool StartFallTimer;
    private Rigidbody2D rigidbody2D;

    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void Init(Sprite sprite,float fallTime,int obstacleDir)
    {
        rigidbody2D.bodyType = RigidbodyType2D.Static;
        this.fallTime = fallTime;
        StartFallTimer = true;
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].sprite = sprite;
        }
        if (obstacleDir == 0)//朝右边
        {
            if (obstacle != null)
            {
                obstacle.transform.localPosition = new Vector3(-obstacle.transform.localPosition.x, obstacle.transform.localPosition.y, 0);
            }
        }
       
    }

    void Update()
    {
        if (GameManager.Instance.isGameStart==false && GameManager.Instance.PlayerIsMove==false)
            return;
        if (StartFallTimer)
        {
            fallTime -= Time.deltaTime;
            if (fallTime <= 0)
            {
                StartFallTimer = false;
                if (rigidbody2D.bodyType != RigidbodyType2D.Dynamic)
                {
                    rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                    StartCoroutine(DealyHide());
                }
            }
        }

        if (transform.position.y - Camera.main.transform.position.y < -6f)
        {
            StartCoroutine(DealyHide());
        }
    }

    private IEnumerator DealyHide()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
