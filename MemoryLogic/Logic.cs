using System;
using System.Collections.Generic;

namespace MemoryLogic
{
    public class Logic
    {

        private int mNumberOfPairs;
        public int NumberOfPairs
        {
            get {return mNumberOfPairs;}
        }
        private int mSizeOfPair;
        public int SizeOfPair
        {
            get { return mSizeOfPair; }
        }
        private Card[] mGamefield;
        public Card[] Gamefield {
            get { return mGamefield;}
        }
        public Logic(int numberOfPairs = 5, int sizeOfPair = 2) {
            if (numberOfPairs < 3) numberOfPairs = 3;
            if (sizeOfPair < 2) sizeOfPair = 2;
            CreateField(numberOfPairs,sizeOfPair);
        }
        private void CreateField(int numberOfPairs = 5, int sizeOfPair = 2) { 
            List<Card> CardListA = new();
            Random rand = new();
            int gameFieldSize = numberOfPairs * sizeOfPair;
            Card[] gameField = new Card[gameFieldSize];

            for (int counterNumberOfPairs = 1; counterNumberOfPairs <= numberOfPairs; counterNumberOfPairs++) {
                for (int counterPairSize = 1; counterPairSize <= sizeOfPair; counterPairSize++) CardListA.Add(new(counterNumberOfPairs));
            }

            for (int counter = 0;counter < gameField.Length; counter++){
                Card card = CardListA[rand.Next(0, CardListA.Count)];
                CardListA.Remove(card);
                gameField[counter] = card;
            }

            mNumberOfPairs = numberOfPairs;
            mSizeOfPair = sizeOfPair;
            mGamefield = gameField;
        }

        public void Turn()
        {
            throw new NotImplementedException();
        }
    }
}
