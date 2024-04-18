using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.EventSystems;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private ActiveCanvas activeCanvas;
    [SerializeField] private Battle battle;
    public Transform movePoint;
    [SerializeField] private Rigidbody2D rigidbody2D_;
    [SerializeField] private SortingGroup group;
    public Collider2D boundary;
    public Animator playerAnimator;
    public bool isFight;
    public bool isWayMove = false;
    private Vector2 targetPos = Vector2.zero;
    public Coroutine moveCoroutine;

    private void Start()
    {
        gameObject.transform.position = GameManager.Instance.StartPos;
        group.sortingLayerName = GameManager.Instance.LayerName;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Monster")
        {
            Monster mon = collision.GetComponent<Monster>();
            if(!mon.isDead)
            { // 살아있을 때만 전투
                isFight = true;
                movePoint.gameObject.SetActive(false);
                battle.mon = mon;
            }
        }
    }

    void Update()
    {
        if(isWayMove || activeCanvas.gameObject.activeSelf)
        { // 웨이포인트를 찍었다면 // 액티브 캔버스가 켜져있다면 이동 멈춤
            if(moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            movePoint.gameObject.SetActive(false);

            playerAnimator.SetBool("Idle", true);
            playerAnimator.SetBool("Run", false);
            moveCoroutine = null;
            return;
        }
        if (isFight)
        { // 싸움이 시작됐다면 스탑
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            movePoint.gameObject.SetActive(false);

            // 애니메이션 Idle상태로 멈추기
            playerAnimator.SetBool("Idle", true);
            playerAnimator.SetBool("Run", false);
            moveCoroutine = null;

            // 액티브 캔버스 키고 결투 패널 켜주기
            activeCanvas.gameObject.SetActive(true);
            activeCanvas.versusPanel.SetActive(true);
            return;
        }

        if(Input.GetMouseButton(0))
        {
            if (!IsTouchingUI())
            {
                targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                if (moveCoroutine != null)
                {
                    StopCoroutine(moveCoroutine);
                }
                moveCoroutine = StartCoroutine(Move());
            }
        }
    }

    private IEnumerator Move()
    {
        // movePoint 위치 조정
        movePoint.gameObject.SetActive(true);

        // 애니메이션 변경
        playerAnimator.SetBool("Idle", false);
        playerAnimator.SetBool("Run", true);
        // 이동 방향에 따라 플레이어를 뒤집음
        if (transform.position.x > targetPos.x)
        {
            // 목표 위치가 플레이어의 오른쪽에 있을 때
            transform.localScale = new Vector3(1, 1, 1); // 원래 방향으로 회전
        }
        else
        {
            // 목표 위치가 플레이어의 왼쪽에 있을 때
            transform.localScale = new Vector3(-1, 1, 1); // 반대로 회전 (x축 플립)
        }


        while (Vector2.Distance(transform.position, targetPos) > 0.1f)
        {
            Vector3 forPos = (Vector3)targetPos - transform.position;

            // 현재 위치와 목표 위치를 향해 이동
            Vector3 newPosition = transform.position + (forPos.normalized * Time.deltaTime * GameManager.Instance.MoveSpeed * 0.1f);

            // 바운더리 영역 내에서만 이동하도록 제한
            if (boundary.bounds.Contains(newPosition))
            {
                rigidbody2D_.MovePosition(newPosition);
            }
            movePoint.position = targetPos;
            GameManager.Instance.LastPos = transform.position;

            yield return null;
        }

        movePoint.gameObject.SetActive(false);
        // 이동이 완료되면 애니메이션을 Idle로 전환
        playerAnimator.SetBool("Idle", true);
        playerAnimator.SetBool("Run", false);
        moveCoroutine = null;
    }

    private bool IsTouchingUI()
    {//터치한 위치가 UI위인지 체크하는 메서드
        if (Input.touchCount > 0 && Application.platform == RuntimePlatform.Android)
        { // 안드로이드 터치
            foreach (Touch touch in Input.touches)
            {
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    return true;
                }
            }
        }
        else
        {
            // 마우스 입력 처리
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return true;
            }
        }
        return false;
    }
}
