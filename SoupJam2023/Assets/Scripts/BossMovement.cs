using UnityEngine.Events;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [System.Serializable] //mode
    private enum MoveMode
    {
        SetVelocity, UseForce
    }

    [Header("Settings")]
    public float topSpeed;
    public float acceleration;
    [Space(10f)]
    public UnityEvent<Vector2> onChangeMoveDir;

    [Header("Technical")]
    [Tooltip("Sets the way movement is handled" +
        "\nSet Velocity: Directly sets the velocity of the object." +
        "\nUse Force: Uses force to move the object.")]
    [SerializeField] private MoveMode mode;

    //vars
    private Vector2 moveDir = Vector2.zero;
    private float speed;

    //external components
    private Rigidbody2D rigidBody;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    public void SetMoveDir(Vector2 input)
    {
        input *= transform.right;
        if (input.magnitude > 1f) { input.Normalize(); } //normalize input
        if (input != moveDir) { onChangeMoveDir?.Invoke(input); } //check dir change
        moveDir = input;
        Debug.Log(transform.up);
    }

    private void FixedUpdate()
    {
        UpdateSpeed();
        if (moveDir != Vector2.zero)
        {
            Move();
        }
    }

    private void UpdateSpeed()
    {
        if (moveDir.magnitude > 0.1f) { speed += acceleration; } //accelerate
        else { speed -= acceleration; } //fix for akward 'hitching' when key is briefly released
        speed = Mathf.Clamp(speed, 0, topSpeed);
    }

    private void Move()
    {
        Vector2 toMove = moveDir * (speed * Time.fixedDeltaTime * 100);
        switch (mode)
        {
            case MoveMode.SetVelocity:
                rigidBody.velocity = toMove;
                break;

            case MoveMode.UseForce:
                rigidBody.AddForce(toMove);
                break;
        }
    }
}