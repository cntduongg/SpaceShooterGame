using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject playButton;
    public GameObject GameOverGo;
    public GameObject titleImage;    // 👉 hình Title
    public GameObject nhomImage;     // 👉 hình Nhóm (credits)

    [Header("Gameplay References")]
    public GameObject playerShip;
    public GameObject enemySpawner;
    public GameScore gameScore;      // quản lý điểm hiện tại
    public GameObject gameplayLogo;  // 👉 logo hiển thị trong Gameplay

    public enum GameManagerState
    {
        Opening,
        Gameplay,
        GameOver,
    }

    private GameManagerState GM;

    void Start()
    {
        GM = GameManagerState.Opening;
        UpdateGame();
    }

    void UpdateGame()
    {
        // 🔎 Lấy tham chiếu tới TimeCounter
        TimeCounter timeCounter = FindObjectOfType<TimeCounter>();

        switch (GM)
        {
            case GameManagerState.Opening:
                if (playButton != null)
                    playButton.SetActive(true);

                if (GameOverGo != null)
                    GameOverGo.SetActive(false);

                if (titleImage != null)
                    titleImage.SetActive(true);

                if (nhomImage != null)
                    nhomImage.SetActive(true);

                if (gameplayLogo != null)
                    gameplayLogo.SetActive(false);

                Time.timeScale = 1f; // 👉 reset thời gian

                // 🕒 Khi quay về Opening, dừng đồng hồ (phòng trường hợp vẫn còn chạy)
                if (timeCounter != null)
                    timeCounter.StopTimer();
                break;

            case GameManagerState.Gameplay:
                if (playButton != null)
                    playButton.SetActive(false);

                if (GameOverGo != null)
                    GameOverGo.SetActive(false);

                if (titleImage != null)
                    titleImage.SetActive(false);

                if (nhomImage != null)
                    nhomImage.SetActive(false);

                if (gameplayLogo != null)
                    gameplayLogo.SetActive(true);

                if (playerShip != null)
                    playerShip.GetComponent<PlayerControl>().Init();

                if (enemySpawner != null)
                    enemySpawner.GetComponent<EnemySpawner>().ScheduleEnemySpawn();

                if (gameScore != null)
                    gameScore.ResetScore();

                Time.timeScale = 1f; // 👉 gameplay chạy bình thường

                // 🟢 Khi bắt đầu gameplay: reset + chạy lại đồng hồ
                if (timeCounter != null)
                    timeCounter.StartTimer();
                break;

            case GameManagerState.GameOver:
                if (enemySpawner != null)
                    enemySpawner.GetComponent<EnemySpawner>().UnscheduEnemySpawnder();

                if (GameOverGo != null)
                    GameOverGo.SetActive(true);

                if (gameplayLogo != null)
                    gameplayLogo.SetActive(false);

                // 🛑 Khi GameOver: dừng đồng hồ
                if (timeCounter != null)
                    timeCounter.StopTimer();

                // 👉 Lưu HighScore
                if (GameHIghScore.Instance != null && GameScore.Instance != null)
                {
                    GameHIghScore.Instance.TrySaveHighScore(GameScore.Instance.GetCurrentScore());
                }

                // 👉 Dừng toàn bộ gameplay
                Time.timeScale = 0f;

                // 👉 Đếm 3 giây theo thời gian thực
                StartCoroutine(WaitToReturnOpening());
                break;
        }
    }

    IEnumerator WaitToReturnOpening()
    {
        yield return new WaitForSecondsRealtime(3f); // ⏳ đợi 3s ngoài timeScale
        ChangeToOpeningState();
    }

    public void SetGameManagerState(GameManagerState state)
    {
        GM = state;
        UpdateGame();
    }

    public void StartGamePlay()
    {
        SetGameManagerState(GameManagerState.Gameplay);
    }

    public void ChangeToOpeningState()
    {
        if (GameOverGo != null)
            GameOverGo.SetActive(false);

        if (titleImage != null)
            titleImage.SetActive(true);

        if (nhomImage != null)
            nhomImage.SetActive(true);

        if (gameplayLogo != null)
            gameplayLogo.SetActive(false);

        Time.timeScale = 1f; // 👉 reset lại thời gian khi quay về Opening

        SetGameManagerState(GameManagerState.Opening);
    }
}
