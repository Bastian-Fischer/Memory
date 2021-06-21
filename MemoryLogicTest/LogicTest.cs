using Microsoft.VisualStudio.TestTools.UnitTesting;
using MemoryLogic;
using System;

namespace MemoryLogicTest
{
    [TestClass]
    public class LogicTest
    {

        /* Ablauf 1-Spieler.
         * [TODO]
         * Test ob das Game verloren ist nach einem GameOver-Zug.
         * Test ob das Game gewonnen ist nach einem Zug.
         * Test ob nach Game Win oder Lose keine weiteren Züge möglich sind.
         * [Optional] 
         * Wenn Max Zeit der Runde ereicht und nicht 2 Karten aufgedeckt, ausgewählte Karten umdrehen und ein Leben abzug.
         * Rundenbasiert mit Zeit???
         * Ende des Spiels Speichern mit Punkteanzeige
         * Wenn max Karten pro Runde vor Zeitablauf aufgedeckt dann Prozentuale Bonuspunkte für den Zug
         * Wenn alle aufgedeckten Karten gleich, dann ein Punkteverteilung und Karten aufgedeckt lassen.
         * [Erledigt]
         * Am Anfang jeder Runde nur die schon aufgedeckten Pärchen aufgedeckt anzeigen.
         * Wenn 1 Karte ausgewählt, Karte 1 aufdecken.
         * Wenn alle aufgedeckten Karten ungleich Karten verdecken.
         * Leben nicht unter 1 einstellbar
         * Gewonnen wenn alle Karten aufgedeckt.
         * Spieler hat verloren wenn alle Leben weg.
         * Zufällige Kartenanordnung
         * Wenn 2 Karte ausgewählt, Karte 2 sichtbar machen und prüfen ob Karte 1 = Karte 2.
         * Wenn eine Karte ein zweites mahl oder öfter, aufgedeckt wird ohne erfolgreichen Zug, dann abzug eines Lebens
         * Neues Spiel starten.
         * Nicht weniger als 3 Paare.
         * Pargröße nicht kleiner als 2.
         * Anfangs alle Karten verdeckt.
         * Anfangs keine Karte auf finish.
         * Anzahl der Karten pro Paar korrekt
         * Runde zuende nach erfolgreichen aufdecken.
         * Rundenbasiertes aufdecken.
         * Anzahl der Karten festlegen.
         * Spielfeld erzeugen.
         */

        [TestMethod]
        public void LiveNotUnderOneOnStart()
        {
            Logic l = new(5,2,0);
            Assert.IsTrue(l.Live > 0);
        }
        [TestMethod]
        public void LiveDeductionAfterUseCardAgain() {
            Logic l = new(10,2,5);
            int CardPositionA = 0;
            int CardPositionB = 0;
            int CardPositionC = 0;
            for (int counter = 0; counter < l.Gamefield.Length; counter++)
            {
                if (l.Gamefield[counter].CardId == 1)
                {
                    if (l.Gamefield[counter].CardId != l.Gamefield[CardPositionA].CardId && CardPositionB == 0)
                    {
                        CardPositionB = counter;
                    }
                    else if (l.Gamefield[counter].CardId != l.Gamefield[CardPositionA].CardId && l.Gamefield[counter].CardId != l.Gamefield[CardPositionB].CardId)
                    {
                        CardPositionC = counter;
                    }
                }
            }
            l.TurnStep(CardPositionA);
            l.TurnStep(CardPositionB);
            l.TurnStep(CardPositionB);
            l.TurnStep(CardPositionC);
            Assert.IsTrue(l.Live < 5);
        }
        [TestMethod]
        public void PairVisibleAfterTurn() {
            Logic l = new(3,2);
            int CardPositionA = -1;
            int CardPositionB = -1;
            for (int counter = 0; counter < l.Gamefield.Length; counter++)
            {
                if (l.Gamefield[counter].CardId == 1) {
                    if (CardPositionA == -1) { 
                        CardPositionA = counter; 
                    } else {
                        CardPositionB = counter; 
                    }
                } 
            }
            l.TurnStep(CardPositionA);
            l.TurnStep(CardPositionB);
            Assert.IsTrue(l.Gamefield[CardPositionA].Visibility);
            Assert.IsTrue(l.Gamefield[CardPositionB].Visibility);
        }
        [TestMethod]
        public void PairFinishedAfterTurn()
        {
            Logic l = new(3, 2);
            int CardPositionA = -1;
            int CardPositionB = -1;
            for (int counter = 0; counter < l.Gamefield.Length; counter++)
            {
                if (l.Gamefield[counter].CardId == 1)
                {
                    if (CardPositionA == -1)
                    {
                        CardPositionA = counter;
                    }
                    else
                    {
                        CardPositionB = counter;
                    }
                }
            }
            l.TurnStep(CardPositionA);
            l.TurnStep(CardPositionB);
            Assert.IsTrue(l.Gamefield[CardPositionA].Finished);
            Assert.IsTrue(l.Gamefield[CardPositionB].Finished);
        }
        [TestMethod]
        public void TurnResultAfterFinishedIsPairFinished()
        {
            Logic l = new(3, 2);
            int CardPositionA = -1;
            int CardPositionB = -1;
            for (int counter = 0; counter < l.Gamefield.Length; counter++)
            {
                if (l.Gamefield[counter].CardId == 1)
                {
                    if (CardPositionA == -1)
                    {
                        CardPositionA = counter;
                    }
                    else
                    {
                        CardPositionB = counter;
                    }
                }
            }
            l.TurnStep(CardPositionA);
            Assert.IsTrue(TurnResult.PairFinished == l.TurnStep(CardPositionB));            
        }
        [TestMethod]
        public void TurnResultEndWithTurnFinished()
        {
            Logic l = new(3, 2);
            int CardPositionA = 0;
            int CardPositionB = 1;
            for (int counter = 0; counter < l.Gamefield.Length; counter++)
            {
                if (l.Gamefield[counter].CardId == 1)
                {
                    if (l.Gamefield[counter].CardId != l.Gamefield[CardPositionA].CardId)
                    {
                        CardPositionB = counter;
                    }
                }
            }
            l.TurnStep(CardPositionA);
            Assert.IsTrue(TurnResult.TurnFinished == l.TurnStep(CardPositionB));
        }
        [TestMethod]
        public void Createable()
        {
            Logic l = new();
            Assert.IsNotNull(l);
        }
        [TestMethod]
        public void GamefieldHasRightSize() {
            int pairs = 5;
            int pairSize = 2;
            Logic l = new(pairs, pairSize);
            Assert.IsTrue(l.Gamefield.Length == pairs * pairSize);
        }
        [TestMethod]
        public void NumberOfPairsNotUnder3()
        {
            int pairs = 2;
            int pairSize = 2;
            Logic l = new(pairs, pairSize);
            Assert.IsTrue(l.NumberOfPairs > 2);
        }
        [TestMethod]
        public void PairSizeNotUnder2()
        {
            int pairs = 5;
            int pairSize = 1;
            Logic l = new(pairs, pairSize);
            Assert.IsTrue(l.SizeOfPair > 1);
        }
        [TestMethod]
        public void OnStartAllCardsNotVisible()
        {
            int pairs = 5;
            int pairSize = 1;
            Logic l = new(pairs, pairSize);
            foreach (var item in l.Gamefield)
            {
                Assert.IsTrue(!item.Visibility);
            }
        }
        [TestMethod]
        public void OnStartAllCardsNotFinish()
        {
            int pairs = 5;
            int pairSize = 1;
            Logic l = new(pairs, pairSize);
            foreach (var item in l.Gamefield)
            {
                Assert.IsTrue(!item.Finished);
            }
        }
        [TestMethod]
        public void NumberOfCardsPerPair()
        {
            int trys = 3;
            int pairs;
            int pairSize;
            int cardPerPairCounter;
            int pairId;
            Random rand = new();
            for (int counterTry = 0; counterTry < trys; counterTry++)
            {
                cardPerPairCounter = 0;
                pairs = rand.Next(3,10);
                pairSize = rand.Next(2,6);
                pairId = rand.Next(1, pairs + 1);
                Logic l = new(pairs, pairSize);
                foreach (var item in l.Gamefield)
                {
                    if (item.CardId == pairId) {
                        cardPerPairCounter++;
                    }
                    
                }
                Assert.IsTrue(cardPerPairCounter == pairSize);
            }   
        }
        [TestMethod]
        public void GamefieldIsRandomized() {
            int divisor = 2;
            int pairs = 5;
            int pairSize = 2;
            int trys = 10;
            int directPairCount = 0;
            int lastCardId;
            for (int count = 0; count < trys; count++)
            {
                lastCardId = -1;
                Logic l = new(pairs, pairSize);
                foreach (var card in l.Gamefield)
                {
                    if (card.CardId == lastCardId) directPairCount++;
                    lastCardId = card.CardId;
                }
            }
            
            Assert.IsTrue(directPairCount < (trys * pairs)/divisor);
        }
        [TestMethod]
        public void AfterRoundAllFinishedCardsAreVisible() {
            Logic l = new(3, 2);
            int CardPositionA = -1;
            int CardPositionB = -1;
            int CardPositionC = -1;
            int CardPositionD = -1;
            for (int counter = 0; counter < l.Gamefield.Length; counter++)
            {
                if (l.Gamefield[counter].CardId == 1)
                {
                    if (CardPositionA == -1)
                    {
                        CardPositionA = counter;
                    }
                    else
                    {
                        CardPositionB = counter;
                    }
                }
                if (l.Gamefield[counter].CardId == 2)
                {
                    if (CardPositionC == -1)
                    {
                        CardPositionC = counter;
                    }
                    else
                    {
                        CardPositionD = counter;
                    }
                }
            }
            l.TurnStep(CardPositionA);
            l.TurnStep(CardPositionB);
            l.TurnStep(CardPositionC);
            l.TurnStep(CardPositionD);

            for (int counter = 0; counter < l.Gamefield.Length; counter++)
            {
                if (counter == CardPositionA || counter == CardPositionB ||  
                    counter == CardPositionC || counter == CardPositionD )
                    Assert.IsTrue(l.Gamefield[counter].Visibility);
                else
                    Assert.IsTrue(!l.Gamefield[counter].Visibility);
            }
        }
    }
}
