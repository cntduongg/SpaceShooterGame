using UnityEngine;

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
                    gameplayLogo.SetActive(false); // 👉 logo gameplay chưa hiện
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
                    gameplayLogo.SetActive(true); // 👉 hiện logo khi gameplay

                if (playerShip != null)
                    playerShip.GetComponent<PlayerControl>().Init();

                if (enemySpawner != null)
                    enemySpawner.GetComponent<EnemySpawner>().ScheduleEnemySpawn();

                if (gameScore != null)
                    gameScore.ResetScore(); // reset điểm khi bắt đầu ván mới
                break;

            case GameManagerState.GameOver:
                if (enemySpawner != null)
                    enemySpawner.GetComponent<EnemySpawner>().UnscheduEnemySpawnder();

                if (GameOverGo != null)
                    GameOverGo.SetActive(true);

                if (gameplayLogo != null)
                    gameplayLogo.SetActive(false); // 👉 tắt logo gameplay khi game over

                // 👉 Lưu HighScore
                if (GameHIghScore.Instance != null && GameScore.Instance != null)
                {
                    GameHIghScore.Instance.TrySaveHighScore(GameScore.Instance.GetCurrentScore());
                }

                // Sau 3 giây quay về Opening để hiện lại Title + Nhóm
                Invoke("ChangeToOpeningState", 3f);
                break;
        }
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
            gameplayLogo.SetActive(false); // 👉 ẩn logo gameplay khi quay lại Opening

        SetGameManagerState(GameManagerState.Opening);
    }
}
