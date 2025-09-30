using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyGO;

    // 🟡 CẤU HÌNH SPAWN THEO NHÓM
    public int minEnemiesPerWave = 1;    // Số enemy tối thiểu mỗi đợt
    public int maxEnemiesPerWave = 2;    // Số enemy tối đa mỗi đợt (bắt đầu từ 2)
    public int maxEnemiesLimit = 8;      // Số enemy tối đa có thể spawn (lên đến 8)

    // 🟡 CẤU HÌNH ĐỘ KHÓ TĂNG DẦN
    public float timeBetweenWaves = 3f;  // Thời gian giữa các đợt spawn
    public float difficultyIncreaseInterval = 15f; // Mỗi 15 giây tăng độ khó
    public float waveTimeDecrement = 0.2f; // Giảm thời gian giữa các wave

    private bool isSpawning = false;
    private float currentWaveTime;

    void Start()
    {
        currentWaveTime = timeBetweenWaves;
        Debug.Log("🟢 EnemySpawner Start - Sẵn sàng spawn theo nhóm!");
    }

    void SpawnWave()
    {
        if (!isSpawning) return;

        // 🟡 TÍNH SỐ ENEMY SPAWN TRONG WAVE NÀY
        int enemiesToSpawn = Random.Range(minEnemiesPerWave, maxEnemiesPerWave + 1);
        enemiesToSpawn = Mathf.Min(enemiesToSpawn, maxEnemiesLimit); // Không vượt quá giới hạn

        Debug.Log($"🌊 Spawning wave: {enemiesToSpawn} enemies");

        // 🟡 SPAWN NHIỀU ENEMY CÙNG LÚC
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnSingleEnemy();
        }

        // 🟡 LÊN LỊCH WAVE TIẾP THEO
        ScheduleNextWave();
    }

    void SpawnSingleEnemy()
    {
        if (EnemyGO == null)
        {
            Debug.LogError("❌ EnemyGO chưa được gán prefab!");
            return;
        }

        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        GameObject anEnemy = Instantiate(EnemyGO);

        // 🟡 RẢI ĐỀU VỊ TRÍ SPAWN
        float spawnX = Random.Range(min.x + 0.1f, max.x - 0.1f); // Tránh spawn sát rìa
        float spawnY = max.y + Random.Range(0f, 0.5f); // Cao hơn màn hình một chút

        anEnemy.transform.position = new Vector2(spawnX, spawnY);

        Debug.Log($"✅ Enemy {Time.frameCount} spawned tại: {anEnemy.transform.position}");
    }

    void ScheduleNextWave()
    {
        if (!isSpawning) return;

        Debug.Log($"⏰ Wave tiếp theo sau: {currentWaveTime:F1} giây");
        Invoke("SpawnWave", currentWaveTime);
    }

    void IncreaseDifficulty()
    {
        // 🟡 TĂNG SỐ LƯỢNG ENEMY MỖI WAVE
        if (maxEnemiesPerWave < maxEnemiesLimit)
        {
            maxEnemiesPerWave++;
            Debug.Log($"📈 TĂNG ĐỘ KHÓ! Số enemy mỗi wave: {maxEnemiesPerWave}");
        }

        // 🟡 GIẢM THỜI GIAN GIỮA CÁC WAVE
        if (currentWaveTime > 0.5f) // Không dưới 0.5 giây
        {
            currentWaveTime -= waveTimeDecrement;
            currentWaveTime = Mathf.Max(currentWaveTime, 0.5f);
            Debug.Log($"⚡ Tăng tốc độ! Thời gian giữa waves: {currentWaveTime:F1}s");
        }

        // 🟡 KIỂM TRA ĐẠT ĐỘ KHÓ TỐI ĐA
        if (maxEnemiesPerWave >= maxEnemiesLimit && currentWaveTime <= 0.5f)
        {
            Debug.Log("🚀 Đạt độ khó tối đa! 8 enemies mỗi wave với tốc độ cao!");
            // Có thể dừng tăng độ khó hoặc thêm cơ chế khác
        }
    }

    public void ScheduleEnemySpawn()
    {
        Debug.Log("🚀 Bắt đầu spawn enemy theo wave!");
        isSpawning = true;

        // 🟡 ĐẶT LẠI CẤU HÌNH KHI BẮT ĐẦU GAME MỚI
        maxEnemiesPerWave = 2; // Bắt đầu với 1-2 enemies
        currentWaveTime = timeBetweenWaves;

        // 🟡 BẮT ĐẦU WAVE ĐẦU TIÊN NGAY LẬP TỨC
        Debug.Log("🎯 Wave enemy đầu tiên NGAY BÂY GIỜ!");
        SpawnWave();

        // 🟡 BẮT ĐẦU TĂNG ĐỘ KHÓ
        Debug.Log($"⚡ Bắt đầu tăng độ khó mỗi {difficultyIncreaseInterval} giây");
        InvokeRepeating("IncreaseDifficulty", difficultyIncreaseInterval, difficultyIncreaseInterval);
    }

    public void UnscheduEnemySpawnder()
    {
        Debug.Log("🛑 Dừng spawn enemy và tăng độ khó");
        isSpawning = false;
        CancelInvoke("SpawnWave");
        CancelInvoke("IncreaseDifficulty");
    }

    // 🟡 METHOD ĐỂ TUỲ CHỈNH ĐỘ KHÓ
    public void SetDifficulty(int startMaxEnemies, float startWaveTime, float difficultySpeed)
    {
        maxEnemiesPerWave = startMaxEnemies;
        currentWaveTime = startWaveTime;
        difficultyIncreaseInterval = difficultySpeed;
        Debug.Log($"🎮 Điều chỉnh độ khó: {startMaxEnemies} enemies, {startWaveTime}s/wave, tăng mỗi {difficultySpeed}s");
    }

    // 🟡 METHOD ĐỂ THAY ĐỔI GIỚI HẠN TỐI ĐA
    public void SetMaxEnemiesLimit(int maxLimit)
    {
        maxEnemiesLimit = Mathf.Clamp(maxLimit, 1, 20); // Giới hạn từ 1-20
        Debug.Log($"🎯 Đặt giới hạn enemy tối đa: {maxEnemiesLimit}");
    }
}