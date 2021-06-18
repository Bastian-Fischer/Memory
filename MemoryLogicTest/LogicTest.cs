using Microsoft.VisualStudio.TestTools.UnitTesting;
using MemoryLogic;
using System;

namespace MemoryLogicTest
{
    [TestClass]
    public class LogicTest
    {

        /* Ablauf 1-Spieler.
         * ______________________________
         * # Neues Spiel starten.
         * # Anzahl der Karten festlegen.
         * Rundenbasiert mit Zeit???
         * # Spielfeld erzeugen.
         * Zufällige Kartenanordnung
         * # Nicht weniger als 3 Paare.
         * # Pargröße nicht kleiner als 2.
         * # Anfangs alle Karten verdeckt.
         * # Anfangs keine Karte auf finish.
         * # Anzahl der Karten pro Paar korrekt
         * Runde zuende nach erfolgreichen aufdecken.
         * Rundenbasiertes aufdecken.
         * Am Anfang jeder Runde nur die schon aufgedeckten Pärchen aufgedeckt anzeigen.
         * Wenn 1 Karte ausgewählt, Karte 1 aufdecken.
         * Wenn 2 Karte ausgewählt, Karte 2 sichtbar machen und prüfen ob Karte 1 = Karte 2.
         * Wenn Karte 1 oder 2 schon einmal Aufgedeckt wurde, dann abzug eines Lebens
         * Wenn Karte 1 u. 2 gleich, dann ein Punkt und Karten aufgedeckt lassen.
         * Wenn Karte 1 u. 2 ungleich beide Karten verdecken.
         * Wenn Max Zeit der Runde ereicht und nicht 2 Karten aufgedeckt, ausgewählte Karten umdrehen und ein Leben abzug.
         * Wenn 2 Karten vor Zeitablauf aufgedeckt dann Prozentuale Bonuspunkte für den Zug
         * Gewonnen wenn alle Karten aufgedeckt.
         * 
         * Spieler hat verloren wenn alle Leben weg.
         * Ende des Spiels Speichern mit Punkteanzeige
         */
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
            l.Turn(CardPositionA);
            l.Turn(CardPositionB);
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
            l.Turn(CardPositionA);
            l.Turn(CardPositionB);
            Assert.IsTrue(l.Gamefield[CardPositionA].Finished);
            Assert.IsTrue(l.Gamefield[CardPositionB].Finished);
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
    }
}
