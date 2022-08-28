using UnityEngine;

public class TestEnemy : MonoBehaviour {
    
    [SerializeField]
    private int maxHealth = 100;
    [SerializeField]
    private int health;

    private void Start() {
        health = maxHealth;
    }

    public  void damage(int amount) {
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
