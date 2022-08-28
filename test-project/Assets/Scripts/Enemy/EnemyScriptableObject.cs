using UnityEngine;

[CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "ScriptableObjects/EnemyScriptableObject")]
public class EnemyScriptableObject : ScriptableObject {
    [field: SerializeField] public float maxHealth {get; private set;} = 100f;
    [field: SerializeField] public float movementSpeed {get; private set;} = 100f;
    [field: SerializeField] public float attackDamage {get; private set;} = 10f;
    [field: SerializeField] public float detectionRange {get; private set;} = 10f;
}
