using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovePad : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [Header("½ºÆ½")]
    [SerializeField] private RectTransform rectPadFrame;
    [SerializeField] private RectTransform rectPadStick;
    private float frameRadius;
    public bool isTouch = false;
    public Vector2 targetPos = Vector2.zero;

    void Start()
    {
        frameRadius = rectPadFrame.rect.width * 0.5f;
    }

    void Update()
    {
        
    }

    
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 value = eventData.position - (Vector2)rectPadFrame.position;
        
        value = Vector2.ClampMagnitude(value, frameRadius);

        rectPadStick.localPosition = value;
        value = value.normalized;

        targetPos = new Vector2(value.x * GameManager.Instance.MoveSpeed * Time.deltaTime, value.y * GameManager.Instance.MoveSpeed * Time.deltaTime);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isTouch = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isTouch = false;
        rectPadStick.localPosition = Vector3.zero;
    }
}
