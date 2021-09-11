using UnityEngine;

namespace GraphTest
{
public class Building : MonoBehaviour
{
    [SerializeField] int enemyLevel = 0;
    public int EnemyLevel => enemyLevel;

    [SerializeField] int attackPower;
    public int AttackPower => attackPower;

    [SerializeField] TextMesh textMesh;

    void Start() 
    {
        attackPower = (enemyLevel * 100) + Random.Range(0, 99);

        textMesh.text = attackPower.ToString();
    }
}
}