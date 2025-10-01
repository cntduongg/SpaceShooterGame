using UnityEngine;
using TMPro;
using System.IO;

[System.Serializable]
public class HighScoreData
{
    public int highScore;
}

public class GameHIghScore : MonoBehaviour
{
    public static GameHIghScore Instance;

    [Header("UI References")]
    public TextMeshProUGUI highScoreText; // UI hiển thị High Score

    private int highScore;
    private string savePath;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        savePath = Path.Combine(Application.persistentDataPath, "highscore.json");
        LoadHighScore();
    }

    void Start()
    {
        UpdateHighScoreUI();
    }

    // 👉 Lưu HighScore nếu newScore > highScore (và ghi đè file cũ)
    public void TrySaveHighScore(int newScore)
    {
        Debug.Log("🔎 TrySaveHighScore được gọi với score = " + newScore);

        if (newScore > highScore)
        {
            highScore = newScore;

            // Xóa file cũ nếu có
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
                Debug.Log("🗑️ HighScore cũ đã bị xóa");
            }

            // Ghi highscore mới
            HighScoreData data = new HighScoreData { highScore = highScore };
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(savePath, json);

            Debug.Log("💾 HighScore mới được lưu: " + highScore);
        }
        else
        {
            Debug.Log("⚠ Điểm " + newScore + " < HighScore hiện tại (" + highScore + "), không cập nhật.");
        }

        UpdateHighScoreUI();
    }

    public void LoadHighScore()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            HighScoreData data = JsonUtility.FromJson<HighScoreData>(json);
            highScore = data.highScore;
            Debug.Log("📂 HighScore loaded: " + highScore);
        }
        else
        {
            highScore = 0;
            Debug.Log("📂 Không tìm thấy file HighScore, đặt = 0");
        }
    }

    private void UpdateHighScoreUI()
    {
        if (highScoreText != null)
        {
            // luôn ghi đè lại toàn bộ nội dung
            highScoreText.text = "HIGHSCORE: " + highScore.ToString();

            Debug.Log("🖥️ HighScore UI hiển thị: " + highScore + " | Text gắn vào: " + highScoreText.name);
        }
        else
        {
            Debug.LogError("❌ Chưa gán HighScoreText trong Inspector!");
        }
    }

    public int GetHighScore()
    {
        return highScore;
    }

    // 👉 Reset HighScore nếu cần test lại
    public void ResetHighScore()
    {
        highScore = 0;
        if (File.Exists(savePath)) File.Delete(savePath);
        UpdateHighScoreUI();
        Debug.Log("🗑️ HighScore reset về 0!");
    }
}