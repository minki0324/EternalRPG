using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamareMove : MonoBehaviour
{
    [SerializeField] private Transform target;
    public Collider2D Boundary;

    private float cameraHalfWidth;
    private float cameraHalfHeight;

    private void Start()
    {
        // ī�޶��� �� �ʺ�� �� ���̸� ����մϴ�.
        Camera mainCamera = Camera.main;
        cameraHalfHeight = mainCamera.orthographicSize;
        cameraHalfWidth = cameraHalfHeight * mainCamera.aspect;
    }

    private void LateUpdate()
    {
        if (target != null && Boundary != null)
        {
            // ī�޶� �÷��̾ ���󰡵�, ������ ������ ����� �ʵ��� �մϴ�.
            float clampedX = Mathf.Clamp(target.position.x, Boundary.bounds.min.x + cameraHalfWidth, Boundary.bounds.max.x - cameraHalfWidth);
            float clampedY = Mathf.Clamp(target.position.y, Boundary.bounds.min.y + cameraHalfHeight, Boundary.bounds.max.y - cameraHalfHeight);
            transform.position = new Vector3(clampedX, clampedY, transform.position.z);
        }
    }
}
