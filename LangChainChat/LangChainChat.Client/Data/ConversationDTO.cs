namespace LangChainChat.Client.Data;

public class ConversationDTO
{
    public Guid ConversationId { get; set; }
    public string ModelName { get; set; }
    public string? ConversationName { get; set; }
    public DateTime CreatedAt { get; set; }

    
}