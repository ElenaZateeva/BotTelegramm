// Telegramm бот - создание
using Telegram.Bot;
using Telegram.Bot.Types;

#nullable disable

var botClient = new TelegramBotClient("5739326854:AAENj3EL5z__JkQfLbBcbJSnN04iBHd2FBM");

User bot = botClient.GetMeAsync().Result;
List<long> users = new List<long>();
string secretKey = "123";
long adminId = 0;

while (true)
{
    Update[] updates = await botClient.GetUpdatesAsync();
    for (int i = 0; i < updates.Length; i++)
    {
        AddUser(updates[i].Message.From.Id);
        SetAdmin(updates[i].Message.Text, updates[i].Message.From.Id);
        // Если СМС идет от АДМИН, то получают его ВСЕ
        if (updates[i].Message.From.Id == adminId)
        {
            if (updates[i].Message.Text == "GET")
            {
                SendUserIdsToAdmin(updates[i].Message);
            }
            if (updates[i].Message.Text.Contains("everyone"))
            {
                updates[i].Message.Text = updates[i].Message.Text.Remove(0, 8);
                SendMessageToEveryOne(updates[i].Message);
            }
            if (updates[i].Message.Text.Contains("personal"))
            {
                SendToUser(updates[i]);
            }
        }
        updates = await botClient.GetUpdatesAsync(updates[^1].Id + 1);
        break;
    }
}

async Task SendMessageToEveryOne(Message message)
{
    for (int i = 0; i < users.Count; i++)
    {
        // Отвечаем на входящие сообщения Всем
        await botClient.SendTextMessageAsync(new ChatId(users[i]), message.Text);
    }
}

void AddUser(long userId)
{
    if (!users.Contains(userId))
    {
        //Если нет еще в списке такого пользователя, то добавяем
        users.Add(userId);
    }

}
void SetAdmin(string message, long userId)
{
    // Если кто-то ввел секретный ключ - становится АДМИНом навсегда
    if (adminId == 0 && message == secretKey)
    {
        adminId = userId;
    }
}

async Task SendUserIdsToAdmin(Message message)
{
    for (int i = 0; i < users.Count; i++)
    {
        // Отвечаем на входящие сообщения
        await botClient.SendTextMessageAsync(new ChatId(message.From.Id), users[i].ToString());
    }
}

async Task SendToUser(Update update)
{
    string[] texts = update.Message.Text.Split(" ");
    bool isParsed = long.TryParse(texts[1], out long userId);
    if (isParsed)
    {
        string message = "";
        for (int o = 2; o < texts.Length; o++)
        {
            message += $"{texts[o]} ";
        }
        await botClient.SendTextMessageAsync(new ChatId(userId), message);
    }
}