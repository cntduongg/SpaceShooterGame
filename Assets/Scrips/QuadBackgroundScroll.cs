using UnityEngine;

public class QuadBackgroundScroll : MonoBehaviour
{
    public float scrollSpeed = 0.2f; // tốc độ cuộn
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        float offset = Time.time * scrollSpeed;
        rend.material.mainTextureOffset = new Vector2(0, offset);
    }
}
