using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 6f;

    private Rigidbody2D rb;
    private Animator animator; // Animator bileþeni için deðiþken
    private Vector2 moveInput;
    public Vector2 LastMoveDir { get; private set; } = Vector2.right; // Halat için

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // Karakterin üzerindeki Animator bileþenini alýyoruz
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        moveInput = new Vector2(x, y).normalized;

        if (moveInput.sqrMagnitude > 0.01f) // Halat için yönü kaydet
            LastMoveDir = moveInput;

        // --- ANÝMASYON TETÝKLEME ---
        if (animator != null)
        {
            // Eðer hareket varsa (hýz 0'dan büyükse) isRunning true olur.
            // Bu satýr Animator'daki o oklarý (Transition) kontrol eder.
            bool isWalking = moveInput.sqrMagnitude > 0.01f;
            animator.SetBool("isRunning", isWalking);
        }
    }

    void FixedUpdate()
    {
        // Karakteri fizik kurallarýyla hareket ettiriyoruz
        rb.linearVelocity = moveInput * moveSpeed;
    }
}