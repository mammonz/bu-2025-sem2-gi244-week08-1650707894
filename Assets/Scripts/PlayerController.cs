using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 10f;
    public float gravityMultiplier = 1f;
    public bool gameOver = false;

    public ParticleSystem fxDirt;

    public GameObject fxExplosion;

    public AudioClip sfxCrash;
    public AudioClip sfxJump;

    public Animator animator;

    private Rigidbody rb;
    private InputAction jumpAction;
    private AudioSource audioSource;
    

    private bool isOnGround = true;
    private bool isGravityFlipped = false;
    private float originalGravityY;

    // F3: Item states
    public bool IsImmortal { get; private set; }
    public bool HasSpeedBoost { get; private set; }
    private float immortalTimer;
    private float speedBoostTimer;

    [Header("Item Visuals")]
    public GameObject immortalEffect;
    public GameObject speedBoostEffect;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        jumpAction = InputSystem.actions.FindAction("Jump");
        if (jumpAction != null && !jumpAction.enabled)
            jumpAction.Enable();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Start()
    {
        originalGravityY = Physics.gravity.y;
        Physics.gravity = new Vector3(0, originalGravityY * gravityMultiplier, 0);

        animator.SetFloat("Speed_f", 1.0f);

        if (GameManager.Instance != null)
            GameManager.Instance.OnZoneChanged += HandleZoneChanged;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnZoneChanged -= HandleZoneChanged;

        Physics.gravity = new Vector3(0, originalGravityY, 0);
    }

    void Update()
    {
        if (gameOver)
        {
            return;
        }

        if (jumpAction.triggered && isOnGround)
        {
            //Debug.Log("Jump");
            float direction = isGravityFlipped ? -1f : 1f;
            rb.AddForce(jumpForce * direction * Vector3.up, ForceMode.Impulse);
            isOnGround = false;
            animator.SetTrigger("Jump_trig");
            fxDirt.Stop();
            audioSource.PlayOneShot(sfxJump);
        }

        UpdateItemTimers();
    }

    // F1: Gravity flip for UpsideDown zone
    private void HandleZoneChanged(ZoneType zone)
    {
        if (zone == ZoneType.UpsideDown)
            FlipGravity(true);
        else if (isGravityFlipped)
            FlipGravity(false);
    }

    private void FlipGravity(bool flipped)
    {
        isGravityFlipped = flipped;
        float gravY = Mathf.Abs(originalGravityY) * gravityMultiplier;
        Physics.gravity = new Vector3(0, flipped ? gravY : -gravY, 0);

        Vector3 scale = transform.localScale;
        scale.y = flipped ? -Mathf.Abs(scale.y) : Mathf.Abs(scale.y);
        transform.localScale = scale;
    }

    // F3: Item activation
    public void ActivateSpeedBoost(float duration, float multiplier)
    {
        HasSpeedBoost = true;
        speedBoostTimer = duration;
        if (GameManager.Instance != null)
            GameManager.Instance.SpeedMultiplier = multiplier;
        if (speedBoostEffect != null)
            speedBoostEffect.SetActive(true);
    }

    public void ActivateImmortality(float duration)
    {
        IsImmortal = true;
        immortalTimer = duration;
        if (immortalEffect != null)
            immortalEffect.SetActive(true);
    }

    private void UpdateItemTimers()
    {
        if (HasSpeedBoost)
        {
            speedBoostTimer -= Time.deltaTime;
            if (speedBoostTimer <= 0f)
            {
                HasSpeedBoost = false;
                if (GameManager.Instance != null)
                    GameManager.Instance.SpeedMultiplier = 1f;
                if (speedBoostEffect != null)
                    speedBoostEffect.SetActive(false);
            }
        }

        if (IsImmortal)
        {
            immortalTimer -= Time.deltaTime;
            if (immortalTimer <= 0f)
            {
                IsImmortal = false;
                if (immortalEffect != null)
                    immortalEffect.SetActive(false);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            fxDirt.Play();
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            // F3: Immortal = destroy obstacle instead of dying
            if (IsImmortal)
            {
                Destroy(collision.gameObject);
                return;
            }

            Debug.Log("Game Over!");
            gameOver = true;

            animator.SetBool("Death_b", true);
            animator.SetInteger("DeathType_int", 1);
            fxDirt.Stop();

            Instantiate(fxExplosion, transform.position, Quaternion.identity);

            audioSource.PlayOneShot(sfxCrash);
        }
    }

    // F2: Coin collection
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            if (ScoreManager.Instance != null)
                ScoreManager.Instance.AddCoin();
            Destroy(other.gameObject);
        }
    }
}
