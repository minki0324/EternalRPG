using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class WayPoint : MonoBehaviour
{
    [SerializeField] private GameObject ignoreRayCanvas;
    [SerializeField] private GameObject transitionObj;
    [SerializeField] private Image transitionImage;
    [SerializeField] private Transform targetWay;
    [SerializeField] private Vector2 offSet;

    private SortingGroup group = null;
    private Transform player = null;
    private PlayerMove playerMove;
    private float delay = 0.2f;
    [SerializeField] private bool isUP = false;

    private Coroutine moveCoroutine;

    [SerializeField] private string toLayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(moveCoroutine != null)
            { // �̵� �ڷ�ƾ�� �������̸� �ߺ� �߻��̹Ƿ� �޼ҵ� ����
                return;
            }
            if(group == null)
            { // ã�� �ʾ��� ���� ã���ֱ�
                group = collision.gameObject.GetComponentInChildren<SortingGroup>();
            }
            if(playerMove == null)
            { // ã�� �ʾ��� ���� ã���ֱ�
                playerMove = collision.GetComponent<PlayerMove>();
            }
            playerMove.movePoint.gameObject.SetActive(false);
            
            if (isUP)
            {
                // ���̾� ����
                collision.gameObject.layer = LayerMask.NameToLayer(toLayer);
                group.sortingLayerName = toLayer;
            }

            GameManager.Instance.LayerName = toLayer;
            ignoreRayCanvas.SetActive(true);
            player = collision.GetComponent<Transform>();
            playerMove.isWayMove = true;
            // �̵� �ڷ�ƾ ����
            moveCoroutine = StartCoroutine(WayMove(collision));
        }
    }

    private IEnumerator WayMove(Collider2D collision)
    {
        yield return delay;

        Vector3 originScale = player.localScale;
        float elapsedTime = 0f;
        float duration = 0.45f; // �� �����ϸ� �ִϸ��̼��� ���� �ð�

        while (elapsedTime < duration)
        {
            player.localScale = Vector3.Lerp(originScale, Vector3.zero, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transitionObj.SetActive(true);
        if(TransitionFade.instance.FadeCoroutine != null)
        {
            StopCoroutine(TransitionFade.instance.FadeCoroutine);
        }
        TransitionFade.instance.FadeCoroutine = StartCoroutine(TransitionFade.instance.fade(transitionImage, true));

        // Loading.isLoading�� false�� �� ������ ���
        while (TransitionFade.instance.isLoading)
        {
            yield return null;
        }

        // ���� ��������Ʈ�� �̵�
        player.position = (Vector2)targetWay.position + offSet;
        GameManager.Instance.LastPos = player.position;

        transitionObj.SetActive(false);

        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            player.localScale = Vector3.Lerp(Vector3.zero, originScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return delay;
        ignoreRayCanvas.SetActive(false);
        moveCoroutine = null;
        playerMove.isWayMove = false;

        if(!isUP)
        {
            // ���̾� ����
            collision.gameObject.layer = LayerMask.NameToLayer(toLayer);
            GameManager.Instance.LayerName = toLayer;
            group.sortingLayerName = toLayer;
        }
    }
}
