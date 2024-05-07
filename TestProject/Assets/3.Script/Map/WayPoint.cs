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
            { // 이동 코루틴이 실행중이면 중복 발생이므로 메소드 나감
                return;
            }
            if(group == null)
            { // 찾지 않았을 때만 찾아주기
                group = collision.gameObject.GetComponentInChildren<SortingGroup>();
            }
            if(playerMove == null)
            { // 찾지 않았을 때만 찾아주기
                playerMove = collision.GetComponent<PlayerMove>();
            }
            playerMove.movePoint.gameObject.SetActive(false);
            
            if (isUP)
            {
                // 레이어 변경
                collision.gameObject.layer = LayerMask.NameToLayer(toLayer);
                group.sortingLayerName = toLayer;
            }

            GameManager.Instance.LayerName = toLayer;
            ignoreRayCanvas.SetActive(true);
            player = collision.GetComponent<Transform>();
            playerMove.isWayMove = true;
            // 이동 코루틴 실행
            moveCoroutine = StartCoroutine(WayMove(collision));
        }
    }

    private IEnumerator WayMove(Collider2D collision)
    {
        yield return delay;

        Vector3 originScale = player.localScale;
        float elapsedTime = 0f;
        float duration = 0.45f; // 각 스케일링 애니메이션의 지속 시간

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

        // Loading.isLoading이 false가 될 때까지 대기
        while (TransitionFade.instance.isLoading)
        {
            yield return null;
        }

        // 다음 웨이포인트로 이동
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
            // 레이어 변경
            collision.gameObject.layer = LayerMask.NameToLayer(toLayer);
            GameManager.Instance.LayerName = toLayer;
            group.sortingLayerName = toLayer;
        }
    }
}
