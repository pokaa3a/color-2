using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CardManager
{
    private const int maxNumCards = 10;

    public Card selectedCard = null;
    private List<CardHolder> cardHolders = new List<CardHolder>();
    private List<Card> cards = new List<Card>();
    private BigCardHolder bigCardHolder;

    // Singleton
    private static CardManager _instance;
    public static CardManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new CardManager();
            }
            return _instance;
        }
    }

    private CardManager()
    {
        int id = 0;
        for (int iCard = 0; iCard < maxNumCards; ++iCard)
        {
            GameObject obj = GameObject.Find(ObjectPath.card + iCard.ToString());
            CardHolder cardHolder = new CardHolder(obj, id++);
            cardHolders.Add(cardHolder);
        }

        GameObject bigCardObj = GameObject.Find(ObjectPath.bigCard);
        bigCardHolder = new BigCardHolder(bigCardObj);
        bigCardHolder.spritePath = SpritePath.Card.Big.empty;
        bigCardHolder.enabled = false;
    }

    public void RefreshHandCards()
    {
        DealCards();
        int iCard;
        for (iCard = 0; iCard < cards.Count; ++iCard)
        {
            cardHolders[iCard].InserCard(cards[iCard]);
        }
        for (; iCard < maxNumCards; ++iCard)
        {
            cardHolders[iCard].Disable();
        }
    }

    private void DealCards()
    {
        cards.Clear();

        // 0
        CardDrawColor cardRed = new CardDrawColor(Color.Red);
        cards.Add(cardRed);
        // 1
        CardDrawColor cardBlue = new CardDrawColor(Color.Blue);
        cards.Add(cardBlue);
        // 2
        CardDrawColor cardYellow = new CardDrawColor(Color.Yellow);
        cards.Add(cardYellow);
        // 3
        CardAttack cardAttack = new CardAttack();
        cards.Add(cardAttack);
        // 4
        CardAttackSameColor cardAttackSameColor = new CardAttackSameColor();
        cards.Add(cardAttackSameColor);
        // 5
        CardAttackConnected cardAttackConnected = new CardAttackConnected();
        cards.Add(cardAttackConnected);
        // 6
        CardEraseColor cardEraseColor = new CardEraseColor();
        cards.Add(cardEraseColor);
    }

    public void LongPressCard(Card card)
    {
        if (card != null)
        {
            bigCardHolder.spritePath = card.bigSpritePath;
            bigCardHolder.enabled = true;
        }
    }

    public void StopLongPressCard()
    {
        bigCardHolder.enabled = false;
    }

    public void Act(Vector2 xy)
    {
        if (selectedCard == null)
        {
            return;
        }

        if (selectedCard.Act(xy))
        {
            // actions of this card have finished
            selectedCard = null;
        }
    }
}
