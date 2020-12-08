using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfAppServer;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ServerApp : Window
    {
        ChatServer Server;
        // Strings for butten UI
        private const String START_SERVER_TEXT = "Starten";
        private const String CLOSE_SERVER_TEXT = "Sluiten";

        // Server settings
        private bool ServerStarted;

        public ServerApp()
        {
            InitializeComponent();
            BtnStartServer.Content = START_SERVER_TEXT;
            ServerStarted = false;
        }

        /// <summary>
        /// <c>Event_Handler</c> BtnStartServer
        /// </summary>
        private void BtnStartServer_Click(object sender, RoutedEventArgs e)
        {
            if (!ServerStarted)
            {
                if (IsValidIP(InputServerIP.Text)
                    && IsPortNrValid(InputPortNumber.Text)
                    && IsValidBufferSize(InputBufferSize.Text))
                {
                    int portNr = int.Parse(InputPortNumber.Text);
                    int BufferSize = int.Parse(InputBufferSize.Text);
                    AddToChatList("Server Starten...");
                    AddToChatList("Druk op 'Sluiten' of verzend 'bye' om connectie te sluiten");
                    Server = new ChatServer(portNr, BufferSize, InputServerIP.Text,
                        (message) => AddToChatList(message), () => UpdateBtnServerStart());
                    ServerStarted = true;
                    UpdateBtnServerStart();
                    Server.StartListening();
                }
                else
                {
                    UpdateErrorDisplay("Foute gegevens, controleer probeer opnieuw");
                }
            }
            else
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// <c>Event_Handler</c> BtnSendMessage Click
        /// </summary>
        private void BtnSendMessage_Click(object sender, RoutedEventArgs e)
        {
            if (InputMessage.Text != "")
            {
                if (ServerStarted)
                {
                    String message = InputMessage.Text;
                    UpdateErrorDisplay();
                    AddToChatList(message);
                    Server.BroadCast(message, null);
                    if(message == "bye")
                    {
                        CloseConnection();
                    }
                    // Empty message field
                    InputMessage.Clear();
                    InputMessage.Focus();
                }
                else
                {
                    UpdateErrorDisplay("Start eerst de server!");
                }
            }
            else
            {
                UpdateErrorDisplay();
            }
        }

        private void CloseConnection()
        {
            AddToChatList("Verbinding sluiten...");
            // Don't switch these around this wil break Interface
            ServerStarted = false;
            Server.CloseServer();
        }

        /// <summary>
        /// Adds a message to the chatlist
        /// </summary>
        private void AddToChatList(String message)
        {
            Dispatcher.Invoke(() =>
            {
                ListBoxItem item = new ListBoxItem { Content = message };
                ChatList.Items.Add(item);
                // Scroll to item
                ChatList.ScrollIntoView(item);
            });
        }

        /// <summary>
        /// Update interface with Error, by default empty
        /// </summary>
        /// <param name="errorMessage"></param>
        private void UpdateErrorDisplay(String errorMessage = "")
        {
            ErrorTextBlock.Text = errorMessage;
        }

        /// <summary>
        /// Update the text of BtnStartServer
        /// </summary>
        private void UpdateBtnServerStart()
        {
            Dispatcher.Invoke(() =>
            {
                if (!ServerStarted)
                {
                    BtnStartServer.Content = START_SERVER_TEXT;
                }
                else
                {
                    BtnStartServer.Content = CLOSE_SERVER_TEXT;
                }
            });
        }
       
        /// <summary>
        /// Event handler for InputServerIP_KeyUp, checks if valid input else update error display
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InputServerIP_KeyUp(object sender, KeyEventArgs e)
        {
            if (!IsValidIP(InputServerIP.Text))
            {
                UpdateErrorDisplay("Ongeldig IP adres.");
            }
            else
            {
                UpdateErrorDisplay();
            }
        }

        /// <summary>
        /// Event handler for InportPortNumber_KeyUp, checks if input is valid else updates error display
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InputPortNumber_KeyUp(object sender, KeyEventArgs e)
        {
            if (IsPortNrValid(InputPortNumber.Text))
            {
                UpdateErrorDisplay();
            }
            else
            {
                UpdateErrorDisplay("Ongeldig poort nummer.");
            }
        }

        /// <summary>
        /// Event handler for InputBufferSize_KeyUp, checks if bufferSize is valid else updates error display
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InputBufferSize_KeyUp(object sender, KeyEventArgs e)
        {
            if (IsValidBufferSize(InputBufferSize.Text))
            {
                UpdateErrorDisplay();
            }
            else
            {
                UpdateErrorDisplay("Ongeldig poort nummer.");
            }
        }

        /// <summary>
        /// Checks if given string can be converted into an int and is its a valid number based on <code>IsValidPortNumber</code>
        /// </summary>
        /// <param name="portNr"></param>
        /// <returns></returns>
        private bool IsPortNrValid(String portNr)
        {
            bool Isvalid = false;
            if (portNr != "")
            {
                if (int.TryParse(portNr, out _))
                {
                    if (IsValidPortNumber(int.Parse(portNr)))
                    {
                        Isvalid = true;
                    }
                }
            }
            return Isvalid;
        }

        /// <summary>
        /// Check if param is valid ip4
        /// </summary>
        /// <param name="IPAdress"></param>
        /// <returns></returns>
        private bool IsValidIP(String IPAdress)
        {
            const int allowedSplitValues = 4;
            var splitValues = IPAdress.Split('.');
            if (splitValues.Length != allowedSplitValues || string.IsNullOrWhiteSpace(IPAdress)) return false; // Check if split value is 4.
            return splitValues.All(r => byte.TryParse(r, out byte tempForParsing)); // Returns true if all split values can be parsed to bytes
        }

        /// <summary>
        /// Check if int is a valid port number.
        /// Based on valid port numbers: https://en.wikipedia.org/wiki/List_of_TCP_and_UDP_port_numbers#Dynamic,_private_or_ephemeral_ports
        /// </summary>
        /// <param name="portNumber"></param>
        /// <returns></returns>
        private bool IsValidPortNumber(int portNumber)
        {
            const int minValidPortNumber = 49152;
            const int maxValidPortNumber = 65535;
            if (portNumber >= minValidPortNumber && portNumber <= maxValidPortNumber)
            {
                return true;
            }

            return false;
        }

        private bool IsValidBufferSize(string BufferSize)
        {
            Regex RegMatch = new Regex(@"^[0-9]*$");
            // buffersize can be between 0 and 1024, and only contains numbers -> if valid, field is valid
            if (RegMatch.IsMatch(BufferSize) && !string.IsNullOrEmpty(BufferSize) &&
               int.Parse(BufferSize) <= 1024 && int.Parse(BufferSize) > 0)
            {
                return true;
            }
            return false;
        }
    }
}