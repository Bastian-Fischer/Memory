using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MemoryLogicTest
{
    [TestClass]
    public class LogicTest
    {

        /* Ablaufplan 1-Spieler.
         * ______________________________
         * Neues Spiel starten.
         * Anzahl der Karten festlegen.
         * Rundenbasiert mit Zeit???
         * Spielfeld erzeugen.
         * Anfangs alle Karten verdeckt.
         * Rundenbasiertes aufdecken.
         * In jeder Runde nur die schon aufgedeckten P�rchen anzeigen.
         * Wenn 1 Karte ausgew�hlt, Karte 1 aufdecken.
         * Wenn 2 Karte ausgew�hlt, Karte 2 sichtbar machen und pr�fen ob Karte 1 = Karte 2.
         * Wenn Karte 1 oder 2 schon einmal Aufgedeckt wurde, dann abzug eines Lebens
         * Wenn Karte 1 u. 2 gleich, dann ein Punkt und Karten aufgedeckt lassen.
         * Wenn Karte 1 u. 2 ungleich beide Karten verdecken.
         * Wenn Max Zeit der Runde ereicht und nicht 2 Karten aufgedeckt, ausgew�hlte Karten umdrehen und ein Leben abzug.
         * Wenn 2 Karten vor Zeitablauf aufgedeckt dann Prozentuale Bonuspunkte f�r den Zug
         * Gewonnen wenn alle Karten aufgedeckt.
         * 
         * Spieler hat verloren wenn alle Leben weg.
         * Ende des Spiels Speichern mit Punkteanzeige
         */




        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
