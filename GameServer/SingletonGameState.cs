
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    class SingletonGameState
    {
        private static SingletonGameState instance = null;
        private static readonly object padlock = new object();
        private GameState gameState = null;

        private SingletonGameState()
        {
            gameState = new GameState();
        }

        public static SingletonGameState GetInstance()
        {
            lock (padlock)
            {
                if (SingletonGameState.instance == null)
                {
                    instance = new SingletonGameState();
                }

                return instance;
            }
        }

        public void SetGameState(GameState _gameState)
        {
            lock (padlock)
            {
                gameState = _gameState;
            }
        }

        public GameState GetGameState()
        {
            return gameState;
        }
    }
}
