using UnityEngine;

public enum ItemType { SpeedBoost, Immortality }

public class ItemPickup : MonoBehaviour
{
    public ItemType itemType;
    public float duration = 5f;
    public float speedMultiplier = 1.5f;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Potion");
        PlayerController player = other.GetComponent<PlayerController>();
        if (player == null) return;

        switch (itemType)
        {
            case ItemType.SpeedBoost:
                player.ActivateSpeedBoost(duration, speedMultiplier);
                break;
            case ItemType.Immortality:
                player.ActivateImmortality(duration);
                break;
        }

        Destroy(gameObject);
    }
}
