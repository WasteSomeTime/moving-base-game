using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Camera cam;

    private Vector2 movement;
    private Vector2 mousePos;
    private string direction = "";

    // Update is called once per frame
    private void Update() {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate() {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        /* Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle; */

        string currDir = getDirection();
        if ( currDir != direction) {
            direction = currDir;
            rotatePlayer();
        }
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
