using System.Net.Http.Json;

namespace LangChainChat.Client.Data;

public class ChainAPI
{
    private readonly HttpClient _client;

    public ChainAPI(HttpClient client)
    {
        _client = client;
    }

    public async Task<List<string>> ListModels()
    {
        var response = await _client.GetAsync("/serve/models");
        var content = await response.Content.ReadFromJsonAsync<List<string>>();
        return content!;
    }

    public async Task<List<ConversationDTO>> ListConversations()
    {
        var response = await _client.GetAsync("/serve/conversations");
        var content = await response.Content.ReadFromJsonAsync<List<ConversationDTO>>();
        return content!;
    }

    public async Task<ConversationDTO> CreateConversation(string modelName)
    {
        var response = await _client.PostAsJsonAsync("/serve/conversations", new{modelName});
        var content = await response.Content.ReadFromJsonAsync<ConversationDTO>();
        return content!;
    }

    public async Task<MessageDTO> ProcessMessage(PostMessageDTO message, Guid conversationId)
    {
        var response = await _client.PostAsJsonAsync($"/serve/conversations/{conversationId}/messages", message);
        var content = await response.Content.ReadFromJsonAsync<MessageDTO>();
        return content!;
    }

    public async Task<ConversationDTO> GetConversation(Guid conversationId)
    {
        var response = await _client.GetAsync($"/serve/conversations/{conversationId}");
        var content = await response.Content.ReadFromJsonAsync<ConversationDTO>();
        return content!;
    }

    public async Task<List<MessageDTO>> ListMessages(Guid conversationId)
    {
        var response = await _client.GetAsync($"/serve/conversations/{conversationId}/messages");
        var content = await response.Content.ReadFromJsonAsync<List<MessageDTO>>();
        return content!;
    }

    public async Task DeleteConversation(Guid conversationId)
    {
        await _client.DeleteAsync($"/serve/conversations/{conversationId}");
    }

}