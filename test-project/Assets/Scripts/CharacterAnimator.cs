using UnityEngine;

public class CharacterAnimator : MonoBehaviour {
    private Animator animator;
    private string currentState;

    // animation constants
    private const string IDLE = "Character_Idle";

    private const string MOVE_UP = "Character_Move_Up";
    private const string MOVE_TOP_LEFT = "Character_Move_TopLeft";
    private const string MOVE_TOP_RIGHT = "Character_Move_TopRight";
    private const string MOVE_DOWN = "Character_Move_Down";
    private const string MOVE_DOWN_LEFT = "Character_Move_DownLeft";
    private const string MOVE_DOWN_RIGHT = "Character_Move_DownRight";
    private const string MOVE_LEFT = "Character_Move_Left";
    private const string MOVE_RIGHT = "Character_Move_Right";

    private const string DODGE_UP = "Character_Dodge_Up";
    private const string DODGE_DOWN = "Character_Dodge_Down";
    private const string DODGE_LEFT = "Character_Dodge_Left";
    private const string DODGE_RIGHT = "Character_Dodge_Right";

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void ChangeAnimationState(string newState) {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }

    public void move(Vector2 direction) {
        if (direction == Vector2.zero) {
            ChangeAnimationState(IDLE);
            return;
        }
        if (direction.y > 0) {
            if (direction.x > 0) {
                ChangeAnimationState(MOVE_TOP_RIGHT);
                return;
            }
            else if (direction.x < 0) {
                ChangeAnimationState(MOVE_TOP_LEFT);
                return;
            }
            ChangeAnimationState(MOVE_UP);
            return;
        }
        if (direction.y < 0) {
            if (direction.x > 0) {
                ChangeAnimationState(MOVE_DOWN_RIGHT);
                return;
            }
            else if (direction.x < 0) {
                ChangeAnimationState(MOVE_DOWN_LEFT);
                return;
            }
            ChangeAnimationState(MOVE_DOWN);
            return;
        }
        if (direction.x > 0) {
            ChangeAnimationState(MOVE_RIGHT);
            return;
        }
        if (direction.x < 0) {
            ChangeAnimationState(MOVE_LEFT);
            return;
        }
    }

    public void dodge(Vector2 direction) {
        if (direction == Vector2.zero) {
            ChangeAnimationState(IDLE);
            return;
        }
        if (direction.x > 0) {
            ChangeAnimationState(DODGE_RIGHT);
            return;
        }
        if (direction.x < 0) {
            ChangeAnimationState(DODGE_LEFT);
            return;
        }
        if (direction.y > 0) {
            ChangeAnimationState(DODGE_UP);
            return;
        }
        if (direction.y < 0) {
            ChangeAnimationState(DODGE_DOWN);
            return;
        }
    }
}
