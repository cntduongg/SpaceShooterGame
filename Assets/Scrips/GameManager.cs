using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playButton;
    public GameObject playerShip;
    public GameObject enemySpawner;

    public enum GameManagerState
    {
        Opening,
        Gameplay,
        GameOver,
    }
    GameManagerState GM;

    void Start()
    {
        GM = GameManagerState.Opening;
        Debug.Log("🟢 GameManager Start được gọi");
        UpdateGame();
    }

    void UpdateGame()
    {
        Debug.Log($"🔄 Chuyển sang state: {GM}");

        switch (GM)
        {
            case GameManagerState.Opening:
                Debug.Log("📋 Opening State");
                if (playButton != null)
                {
                    playButton.SetActive(true);
                    Debug.Log("✅ Hiển thị Play button");
                }
                else
                {
                    Debug.LogError("❌ PlayButton không được gán!");
                }
                break;

            case GameManagerState.Gameplay:
                Debug.Log("🎮 Gameplay State - Bắt đầu game");
                if (playButton != null)
                {
                    playButton.SetActive(false);
                    Debug.Log("✅ Ẩn Play button");
                }

                if (playerShip != null)
                {
                    playerShip.GetComponent<PlayerControl>().Init();
                    Debug.Log("✅ Khởi tạo Player");
                }
                else
                {
                    Debug.LogError("❌ PlayerShip không được gán!");
                }

                if (enemySpawner != null)
                {
                    Debug.Log("✅ Gọi ScheduleEnemySpawn");
                    enemySpawner.GetComponent<EnemySpawner>().ScheduleEnemySpawn();
                }
                else
                {
                    Debug.LogError("❌ EnemySpawner không được gán!");
                }
                break;

            case GameManagerState.GameOver:
                Debug.Log("💀 GameOver State");
                if (enemySpawner != null)
                    enemySpawner.GetComponent<EnemySpawner>().UnscheduEnemySpawnder();
                Invoke("ChangeToOpeningState", 8f);
                break;
        }
    }

    public void SetGameManagerState(GameManagerState state)
    {
        Debug.Log($"🎯 SetGameManagerState: {state}");
        GM = state;
        UpdateGame();
    }

    public void StartGamePlay()
    {
        Debug.Log("🎮 StartGamePlay được gọi từ Play button");
        SetGameManagerState(GameManagerState.Gameplay);
    }

    public void ChangeToOpeningState()
    {
        Debug.Log("🔙 Quay về Opening State");
        SetGameManagerState(GameManagerState.Opening);
    }
}