using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    private ManagerVars vars;
    private bool isMoveLeft = false;
    private bool isJumping = false;
    private bool isMove = false;

    private Vector3 nextPlatformLeft = new Vector3(-0.554f,-2.4f+ 0.645f,0), nextPlatformRight = new Vector3(0.554f, -2.4f + 0.645f, 0);

    public GameObject rayDown,rayLfet,rayRight;
    private Rigidbody2D m_rigidbody;
    private SpriteRenderer m_renderer;
    private AudioSource m_AudioSource;

    public LayerMask PlatformLayer,ObstacleLayer;

    private void Awake()
    {
        vars = ManagerVars.GetManagerVars();
        EventCenter.AddListener<int>(EventDefine.ChangeSkin, ChangeSkin);
        EventCenter.AddListener<bool>(EventDefine.isMuteOn, IsMuteOn);
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_renderer = GetComponent<SpriteRenderer>();
        m_AudioSource = GetComponent<AudioSource>();
    }
    void Start()
    {
        ChangeSkin(GameManager.Instance.GetCurrenSelectSkin());
    }

    void OnDestroy()
    {
        EventCenter.RemoveListener<int>(EventDefine.ChangeSkin, ChangeSkin);
        EventCenter.RemoveListener<bool>(EventDefine.isMuteOn, IsMuteOn);
    }
    private void IsMuteOn(bool value)
    {
        m_AudioSource.mute = !value;
    }
    private void ChangeSkin(int index)
    {
        m_renderer.sprite = vars.characterSkinSprites[index];
    }

    private bool IsPointerOverGameObject(Vector2 mousePos)
    {
        //创建一个点击事件
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = mousePos;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        //向点击位置发射一条射线，检测是否点击的UI
        EventSystem.current.RaycastAll(eventData, raycastResults);
        return raycastResults.Count > 0;
    }

    private void Update()
    {
        //if (GameManager.Instance.isGameStart == false || GameManager.Instance.isGameOver == true
        //    || GameManager.Instance.isPause)
        //    return;

        //if (Application.platform == RuntimePlatform.Android || Application.platform==RuntimePlatform.IPhonePlayer)
        //{
        //    if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        //        return;
        //}
        //else
        //{
        //    if (EventSystem.current.IsPointerOverGameObject())
        //        return;
        //}
        if (IsPointerOverGameObject(Input.mousePosition)) return;
        

        if (Input.GetMouseButtonDown(0) && isJumping == false ) 
        {
            if (isMove == false)
            {
                EventCenter.Broadcast(EventDefine.PlayerMover);
                isMove = true;
            }
            m_AudioSource.PlayOneShot(vars.jumpClip);
            EventCenter.Broadcast(EventDefine.DecidePath);
            isJumping = true;
            Vector3 mousePos = Input.mousePosition;
            if (mousePos.x > Screen.width / 2)//点击屏幕右边，向右跳
            {
                isMoveLeft = false;
            }
            else if (mousePos.x <= Screen.width / 2) //点击屏幕左边，向左跳
            {
                isMoveLeft = true;
            }
            Jump();
        }


        //游戏结束
        if (m_rigidbody.velocity.y < 0 && IsRayPlatform() == false && GameManager.Instance.isGameOver == false)
        {
            m_AudioSource.PlayOneShot(vars.fallClip);
            m_renderer.sortingLayerName = "Default";
            GetComponent<BoxCollider2D>().enabled = false;
            GameManager.Instance.isGameOver = true;
            StartCoroutine(DelayGameOverPlaneShow());
            
        }

        if (isJumping&& isRayObstacle() && GameManager.Instance.isGameOver == false)
        {
            m_AudioSource.PlayOneShot(vars.fallClip);
            GameObject deathEffect = GamePool.Instance.GetDeathEffect();
            deathEffect.SetActive(true);
            deathEffect.transform.position = transform.position;
            GameManager.Instance.isGameOver = true;
            m_renderer.enabled = false;
            StartCoroutine(DelayGameOverPlaneShow());
        }

        if (transform.position.y - Camera.main.transform.position.y < -6f && GameManager.Instance.isGameOver == false) 
        {
            m_AudioSource.PlayOneShot(vars.hitClip);
            GameManager.Instance.isGameOver = true;
            StartCoroutine(DelayGameOverPlaneShow());
        }
    }

    IEnumerator DelayGameOverPlaneShow()
    {
        yield return new WaitForSeconds(0.8f);

        EventCenter.Broadcast(EventDefine.ShowGameOver);
    }

    private GameObject lastHit = null;
    /// <summary>
    /// 是否检测到平台
    /// </summary>
    /// <returns></returns>
    private bool IsRayPlatform()
    {
        RaycastHit2D hit = Physics2D.Raycast(rayDown.transform.position, Vector2.down, 1f, PlatformLayer);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Platform")
            {
                if (lastHit != hit.collider.gameObject)
                {
                    if (lastHit == null)
                    {
                        lastHit = hit.collider.gameObject;
                        return true;
                    }
                    EventCenter.Broadcast(EventDefine.AddScore);
                    lastHit = hit.collider.gameObject;
                }
                
                return true;
            }
        }
        return false;
    }

    private bool isRayObstacle()
    {
        RaycastHit2D hitLeft = Physics2D.Raycast(rayLfet.transform.position, Vector2.left, 0.15f, ObstacleLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(rayRight.transform.position, Vector2.right, 0.15f, ObstacleLayer);

        if (hitLeft.collider != null)
        {
            if (hitLeft.collider.tag == "Obstacle")
            {
                return true;
            }
        }

        if (hitRight.collider != null)
        {
            if (hitRight.collider.tag == "Obstacle")
            {
                return true;
            }
        }

        return false;
    }

    private void Jump()
    {
        if (isJumping)
        {
            if (isMoveLeft)
            {
                transform.localScale = new Vector3(-1, 1, 1);

                //transform.DOMove(new Vector3(nextPlatformLeft.x, nextPlatformLeft.y + 0.8f), 0.2f);
                transform.DOLocalMoveX(nextPlatformLeft.x, 0.2f);
                transform.DOLocalMoveY(nextPlatformLeft.y + 0.8f, 0.15f);
            }
            else
            {
                //transform.DOMove(new Vector3(nextPlatformRight.x, nextPlatformRight.y + 0.8f), 0.2f);
                transform.localScale = new Vector3(1, 1, 1);
                transform.DOLocalMoveX(nextPlatformRight.x, 0.2f);
                transform.DOLocalMoveY(nextPlatformRight.y + 0.8f, 0.15f);

            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.tag == "Platform")
        {
            isJumping = false;

            Vector3 currentPlatformPos = collision.transform.position;
            nextPlatformLeft = new Vector3(currentPlatformPos.x -
                vars.nextXpos, currentPlatformPos.y + vars.nextYpos, 0);
            nextPlatformRight = new Vector3(currentPlatformPos.x +
                vars.nextXpos, currentPlatformPos.y + vars.nextYpos, 0);

           // Debug.Log(nextPlatformLeft + " " + nextPlatformRight+" "+currentPlatformPos);
        }
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Diamond")
        {
            m_AudioSource.PlayOneShot(vars.diamondClip);
            EventCenter.Broadcast(EventDefine.AddDiamond);
            collision.gameObject.SetActive(false);
        }
    }
}
