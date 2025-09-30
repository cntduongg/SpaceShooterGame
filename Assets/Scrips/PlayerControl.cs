using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class PlayerControl : MonoBehaviour
{
    public GameObject GameManagerGO;
    public GameObject PlayerBulletGo;
    public GameObject bulletPosition1;
    public GameObject bulletPosition2;
    public GameObject ExplosionGO;
    public TextMeshProUGUI LiveUiText;

    const int Maxlives = 3;
    int lives;

    public float speed;

    // lưu nửa chiều rộng/chiều cao của sprite
    float halfWidth;
    float halfHeight;

    void Start()
    {
        // Lấy sprite renderer để tính nửa kích thước player
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        halfWidth = sr.bounds.extents.x;
        halfHeight = sr.bounds.extents.y;
    }

    public void Init()
    {
        lives = Maxlives;
        LiveUiText.text = lives.ToString();
        gameObject.SetActive(true);
    }

    void Update()
    {
        // fire bullet when the spacebar is press
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject bullet01 = Instantiate(PlayerBulletGo);
            bullet01.transform.position = bulletPosition1.transform.position;

            GameObject bullet02 = Instantiate(PlayerBulletGo);
            bullet02.transform.position = bulletPosition2.transform.position;
        }

        float x = Input.GetAxisRaw("Horizontal"); // -1(left),0(no input),1(right)
        float y = Input.GetAxisRaw("Vertical");   // -1(down),0(no input),1(up)
        Vector2 direction = new Vector2(x, y).normalized;

        // gọi function để di chuyển
        Move(direction);
    }

    void Move(Vector2 direction)
    {
        // điểm dưới cùng bên trái (viewport 0,0)
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        // điểm trên cùng bên phải (viewport 1,1)
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        // Giới hạn lại theo kích thước sprite
        min.x += halfWidth;
        max.x -= halfWidth;

        min.y += halfHeight;
        max.y -= halfHeight;

        // Lấy vị trí hiện tại
        Vector2 pos = transform.position;

        // Tính toán vị trí mới
        pos += direction * speed * Time.deltaTime;

        // Clamp lại để không ra khỏi màn hình
        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);

        // Cập nhật vị trí
        transform.position = pos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.tag == "EnemyShipTag") || (collision.tag == "EnemyBulletTag"))
        {
            PlayExplosion();
            lives--;
            LiveUiText.text = lives.ToString();

            if (lives == 0)
            {
                GameManagerGO.GetComponent<GameManager>()
                             .SetGameManagerState(GameManager.GameManagerState.GameOver);

                // Destroy(gameObject);
                gameObject.SetActive(false);
            }
        }
    }

    void PlayExplosion()
    {
        GameObject explo = Instantiate(ExplosionGO);
        explo.transform.position = transform.position;
    }
}
