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
    public AudioClip shootClip;
    public AudioClip explosionClip;
    public AudioSource engineAudio;
    private AudioSource audioSource;

    float halfWidth;
    float halfHeight;

    private bool infiniteLives = false;
    private int savedLives;

    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        halfWidth = sr.bounds.extents.x;
        halfHeight = sr.bounds.extents.y;

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
        // Toggle infinite lives with L key
        if (Input.GetKeyDown(KeyCode.L))
        {
            HandleInfiniteLivesToggle();
        }

        // Fire bullet when the spacebar is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject bullet01 = Instantiate(PlayerBulletGo);
            bullet01.transform.position = bulletPosition1.transform.position;

            GameObject bullet02 = Instantiate(PlayerBulletGo);
            bullet02.transform.position = bulletPosition2.transform.position;

            if (shootClip != null)
                audioSource.PlayOneShot(shootClip);
        }

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector2 direction = new Vector2(x, y).normalized;

        Move(direction);

        // Engine audio: play when moving, stop when idle
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
        if (infiniteLives)
        {
            PlayExplosion();
            return;
        }

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

        if (explosionClip != null)
            AudioSource.PlayClipAtPoint(explosionClip, transform.position);
    }

    private void HandleInfiniteLivesToggle()
    {
        infiniteLives = !infiniteLives;

        if (infiniteLives)
        {
            savedLives = lives;
        }
        else
        {
            lives = savedLives;
        }
    }
}