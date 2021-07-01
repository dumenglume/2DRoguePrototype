using UnityEngine;

[CreateAssetMenu(fileName = "New CardData", menuName = "CardData")]
public class CardData : ScriptableObject
{
    public enum ThisCardType { player, enemy, gold, empty }
    [SerializeField] ThisCardType cardType;
    public ThisCardType CardType { get{ return cardType; } set{ cardType = value; } }

    [SerializeField] string cardName;
    public string CardName => cardName;

    [SerializeField] string cardDescription;
    public string CardDescription => cardDescription;

    [SerializeField] Sprite cardSprite;
    public Sprite CardSprite => cardSprite;

    [SerializeField] int powerLevel;
    public int PowerLevel => powerLevel;

    [SerializeField] int attackPower;
    public int AttackPower => attackPower;

    public void SetAttackPower()
    {
        attackPower = powerLevel;
    }
}
