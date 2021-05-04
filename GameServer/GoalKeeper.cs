using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

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
            this.id = id;
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
                double playerDistanceToBall = GamePlay.calculateDistance(positionX, positionY, gameState.PictureBallX, gameState.PictureBallY);
                if (playerDistanceToBall > 10)
                {
                    positionX += (Int32)(2 * moveX / playerDistanceToBall);
                    positionY += (Int32)(2 * moveY / playerDistanceToBall);
                }
                else
                {
                    doKicking(40);
                }
            }
            else
            {
                if (this.id == 1)
                {
                    positionX = 70;
                    positionY = 249;
                }
                else
                {
                    positionX = 708;
                    positionY = 249;
                }
            }
        }

        private void doKicking(int kickForce)
        {
            GameState gameState = SingletonGameState.GetInstance().GetGameState();
            double moveX = gameState.PictureBallX - positionX;
            double moveY = gameState.PictureBallY - positionY;
            double playerDistanceToBall = GamePlay.calculateDistance(positionX, positionY, gameState.PictureBallX, gameState.PictureBallY);
            int targetBallPositionX = gameState.PictureBallX + (Int32)(2 * kickForce * moveX / playerDistanceToBall);
            int targetBallPositionY = gameState.PictureBallY + (Int32)(2 * kickForce * moveY / playerDistanceToBall);

            if (GamePlay.isBallMovingValid(targetBallPositionX, targetBallPositionY))
            {
                int i = 0;
                new Thread(delegate ()
                {
                    while (true)
                    {
                        targetBallPositionX = gameState.PictureBallX + (Int32)(2 * kickForce * moveX / playerDistanceToBall) / 10;
                        targetBallPositionY = gameState.PictureBallY + (Int32)(2 * kickForce * moveY / playerDistanceToBall) / 10;

                        if (!GamePlay.isBallMovingValid(gameState.PictureBallX, gameState.PictureBallY))
                        {
                            return;
                        }

                        gameState.PictureBallX = targetBallPositionX;
                        gameState.PictureBallY = targetBallPositionY;

                        if (i == 10)
                            return;
                        i++;
                        Thread.Sleep(50);
                    }
                }).Start();
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



        public void updatePositionInGameState()
        {
            GameState gameState = SingletonGameState.GetInstance().GetGameState();
            if (this.id == 1)
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
