using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    public GameObject EnemyBulletGO;

    void Start()
    {
        Debug.Log("🔫 EnemyGun Start - Bắt đầu khởi tạo");
        Invoke("FireEnemyBullet", 1f);
    }

    void FireEnemyBullet()
    {
        Debug.Log("🎯 FireEnemyBullet ĐƯỢC GỌI!");

        // Tìm player
        GameObject player = GameObject.Find("PlayGO");
        if (player == null)
        {
            Debug.LogError("❌ KHÔNG TÌM THẤY PlayerGO!");
            return;
        }

        // Kiểm tra prefab đạn
        if (EnemyBulletGO == null)
        {
            Debug.LogError("❌ EnemyBulletGO CHƯA ĐƯỢC GÁN!");
            return;
        }

        // Tạo đạn
        GameObject bullet = Instantiate(EnemyBulletGO);
        bullet.transform.position = transform.position;

        // Tính hướng bắn
        Vector2 direction = (player.transform.position - transform.position).normalized;
        Debug.Log($"📏 Hướng bắn: {direction}");

        // Set direction
        EnemyBullet bulletScript = bullet.GetComponent<EnemyBullet>();
        if (bulletScript != null)
        {
            bulletScript.SetDirection(direction);
            Debug.Log("✅ ĐẠN ĐƯỢC BẮN THÀNH CÔNG!");
        }
        else
        {
            Debug.LogError("❌ KHÔNG CÓ SCRIPT EnemyBullet TRÊN PREFAB ĐẠN!");
        }
    }
}