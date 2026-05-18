using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject obstaclePrefab;
    public float spawnRate = 2f;

    [Header("F1: Zone Prefabs")]
    public GameObject upperObstaclePrefab;
    public Transform upperSpawnPoint;
    public GameObject coinPrefab;
    public Transform coinSpawnPoint;

    [Header("F1: Difficulty Scaling")]
    public float minSpawnRate = 0.8f;
    public float spawnRateDecreasePerDifficulty = 0.3f;

    [Header("F2: Coin Settings")]
    public float coinSpawnChance = 0.4f;
    public int maxCoinsPerGroup = 5;
    public float coinSpacing = 2f;

    [Header("F3: Item Prefabs")]
    public GameObject speedBoostPrefab;
    public GameObject immortalityPrefab;
    public float itemSpawnChance = 0.15f;

    private float currentSpawnRate;

    void Start()
    {
        currentSpawnRate = spawnRate;
        InvokeRepeating(nameof(Spawn), 0, spawnRate);
    }

    void Update()
    {
        if (GameManager.Instance == null) return;

        float newRate = Mathf.Max(
            spawnRate - (GameManager.Instance.Difficulty * spawnRateDecreasePerDifficulty),
            minSpawnRate
        );

        if (!Mathf.Approximately(newRate, currentSpawnRate))
        {
            currentSpawnRate = newRate;
            CancelInvoke(nameof(Spawn));
            InvokeRepeating(nameof(Spawn), currentSpawnRate, currentSpawnRate);
        }
    }

    void Spawn()
    {
        GameObject player = GameObject.Find("Player");
        bool isGameOver = player.GetComponent<PlayerController>().gameOver;
        if (isGameOver)
        {
            return;
        }

        // Spawn main obstacle
        Instantiate(
            obstaclePrefab,
            spawnPoint.position,
            obstaclePrefab.transform.rotation
        );

        ZoneType zone = GameManager.Instance != null ? GameManager.Instance.CurrentZone : ZoneType.Normal;

        // F1: Spawn upper obstacle in TwoFloor zone
        if (zone == ZoneType.TwoFloor && upperObstaclePrefab != null && upperSpawnPoint != null)
        {
            if (Random.value > 0.4f)
            {
                Instantiate(
                    upperObstaclePrefab,
                    upperSpawnPoint.position,
                    upperObstaclePrefab.transform.rotation
                );
            }
        }

        // F2: Spawn coins
        if (coinPrefab != null && Random.value < coinSpawnChance)
        {
            int count = Random.Range(1, maxCoinsPerGroup + 1);
            Vector3 basePos = coinSpawnPoint != null ? coinSpawnPoint.position : spawnPoint.position + Vector3.up * 2f;

            for (int i = 0; i < count; i++)
            {
                Vector3 pos = basePos + Vector3.left * (i * coinSpacing);
                Instantiate(coinPrefab, pos, Quaternion.identity);
            }
        }

        // F3: Spawn items
        if (Random.value < itemSpawnChance)
        {
            GameObject itemPrefab = Random.value > 0.5f ? speedBoostPrefab : immortalityPrefab;
            if (itemPrefab != null)
            {
                Vector3 itemPos = spawnPoint.position + Vector3.up * 1.5f + Vector3.left * Random.Range(0f, 3f);
                Instantiate(itemPrefab, itemPos, Quaternion.identity);
            }
        }
    }
}
