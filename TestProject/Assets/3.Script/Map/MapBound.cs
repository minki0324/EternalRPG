using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBound : MonoBehaviour
{
    public string MapName;
    [SerializeField] private CamareMove cameraMove;
    public Collider2D Boundary;
    [SerializeField] private ScreenTransitionSimpleSoft loading;
    [SerializeField] private HUDCanvas canvas;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            StartCoroutine(WaitLoading(collision));
        }
    }

    private IEnumerator WaitLoading(Collider2D collision)
    {
        while(loading.isLoading)
        {
            yield return null;
        }
        collision.GetComponent<PlayerMove>().boundary = Boundary;
        cameraMove.Boundary = Boundary;
        canvas.mapText.text = MapName;
        PrintLog.Instance.StaticLog(MapName);
    }
}
