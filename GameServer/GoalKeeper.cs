using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    class GoalKeeper
    {
        private int id;
        private int positionX;
        private int positionY;
        private int northBorder;
        private int southBorder;
        private int eastBorder;
        private int westBorder;

        public GoalKeeper(int id, int positionX, int positionY, int northBorder, int southBorder, int eastBorder, int westBorder)
        {
            this.positionX = positionX;
            this.positionY = positionY;
            this.northBorder = northBorder;
            this.southBorder = southBorder;
            this.eastBorder = eastBorder;
            this.westBorder = westBorder;
        }

        public void moveGoalKeeper()
        {
            // Ha a labda a 16-oson belül van, akkor a kapus elindul az irányába, hogy elrúgja azt.
             if (isBallCloseToGoal())
            {
                GameState gameState = SingletonGameState.GetInstance().GetGameState();
                double moveX = gameState.PictureBallX - positionX;
                double moveY = gameState.PictureBallY - positionY;
                double playerDistanceToBall = calculateDistance(positionX, positionY, gameState.PictureBallX, gameState.PictureBallY);
                if (playerDistanceToBall > 10)
                {
                    positionX += (Int32)(2 * moveX / playerDistanceToBall);
                    positionY += (Int32)(2 * moveY / playerDistanceToBall);
                }
                else
                {

                }
            }
        }

        private bool isBallCloseToGoal()
        {
            GameState gameState = SingletonGameState.GetInstance().GetGameState();
            if (gameState.PictureBallX > westBorder && gameState.PictureBallX < eastBorder
             && gameState.PictureBallY > northBorder && gameState.PictureBallY < southBorder)
                return true;
            return false;
        }

        private double calculateDistance(double firstX, double firstY, double secondX, double secondY)
        {
            double moveX = firstX - secondX;
            double moveY = firstY - secondY;
            double distance = Math.Sqrt(Math.Pow(moveX, 2) + Math.Pow(moveY, 2));
            return distance;
        }

        public void updatePositionInGameState()
        {
            GameState gameState = SingletonGameState.GetInstance().GetGameState();
            if (id == 1)
            {
                gameState.PcictureHomeGoalKeeperX = positionX;
                gameState.PcictureHomeGoalKeeperY = positionY;
            }
            else
            {
                gameState.PictureAwayGoalKeeperX = positionX;
                gameState.PictureAwayGoalKeeperY = positionY;
            }
        }

        public int Id { get => id; set => id = value; }
        public int PositionX { get => positionX; set => positionX = value; }
        public int PositionY { get => positionY; set => positionY = value; }
        public int NorthBorder { get => northBorder; set => northBorder = value; }
        public int SouthBorder { get => southBorder; set => southBorder = value; }
        public int EastBorder { get => eastBorder; set => eastBorder = value; }
        public int WestBorder { get => westBorder; set => westBorder = value; }
    }
}
