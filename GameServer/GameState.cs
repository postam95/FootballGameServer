using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    class GameState
    {
        private int gameActualState;
        private string player1Name;
        private string player2Name;
        private int player1Value;
        private int player2value;
        private int pictureHomePlayer1X;
        private int pictureHomePlayer1Y;
        private int pcictureHomeGoalKeeperX;
        private int pcictureHomeGoalKeeperY;
        private int pictureAwayPlayer1X;
        private int pictureAwayPlayer1Y;
        private int pictureAwayGoalKeeperX;
        private int pictureAwayGoalKeeperY;
        private int pictureBallX;
        private int pictureBallY;


        public GameState()
        {
            this.GameActualState = 0;
            this.Player1Name = "Player1";
            this.Player2Name = "Player2";
            this.Player1Value = 0;
            this.Player2value = 0;
            this.PictureHomePlayer1X = 326;
            this.PictureHomePlayer1Y = 249;
            this.PcictureHomeGoalKeeperX = 70;
            this.PcictureHomeGoalKeeperY = 249;
            this.PictureAwayPlayer1X = 444;
            this.PictureAwayPlayer1Y = 249;
            this.PictureAwayGoalKeeperX = 708;
            this.PictureAwayGoalKeeperY = 249;
            this.PictureBallX = 397;
            this.PictureBallY = 261;
        }

        public GameState(int gameActualState, string player1Name, string player2Name, int player1Value, int player2value, int pictureHomePlayer1X, int pictureHomePlayer1Y, int pcictureHomeGoalKeeperX, int pcictureHomeGoalKeeperY, int pictureAwayPlayer1X, int pictureAwayPlayer1Y, int pictureAwayGoalKeeperX, int pictureAwayGoalKeeperY, int pictureBallX, int pictureBallY)
        {
            this.GameActualState = gameActualState;
            this.Player1Name = player1Name;
            this.Player2Name = player2Name;
            this.Player1Value = player1Value;
            this.Player2value = player2value;
            this.PictureHomePlayer1X = pictureHomePlayer1X;
            this.PictureHomePlayer1Y = pictureHomePlayer1Y;
            this.PcictureHomeGoalKeeperX = pcictureHomeGoalKeeperX;
            this.PcictureHomeGoalKeeperY = pcictureHomeGoalKeeperY;
            this.PictureAwayPlayer1X = pictureAwayPlayer1X;
            this.PictureAwayPlayer1Y = pictureAwayPlayer1Y;
            this.PictureAwayGoalKeeperX = pictureAwayGoalKeeperX;
            this.PictureAwayGoalKeeperY = pictureAwayGoalKeeperY;
            this.PictureBallX = pictureBallX;
            this.PictureBallY = pictureBallY;
        }

        public int GameActualState { get => gameActualState; set => gameActualState = value; }
        public string Player1Name { get => player1Name; set => player1Name = value; }
        public string Player2Name { get => player2Name; set => player2Name = value; }
        public int Player1Value { get => player1Value; set => player1Value = value; }
        public int Player2value { get => player2value; set => player2value = value; }
        public int PictureHomePlayer1X { get => pictureHomePlayer1X; set => pictureHomePlayer1X = value; }
        public int PictureHomePlayer1Y { get => pictureHomePlayer1Y; set => pictureHomePlayer1Y = value; }
        public int PcictureHomeGoalKeeperX { get => pcictureHomeGoalKeeperX; set => pcictureHomeGoalKeeperX = value; }
        public int PcictureHomeGoalKeeperY { get => pcictureHomeGoalKeeperY; set => pcictureHomeGoalKeeperY = value; }
        public int PictureAwayPlayer1X { get => pictureAwayPlayer1X; set => pictureAwayPlayer1X = value; }
        public int PictureAwayPlayer1Y { get => pictureAwayPlayer1Y; set => pictureAwayPlayer1Y = value; }
        public int PictureAwayGoalKeeperX { get => pictureAwayGoalKeeperX; set => pictureAwayGoalKeeperX = value; }
        public int PictureAwayGoalKeeperY { get => pictureAwayGoalKeeperY; set => pictureAwayGoalKeeperY = value; }
        public int PictureBallX { get => pictureBallX; set => pictureBallX = value; }
        public int PictureBallY { get => pictureBallY; set => pictureBallY = value; }
    }
}
