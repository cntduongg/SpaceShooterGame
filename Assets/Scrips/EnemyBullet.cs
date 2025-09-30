using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Vector2 direction;

    public void SetDirection(Vector2 dir)
    {
        Debug.Log("🎯 EnemyBullet.SetDirection được gọi!");
        direction = dir.normalized;
    }

    void Update()
    {
        // Di chuyển đạn
        if (direction != Vector2.zero)
        {
            transform.Translate(direction * 5f * Time.deltaTime);
        }

        // Hủy khi ra khỏi màn hình
        if (Mathf.Abs(transform.position.x) > 10f || Mathf.Abs(transform.position.y) > 8f)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.tag == "PlayerShipTag"))
        {
            Destroy
                (gameObject);
        }
    }
}