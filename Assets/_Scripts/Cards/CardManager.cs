using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    
    [SerializeField] private List<Card> _cards;
    [SerializeField] private Card _selectedCard;

    private void Awake()
    {
        foreach (Card card in _cards)
        {
            card.Initialize();
        }
    }

    public void SelectCard(Card pCard)
    {
        _selectedCard = pCard;
    }

    public void CheckPatternCurrentCard(Tile pSelectedTile, Tile[] pNeighbours)
    {
        HexagonalPatternChecker.CheckPattern(_selectedCard.patternData.patternType, pSelectedTile, pNeighbours, _selectedCard.patternData);
    }
}
