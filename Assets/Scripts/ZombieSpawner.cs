using System.Collections;
using System.Collections.Generic;
using System.Collections.Generic;
using UnityEngine;

// 좀비 게임 오브젝트를 주기적으로 생성
public class ZombieSpawner : MonoBehaviour
{
    public Zombie[] zombiePrefabs; // 직접 생성할 좀비 프리팹들 (5개 정도)
    public float spawnInterval = 10f; // 몇 초마다 스폰할지
    public Transform[] spawnPoints; // 좀비 AI를 소환할 위치들


    private List<Zombie> zombies = new List<Zombie>(); // 생성된 좀비들을 담는 리스트
    private int wave; // 현재 웨이브


    private void OnEnable()
    {
        StartCoroutine(SpawnLoop());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }


    // 현재 웨이브에 맞춰 좀비들을 생성
    IEnumerator SpawnLoop()
    {
        while (true)
        {
            if (GameManager.instance != null && GameManager.instance.isGameover)
                yield break;

            SpawnWave();
            yield return new WaitForSeconds(spawnInterval);
        }
    }


    private void SpawnWave()
    {
        wave++;

        int spawnCount = Mathf.RoundToInt(wave * 1.5f);

        for (int i = 0; i < spawnCount; i++)
        {
            CreateZombie();
        }

        UIManager.instance.UpdateWaveText(wave, zombies.Count);
    }

    // 좀비를 생성하고 생성한 좀비에게 추적할 대상을 할당
    private void CreateZombie()
    {
        // 랜덤 좀비 프리팹 선택
        Zombie prefab = zombiePrefabs[Random.Range(0, zombiePrefabs.Length)];

        // 랜덤 위치 선택
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // 생성
        Zombie zombie = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);

        // 리스트에 등록
        zombies.Add(zombie);

        // 죽었을 때 처리
        zombie.onDeath += () => zombies.Remove(zombie);
        zombie.onDeath += () => Destroy(zombie.gameObject, 10f);
        zombie.onDeath += () => GameManager.instance.AddScore(1);
    }
}