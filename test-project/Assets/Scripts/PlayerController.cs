using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    // components
    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private PlayerAnimManager animations;

    [SerializeField] private Transform rotationPoint;
    [SerializeField] private Transform attackPoint;
    private Camera cam;

    // inputs
    private InputAction moveAction;
    private InputAction dodgeAction;
    private InputAction attackAction;
    private InputAction interactAction;
    private InputAction mouseAction;

    private enum State {
        Normal,
        Dodge
    }

    [SerializeField] private float movementSpeed = 300f;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private LayerMask enemyLayers;
    private Vector2 moveDirection;
    private Vector2 lastMoveDirection;
    private Vector3 mousePosition;
    private State state;
    private Vector2 dodgeDirection;
    private float dodgeSpeed;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        animations = GetComponent<PlayerAnimManager>();
        cam = Camera.main;
        moveAction = playerInput.actions["Move"];
        dodgeAction = playerInput.actions["Dodge"];
        attackAction = playerInput.actions["Attack"];
        interactAction = playerInput.actions["Interact"];
        mouseAction = playerInput.actions["MousePos"];

        state = State.Normal;
    }

    private void Start() {
        dodgeAction.performed += Dodge;
        attackAction.performed += Attack;
        interactAction.performed += Interact;
    }

    private void Update() {
        switch(state) {
            case State.Normal:
                moveDirection = moveAction.ReadValue<Vector2>();
                if (moveDirection.x != 0 || moveDirection.y != 0) {
                    //not idle
                    lastMoveDirection = moveDirection;
                }
                break;
            case State.Dodge:
                float dodgeSpeedDropMultiplier = 5f;
                dodgeSpeed -= dodgeSpeed * dodgeSpeedDropMultiplier * Time.fixedDeltaTime;
                float dodgeSpeedMinimum = 10f;
                if (dodgeSpeed < dodgeSpeedMinimum) {
                    state = State.Normal;
                }
                break;
        }
        mousePosition = cam.ScreenToWorldPoint(mouseAction.ReadValue<Vector2>());
    }

    private void FixedUpdate() {
        switch(state) {
            case State.Normal:
                Move();
                break;
            case State.Dodge:
                rb.velocity = dodgeDirection * dodgeSpeed;
                break;
        }
        Vector3 mouseDirection = mousePosition - rotationPoint.position;
        float angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg - 90f;
        rotationPoint.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }

    // player movement function
    private void Move() {
        rb.velocity = moveDirection * movementSpeed * Time.fixedDeltaTime;
        animations.playMove(moveDirection);
    }

    // player dodge function
    private void Dodge(InputAction.CallbackContext context) {
        if (state == State.Normal) {
            dodgeDirection = lastMoveDirection;
            dodgeSpeed = 120f;
            state = State.Dodge;
            Debug.Log("dodge");
        }
    }

    // player basic attack function
    private void Attack(InputAction.CallbackContext context) {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach(Collider2D enemy in hitEnemies) {
            enemy.GetComponent<TestEnemy>().damage(attackDamage);
        }
        
        Debug.Log("attack");
    }

    private void OnDrawGizmosSelected() {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    // player interaction function
    private void Interact(InputAction.CallbackContext context) {
        Debug.Log("interact");
    }
}
