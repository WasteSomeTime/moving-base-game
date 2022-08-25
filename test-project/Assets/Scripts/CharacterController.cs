using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour {
    // components
    private PlayerInput playerInput;
    private Rigidbody2D body;
    private Transform rotationPoint;
    private Transform attackPoint;
    private CharacterAnimator animator;
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

    private State state;
    private Vector2 moveDirection;
    private Vector2 previousDirection;
    private Vector2 dodgeDirection;
    private Vector3 mousePosition;
    [SerializeField] private LayerMask enemyLayers;
    
    // stats
    [SerializeField] private float movementSpeed = 300f;
    [SerializeField] private float dodgeSpeed;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private int attackDamage = 10;

    private void Awake() {
        // player inputs
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        dodgeAction = playerInput.actions["Dodge"];
        attackAction = playerInput.actions["Attack"];
        interactAction = playerInput.actions["Interact"];
        mouseAction = playerInput.actions["MousePos"];

        // components
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<CharacterAnimator>();
        cam = Camera.main;

        // attack points
        rotationPoint = transform.Find("RotationPoint");
        attackPoint = rotationPoint.Find("AttackPoint");

        state = State.Normal;
    }

    private void Start() {
        dodgeAction.performed += DetectDodge;
        attackAction.performed += Attack;
    }

    private void Update() {
        if (state == State.Dodge) {
            float dodgeSpeedDropMultiplier = 10f;
            dodgeSpeed -= dodgeSpeed * dodgeSpeedDropMultiplier * Time.deltaTime;
            float dodgeSpeedMinimum = 1f;
            if (dodgeSpeed < dodgeSpeedMinimum) {
                state = State.Normal;
                playerInput.enabled = true;
            }
        }
        moveDirection = moveAction.ReadValue<Vector2>();
        if (moveDirection != Vector2.zero) {
            previousDirection = moveDirection;
        }
        mousePosition = cam.ScreenToWorldPoint(mouseAction.ReadValue<Vector2>());
    }

    private void FixedUpdate() {
        switch(state) {
            case State.Normal:
                body.velocity = moveDirection * movementSpeed * Time.fixedDeltaTime;
                animator.move(moveDirection);
                break;
            case State.Dodge:
                body.velocity = dodgeDirection * dodgeSpeed;
                animator.dodge(dodgeDirection);
                break;
        }
        rotationPoint.rotation = getMouseRotation();
    }

    // runs when dodge button is pressed
    private void DetectDodge(InputAction.CallbackContext context) {
        if (state == State.Dodge) return;
        dodgeDirection = previousDirection;
        dodgeSpeed = 60f;
        playerInput.enabled = false;
        state = State.Dodge;
    }

    private void Attack(InputAction.CallbackContext context) {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach(Collider2D enemy in hitEnemies) {
            enemy.GetComponent<TestEnemy>().damage(attackDamage);
        }
    }

    // returns rotation value based on the mouse position
    private Quaternion getMouseRotation() {
        Vector3 mouseDirection = mousePosition - rotationPoint.position;
        float angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg - 90f;
        return Quaternion.Euler(new Vector3(0f, 0f, angle));
    }

    // draw in editor on selection
    private void OnDrawGizmosSelected() {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
