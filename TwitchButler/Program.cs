using AsyncAwaitBestPractices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Transactions;

namespace TwitchButler
{
    public enum PossibleCommands
    {
        Up,
        Down,
        Left,
        Right,
        Start,
        Select,
        A,
        B,
        L,
        R
    }

    class Program
    {
        

        static async Task Main(string[] args)
        {
            string password = Environment.GetEnvironmentVariable("TWITCH_OAUTH");
            string botUsername = "TwitchButler";

            var twitchBot = new TwitchBot(botUsername, password);
            twitchBot.Start().SafeFireAndForget();
            //We could .SafeFireAndForget() these two calls if we want to
            await twitchBot.JoinChannel("actuallytobby");
            await twitchBot.SendMessage("actuallytobby", "Hey my bot has started up");

            twitchBot.OnMessage += async (sender, twitchChatMessage) =>
            {
                if (twitchChatMessage.Message.ToLower() == "up")
                {
                    twitchBot.enteredCommands.Enqueue(PossibleCommands.Up);
                }

                if (twitchChatMessage.Message.ToLower() == "down")
                {
                    twitchBot.enteredCommands.Enqueue(PossibleCommands.Down);
                }


                if (twitchChatMessage.Message.ToLower() == "left")
                {
                    twitchBot.enteredCommands.Enqueue(PossibleCommands.Left);
                }


                if (twitchChatMessage.Message.ToLower() == "right")
                {
                    twitchBot.enteredCommands.Enqueue(PossibleCommands.Right);
                }


                if (twitchChatMessage.Message.ToLower() == "a")
                {
                    twitchBot.enteredCommands.Enqueue(PossibleCommands.A);
                }


                if (twitchChatMessage.Message.ToLower() == "b")
                {
                    twitchBot.enteredCommands.Enqueue(PossibleCommands.B);
                }


                if (twitchChatMessage.Message.ToLower() == "start")
                {
                    twitchBot.enteredCommands.Enqueue(PossibleCommands.Start);
                }


                if (twitchChatMessage.Message.ToLower() == "select")
                {
                    twitchBot.enteredCommands.Enqueue(PossibleCommands.Select);
                }

                if (twitchChatMessage.Message.ToLower() == "l")
                {
                    twitchBot.enteredCommands.Enqueue(PossibleCommands.L);
                }

                if (twitchChatMessage.Message.ToLower() == "r")
                {
                    twitchBot.enteredCommands.Enqueue(PossibleCommands.R);
                }


                Console.WriteLine($"{twitchChatMessage.Sender} said '{twitchChatMessage.Message}'");
                //Listen for !hey command
                if (twitchChatMessage.Message.StartsWith("!hey"))
                {
                    await twitchBot.SendMessage(twitchChatMessage.Channel, $"Hey there {twitchChatMessage.Sender}");
                }
            };

            await Task.Delay(-1);
        }
    }
}
