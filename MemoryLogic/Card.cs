namespace MemoryLogic
{
    public class Card
    {
        protected int mCardId;
        public int CardId {
            get {return mCardId;}
        }
        protected int mTrys;
        public int Try {
            get {return mTrys; }  
        }
        protected bool mVisibility;
        public bool Visibility {
            get { return mVisibility; }
        }
        protected bool mFinished;
        public bool Finished
        {
            get { return mFinished; }
        }
        /// <summary>
        ///     Set the values for mVisibility,mFinished,mCardId,mTrys.
        /// </summary>
        /// <param name="id">Identifier for the Card.</param>
        public Card(int id) {
            mVisibility = false;
            mCardId = id;
            mTrys = 0;
            mFinished = false;
        }
        /// <summary>
        /// If the card ist not Visible before flip it and set the tries one higher.
        /// If the card is finished or already visible do nothing.
        /// </summary>
        /// <returns>returns false if using Fails (Card already visible or finished) else returns true.</returns>
        public bool Use() {
            if(mVisibility == false && mFinished == false)
            {
                Flip();
                mTrys++;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Flip the Card front (true)/ back (false).
        /// </summary>
        private void Flip() {
            mVisibility = !mVisibility;
        }
    }
}