using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Teleport : MonoBehaviour
{
    public Location TeleportLocation;
    [SerializeField] private TMP_Dropdown teleportLocationDropdown;
    [SerializeField] private Image miniMapImage;
    [SerializeField] private Transform[] mapWayPoint;
    [SerializeField] private Vector3[] offSet;

    [SerializeField] private Transform player;
    [SerializeField] private PlayerMove playerMove;
    [SerializeField] private GameObject ignoreRayCanvas;
    [SerializeField] private GameObject transitionObj;
    [SerializeField] private Image transitionImage;
    private Coroutine moveCoroutine;

    private float delay = 0.2f;

    public void TeleportLocationType()
    {
        int selectDropDown = teleportLocationDropdown.value;
        switch (teleportLocationDropdown.options[selectDropDown].text)
        {
            case "깊은 숲":
                TeleportLocation = (Location)0;
                break;
            case "정령의 숲":
                TeleportLocation = (Location)1;
                break;
            case "붉은 기운의 숲":
                TeleportLocation = (Location)2;
                break;
            case "광산 초입부":
                TeleportLocation = (Location)3;
                break;
            case "죽은 나무 동굴":
                TeleportLocation = (Location)4;
                break;
            case "광산 나가는길":
                TeleportLocation = (Location)5;
                break;
        }
        miniMapImage.sprite = teleportLocationDropdown.options[selectDropDown].image;
    }

    public void TeleportButton()
    {
        ignoreRayCanvas.SetActive(true);
        playerMove.isWayMove = true;
        playerMove.movePoint.gameObject.SetActive(false);
        if(moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(WayMove());
    }

    private IEnumerator WayMove()
    {
        yield return delay;

        Vector3 originScale = player.localScale;
        float elapsedTime = 0f;
        float duration = 0.7f; // 각 스케일링 애니메이션의 지속 시간

        while (elapsedTime < duration)
        {
            player.localScale = Vector3.Lerp(originScale, Vector3.zero, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transitionObj.SetActive(true);
        if (TransitionFade.instance.FadeCoroutine != null)
        {
            StopCoroutine(TransitionFade.instance.FadeCoroutine);
        }
        TransitionFade.instance.FadeCoroutine = StartCoroutine(TransitionFade.instance.fade_out(transitionImage, true));

        // Loading.isLoading이 false가 될 때까지 대기
        while (TransitionFade.instance.isLoading)
        {
            yield return null;
        }

        // 다음 웨이포인트로 이동
        player.position = (Vector2)mapWayPoint[(int)TeleportLocation].position + (Vector2)offSet[(int)TeleportLocation];
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
    }
}
