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
        if (isDead) return; // Dừng di chuyển nếu đã chết

        Vector2 position = transform.position;
        position = new Vector2(position.x, position.y - speed * Time.deltaTime);
        transform.position = position;

        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        if (transform.position.y < min.y)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return; // Tránh xử lý nhiều lần

        if ((collision.tag == "PlayerShip") || (collision.tag == "PlayerBullletTag"))
        {
            isDead = true;
            Debug.Log("💥 Enemy bị bắn trúng!");

            // Tắt mọi thành phần hiển thị và tương tác
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;

            // Dừng mọi chuyển động
            if (GetComponent<Rigidbody2D>() != null)
                GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;

            PlayExplo();
            Destroy(gameObject, 1f); // Hủy sau 1 giây
        }
    }

    void PlayExplo()
    {
        Debug.Log("🔍 Bắt đầu PlayExplo");

        if (ExplosionGO == null)
        {
            Debug.LogError("❌ ExplosionGO là NULL!");
            return;
        }

        GameObject exp = Instantiate(ExplosionGO);
        Debug.Log($"✅ Đã Instantiate explosion: {exp.name}");

        exp.transform.position = transform.position;
        Debug.Log($"📍 Vị trí explosion: {exp.transform.position}");

        // Kiểm tra component
        SpriteRenderer sr = exp.GetComponent<SpriteRenderer>();
        if (sr != null) Debug.Log("✅ Explosion có SpriteRenderer");
        else Debug.Log("❌ Explosion không có SpriteRenderer");
    }
}