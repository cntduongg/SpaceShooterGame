using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    float speed;
    public GameObject ExplosionGO; // Thêm tham chiếu đến prefab nổ

    void Start()
    {
        speed = 8f;
    }

    void Update()
    {
        Vector2 position = transform.position;
        position = new Vector2(position.x, position.y + speed * Time.deltaTime);
        transform.position = position;

        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        if (transform.position.y > max.y)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyShipTag")
        {
            // ✅ Spawn hiệu ứng nổ tại vị trí enemy
            if (ExplosionGO != null)
            {
                Instantiate(ExplosionGO, collision.transform.position, Quaternion.identity);
            }

            Destroy(collision.gameObject); // Hủy enemy
            Destroy(gameObject);           // Hủy đạn
            Debug.Log("💥 Đạn trúng enemy -> explosion!");
        }
    }
}
