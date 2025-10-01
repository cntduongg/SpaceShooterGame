using UnityEngine;
using TMPro;

public class GameScore : MonoBehaviour
{
    public static GameScore Instance;
    public TextMeshProUGUI scoreText;  // UI hiển thị điểm hiện tại
    private int score;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        ResetScore();
    }

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("🏆 + " + amount + " | Tổng điểm: " + score);
        UpdateScoreUI();
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = score.ToString(); // 👉 chỉ hiện số điểm
        else
            Debug.LogError("❌ Chưa gán ScoreText trong GameScore!");
    }

    public int GetCurrentScore()
    {
        return score;
    }
}