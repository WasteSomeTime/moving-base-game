using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    
    public float movementSpeed = 5f;
    public float dodgeForce = 5000f;
    private Vector2 movement;
    private string direction;

    // components
    private Rigidbody2D rb;
    private PlayerInput playerInput;

    // inputs
    private InputAction moveAction;
    private InputAction dodgeAction;
    private InputAction attackAction;
    private InputAction interactAction;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        dodgeAction = playerInput.actions["Dodge"];
        attackAction = playerInput.actions["Attack"];
        interactAction = playerInput.actions["Interact"];
    }

    private void Start() {
        dodgeAction.performed += Dodge;
        attackAction.performed += Attack;
        interactAction.performed += Interact;
    }

    private void Update() {
        movement = moveAction.ReadValue<Vector2>();
    }

    private void FixedUpdate() {
        Move();
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
        Debug.Log("attack");
    }

    // player interaction function
    private void Interact(InputAction.CallbackContext context) {
        Debug.Log("interact");
    }

    private void rotatePlayer() {
        switch (direction) {
            case "n":
                rb.rotation = 0f;
                Debug.Log("north");
                break;
            case "ne":
                rb.rotation = -45f;
                Debug.Log("north east");
                break;
            case "nw":
                rb.rotation = 45f;
                Debug.Log("north west");
                break;
            case "s":
                rb.rotation = 180f;
                Debug.Log("south");
                break;
            case "se":
                rb.rotation = -135f;
                Debug.Log("south east");
                break;
            case "sw":
                rb.rotation = 135f;
                Debug.Log("south west");
                break;
            case "e":
                rb.rotation = -90f;
                Debug.Log("east");
                break;
            case "w":
                rb.rotation = 90f;
                Debug.Log("west");
                break;
            default:
                rb.rotation = 180f;
                Debug.Log("idle");
                break;
        }
    }

    // get direction the player is facing
    private string getDirection() {
        if (movement.y == 1) {
            if (movement.x == 1) {
                return "ne";
            }
            else if (movement.x == -1) {
                return "nw";
            }
            return "n";
        }
        else if (movement.y == -1) {
            if (movement.x == 1) {
                return "se";
            }
            else if (movement.x == -1) {
                return "sw";
            }
            return "s";
        }
        else {
            if (movement.x == 1) {
                return "e";
            }
            else if (movement.x == -1) {
                return "w";
            }
            return "";
        }
    }
}
