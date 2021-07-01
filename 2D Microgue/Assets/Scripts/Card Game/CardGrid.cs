using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardGrid : MonoBehaviour
{
    static CardGrid instance;
    public static CardGrid Instance => instance;

    [SerializeField] int cardsPerColumn = 3;
    [SerializeField] int cardsPerRow    = 3;
    [SerializeField] int spaceBetweenCards = 1;
    public int SpaceBetweenCards => spaceBetweenCards;
    [SerializeField] Vector2Int playerStartGridPosition = new Vector2Int(1, 1);

    [SerializeField] GameObject cardPrefab;

    [SerializeField] CardData playerCardData;
    CardDisplay playerCard;
    bool playerExists = false;

    [SerializeField] List<CardData> cardDataDeck;
    CardDisplay[,] cardsOnScreen;
    public CardDisplay[,] CardsOnScreen => cardsOnScreen;
    List<CardData> discardDeck;

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    void Start()
    {
        cardsOnScreen = new CardDisplay[cardsPerRow, cardsPerColumn];
        CenterPlayerPosition(); // TODO Add inspector property to toggle auto centering player
        SetCameraOrgographicSize(cardsPerColumn * 2);
        CreateGrid();
    }

    void CenterPlayerPosition()
    {
        playerStartGridPosition = new Vector2Int(Mathf.FloorToInt(cardsPerRow * 0.5f), Mathf.FloorToInt(cardsPerColumn * 0.5f));
    }

    void SetCameraOrgographicSize(float _orgographicSize)
    {
        float cameraPosX = (cardsPerColumn - 1) * 0.5f;
        float cameraPosY = (cardsPerRow - 1) * 0.5f;

        Camera.main.transform.position = new Vector3(cameraPosX, cameraPosY, -10f);
        Camera.main.orthographicSize = _orgographicSize;
    }

    void CreateGrid()
    {
        for (int y = 0; y < cardsPerColumn; y++) // NOTE Change this back to 0 < carsPerColumn, etc. if wanting to start grid with 0's
        {
            for (int x = 0; x < cardsPerRow; x++)
            {
                cardsOnScreen[x, y] = CreateNewCard(x, y, Random.Range(0.0f, 1.0f)); // TODO Switch delay to random amount for each card
                // Debug.Log(cardsOnScreen[x, y]);
            }
        }
    }

    CardDisplay CreateNewCard(int _x, int _y, float _delay)
    {
        GameObject newCardObject           = Instantiate(cardPrefab, new Vector2(_x * spaceBetweenCards, _y * spaceBetweenCards), Quaternion.identity);
        newCardObject.transform.parent     = transform;

        CardDisplay newCardDisplay         = newCardObject.GetComponent<CardDisplay>();
        newCardDisplay.CardGridPosition    = new Vector2Int(_x, _y);

        newCardDisplay.OnCardClicked        += TriggerCard;
        newCardDisplay.MovePlayerToThisCard += PlayerMoveToCard;

        int playerStartX = (playerStartGridPosition.x * spaceBetweenCards); // TODO Switch to checking gridPosition rather than world position
        int playerStartY = (playerStartGridPosition.y * spaceBetweenCards);

        if (_x == playerStartX && _y == playerStartY && !playerExists) // TODO Adjust this method so this only occurs on the first call and is not computed in subsequent calls (move to Create Grid method?)
        {
            newCardDisplay.SetCardData(playerCardData); 
            playerCard = newCardDisplay; 
            newCardDisplay.IsPlayer = true;
            playerExists = true;
        }

        else
        {
            newCardDisplay.SetCardData(DrawRandomCardData());
        }

        newCardObject.name = newCardDisplay.CardData.CardName + " " + _x + ", " + _y;
        newCardDisplay.TweenAppear(_delay);

        return newCardDisplay;
    }

    CardData DrawRandomCardData()
    {
        CardData thisCard;

        if (cardDataDeck.Count == 0) { throw new System.Exception("No cards in deck."); }

        int randomCardIndex = Random.Range(0, cardDataDeck.Count);
        thisCard = cardDataDeck[randomCardIndex];
        cardDataDeck.RemoveAt(randomCardIndex);

        return thisCard;
    }

    void TriggerCard(CardDisplay _cardBeingTriggered)
    {
        if (_cardBeingTriggered.IsPlayer) { return; } // TODO Allow for turn skipping later?

        if (Vector2.Distance(_cardBeingTriggered.CardGridPosition, playerCard.CardGridPosition) != CardGrid.Instance.SpaceBetweenCards) { throw new System.Exception("Cards are not proper distance to move."); }

        _cardBeingTriggered.TriggerCardActions(0.0f); // TODO Move this to prerequisite method which will trigger player movement rather than clicks triggering movement directly
    }

    void PlayerMoveToCard(CardDisplay _cardToMoveTo) // TODO Move to a PlayerMovement script
    {
        Vector2 cardShiftDirection = GetMovementDirection(playerCard, _cardToMoveTo).normalized;

        playerCard.TweenMove(cardShiftDirection, _cardToMoveTo.CardGridPosition, playerCard.MoveAnimationSpeed); // TODO Reference playerCard animation delay
    }

    Vector2 GetMovementDirection(CardDisplay _cardToMove, CardDisplay _cardBeingReplaced)
    {
        return ((Vector2) _cardBeingReplaced.CardGridPosition - (Vector2) _cardToMove.CardGridPosition);
    }
}
