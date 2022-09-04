using UnityEngine;
using Pathfinding;
using Utility;

public class EnemyController : MonoBehaviour {
    
    private enum State {
        Idle,
        ChaseTarget,
        AttackTarget
    }
    
    private State state;
    private Vector2 startPosition, roamPosition;
    private float detectionRange = 5f;
    private float attackRange = 2f;

    // components
    private AIPath aiPath;
    private Transform target;

    private void Awake() {
        aiPath = GetComponent<AIPath>();
        target = GameObject.Find("Character").GetComponent<Transform>();
        state = State.Idle;
    }

    private void Start() {
        startPosition = transform.position;
        roamPosition  = GetRoamingPosition();
    }

    private void Update() {
        switch (state) {
            case State.Idle:
                Idle();
                break;
            case State.ChaseTarget:
                ChaseTarget();
                break;
            case State.AttackTarget:
                AttackTarget();
                break;
        }
    }

    // enemy roaming around spawnpoint
    private void Idle() {
        aiPath.destination = roamPosition;
        float reachedPositionDistance = 0.5f;
        if (Vector2.Distance(transform.position, roamPosition) < reachedPositionDistance) {
            roamPosition  = GetRoamingPosition();
        }
        FindTarget();
    }

    // follow target
    private void ChaseTarget() {
        aiPath.destination = target.position;
        if (Vector2.Distance(transform.position, target.position) > 2 * detectionRange) {
            state = State.Idle;
            return;
        }

        if (Vector2.Distance(transform.position, target.position) < attackRange) {
            state = State.AttackTarget;
            return;
        }
    }

    // attack target in range
    private void AttackTarget() {
        Debug.Log("Attacking target");
        aiPath.destination = transform.position;
        state = State.ChaseTarget;
    }

    // get a random positio around spawnpoint
    private Vector2 GetRoamingPosition() {
        return startPosition + Utils.RandomDirection() * Random.Range(2f, 6f);
    }

    // find target in detection range
    private void FindTarget() {
        if (Vector2.Distance(transform.position, target.position) < detectionRange) {
            state = State.ChaseTarget;
        }
    }

    // draw in editor on selection
    private void OnDrawGizmosSelected() {
        switch (state) {
            case State.Idle:
                Gizmos.DrawWireSphere(transform.position, detectionRange);
                break;
            case State.ChaseTarget:
                Gizmos.DrawWireSphere(transform.position, attackRange);
                break;
            case State.AttackTarget:
                Gizmos.DrawWireSphere(transform.position, attackRange);
                break;
        }
    }
}
