using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class FitBackgroundQuad : MonoBehaviour
{
    void Start()
    {
        // Lấy kích thước camera
        float camHeight = Camera.main.orthographicSize * 2f;
        float camWidth = camHeight * Camera.main.aspect;

        // Lấy kích thước gốc của texture
        Renderer r = GetComponent<Renderer>();
        float texWidth = r.sharedMaterial.mainTexture.width;
        float texHeight = r.sharedMaterial.mainTexture.height;

        // Tính tỉ lệ texture (theo pixel)
        float texAspect = texWidth / texHeight;
        float camAspect = camWidth / camHeight;

        // Scale Quad cho khớp camera
        if (camAspect >= texAspect)
        {
            // Camera rộng hơn → scale theo chiều ngang
            float scale = camWidth / texWidth;
            transform.localScale = new Vector3(camWidth, camWidth / texAspect, 1);
        }
        else
        {
            // Camera cao hơn → scale theo chiều dọc
            transform.localScale = new Vector3(camHeight * texAspect, camHeight, 1);
        }
    }
}
