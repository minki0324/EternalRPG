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
        // 카메라의 반 너비와 반 높이를 계산합니다.
        Camera mainCamera = Camera.main;
        cameraHalfHeight = mainCamera.orthographicSize;
        cameraHalfWidth = cameraHalfHeight * mainCamera.aspect;
    }

    private void LateUpdate()
    {
        if (target != null && Boundary != null)
        {
            // 카메라가 플레이어를 따라가되, 설정한 영역을 벗어나지 않도록 합니다.
            float clampedX = Mathf.Clamp(target.position.x, Boundary.bounds.min.x + cameraHalfWidth, Boundary.bounds.max.x - cameraHalfWidth);
            float clampedY = Mathf.Clamp(target.position.y, Boundary.bounds.min.y + cameraHalfHeight, Boundary.bounds.max.y - cameraHalfHeight);
            transform.position = new Vector3(clampedX, clampedY, transform.position.z);
        }
    }
}
