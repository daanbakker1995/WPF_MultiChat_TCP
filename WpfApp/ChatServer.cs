using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppServer
{
    class ChatServer
    {
        // Settings for server
        public int PORTNR { get; }
        public int BUFFERSIZE { get; }
        public string IPADRESS { get; }
        private string EndOfTransitionCharacter = "@-_@";

        // TcpListener for listening for clients
        private TcpListener TcpListener = null;

        // List of TcpCLients
        private List<TcpClient> TcpClients = new List<TcpClient>();

        // boolean holding the Server Status
        private bool ServerStarted { get; set; }

        // Delegates/Actions to update UI
        private Action<string> AddMessageToChat;
        private Action ToggleStartButton;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="PortNr"></param>
        /// <param name="BuffferSize"></param>
        /// <param name="IPAdress"></param>
        /// <param name="AddMessageToChat"></param>
        /// <param name="ToggleStartButton"></param>
        public ChatServer(int PortNr, int BuffferSize, String IPAdress, Action<String> AddMessageToChat, Action ToggleStartButton)
        {
            this.PORTNR = PortNr;
            this.BUFFERSIZE = BuffferSize;
            this.IPADRESS = IPAdress;
            this.AddMessageToChat = AddMessageToChat;
            this.ToggleStartButton = ToggleStartButton;

            ServerStarted = false;
            TcpClients = new List<TcpClient>();
        }
        
        public void StartListening()
        {
            try
            {
                ServerStarted = true;
                TcpListener = new TcpListener(IPAddress.Parse(IPADRESS), PORTNR);
                TcpListener.Start();
                AddMessageToChat("Luisteren naar chatclients...");
                TcpListener.BeginAcceptTcpClient(AcceptClients, TcpListener);
            } catch(Exception e)
            {
                AddMessageToChat($"Server Error: {e.Message}");

            }
        }

        private async void AcceptClients(IAsyncResult result)
        {
            // Do someting only if server is started.
            if (ServerStarted)
            {
                // Use the returned TCP client from the TcpListener
                using (TcpClient tcpClient = TcpListener.EndAcceptTcpClient(result))
                {
                    try
                    {
                        // Check if server is not closed
                        if (ServerStarted)
                        {
                            // Add Client to list of clients
                            TcpClients.Add(tcpClient);
                            // Listen for new client to connect
                            TcpListener.BeginAcceptTcpClient(AcceptClients, TcpListener);
                            // Receive data on net Taks
                            await Task.Run(() => ReceiveData(tcpClient));

                        }
                    }
                    catch (Exception e)
                    {
                        // Add exception to chat if server started and an exception caught
                        if (ServerStarted) AddMessageToChat("AcceptClients error:" + e.Message);
                    }
                }
            }
        }

        private void ReceiveData(TcpClient tcpClient)
        {
            // Using streamwriter from the TCP Client
            using (NetworkStream networkStream = tcpClient.GetStream())
            {
                try
                { 
                    // Send feedback to server UI
                    AddMessageToChat("Nieuwe chat deelnemer!");
                    while (networkStream != null && networkStream.CanRead)
                    {
                        // Receive data from stream
                        byte[] byteArray = new byte[BUFFERSIZE];
                        int resultSize = networkStream.Read(byteArray, 0, BUFFERSIZE);
                        string message = Encoding.ASCII.GetString(byteArray, 0, resultSize);
                        
                        // Make one message from received bytes
                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.Append(message);
                        
                        //end of Message
                        if (message.EndsWith(EndOfTransitionCharacter))
                        {
                            // Make message readable
                            string clientMessage = stringBuilder.ToString(); 
                            clientMessage = clientMessage.Remove(clientMessage.Length - EndOfTransitionCharacter.Length);
                            if (clientMessage == "bye")
                                break;
                            // Display message in chat
                            AddMessageToChat(clientMessage);
                            // Send message to other connected clients
                            BroadCast(clientMessage, tcpClient);
                            // Empty stringBuilder for new message
                            stringBuilder = new StringBuilder();
                        }
                    }
                    if (networkStream.CanRead)
                    {
                        RemoveClient(tcpClient);
                    }
                }
                catch(Exception e)
                {
                    if (networkStream.CanRead) {
                        // Send error to chat and remove client
                        AddMessageToChat($"Fout bij ontvangen bericht(en)");
                        RemoveClient(tcpClient);
                    }
                }
            }
        }

        /// <summary>
        /// Send a message to all connected clients
        /// </summary>
        /// <param name="clientMessage"></param>
        /// <param name="tcpClient"></param>
        public void BroadCast(string clientMessage, TcpClient tcpClient)
        {
            // Make message
            clientMessage += EndOfTransitionCharacter;
            // Send message to all connected clients that are not the tcpClient
            foreach (TcpClient client in TcpClients.Where(client => client != tcpClient))
            {
                var networkStream = client.GetStream();
                var buffer = Encoding.ASCII.GetBytes(clientMessage);
                networkStream.Write(buffer, 0, buffer.Length);
            }
        }

        /// <summary>
        /// End connection with a specific client
        /// </summary>
        /// <param name="client"></param>
        private void RemoveClient(TcpClient client)
        {
            AddMessageToChat("Client heeft chat verlaten");
            // Close client and remove from List
            client.Close();
            TcpClients.Remove(client);
        }

        /// <summary>
        /// Stop TcpListener, reset client list and server status
        /// </summary>
        public void CloseServer()
        {

            // Send bye to clients and stop the listener
            ServerStarted = false;
            BroadCast("bye",null);
            TcpListener.Stop();
            // Reset list
            TcpClients = new List<TcpClient>();
            // Update UI
            ToggleStartButton();
            AddMessageToChat("Verbinding gesloten..");
        }
    }
}
