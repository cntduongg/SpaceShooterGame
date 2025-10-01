using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    public GameObject ExplosionGO;
    float speed;
    private bool isDead = false;

    void Start()
    {
        speed = 2f;
    }

    void Update()
    {
        if (isDead) return; // Dừng nếu enemy đã bị bắn chết

        // Enemy di chuyển xuống
        Vector2 position = transform.position;
        position = new Vector2(position.x, position.y - speed * Time.deltaTime);
        transform.position = position;

        // Nếu rơi khỏi màn hình thì tự hủy
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        if (transform.position.y < min.y)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return; // tránh xử lý nhiều lần

        // Enemy bị đạn Player bắn trúng
        if (collision.CompareTag("PlayerBulletTag"))
        {
            isDead = true;

            // Hủy đạn Player
            Destroy(collision.gameObject);

            // Vô hiệu hóa enemy
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;

            // Dừng Rigidbody nếu có
            if (GetComponent<Rigidbody2D>() != null)
                GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;

            // Hiệu ứng nổ
            PlayExplo();

            // ➕ Cộng điểm
            if (GameScore.Instance != null)
                GameScore.Instance.AddScore(100);

            // Hủy enemy sau 1 giây (chờ explosion)
            Destroy(gameObject, 1f);
        }
    }

    void PlayExplo()
    {
        if (ExplosionGO == null)
        {
            Debug.LogError("❌ ExplosionGO chưa được gán!");
            return;
        }

        GameObject exp = Instantiate(ExplosionGO);
        exp.transform.position = transform.position;
    }
}
