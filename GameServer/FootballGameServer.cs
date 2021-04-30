using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class FootballGameServer
{
	private TcpListener tcpListener;
	private Thread tcpListenerThread1;
	private TcpClient connectedTcpClient;

	public void Start()
	{
		tcpListenerThread1 = new Thread(new ThreadStart(ListenForIncommingRequests));
		tcpListenerThread1.IsBackground = true;

		tcpListenerThread1.Start();

	}

	private void ListenForIncommingRequests()
	{
		try
		{
			tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8052);
			tcpListener.Start();
			Debug.WriteLine("Server is listening");
			Byte[] bytes = new Byte[1024];
			while (true)
			{
				using (connectedTcpClient = tcpListener.AcceptTcpClient())
				{
					using (NetworkStream stream = connectedTcpClient.GetStream())
					{
						int length;
						while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
						{
							var incommingData = new byte[length];
							Array.Copy(bytes, 0, incommingData, 0, length);
							string clientMessage = Encoding.ASCII.GetString(incommingData);
							Debug.WriteLine("client message received as: " + clientMessage);

							if (clientMessage.StartsWith("Name:"))
							{
								SendMessage("OK_1");
							}
							if (clientMessage.StartsWith("FEL"))
							{
								SendMessage("FEL");
							}
						}
					}
				}
			}
		}
		catch (SocketException socketException)
		{
			Debug.WriteLine("SocketException " + socketException.ToString());
		}
	}


	private void SendMessage(string message)
	{
		if (connectedTcpClient == null)
		{
			return;
		}
		try
		{
			NetworkStream stream = connectedTcpClient.GetStream();
			if (stream.CanWrite)
			{
				byte[] serverMessageAsByteArray = Encoding.ASCII.GetBytes(message);
				// Write byte array to socketConnection stream.               
				stream.Write(serverMessageAsByteArray, 0, serverMessageAsByteArray.Length);
				Debug.WriteLine("Server üzenetet küld");
			}
		}
		catch (SocketException socketException)
		{
			Debug.WriteLine("Socket exception: " + socketException);
		}
	}
}