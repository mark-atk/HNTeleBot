using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputMessageContents;
using Telegram.Bot.Types.ReplyMarkups;

namespace HNTeleBot
{
    class Program
    {
        //282493687:AAFCAAAF0Im0e2TumAWrZKrPOrtXNEmSIsU

        private static readonly TelegramBotClient Bot = new TelegramBotClient(ApiCode.Code);

        static void Main(string[] args)
        {
            Console.WriteLine("Bot starting...");
            Bot.OnCallbackQuery += BotOnCallbackQueryReceived;
            Bot.OnMessage += BotOnMessageReceived;
            Bot.OnMessageEdited += BotOnMessageReceived;
            Bot.OnInlineQuery += BotOnInlineQueryReceived;
            Bot.OnInlineResultChosen += BotOnChosenInlineResultReceived;
            Bot.OnReceiveError += BotOnReceiveError;

            var me = Bot.GetMeAsync().Result;

            Console.Title = me.Username;

            Bot.StartReceiving();
            Console.WriteLine("Bot started.");
            Console.ReadLine();
            Bot.StopReceiving();
        }

        private static void BotOnReceiveError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
        {
            Debugger.Break();
        }

        private static void BotOnChosenInlineResultReceived(object sender, ChosenInlineResultEventArgs chosenInlineResultEventArgs)
        {
            Console.WriteLine($"Received choosen inline result: {chosenInlineResultEventArgs.ChosenInlineResult.ResultId}");
        }

        private static async void BotOnInlineQueryReceived(object sender, InlineQueryEventArgs inlineQueryEventArgs)
        {
            //need to implement for More News Results
        }

        private static async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;

            if (message == null || message.Type != MessageType.TextMessage) return;

            Console.WriteLine("Message received: " + message.Text);
            if (message.Text.StartsWith("/news")) // send a photo
            {
                Feed.GetFeed().ToList().ForEach(async feedItem =>
                {
                    string subject = feedItem.Title.Text;
                    string link = feedItem.Links[0].Uri.AbsoluteUri;
                    string messageItem = String.Concat(subject, " : ", link);

                    await Bot.SendTextMessageAsync(message.Chat.Id, messageItem);
                });

            }
            else
            {
                var usage = @"Usage:
/news   - gets top 5 Hacker News Stories.
";

                await Bot.SendTextMessageAsync(message.Chat.Id, usage,
                    replyMarkup: new ReplyKeyboardHide());
            }
        }

        private static async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            await Bot.AnswerCallbackQueryAsync(callbackQueryEventArgs.CallbackQuery.Id, 
                $"Received {callbackQueryEventArgs.CallbackQuery.Data}");
        }
    }
}
