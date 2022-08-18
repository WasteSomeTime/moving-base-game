using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    
    public float movementSpeed = 5f;
    public float dodgeForce = 5000f;
    public float attackRange = 0.5f;
    public int attackDamage = 10;
    public LayerMask enemyLayers;
    private Vector2 movement;
    private string direction;
    private Vector3 mousePosition;

    // components
    private Rigidbody2D rb;
    private PlayerInput playerInput;

    public Transform rotationPoint;
    public Transform attackPoint;
    private Camera cam;

    // inputs
    private InputAction moveAction;
    private InputAction dodgeAction;
    private InputAction attackAction;
    private InputAction interactAction;
    private InputAction mouseAction;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        cam = Camera.main;
        moveAction = playerInput.actions["Move"];
        dodgeAction = playerInput.actions["Dodge"];
        attackAction = playerInput.actions["Attack"];
        interactAction = playerInput.actions["Interact"];
        mouseAction = playerInput.actions["MousePos"];
    }

    private void Start() {
        dodgeAction.performed += Dodge;
        attackAction.performed += Attack;
        interactAction.performed += Interact;
    }

    private void Update() {
        movement = moveAction.ReadValue<Vector2>();
        mousePosition = cam.ScreenToWorldPoint(mouseAction.ReadValue<Vector2>());
    }

    private void FixedUpdate() {
        Move();
        Vector3 mouseDirection = mousePosition - rotationPoint.position;
        float angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg - 90f;
        rotationPoint.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }

    // player movement function
    private void Move() {
        rb.MovePosition(rb.position + movement * movementSpeed * Time.fixedDeltaTime);

        string currDir = getDirection();
        if (currDir != direction) {
            direction = currDir;
            rotatePlayer();
        }
    }

    // player dodge function
    private void Dodge(InputAction.CallbackContext context) {
        rb.AddForce(movement * dodgeForce);
        Debug.Log("dodge");
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

    private void rotatePlayer() {
        switch (direction) {
            case "n":
                rb.rotation = 0f;
                break;
            case "ne":
                rb.rotation = -45f;
                break;
            case "nw":
                rb.rotation = 45f;
                break;
            case "s":
                rb.rotation = 180f;
                break;
            case "se":
                rb.rotation = -135f;
                break;
            case "sw":
                rb.rotation = 135f;
                break;
            case "e":
                rb.rotation = -90f;
                break;
            case "w":
                rb.rotation = 90f;
                break;
            default:
                rb.rotation = 180f;
                break;
        }
    }

    // get direction the player is facing
    private string getDirection() {
        if (movement.y > 0) {
            if (movement.x > 0) {
                return "ne";
            }
            else if (movement.x < 0) {
                return "nw";
            }
            return "n";
        }
        else if (movement.y < 0) {
            if (movement.x > 0) {
                return "se";
            }
            else if (movement.x < 0) {
                return "sw";
            }
            return "s";
        }
        else {
            if (movement.x > 0) {
                return "e";
            }
            else if (movement.x < 0) {
                return "w";
            }
            return "";
        }
    }
}
