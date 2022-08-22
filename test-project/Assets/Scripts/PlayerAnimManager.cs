using UnityEngine;

public class PlayerAnimManager : MonoBehaviour {
    private string moveDirection;
    // components
    private Rigidbody2D character;

    private void Awake() {
        character = GetComponent<Rigidbody2D>();
    }
    
    public void playMove(Vector2 direction) {
        string currentDirection = getDirection(direction);
        if (currentDirection != moveDirection) {
            moveDirection = currentDirection;
            rotatePlayer();
        }
    }

    private void rotatePlayer() {
        switch (moveDirection) {
            case "n":
                character.rotation = 0f;
                break;
            case "ne":
                character.rotation = -45f;
                break;
            case "nw":
                character.rotation = 45f;
                break;
            case "s":
                character.rotation = 180f;
                break;
            case "se":
                character.rotation = -135f;
                break;
            case "sw":
                character.rotation = 135f;
                break;
            case "e":
                character.rotation = -90f;
                break;
            case "w":
                character.rotation = 90f;
                break;
            default:
                break;
        }
    }
    
    private string getDirection(Vector2 direction) {
        if (direction.y > 0) {
            if (direction.x > 0) {
                return "ne";
            }
            else if (direction.x < 0) {
                return "nw";
            }
            return "n";
        }
        else if (direction.y < 0) {
            if (direction.x > 0) {
                return "se";
            }
            else if (direction.x < 0) {
                return "sw";
            }
            return "s";
        }
        else {
            if (direction.x > 0) {
                return "e";
            }
            else if (direction.x < 0) {
                return "w";
            }
            return "";
        }
    }
}
