using LangChain.Memory;
using LangChain.Providers;
using LangChain.Utilities.Classes.Repository;

namespace LangChainChat;

public static class ConversationBufferExtensions
{
    public static async Task<ConversationBufferMemory> ConvertToConversationBuffer(this List<StoredMessage> list)
    {
        var conversationBufferMemory = new ConversationBufferMemory();
        conversationBufferMemory.Formatter.HumanPrefix = "User";
        conversationBufferMemory.Formatter.AiPrefix = "Assistant";
        List<Message> converted = list
            .Select(x => new Message(x.Content, x.Author == MessageAuthor.User ? MessageRole.Human : MessageRole.Ai)).ToList();
        await conversationBufferMemory.ChatHistory.AddMessages(converted);
        return conversationBufferMemory;
    }
}