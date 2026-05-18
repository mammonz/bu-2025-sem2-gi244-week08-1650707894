using UnityEngine;

public enum ZoneType { Normal, UpsideDown, TwoFloor }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Difficulty")]
    public float difficultyIncreaseInterval = 100f;
    public int maxDifficulty = 3;

    [Header("Zone")]
    public float zoneChangeInterval = 200f;
    public float zoneDuration = 50f;

    public int Difficulty { get; private set; }
    public ZoneType CurrentZone { get; private set; }
    public float SpeedMultiplier { get; set; } = 1f;

    public event System.Action<ZoneType> OnZoneChanged;

    private float nextZoneChangeDistance;
    private float zoneEndDistance;
    private bool inSpecialZone;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        GameObject player = GameObject.Find("Player");
        if (player != null && player.GetComponent<PlayerController>().gameOver)
            return;

        if (ScoreManager.Instance == null) return;
        float distance = ScoreManager.Instance.Distance;

        UpdateDifficulty(distance);
        UpdateZone(distance);
    }

    private void UpdateDifficulty(float distance)
    {
        Difficulty = Mathf.Min((int)(distance / difficultyIncreaseInterval), maxDifficulty);
    }

    private void UpdateZone(float distance)
    {
        if (inSpecialZone && distance >= zoneEndDistance)
        {
            inSpecialZone = false;
            CurrentZone = ZoneType.Normal;
            OnZoneChanged?.Invoke(CurrentZone);
            nextZoneChangeDistance = distance + zoneChangeInterval;
            return;
        }

        if (!inSpecialZone && distance >= nextZoneChangeDistance && Difficulty >= 1)
        {
            inSpecialZone = true;
            zoneEndDistance = distance + zoneDuration;
            CurrentZone = Random.value > 0.5f ? ZoneType.UpsideDown : ZoneType.TwoFloor;
            OnZoneChanged?.Invoke(CurrentZone);
        }
    }
}
