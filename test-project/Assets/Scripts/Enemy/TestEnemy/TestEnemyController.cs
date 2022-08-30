using UnityEngine;
using Pathfinding;

public class TestEnemyController : MonoBehaviour {
    // components
    private AIPath aiPath;
    [SerializeField] private EnemyScriptableObject stats;
    private Transform target;

    [SerializeField] private float health;
    private bool inDetection = false;

    private void Awake() {
        aiPath = GetComponent<AIPath>();
    }

    private void Start() {
        health = stats.maxHealth;
        aiPath.maxSpeed = stats.movementSpeed;
    }

    private void Update() {
        if (target != null) {
            move(target.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.name == "Character") {
            target = other.transform;
            inDetection = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.name == "Character") {
            inDetection = false;
            Invoke("undetect", 4);
        }
    }

    private void undetect() {
        if (!inDetection) target = null;
    }

    private void move(Vector3 movePosition) {
        aiPath.destination = movePosition;
    }

    public void damage(int amount) {
        health -= amount;
        Debug.Log($"Enemy damaged! Health remaining: {health}");
        if (health <= 0) {
            die();
        }
    }

    private void die() {
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        Destroy(gameObject);
        Debug.Log("Enemy died!");
    }
}
