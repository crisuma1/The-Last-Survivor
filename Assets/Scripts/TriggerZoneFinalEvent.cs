using UnityEngine;

public class TriggerZoneFinalEvent : MonoBehaviour
{
    public GameObject monsterSpawner;
    public GameObject alarmLight;
    public AudioSource alarmSiren;
    public GameObject[] bossCores; // ← 여러 개의 본체

    public AudioSource backgroundMusic;
    public GameObject normalLightGroup;   // ← 기존 일반 조명들 부모 오브젝트
    public GameObject ladderToRemove;
    public GameObject destroyEffectPrefab; // 사다리 제거 시 나올 이펙트 프리팹
    public GameObject hudCanvas;

    private bool triggered = false;
    private int bossDestroyedCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            triggered = true;
            StartFinalWave();
        }
    }

    void StartFinalWave()
    {
        if (hudCanvas != null)
            hudCanvas.SetActive(true);

        alarmSiren?.Play();
        alarmLight?.SetActive(true);
        monsterSpawner.SetActive(true);


        if (backgroundMusic != null && backgroundMusic.isPlaying)
            backgroundMusic.Stop();
        if (normalLightGroup != null)
            normalLightGroup.SetActive(false);

        if (ladderToRemove != null)
        {
            // 이펙트 생성
            if (destroyEffectPrefab != null)
            {
                destroyEffectPrefab.SetActive(true);
            }

            Destroy(ladderToRemove);
        }


        foreach (var core in bossCores)
        {
            var entity = core.GetComponent<LivingEntity>();
            if (entity != null)
            {
                entity.onDeath += OnOneBossDestroyed;
            }
        }
    }

    void OnOneBossDestroyed()
    {
        bossDestroyedCount++;

        if (bossDestroyedCount >= bossCores.Length)
        {
            AllBossesDestroyed();
        }
    }

    void AllBossesDestroyed()
    {
        monsterSpawner.SetActive(false);
        alarmSiren?.Stop();
        alarmLight?.SetActive(false);

        backgroundMusic?.Play();

        if (normalLightGroup != null)
            normalLightGroup.SetActive(true);

    }
}
