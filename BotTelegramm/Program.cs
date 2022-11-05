// Telegramm бот - создание
using Telegram.Bot;
using Telegram.Bot.Types;

#nullable disable

var botClient=new TelegramBotClient("5739326854:AAENj3EL5z__JkQfLbBcbJSnN04iBHd2FBM");

User bot=botClient.GetMeAsync().Result;

while (true)
{
    Update[] updates=await botClient.GetUpdatesAsync();
    for (int i = 0; i < updates.Length; i++)
    {
        Console.WriteLine(updates[i].Message.Text);
        Console.WriteLine(updates[i].Message.From.FirstName);
        Console.WriteLine(updates[i].Message.From.Id);
        ReplyToMessage(updates[i].Message);
        updates= await botClient.GetUpdatesAsync(updates[^1].Id+1);
        break;
    }
}

async Task ReplyToMessage(Message message)
{
    await botClient.SendTextMessageAsync(new ChatId(message.From.Id), "Hello");
}