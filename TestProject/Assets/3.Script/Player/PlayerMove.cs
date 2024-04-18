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
            { // ������� ���� ����
                isFight = true;
                movePoint.gameObject.SetActive(false);
                battle.mon = mon;
            }
        }
    }

    void Update()
    {
        if(isWayMove || activeCanvas.gameObject.activeSelf)
        { // ��������Ʈ�� ����ٸ� // ��Ƽ�� ĵ������ �����ִٸ� �̵� ����
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
        { // �ο��� ���۵ƴٸ� ��ž
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            movePoint.gameObject.SetActive(false);

            // �ִϸ��̼� Idle���·� ���߱�
            playerAnimator.SetBool("Idle", true);
            playerAnimator.SetBool("Run", false);
            moveCoroutine = null;

            // ��Ƽ�� ĵ���� Ű�� ���� �г� ���ֱ�
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
        // movePoint ��ġ ����
        movePoint.gameObject.SetActive(true);

        // �ִϸ��̼� ����
        playerAnimator.SetBool("Idle", false);
        playerAnimator.SetBool("Run", true);
        // �̵� ���⿡ ���� �÷��̾ ������
        if (transform.position.x > targetPos.x)
        {
            // ��ǥ ��ġ�� �÷��̾��� �����ʿ� ���� ��
            transform.localScale = new Vector3(1, 1, 1); // ���� �������� ȸ��
        }
        else
        {
            // ��ǥ ��ġ�� �÷��̾��� ���ʿ� ���� ��
            transform.localScale = new Vector3(-1, 1, 1); // �ݴ�� ȸ�� (x�� �ø�)
        }


        while (Vector2.Distance(transform.position, targetPos) > 0.1f)
        {
            Vector3 forPos = (Vector3)targetPos - transform.position;

            // ���� ��ġ�� ��ǥ ��ġ�� ���� �̵�
            Vector3 newPosition = transform.position + (forPos.normalized * Time.deltaTime * GameManager.Instance.MoveSpeed * 0.1f);

            // �ٿ���� ���� �������� �̵��ϵ��� ����
            if (boundary.bounds.Contains(newPosition))
            {
                rigidbody2D_.MovePosition(newPosition);
            }
            movePoint.position = targetPos;
            GameManager.Instance.LastPos = transform.position;

            yield return null;
        }

        movePoint.gameObject.SetActive(false);
        // �̵��� �Ϸ�Ǹ� �ִϸ��̼��� Idle�� ��ȯ
        playerAnimator.SetBool("Idle", true);
        playerAnimator.SetBool("Run", false);
        moveCoroutine = null;
    }

    private bool IsTouchingUI()
    {//��ġ�� ��ġ�� UI������ üũ�ϴ� �޼���
        if (Input.touchCount > 0 && Application.platform == RuntimePlatform.Android)
        { // �ȵ���̵� ��ġ
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
            // ���콺 �Է� ó��
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return true;
            }
        }
        return false;
    }
}
