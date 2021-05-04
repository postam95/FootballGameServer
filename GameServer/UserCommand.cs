using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

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
        private int kickForce;

        public UserCommand()
        {
            ClientId = 0;
            Up = false;
            Down = false;
            Left = false;
            Right = false;
            Kick = false;
            KickForce = 0;
        }

        public UserCommand(int clientId, bool up, bool down, bool left, bool right, bool kick, int kickForce)
        {
            ClientId = clientId;
            Up = up;
            Down = down;
            Left = left;
            Right = right;
            Kick = kick;
            KickForce = kickForce;
        }

        public void doCommand()
        {
            GameState gameState = SingletonGameState.GetInstance().GetGameState();
            // Ha a játékos rúgni szeretne ÉS a játékos elég közel van a labdához, akkor végrehajtódik
            // a labda mozgatása.
            if (kick && isPlayerCloseEnoughToBall())
            // A paraméter meghatározza a labda mozgatásának mértékét. Az 1 a labdavezetés, a nagyobb rúgás.
            if (clientId == 1)
                doKicking(kickForce, gameState.PictureHomePlayer1X, gameState.PictureHomePlayer1Y);
            else
                doKicking(kickForce, gameState.PictureAwayPlayer1X, gameState.PictureAwayPlayer1Y);

            // Ha a mozgás nem fér bele a játéktérbe, akkor nem hajtódik végre a játékos mozgatása.
            if (isPlayerMovingValid())
                doPlayerMoving();

        }

        private void doPlayerMoving()
        {
            GameState gameState = SingletonGameState.GetInstance().GetGameState();
            if (clientId == 1)
            {
                gameState.PictureHomePlayer1X += 2 * Convert.ToInt32(right);
                gameState.PictureHomePlayer1X -= 2 * Convert.ToInt32(Left);
                gameState.PictureHomePlayer1Y -= 2 * Convert.ToInt32(up);
                gameState.PictureHomePlayer1Y += 2 * Convert.ToInt32(down);
            }
            else
            {
                gameState.PictureAwayPlayer1X += 2 * Convert.ToInt32(right);
                gameState.PictureAwayPlayer1X -= 2 * Convert.ToInt32(Left);
                gameState.PictureAwayPlayer1Y -= 2 * Convert.ToInt32(up);
                gameState.PictureAwayPlayer1Y += 2 * Convert.ToInt32(down);
            }
        }

        private bool isPlayerMovingValid()
        {
            GameState gameState = SingletonGameState.GetInstance().GetGameState();
            if (clientId == 1)
            {
                if (gameState.PictureHomePlayer1X + 2 * Convert.ToInt32(right) > 771)
                    return false;
                if (gameState.PictureHomePlayer1X - 2 * Convert.ToInt32(Left) < 10)
                    return false;
                if (gameState.PictureHomePlayer1Y - 2 * Convert.ToInt32(up) < 3)
                    return false;
                if (gameState.PictureHomePlayer1Y + 2 * Convert.ToInt32(down) > 506)
                    return false;
            }
            else
            {
                if (gameState.PictureAwayPlayer1X + 2 * Convert.ToInt32(right) > 771)
                    return false;
                if (gameState.PictureAwayPlayer1X - 2 * Convert.ToInt32(Left) < 10)
                    return false;
                if (gameState.PictureAwayPlayer1Y - 2 * Convert.ToInt32(up) < 3)
                    return false;
                if (gameState.PictureAwayPlayer1Y + 2 * Convert.ToInt32(down) > 506)
                    return false;
            }
            return true;
        }

        private void doKicking(int kickForce, int playerX, int playerY)
        {
            GameState gameState = SingletonGameState.GetInstance().GetGameState();
            int ballX = gameState.PictureBallX;
            int ballY = gameState.PictureBallY;
            int moveX = ballX - playerX;
            int moveY = ballY - playerY;
            double playerDistanceToBall = GamePlay.calculateDistance(playerX, playerY, ballX, ballY);
            int targetBallPositionX = ballX + (Int32)(2 * kickForce * moveX / playerDistanceToBall);
            int targetBallPositionY = ballY + (Int32)(2 * kickForce * moveY / playerDistanceToBall);

            if (kickForce == 1)
            {
                if (!GamePlay.isBallStillInGame(targetBallPositionX, targetBallPositionY))
                    return;

                gameState.PictureBallX = targetBallPositionX;
                gameState.PictureBallY = targetBallPositionY;
            }
            else
            {
                int i = 0;
                new Thread(delegate ()
                {
                    while (true)
                    {
                        targetBallPositionX = gameState.PictureBallX + (Int32)(2 * kickForce * moveX / playerDistanceToBall) / 10;
                        targetBallPositionY = gameState.PictureBallY + (Int32)(2 * kickForce * moveY / playerDistanceToBall) / 10;

                        if (!GamePlay.isBallStillInGame(targetBallPositionX, targetBallPositionY))
                            return;

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

        private bool isPlayerCloseEnoughToBall()
        {
            GameState gameState = SingletonGameState.GetInstance().GetGameState();
            double playerDistanceToBall;
            if (clientId == 1)
            {
                playerDistanceToBall = GamePlay.calculateDistance(gameState.PictureHomePlayer1X, gameState.PictureHomePlayer1Y,
                                                         gameState.PictureBallX,        gameState.PictureBallY);
                if (playerDistanceToBall > 80.0)
                    return false;
            }
            else
            {
                playerDistanceToBall = GamePlay.calculateDistance(gameState.PictureAwayPlayer1X, gameState.PictureAwayPlayer1Y,
                                                         gameState.PictureBallX,        gameState.PictureBallY);
                if (playerDistanceToBall > 80.0)
                    return false;
            }
            return true;
        }

        public bool Up { get => up; set => up = value; }
        public bool Down { get => down; set => down = value; }
        public bool Left { get => left; set => left = value; }
        public bool Right { get => right; set => right = value; }
        public bool Kick { get => kick; set => kick = value; }
        public int KickForce { get => kickForce; set => kickForce = value; }
        public int ClientId { get => clientId; set => clientId = value; }
    }
}
