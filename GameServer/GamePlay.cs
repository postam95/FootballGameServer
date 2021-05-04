using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    class GamePlay
    {
        public static bool isBallMovingValid(int positionX, int positionY)
        {
            if (positionX > 747)
                return false;
            if (positionX < 53)
                return false;
            if (positionY < 45)
                return false;
            if (positionY > 485)
                return false;
            return true;
        }

        public static double calculateDistance(double firstX, double firstY, double secondX, double secondY)
        {
            double moveX = firstX - secondX;
            double moveY = firstY - secondY;
            double distance = Math.Sqrt(Math.Pow(moveX, 2) + Math.Pow(moveY, 2));
            return distance;
        }

        public static bool isHomeGoal(int ballPositionX, int ballPositionY)
        {
            if (ballPositionX < 48 && ballPositionY > 206 && ballPositionY < 329)
                return true;
            return false;
        }

        public static bool isAwayGoal(int ballPositionX, int ballPositionY)
        {
            if (ballPositionX > 752 && ballPositionY > 206 && ballPositionY < 329)
                return true;
            return false;
        }

        public static void scoreHandler(int goal)
        {
            GameState gameState = SingletonGameState.GetInstance().GetGameState();
            if (goal == 2)
            {
                gameState.Player1Value++;
            }
            else
            {
                gameState.Player2value++;
            }
            setInitialPosition();
        }

        public static bool isBallStillInGame(int targetBallPositionX, int targetBallPositionY)
        {
            if (GamePlay.isHomeGoal(targetBallPositionX, targetBallPositionY))
            {
                GamePlay.scoreHandler(1);
                return false;
            }
            if (GamePlay.isAwayGoal(targetBallPositionX, targetBallPositionY))
            {
                GamePlay.scoreHandler(2);
                return false;
            }
            if (!GamePlay.isBallMovingValid(targetBallPositionX, targetBallPositionY))
            {
                return false;
            }
            return true;
        }

        public static void setInitialPosition()
        {
            GameState gameState = SingletonGameState.GetInstance().GetGameState();
            gameState.PictureHomePlayer1X = 326;
            gameState.PictureHomePlayer1Y = 249;
            gameState.PcictureHomeGoalKeeperX = 70;
            gameState.PcictureHomeGoalKeeperY = 249;
            gameState.PictureAwayPlayer1X = 444;
            gameState.PictureAwayPlayer1Y = 249;
            gameState.PictureAwayGoalKeeperX = 708;
            gameState.PictureAwayGoalKeeperY = 249;
            gameState.PictureBallX = 397;
            gameState.PictureBallY = 261;
        }
    }
}
