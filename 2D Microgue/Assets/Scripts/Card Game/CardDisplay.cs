using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    public event System.Action<CardDisplay> OnCardClicked;
    public event System.Action<CardDisplay> OnCardDiscarded;
    public event System.Action<CardDisplay> OnCardMoved;
    public event System.Action<CardDisplay> OnEmptySpaceCreated;
    public event System.Action<CardDisplay> MovePlayerToThisCard;

    [SerializeField] CardData cardData;
    public CardData CardData => cardData;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] TextMesh textName;
    [SerializeField] TextMesh textPowerLevel;

    string cardName;
    string cardDescription;

    int cardPowerLevel;
    int cardAttackPower;

    Vector2Int cardGridPosition;
    public Vector2Int CardGridPosition { get { return cardGridPosition; } set { cardGridPosition = value; } }

    bool isPlayer = false;
    public bool IsPlayer { get { return isPlayer; } set { isPlayer = value; } }
    bool isRevealed = true;
    public bool IsRevealed => isRevealed;
    bool isInteractable = false;
    public bool IsInteractable => isInteractable;
    bool isTriggered = false;
    public bool IsTriggered => isTriggered;

    int moveDistance = 1;
    public int MoveDistance { get { return moveDistance; } set { moveDistance = value; } }

    [Header("Animation Speeds")]
    [SerializeField] float appearAnimiationSpeed  = 0.35f;
    [SerializeField] float discardAnimiationSpeed = 0.35f;
    [SerializeField] float moveAnimationSpeed     = 0.35f;

    public float AppearAnimiationSpeed => appearAnimiationSpeed;
    public float DiscardAnimationSpeed => discardAnimiationSpeed;
    public float MoveAnimationSpeed    => moveAnimationSpeed;

    public void SetCardData(CardData _cardData)
    {
        cardData = _cardData;
        UpdateCardData();
    }

    void UpdateCardData()
    {
        cardName        = cardData.CardName;
        cardDescription = cardData.CardDescription;
        cardPowerLevel  = cardData.PowerLevel;
        cardAttackPower = cardData.AttackPower;

        spriteRenderer.sprite = cardData.CardSprite;
        textName.text         = cardName;
        // textPowerLevel.text   = cardPowerLevel.ToString();

        string debugInfo = cardGridPosition.x + ", " + cardGridPosition.y;
        textPowerLevel.text   = debugInfo;
    }

    public void TweenAppear(float _delay)
    {
        transform.localScale = Vector3.zero;
        LeanTween.scale(gameObject, Vector3.one, appearAnimiationSpeed).setEaseOutElastic().setOnComplete(OnAppear).setDelay(_delay);
    }

    void OnAppear()
    {
        isInteractable = true;
    }

    void OnMouseDown() 
    {
        OnCardClicked?.Invoke(this);
    }

    public void TriggerCardActions(float _delay)
    {
        isTriggered = true;

        TweenDiscard(_delay); // TODO Point to CardData scriptable object to detemine actions

        MovePlayerToThisCard?.Invoke(this);
    }

    public void TweenDiscard(float _delay) // TODO Move this to CardData scriptable object
    {
        LeanTween.scale(gameObject, Vector3.zero, discardAnimiationSpeed).setEaseInOutBack().setOnComplete(OnDiscard).setDelay(_delay);
    }

    void OnDiscard()
    {
        OnCardDiscarded?.Invoke(this);
        gameObject.SetActive(false); // TODO Implement pooling system
    }

    public void TweenMove(Vector2 _directionToMove, Vector2Int _cardBeingReplacedGridPosition, float _delay)
    {
        Vector2Int currentPosition    = cardGridPosition;
        Vector2 directionWithDistance = _directionToMove * CardGrid.Instance.SpaceBetweenCards;
        Vector2 destinationPosition   = currentPosition + directionWithDistance;

        this.cardGridPosition = _cardBeingReplacedGridPosition; // TODO Move this into own method

        LeanTween.move(this.gameObject, destinationPosition, this.moveAnimationSpeed).setEaseInOutCubic().setOnComplete(OnMove).setDelay(_delay);

        if (TryGetAdjacentTile(currentPosition, _directionToMove, out CardDisplay adjacentCard))
        {
            adjacentCard.TweenMove(_directionToMove, currentPosition, _delay);
        }
    }

    bool TryGetAdjacentTile(Vector2Int _currentTilePosition, Vector2 _tileMovementDirection, out CardDisplay result) 
    {
        var directionOffset = _tileMovementDirection;
        directionOffset.y = -directionOffset.y;
        directionOffset.x = -directionOffset.x;
        var adjacentPosition = _currentTilePosition + directionOffset;
        var cardsOnScreen = CardGrid.Instance.CardsOnScreen;

        if (
          adjacentPosition.x >= cardsOnScreen.GetLength(0) || adjacentPosition.x < 0 ||
          adjacentPosition.y >= cardsOnScreen.GetLength(1) || adjacentPosition.y < 0
        ) {
          result = default;
          return false;
        }

        result = cardsOnScreen[Mathf.RoundToInt(adjacentPosition.x), Mathf.RoundToInt(adjacentPosition.y)];

        return true;
    }

    void OnMove()
    {
        UpdateCardData();
        OnCardMoved?.Invoke(this);
        Debug.Log(this + " has moved.");
    }

    Vector2Int GetDirection(CardDisplay _cartBeingReplaced)
    {
        Vector2Int direction = Vector2Int.zero;

        if (_cartBeingReplaced.cardGridPosition.x == cardGridPosition.x)
        {
            direction = _cartBeingReplaced.CardGridPosition.x > cardGridPosition.x ? Vector2Int.right : Vector2Int.left;
        }

        else if (_cartBeingReplaced.cardGridPosition.y == cardGridPosition.y)
        {
            direction = _cartBeingReplaced.CardGridPosition.y > cardGridPosition.y ? Vector2Int.up : Vector2Int.down;
        }

        else
        {
            throw new System.Exception("Target card position is invalid");
        }

        return direction;
    }
}
