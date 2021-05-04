
using GameServer;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;

public class FootballGameServer
{

	private List<TcpClient> listConnectedClients = new List<TcpClient>(new TcpClient[0]);
	private TcpListener tcpListener;
	private Thread tcpListenerThread;
	private Thread sendStateThread;
	private TcpClient connectedTcpClient;
	private GameState gameState = SingletonGameState.GetInstance().GetGameState();
	private GoalKeeper homeGoalKeeper = new GoalKeeper(1, 70, 249, 128, 409, 160, 47);
	private GoalKeeper awayGoalKeeper = new GoalKeeper(2, 708, 249, 128, 409, 767 , 654);

	private int connectedClient = 0;

	public void Start()
	{
		tcpListenerThread = new Thread(new ThreadStart(ListenForIncommingRequests));
		tcpListenerThread.IsBackground = true;
		tcpListenerThread.Start();

		sendStateThread = new Thread(new ThreadStart(sendGameState));
		//ndStateThread.Start();
	}

	private void sendGameState()
    {
        while(true)
		{
			if (gameState.GameActualState == 1)
			{
				SendMessage(ListConnectedClients[0], JsonConvert.SerializeObject(gameState));
			}
			if (gameState.GameActualState == 2)
			{
				SendMessage(ListConnectedClients[0], JsonConvert.SerializeObject(gameState));
				SendMessage(ListConnectedClients[1], JsonConvert.SerializeObject(gameState));
			}

			//Thread.Sleep(20);
		}

    }

	private void ListenForIncommingRequests()
	{
		tcpListener = new TcpListener(IPAddress.Any, 8052);
		tcpListener.Start();
		ThreadPool.QueueUserWorkItem(this.ListenerWorker, null);
	}
	private void ListenerWorker(object token)
	{
		while (tcpListener != null)
		{
			connectedTcpClient = tcpListener.AcceptTcpClient();
	
			ListConnectedClients.Add(connectedTcpClient);
			ThreadPool.QueueUserWorkItem(this.HandleClientWorker, connectedTcpClient);
		}
	}

	private void HandleClientWorker(object token)
	{
		int actClient;

		Byte[] bytes = new Byte[1024];
		UserCommand command = null;
		using (var client = token as TcpClient)
		using (var stream = client.GetStream())
		{
			Console.WriteLine("Client connected successfully!");			
			actClient = ++connectedClient;
			int length;                   
			while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
			{
				stream.WriteTimeout = 500;

				var incommingData = new byte[length];
				Array.Copy(bytes, 0, incommingData, 0, length);                    
				string clientMessage = Encoding.ASCII.GetString(incommingData);
				Console.WriteLine(clientMessage);

				if (clientMessage.StartsWith("Name:"))
                {
					if (gameState.GameActualState == 0)
					{
						gameState.GameActualState = 1;
						gameState.Player1Name = clientMessage.Substring(5);				
                    }
                    else
                    {
						gameState.GameActualState = 2;
						gameState.Player2Name = clientMessage.Substring(5);
					}	
				}
				else{

					command = JsonConvert.DeserializeObject<UserCommand>(clientMessage);
					Console.WriteLine("Client: " + actClient);

					command.doCommand();
					homeGoalKeeper.moveGoalKeeper();
					homeGoalKeeper.updatePositionInGameState();
					awayGoalKeeper.moveGoalKeeper();
					awayGoalKeeper.updatePositionInGameState();
				}

				Thread.Sleep(25);
				SendMessage(token, JsonConvert.SerializeObject(gameState));

			}
			if (connectedTcpClient == null)
			{
				return;
			}
		}
	}
	public void SendMessage(object token, string msg)
	{
		
		if (connectedTcpClient == null)
		{
			return;
		}
		var client = token as TcpClient;
		{
			try
			{
				NetworkStream stream = client.GetStream();
				if (stream.CanWrite)
				{
					stream.WriteTimeout = 500;
					byte[] serverMessageAsByteArray = Encoding.ASCII.GetBytes(msg);           
					stream.Write(serverMessageAsByteArray, 0, serverMessageAsByteArray.Length);
					Console.WriteLine("Szerver üzenet küldés: " + serverMessageAsByteArray);
					stream.Flush();
				}
			}
			catch (SocketException socketException)
			{
				Console.WriteLine("Socket hiba: " + socketException);
				return;
			}
		}
	}
	

	public List<TcpClient> ListConnectedClients { get => listConnectedClients; set => listConnectedClients = value; }

}