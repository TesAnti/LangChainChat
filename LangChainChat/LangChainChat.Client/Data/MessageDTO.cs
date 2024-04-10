

namespace LangChainChat.Client.Data;

public class MessageDTO
{
    public string Content { get; set; }

    public string Author { get; set; }

    public Guid ConversationId { get; set; }

    public Guid MessageId { get; set; }

   
}