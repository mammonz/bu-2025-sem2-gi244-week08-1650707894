using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    public float speed = 10f;

    void Update()
    {
        GameObject player = GameObject.Find("Player");
        bool isGameOver = player.GetComponent<PlayerController>().gameOver;
        if (isGameOver)
        {
            speed = 0;
        }

        // F3: Apply speed multiplier from GameManager
        float speedMult = GameManager.Instance != null ? GameManager.Instance.SpeedMultiplier : 1f;
        transform.Translate(Vector3.left * speed * speedMult * Time.deltaTime);

        if (transform.position.x < -15 && gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }

        // Cleanup coins and items that passed the player
        if (transform.position.x < -15 && (gameObject.CompareTag("Coin") || gameObject.CompareTag("Item")))
        {
            Destroy(gameObject);
        }
    }
}
