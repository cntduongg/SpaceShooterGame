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
    const int Maxlives = 4;   // 👉 số mạng tối đa = 4
    int lives;

    [Header("Audio")]
    public AudioClip shootClip;
    public AudioClip explosionClip;
    public AudioSource engineAudio;

    float halfWidth;
    float halfHeight;

    private bool isImmortal = false; // 👉 chế độ bất tử

    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        halfWidth = sr.bounds.extents.x;
        halfHeight = sr.bounds.extents.y;

        if (engineAudio == null)
        {
            engineAudio = GetComponent<AudioSource>();
            if (engineAudio == null)
            {
                engineAudio = gameObject.AddComponent<AudioSource>();
                engineAudio.loop = true;
            }
        }
    }

    public void Init()
    {
        lives = Maxlives;
        UpdateLivesUI();           
        gameObject.SetActive(true);
        isImmortal = false; // reset về bình thường khi game bắt đầu lại
    }

    void Update()
    {
        // 👉 Toggle bất tử khi bấm L
        if (Input.GetKeyDown(KeyCode.L))
        {
            isImmortal = !isImmortal;
            Debug.Log("🛡️ Bất tử: " + isImmortal);
        }

        // Bắn đạn
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject bullet01 = Instantiate(PlayerBulletGo);
            bullet01.transform.position = bulletPosition1.transform.position;

            GameObject bullet02 = Instantiate(PlayerBulletGo);
            bullet02.transform.position = bulletPosition2.transform.position;

            if (shootClip != null)
                SoundManager.Instance.PlaySound(shootClip);
        }

        // Di chuyển
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector2 direction = new Vector2(x, y).normalized;

        Move(direction);

        if (engineAudio != null)
        {
            if (Mathf.Abs(y) > 0.1f)
            {
                if (!engineAudio.isPlaying) engineAudio.Play();
            }
            else
            {
                if (engineAudio.isPlaying) engineAudio.Stop();
            }
        }
    }

    void Move(Vector2 direction)
    {
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        min.x += halfWidth;
        max.x -= halfWidth;
        min.y += halfHeight;
        max.y -= halfHeight;

        Vector2 pos = transform.position;
        pos += direction * speed * Time.deltaTime;
        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);

        transform.position = pos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isImmortal) return; // 👉 khi bất tử thì bỏ qua va chạm

        if ((collision.tag == "EnemyShipTag") || (collision.tag == "EnemyBulletTag"))
        {
            PlayExplosion();
            lives--;
            UpdateLivesUI();

            if (collision.tag == "EnemyShipTag")
            {
                Destroy(collision.gameObject);
            }

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

        if (explosionClip != null)
            SoundManager.Instance.PlaySound(explosionClip);
    }

    void UpdateLivesUI()
    {
        if (LiveUiText != null)
        {
            // 👉 chỉ hiển thị số mạng còn lại
            LiveUiText.text = lives.ToString();
        }
    }
}
