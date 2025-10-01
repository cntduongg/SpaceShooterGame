using UnityEngine;

[ExecuteAlways] // Để chạy cả trong Editor lẫn build
public class ForceAspect : MonoBehaviour
{
    [Tooltip("Tỉ lệ mong muốn: width / height")]
    public float targetAspect = 14f / 16f;

    private Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            Debug.LogError("ForceAspect: Camera không tìm thấy!");
        }
    }

    void Update()
    {
        if (cam == null) return;

        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        if (scaleHeight < 1.0f)
        {
            Rect rect = cam.rect;
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
            cam.rect = rect;
        }
        else
        {
            float scaleWidth = 1.0f / scaleHeight;
            Rect rect = cam.rect;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
            cam.rect = rect;
        }
    }
}
