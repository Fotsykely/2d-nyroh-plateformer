using UnityEngine;

[CreateAssetMenu(fileName = "PatrolEnemyData", menuName = "Nyroh/Enemies/PatrolEnemyData")]
public class PatrolEnemyData : EnemyData
{
    [Header("Patrol")]
    public float moveSpeed = 2f;
    public float patrolRange = 4f;
}
