using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    private void Awake()
    {
        // 현재 GameObject에 부착된 Camera 컴포넌트를 가져오는 코드
        Camera cam = GetComponent<Camera>();

        // 현재 카메라의 뷰포트 영역을 가져오는 코드
        Rect viewportRect = cam.rect;

        // 원하는 가로 세로 비율을 계산하는 코드
        float screenAspectRatio = (float)Screen.width / Screen.height;
        float targetAspectRatio = 18.5f / 9f; // 원하는 고정 비율 설정 (예: 16:9)

        // 화면 가로 세로 비율에 따라 뷰포트 영역을 조정하는 코드
        if (screenAspectRatio < targetAspectRatio)
        {
            // 화면이 더 '높다'면 (세로가 더 길다면) 세로를 조절하는 코드
            viewportRect.height = screenAspectRatio / targetAspectRatio;
            viewportRect.y = (1f - viewportRect.height) / 2f;
        }
        else
        {
            // 화면이 더 '넓다'면 (가로가 더 길다면) 가로를 조절하는 코드.
            viewportRect.width = targetAspectRatio / screenAspectRatio;
            viewportRect.x = (1f - viewportRect.width) / 2f;
        }

        // 조정된 뷰포트 영역을 카메라에 설정하는 코드
        cam.rect = viewportRect;
    }

    //void Start()
    //{
    //    float targetAspect = 18.5f / 9f; // 원하는 비율
    //    float windowAspect = (float)Screen.width / (float)Screen.height;
    //    float scaleHeight = windowAspect / targetAspect;

    //    Camera camera = Camera.main;

    //    if (scaleHeight < 1.0f)
    //    {
    //        Rect rect = camera.rect;

    //        rect.width = 1.0f;
    //        rect.height = scaleHeight;
    //        rect.x = 0;
    //        rect.y = (1.0f - scaleHeight) / 2.0f;

    //        camera.rect = rect;
    //    }
    //    else
    //    {
    //        float scaleWidth = 1.0f / scaleHeight;

    //        Rect rect = camera.rect;

    //        rect.width = scaleWidth;
    //        rect.height = 1.0f;
    //        rect.x = (1.0f - scaleWidth) / 2.0f;
    //        rect.y = 0;

    //        camera.rect = rect;
    //    }
    //}
}