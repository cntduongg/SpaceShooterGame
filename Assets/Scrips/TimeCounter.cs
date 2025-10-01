using UnityEngine;
using TMPro;

public class TimeCounter : MonoBehaviour
{
    [Header("UI hiển thị thời gian")]
    public TextMeshProUGUI timeText;   // Gán TextMeshPro vào đây trong Inspector

    private float elapsedTime = 0f;
    private bool isCounting = false;

    void Start()
    {
        // ✅ Khi game bắt đầu (nhấn Play), tự động reset và chạy
        ResetTimer();
        StartTimer();
    }

    void Update()
    {
        if (!isCounting) return;

        elapsedTime += Time.deltaTime;

        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        if (timeText != null)
            timeText.text = $"{minutes:00}:{seconds:00}";
    }

    // 👉 Gọi khi muốn reset và bắt đầu lại đồng hồ
    public void StartTimer()
    {
        elapsedTime = 0f;
        isCounting = true;
    }

    // 👉 Gọi khi GameOver để dừng đồng hồ
    public void StopTimer()
    {
        isCounting = false;
    }

    // 👉 Reset hiển thị về 00:00
    public void ResetTimer()
    {
        elapsedTime = 0f;
        if (timeText != null)
            timeText.text = "00:00";
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }
}

