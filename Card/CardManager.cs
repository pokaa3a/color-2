using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CardManager
{
    private const int maxNumCards = 5;

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

        CardDrawColor cardRed = new CardDrawColor(Color.Red);
        cards.Add(cardRed);
        CardDrawColor cardBlue = new CardDrawColor(Color.Blue);
        cards.Add(cardBlue);
        CardDrawColor cardYellow = new CardDrawColor(Color.Yellow);
        cards.Add(cardYellow);
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
}
