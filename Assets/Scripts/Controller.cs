using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using str;
using UnityEngine.EventSystems;

namespace TriPeaks
{
    static class Undo
    {
        public static List<string> nameOfGameObject = new();
        public static List<int> occurenceHierarchy = new();
        public static List<string> liveCardHistory = new();
    } 
    class Cards
    {
        private readonly string[] cards = new string[13];

        public void Insert(string[] deck)
        {
            int i = 0;
            while(i < 13)
            {
                cards[i] = deck[i++];
            }
        }

        public bool Check(string liveCard, string selectedCard)
        {
            int count = 0;
            while(count < 13)
            {
                if (cards[count] == CustomStringFunctions.NewSubString(liveCard, liveCard.LastIndexOf("_") + 1, liveCard.Length))
                    break;
                count++;
            }
            if (count > 0 && count < cards.Length - 1)
            {
                if (cards[count + 1] == CustomStringFunctions.NewSubString(selectedCard, selectedCard.LastIndexOf("_") + 1, selectedCard.Length) || 
                    cards[count - 1] == CustomStringFunctions.NewSubString(selectedCard, selectedCard.LastIndexOf("_") + 1, selectedCard.Length))
                    return true;
                else
                    return false;
            }
            else if (count == 0)
            {
                if (cards[count + 1] == CustomStringFunctions.NewSubString(selectedCard, selectedCard.LastIndexOf("_") + 1, selectedCard.Length) || 
                    cards[cards.Length - 1] == CustomStringFunctions.NewSubString(selectedCard, selectedCard.LastIndexOf("_") + 1, selectedCard.Length))
                    return true;
                else
                    return false;
            }
            else if (count == cards.Length - 1)
            {
                if (cards[0] == CustomStringFunctions.NewSubString(selectedCard, selectedCard.LastIndexOf("_") + 1, selectedCard.Length)|| 
                    cards[count - 1] == CustomStringFunctions.NewSubString(selectedCard, selectedCard.LastIndexOf("_") + 1, selectedCard.Length))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
    }

    public class Controller : MonoBehaviour
    {
        [SerializeField] List<GameObject> deck = new();
        [SerializeField] List<GameObject> pyramid = new();
        [SerializeField] List<Image> eachCardSprites = new();

        public List<Sprite> frontFace = new();
        public Sprite backFace;

        private string selectedCard, liveCard;
        readonly Cards obj = new();
        readonly System.Random _rand = new();

        private void Awake()
        {
            string[] cardNames = new string[52];
            string[] cards = { "King", "Queen", "Jack", "10", "9", "8", "7", "6", "5", "4", "3", "2", "A" };
            string[] suits = { "Clovers", "Pikes", "Hearts", "Tiles" };

            int index = 0;
            string color = "Black";
            
            obj.Insert(cards);
            
            for (int i = 0; i < 4; i+=2)
            {
                for(int j = i; j < i + 2; j++)
                {
                    for (int k = 0; k < 13; k++)
                        cardNames[index++] = "" + color + "_" + suits[j] + "_" + cards[k];
                }
                color = "Red";
            }
            
            _ = GenerateRandomListOfCards(cardNames);

            for (int i = 0; i < 52; i++)
            {
                frontFace.Add(Resources.Load<Sprite>($"{CustomStringFunctions.NewSubString(cardNames[i], 0, cardNames[i].IndexOf("_"))}\\" +
                    $"{CustomStringFunctions.NewSubString(cardNames[i], cardNames[i].IndexOf("_") + 1, cardNames[i].LastIndexOf("_"))}\\" +
                    $"{CustomStringFunctions.NewSubString(cardNames[i], cardNames[i].LastIndexOf("_") + 1, cardNames[i].Length)}"));
                if (i > 27 && i != 29)
                    eachCardSprites[i].sprite = frontFace[i];
                else
                    eachCardSprites[i].sprite = backFace;

                eachCardSprites[i].transform.name = cardNames[i];
            }
            liveCard = deck[0].name;
        }

        public string[] GenerateRandomListOfCards(string[] listToShuffle)
        {
            for (int i = listToShuffle.Length - 1; i > 0; i--)
            {
                var k = _rand.Next(i + 1);
                var value = listToShuffle[k];
                listToShuffle[k] = listToShuffle[i];
                listToShuffle[i] = value;
            }
            return listToShuffle;
        }

        private void Discard(Image img)
        {
            Undo.liveCardHistory.Add(deck[0].name);
            deck[0].GetComponent<Image>().overrideSprite = img.sprite;
            deck[0].transform.name = img.transform.name;
            liveCard = deck[0].name;
            EventSystem.current.currentSelectedGameObject.SetActive(false);
            Undo.nameOfGameObject.Add(img.name);
            Undo.occurenceHierarchy.Add(Undo.nameOfGameObject.Count);
        }

        public void OnClickEnter()
        {
            foreach (GameObject card in pyramid)
            {
                if (card.transform.name == EventSystem.current.currentSelectedGameObject.name)
                {
                    selectedCard = EventSystem.current.currentSelectedGameObject.name;
                    if (obj.Check(liveCard, selectedCard))
                    {
                        Discard(EventSystem.current.currentSelectedGameObject.GetComponent<Image>());
                    }
                    break;
                }
            }
        }

        int i = 0;
        public void DeckCardsReplace()
        {
            if (i == 24)
            {
                deck[1].SetActive(false);
                SearchForPossibleMoves();
            }
            else
            {
                Undo.liveCardHistory.Add(deck[0].name);
                deck[0].GetComponent<Image>().overrideSprite = frontFace[i+25];
                deck[0].name = eachCardSprites[i+25].name;
                liveCard = deck[0].name;
                Undo.nameOfGameObject.Add(eachCardSprites[i+25].name);
                Undo.occurenceHierarchy.Add(Undo.nameOfGameObject.Count);
                i++;
            }
        }

        private void SearchForPossibleMoves()
        {
            if (true)
            { EndGame(); }
        }

        private void EndGame()
        {
            print("EndGame");
            //gameOverMenu.SetActive(true);
        }

        public void UndoClick()
        {

        }
    }
}
