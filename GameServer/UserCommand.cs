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
            this.ClientId = 0;
            this.Up = false;
            this.Down = false;
            this.Left = false;
            this.Right = false;
            this.Kick = false;
            this.KickForce = 0;
        }

        public UserCommand(int clientId, bool up, bool down, bool left, bool right, bool kick, int kickForce)
        {
            this.clientId = clientId;
            this.up = up;
            this.down = down;
            this.left = left;
            this.right = right;
            this.kick = kick;
            this.kickForce = kickForce;
        }

        public void doCommand()
        {
            // Ha a játékos rúgni szeretne ÉS a játékos elég közel van a labdához, akkor végrehajtódik
            // a labda mozgatása.
            if (kick && isPlayerCloseEnoughToBall())
            // A paraméter meghatározza a labda mozgatásának mértékét. Az 1 a labdavezetés, a nagyobb rúgás.
                doKicking(kickForce);

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

        private void doKicking(int kickForce)
        {
            GameState gameState = SingletonGameState.GetInstance().GetGameState();

            if (clientId == 1)
            {
                double moveX = gameState.PictureBallX - gameState.PictureHomePlayer1X;
                double moveY = gameState.PictureBallY - gameState.PictureHomePlayer1Y;
                double playerDistanceToBall = calculateDistance(gameState.PictureHomePlayer1X, gameState.PictureHomePlayer1Y,
                                                         gameState.PictureBallX, gameState.PictureBallY);
                int targetBallPositionX = gameState.PictureBallX + (Int32)(2 * kickForce * moveX / playerDistanceToBall);
                int targetBallPositionY = gameState.PictureBallY + (Int32)(2 * kickForce * moveY / playerDistanceToBall);
            
        
                
                    if (kickForce == 1)
                    {
                        if (isHomeGoal(targetBallPositionX, targetBallPositionY))
                        {
                            scoreHandler(1);
                            return;
                        }
                        if (isAwayGoal(targetBallPositionX, targetBallPositionY))
                        {
                            scoreHandler(2);
                            return;
                        }
                        if (!isBallMovingValid(gameState.PictureBallX, gameState.PictureBallY))
                        {
                            return;
                        }

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

                                if (isHomeGoal(targetBallPositionX, targetBallPositionY))
                                {
                                    scoreHandler(1);
                                    return;
                                }
                                if (isAwayGoal(targetBallPositionX, targetBallPositionY))
                                {
                                    scoreHandler(2);
                                    return;
                                }
                                if (!isBallMovingValid(gameState.PictureBallX, gameState.PictureBallY))
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
            else
            {
                double moveX = gameState.PictureBallX - gameState.PictureAwayPlayer1X;
                double moveY = gameState.PictureBallY - gameState.PictureAwayPlayer1Y;
                double playerDistanceToBall = calculateDistance(gameState.PictureAwayPlayer1X, gameState.PictureAwayPlayer1Y,
                                                         gameState.PictureBallX, gameState.PictureBallY);
                int targetBallPositionX = gameState.PictureBallX + (Int32)(2 * kickForce * moveX / playerDistanceToBall);
                int targetBallPositionY = gameState.PictureBallY + (Int32)(2 * kickForce * moveY / playerDistanceToBall);
            
                
                    if (kickForce == 1)
                    {

                        if (isHomeGoal(targetBallPositionX, targetBallPositionY))
                        {
                            scoreHandler(1);
                            return;
                        }
                        if (isAwayGoal(targetBallPositionX, targetBallPositionY))
                        {
                            scoreHandler(2);
                            return;
                        }
                        if (!isBallMovingValid(gameState.PictureBallX, gameState.PictureBallY))
                        {
                            return;
                        }

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

                                if (isHomeGoal(targetBallPositionX, targetBallPositionY))
                                {
                                    scoreHandler(1);
                                    return;
                                }
                                if (isAwayGoal(targetBallPositionX, targetBallPositionY))
                                {
                                    scoreHandler(2);
                                    return;
                                }
                                if (!isBallMovingValid(gameState.PictureBallX, gameState.PictureBallY))
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
        }

        private bool isBallMovingValid(int positionX, int positionY)
        {
            if (positionX > 755)
                return false;
            if (positionX < 45)
                return false;
            if (positionY < 37)
                return false;
            if (positionY > 493)
                return false;
            return true;
        }

        private bool isHomeGoal(int ballPositionX, int ballPositionY)
        {
            if (ballPositionX < 35 && ballPositionY > 206 && ballPositionY < 329)
                return true;
            return false;
        }

        private bool isAwayGoal(int ballPositionX, int ballPositionY)
        {
            if (ballPositionX > 765 && ballPositionY > 206 && ballPositionY < 329)
                return true;
            return false;
        }

        private void scoreHandler(int goal)
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

        private void setInitialPosition()
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

        private bool isPlayerCloseEnoughToBall()
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
                playerDistanceToBall = calculateDistance(gameState.PictureAwayPlayer1X, gameState.PictureAwayPlayer1Y,
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
        public int KickForce { get => kickForce; set => kickForce = value; }
        public int ClientId { get => clientId; set => clientId = value; }
    }
}
