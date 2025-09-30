using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speed = 8f;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 position = transform.position;

        position = new Vector2(position.x, position.y + speed*Time.deltaTime);
        transform.position = position;
        // top right point of the screen
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        // if bullet went above the screen
        if (transform.position.y > max.y)
        {
            Destroy(gameObject);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyShipTag")
        {
            // HỦY ENEMY NGAY LẬP TỨC
            Destroy(collision.gameObject); // Hủy enemy
            Destroy(gameObject);           // Hủy đạn
            Debug.Log("💥 Đạn trúng enemy!");
        }
    }
}
