using UnityEngine;
using TMPro;

public class TimeCounter : MonoBehaviour
{
    [Header("UI hiển thị thời gian")]
    public TextMeshProUGUI timeText;   // Gán vào Text UI trên Canvas trong Inspector

    private float elapsedTime = 0f;     // Tổng thời gian đã trôi qua (giây)
    private bool isCounting = false;    // Kiểm soát có đếm hay không

    void Start()
    {
        ResetTimer();       // Bắt đầu từ 0
        StartTimer();       // Tự động bắt đầu khi game chạy
    }

    void Update()
    {
        if (isCounting)
        {
            elapsedTime += Time.deltaTime;

            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);

            if (timeText != null)
                timeText.text = $"{minutes:00}:{seconds:00}";
        }
    }

    // 👉 Bắt đầu đếm
    public void StartTimer()
    {
        isCounting = true;
    }

    // 👉 Tạm dừng đếm
    public void StopTimer()
    {
        isCounting = false;
    }

    // 👉 Reset về 0
    public void ResetTimer()
    {
        elapsedTime = 0f;
        if (timeText != null)
            timeText.text = "00:00";
    }

    // 👉 Lấy tổng thời gian (giây)
    public float GetElapsedTime()
    {
        return elapsedTime;
    }
}
