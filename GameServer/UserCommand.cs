using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    class UserCommand
    {
        private int clientId;
        private bool up;
        private bool down;
        private bool left;
        private bool right;
        private bool kick;

        public UserCommand()
        {
            this.ClientId = 0;
            this.Up = false;
            this.Down = false;
            this.Left = false;
            this.Right = false;
            this.Kick = false;
        }

        public UserCommand(int clientId, bool up, bool down, bool left, bool right, bool kick)
        {
            this.clientId = clientId;
            this.up = up;
            this.down = down;
            this.left = left;
            this.right = right;
            this.kick = kick;
        }

        public void doCommand()
        {
            if (kick)
                doKicking();

            // Ha a mozgás nem fér bele a játéktérbe, akkor nem hajtódik végre.
            if (!isPlayerMovingValid())
                return;

            if (clientId == 1)
            {
                SingletonGameState.GetInstance().GetGameState().PictureHomePlayer1X += 2 * Convert.ToInt32(right);
                SingletonGameState.GetInstance().GetGameState().PictureHomePlayer1X -= 2 * Convert.ToInt32(Left);
                SingletonGameState.GetInstance().GetGameState().PictureHomePlayer1Y -= 2 * Convert.ToInt32(up);
                SingletonGameState.GetInstance().GetGameState().PictureHomePlayer1Y += 2 * Convert.ToInt32(down);
            } else {
                SingletonGameState.GetInstance().GetGameState().PictureAwayPlayer1X += 2 * Convert.ToInt32(right);
                SingletonGameState.GetInstance().GetGameState().PictureAwayPlayer1X -= 2 * Convert.ToInt32(Left);
                SingletonGameState.GetInstance().GetGameState().PictureAwayPlayer1Y -= 2 * Convert.ToInt32(up);
                SingletonGameState.GetInstance().GetGameState().PictureAwayPlayer1Y += 2 * Convert.ToInt32(down);
            }
        }

        private bool isPlayerMovingValid()
        {
            GameState gameState = SingletonGameState.GetInstance().GetGameState();
            if (clientId == 1)
            {
                if (gameState.PictureHomePlayer1X + 2 * Convert.ToInt32(right) > 751)
                    return false;
                if (gameState.PictureHomePlayer1X - 2 * Convert.ToInt32(Left) < 30)
                    return false;
                if (gameState.PictureHomePlayer1Y - 2 * Convert.ToInt32(up) < 23)
                    return false;
                if (gameState.PictureHomePlayer1Y + 2 * Convert.ToInt32(down) > 486)
                    return false;
            }
            else
            {
                if (gameState.PictureAwayPlayer1X + 2 * Convert.ToInt32(right) > 751)
                    return false;
                if (gameState.PictureAwayPlayer1X - 2 * Convert.ToInt32(Left) < 30)
                    return false;
                if (gameState.PictureAwayPlayer1Y - 2 * Convert.ToInt32(up) < 23)
                    return false;
                if (gameState.PictureAwayPlayer1Y + 2 * Convert.ToInt32(down) > 486)
                    return false;
            }
            return true;
        }

        private void doKicking()
        {
            GameState gameState = SingletonGameState.GetInstance().GetGameState();
            double playerDistanceToBall;

            // Ha a játékos nincs megfelelő távoláson belül a labdához, akkor a rúgás nem hajtódik végre.
            if (!isKickingValid())
                return;

            if (clientId == 1)
            {
                double moveX = gameState.PictureBallX - gameState.PictureHomePlayer1X;
                double moveY = gameState.PictureBallY - gameState.PictureHomePlayer1Y;
                playerDistanceToBall = calculateDistance(gameState.PictureHomePlayer1X, gameState.PictureHomePlayer1Y,
                                                         gameState.PictureBallX, gameState.PictureBallY);
                gameState.PictureBallX += (Int32)(2 * moveX / playerDistanceToBall);
                gameState.PictureBallY += (Int32)(2 * moveY / playerDistanceToBall);
            }
            else
            {
                double moveX = gameState.PictureBallX - gameState.PictureAwayPlayer1X;
                double moveY = gameState.PictureBallY - gameState.PictureAwayPlayer1Y;
                playerDistanceToBall = calculateDistance(gameState.PictureAwayPlayer1X, gameState.PictureAwayPlayer1Y,
                                                         gameState.PictureBallX, gameState.PictureBallY);
                gameState.PictureBallX += (Int32)(2 * moveX / playerDistanceToBall);
                gameState.PictureBallY += (Int32)(2 * moveY / playerDistanceToBall);
            }
        }

        private bool isKickingValid()
        {
            GameState gameState = SingletonGameState.GetInstance().GetGameState();
            double playerDistanceToBall;
            if (clientId == 1)
            {
                playerDistanceToBall = calculateDistance(gameState.PictureHomePlayer1X, gameState.PictureHomePlayer1Y,
                                                         gameState.PictureBallX,        gameState.PictureBallY);
                if (playerDistanceToBall > 80.0)
                    return false;
            }
            else
            {
                playerDistanceToBall = calculateDistance(gameState.PictureHomePlayer1X, gameState.PictureHomePlayer1Y,
                                                         gameState.PictureBallX,        gameState.PictureBallY);
                if (playerDistanceToBall > 80.0)
                    return false;
            }
            return true;
        }

        private double calculateDistance(double firstX, double firstY, double secondX, double secondY)
        {
            double moveX = firstX - secondX;
            double moveY = firstY - secondY;
            double distance = Math.Sqrt(Math.Pow(moveX, 2) + Math.Pow(moveY, 2));
            return distance;
        }

        public bool Up { get => up; set => up = value; }
        public bool Down { get => down; set => down = value; }
        public bool Left { get => left; set => left = value; }
        public bool Right { get => right; set => right = value; }
        public bool Kick { get => kick; set => kick = value; }
        public int ClientId { get => clientId; set => clientId = value; }
    }
}
