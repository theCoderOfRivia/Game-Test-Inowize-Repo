using UnityEngine;


[CreateAssetMenu(fileName = "Zombie Data", menuName = "Enemy/Zombie Data")]
public class EnemyData : ScriptableObject
{
    public enum EnemyType { Melee, Range }
    public EnemyType enemyType;
    public float health;
    public float damage;
    public float attackCooldown;
    public float meleeRadius;
    public float rangeRadius;
    
}
