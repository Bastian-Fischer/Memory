using System;
using System.Collections.Generic;

namespace MemoryLogic
{
    public class Logic
    {
        private List<ScorePoint> mScore;
        private ScorePoint mCurrentPointTyp;
        public List<ScorePoint> Score
        {
            get { return mScore; }
        }
        private int mLive;
        public int Life
        {
            get { return mLive; }
        }
        private List<Card> mTurnStack;
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
        public Logic(int numberOfPairs = 10, int sizeOfPair = 2) {
            int live = numberOfPairs;
            if (live < 1) live = 1;
            if (numberOfPairs < 3) numberOfPairs = 3;
            if (sizeOfPair < 2) sizeOfPair = 2;
            mLive = live;
            mScore = new();
            mTurnStack = new();
            CreateField(numberOfPairs,sizeOfPair);
        }
        public void SetScoreTyp(ScorePoint typ) {
            mCurrentPointTyp = typ;
        }
        public Logic(int live, int numberOfPairs = 10, int sizeOfPair = 2)
        {
            if (live < 1) live = 1;
            if (numberOfPairs < 3) numberOfPairs = 3;
            if (sizeOfPair < 2) sizeOfPair = 2;
            mLive = live;
            mScore = new();
            mTurnStack = new();
            CreateField(numberOfPairs, sizeOfPair);
            mCurrentPointTyp = ScorePoint.Point;
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
        public void AddDeath(int count) {
            mLive -= count;
        }
        public TurnResult TurnStep(int position)
        {
            //So many TurnSteps as sizeOfPair
            int lastCardId = -1;
            bool isPair = true;
            bool allCardsAreFinished = false;
            Card value = mGamefield[position];
            if (value.Use()) {
                mTurnStack.Add(value);     
            }
            else
            {
                return TurnResult.Invalid;
            }
            
            if (mLive <= 0) return TurnResult.GameLose;
            //If TurnStack same size as SizeOfPair Turn End.
            if (mTurnStack.Count == mSizeOfPair){
                for (int counter = 0;counter < mTurnStack.Count; counter++) {
                    if (counter == 0)
                        lastCardId = mTurnStack[counter].CardId;
                    else
                    { 
                        if (mTurnStack[counter].CardId != lastCardId) isPair = false;
                        lastCardId = mTurnStack[counter].CardId; 
                    }
                }
                for (int counter = 0; counter < mTurnStack.Count; counter++)
                {
                    if (isPair)
                    {
                        mTurnStack[counter].SetFinished();
                    }
                    else
                    {                      
                        mTurnStack[counter].SetInvisible();
                    }
                }
                mTurnStack.Clear();
                
                foreach (var item in mGamefield)
                {
                    if (!item.Finished) allCardsAreFinished = false;//testing if all Cards are Finished.
                }
                if(!isPair && mGamefield[position].Try >= 2)mLive--; //Live deduction if card try is over 2 trys.
                if (mLive <= 0) return TurnResult.GameLose;
                if (isPair) mScore.Add(mCurrentPointTyp);
                if (allCardsAreFinished)return TurnResult.GameWin;//Result Win if all cards are finished.
                return (isPair)? TurnResult.PairFinished : TurnResult.TurnFinished;  //Result Turn Finished or Pair Finished.
            }
            if (mLive <= 0) return TurnResult.GameLose;
            return TurnResult.Valid;
        }
    }
}
