using UnityEngine;
using TMPro;

public class PlayerControl : MonoBehaviour
{
    [Header("References")]
    public GameObject GameManagerGO;
    public GameObject PlayerBulletGo;
    public GameObject bulletPosition1;
    public GameObject bulletPosition2;
    public GameObject ExplosionGO;
    public TextMeshProUGUI LiveUiText;

    [Header("Gameplay Settings")]
    public float speed = 5f;
    const int Maxlives = 3;
    int lives;

    [Header("Audio")]
    public AudioClip shootClip;       // âm thanh bắn
    public AudioClip explosionClip;   // âm thanh nổ
    public AudioSource engineAudio;   // âm thanh động cơ (loop)
    private AudioSource audioSource;  // để play OneShot (shoot, explosion)

    // lưu nửa chiều rộng/chiều cao của sprite
    float halfWidth;
    float halfHeight;

    void Start()
    {
        // Lấy sprite renderer để tính nửa kích thước player
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        halfWidth = sr.bounds.extents.x;
        halfHeight = sr.bounds.extents.y;

        // Gắn AudioSource để phát âm thanh bắn/nổ
        audioSource = GetComponent<AudioSource>();
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

            // Play sound bắn
            if (shootClip != null)
                audioSource.PlayOneShot(shootClip);
        }

        float x = Input.GetAxisRaw("Horizontal"); // -1(left),0(no input),1(right)
        float y = Input.GetAxisRaw("Vertical");   // -1(down),0(no input),1(up)
        Vector2 direction = new Vector2(x, y).normalized;

        // gọi function để di chuyển
        Move(direction);

        // Âm thanh động cơ: khi có input thì bật, không thì tắt
        if (direction.magnitude > 0.1f)
        {
            if (!engineAudio.isPlaying)
                engineAudio.Play();
        }
        else
        {
            if (engineAudio.isPlaying)
                engineAudio.Stop();
        }
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

                gameObject.SetActive(false);
            }
        }
    }

    void PlayExplosion()
    {
        GameObject explo = Instantiate(ExplosionGO);
        explo.transform.position = transform.position;

        // Phát âm thanh nổ tại vị trí hiện tại của player
        if (explosionClip != null)
            AudioSource.PlayClipAtPoint(explosionClip, transform.position);
    }
}
