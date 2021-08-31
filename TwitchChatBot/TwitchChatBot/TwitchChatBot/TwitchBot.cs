using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TwitchChatBot
{
    public class TwitchBot
    {
        const string ip = "irc.chat.twitch.tv";
        const int port = 6667;

        private string nick;
        private string password;
        private StreamReader streamReader;
        private StreamWriter streamWriter;
        private TaskCompletionSource<int> connected = new TaskCompletionSource<int>();

        public event TwitchChatEventHandler OnMessage = delegate { };
        public delegate void TwitchChatEventHandler(object sender, TwitchChatMessage e);

        public Queue<PossibleCommands> enteredCommands = new Queue<PossibleCommands>();

        public class TwitchChatMessage : EventArgs
        {
            public string Sender { get; set; }
            public string Message { get; set; }
            public string Channel { get; set; }
        }

        public TwitchBot(string nick, string password)
        {
            this.nick = nick;
            this.password = password;
        }

        public async Task Start()
        {


            var tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(ip, port);
            streamReader = new StreamReader(tcpClient.GetStream());
            streamWriter = new StreamWriter(tcpClient.GetStream()) { NewLine = "\r\n", AutoFlush = true };

            await streamWriter.WriteLineAsync($"PASS {password}");
            await streamWriter.WriteLineAsync($"NICK {nick}");
            connected.SetResult(0);

            while (true)
            {
                // PERFORM TWITCH PLAYS TASK HERE
                if (enteredCommands.Count > 0)
                {
                    PossibleCommands nextCommand = enteredCommands.Dequeue();
                    Console.WriteLine("COMMAND " + nextCommand.ToString() + " WAS ISSUED");
                    switch (nextCommand)
                    {
                        case PossibleCommands.A:
                            SendKeyboard("{A}");
                            break;

                        case PossibleCommands.B:
                            SendKeyboard("{B}");
                            break;

                        case PossibleCommands.Down:
                            SendKeyboard("{DOWN}");
                            break;

                        case PossibleCommands.L:
                            SendKeyboard("{L}");
                            break;

                        case PossibleCommands.Left:
                            SendKeyboard("{LEFT}");
                            break;

                        case PossibleCommands.R:
                            SendKeyboard("{R}");
                            break;

                        case PossibleCommands.Right:
                            SendKeyboard("{RIGHT}");
                            break;

                        case PossibleCommands.Select:
                            SendKeyboard("{G}");
                            break;

                        case PossibleCommands.Start:
                            SendKeyboard("{ENTER}");
                            break;

                        case PossibleCommands.Up:
                            SendKeyboard("{UP}");
                            break;
                    }

                    System.Threading.Thread.Sleep(100);

                }





                string line = await streamReader.ReadLineAsync();
                if (line != null && line != string.Empty)
                {
                    Console.WriteLine(line);

                    string[] split = line.Split(' ');
                    //PING :tmi.twitch.tv 
                    //Respond with PONG :tmi.twitch.tv
                    if (line.StartsWith("PING"))
                    {
                        Console.WriteLine("PONG");
                        await streamWriter.WriteLineAsync($"PONG {split[1]}");
                    }

                    if (split.Length > 2 && split[1] == "PRIVMSG")
                    {
                        //:mytwitchchannel!mytwitchchannel@mytwitchchannel.tmi.twitch.tv 
                        // ^^^^^^^^
                        //Grab this name here
                        int exclamationPointPosition = split[0].IndexOf("!");
                        string username = split[0].Substring(1, exclamationPointPosition - 1);
                        //Skip the first character, the first colon, then find the next colon
                        int secondColonPosition = line.IndexOf(':', 1);//the 1 here is what skips the first character
                        string message = line.Substring(secondColonPosition + 1);//Everything past the second colon
                        string channel = split[2].TrimStart('#');

                        OnMessage(this, new TwitchChatMessage
                        {
                            Message = message,
                            Sender = username,
                            Channel = channel
                        });
                    }
                }

            }
        }

        public async Task SendMessage(string channel, string message)
        {
            await connected.Task;
            await streamWriter.WriteLineAsync($"PRIVMSG #{channel} :{message}");
        }

        public async Task JoinChannel(string channel)
        {
            await connected.Task;
            await streamWriter.WriteLineAsync($"JOIN #{channel}");
        }


        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        public void SendKeyboard(string command)
        {
            IntPtr zero = IntPtr.Zero;
            for (int i = 0; (i < 60) && (zero == IntPtr.Zero); i++)
            {
                System.Threading.Thread.Sleep(500);
                zero = FindWindow(null, "VisualBoyAdvance");
            }
            if (zero != IntPtr.Zero)
            {
                SetForegroundWindow(zero);
                SendKeys.Send(command);
            }
        }
    }
}
