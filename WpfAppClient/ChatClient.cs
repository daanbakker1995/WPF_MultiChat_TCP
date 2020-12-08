using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppClient
{
    class ChatClient
    {
        // Settings for server
        public int PORTNR { get; }
        public int BUFFERSIZE { get; }
        public string IPADRESS { get; }
        private string EndOfTransitionCharacter = "@-_@";

        // TcpListener for listening for clients
        private TcpClient Client { get; set; }

        // boolean holding the connection status
        private bool ConnectedToServer { get; set; }

        // Delegates/Actions to update UI
        private Action<string> AddMessageToChat;
        private Action ToggleStartButton;

        /// <summary>
        /// Contstructor
        /// </summary>
        /// <param name="PortNr"></param>
        /// <param name="BuffferSize"></param>
        /// <param name="IPAdress"></param>
        /// <param name="AddMessageToChat"></param>
        /// <param name="ToggleStartButton"></param>
        public ChatClient(int PortNr, int BuffferSize, String IPAdress, Action<String> AddMessageToChat, Action ToggleStartButton)
        {   // Variables
            PORTNR = PortNr;
            BUFFERSIZE = BuffferSize;
            IPADRESS = IPAdress;
            // Delegates
            this.AddMessageToChat = AddMessageToChat;
            this.ToggleStartButton = ToggleStartButton;
            // Server Setting
            ConnectedToServer = false;
        }

        /// <summary>
        /// Method to connect to the server
        /// </summary>
        public async void Connect()
        {
            try
            {
                using (Client = new TcpClient(IPADRESS, PORTNR))
                {
                    // send feedback to chat
                    AddMessageToChat("Verbonden!");
                    ConnectedToServer = true;
                    // Make Client receivve data from server
                    await Task.Run(() => ReceiveData());
                }
            }
            catch (Exception)
            {
                AddMessageToChat("Fout bij maken connectie.");
                ConnectedToServer = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ReceiveData()
        {
            // Using streamwriter from the TCP Server
            using (NetworkStream networkStream = Client.GetStream())
            {
                try
                {
                    while (true)
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
                            // Empty stringBuilder for new message
                            stringBuilder = new StringBuilder();
                        }
                    }
                    if (ConnectedToServer)
                    {
                        CloseConnection();
                    }
                }
                catch (Exception)
                {
                    if (ConnectedToServer)
                    {
                        // Send error to chat and close connection
                        AddMessageToChat("Onverwachte server fout");
                        CloseConnection(); 
                    }
                }
            }
        }

        /// <summary>
        /// Sends message to connected server
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(String message)
        {
            try
            {
                // get Networkstream
                NetworkStream stream = Client.GetStream();
                // Make message ready to send
                message += EndOfTransitionCharacter;
                byte[] buffer = Encoding.ASCII.GetBytes(message);
                // Write message
                stream.Write(buffer, 0, buffer.Length);
            }
            catch (Exception e)
            {
                AddMessageToChat("Fout bij versturen bericht");
            }
        }

        /// <summary>
        /// Closed connection and reset settings
        /// </summary>
        public void CloseConnection()
        {
            // inform server and close connection
            //SendMessage("bye");
            ConnectedToServer = false;
            Client.Close();
            Client.Dispose();
            // Update UI
            ToggleStartButton();
            AddMessageToChat("Connectie gesloten");
        }
    }

}

