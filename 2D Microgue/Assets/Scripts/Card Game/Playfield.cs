using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playfield : MonoBehaviour
{
    [SerializeField] int width = 3;
    [SerializeField] int height = 3;
    [SerializeField] float spaceBetweenCards = 5f;
    [SerializeField] Vector2Int playerStartPosition = new Vector2Int(1, 1);

    CardDisplay[,] cardGrid;
    
    [SerializeField] CardData playerCard;
    [SerializeField] List<CardData> cardDeck;
    List<CardData> cardsInPlay;
    List<CardData> discardDeck;

    int[,] grid;
    bool[,] gridCheck;

    void Start()
    {
        GenerateCardGrid(width, height);
    }

    void GenerateCardGrid(int _width, int _height)
    {
        cardGrid = new CardDisplay[_width, _height];

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                CardDisplay cardToInstantiate = new CardDisplay();

                if (x == playerStartPosition.x && y == playerStartPosition.y) 
                    { cardToInstantiate.SetCardData(playerCard); }
                else 
                    { cardToInstantiate.SetCardData(DrawRandomCard(x, y)); }

                Vector3 cardPosition = new Vector3(x * spaceBetweenCards, y * spaceBetweenCards, 0f);
                cardGrid[x, y] = Instantiate(cardToInstantiate, cardPosition, Quaternion.identity);
            }
        }
    }

    CardData DrawRandomCard(int _x, int _y)
    {
        CardData thisCard;

        if (cardDeck.Count == 0) { throw new System.Exception("No cards in deck."); }

        int randomCardIndex = Random.Range(0, cardDeck.Count);
        thisCard = cardDeck[randomCardIndex];

        cardDeck.RemoveAt(randomCardIndex);

        return thisCard;
    }
}