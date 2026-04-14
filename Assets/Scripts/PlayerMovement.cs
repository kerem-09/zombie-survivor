using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 6f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    public Vector2 LastMoveDir { get; private set; } = Vector2.right;//Halat için


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if (moveInput.sqrMagnitude > 0.01f)//Halat
            LastMoveDir = moveInput;


        moveInput = new Vector2(x, y).normalized;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }
}
