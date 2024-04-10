using LangChainChat.Client.Data;
using MessageDTO = LangChainChat.Client.Data.MessageDTO;

namespace LangChainChat.Client.Model;

public class AppState
{

    
    private readonly ChainAPI _api;

    public AppState(ChainAPI api)
    {
        _api = api;
    }

    public string? SelectedModel { get; set; }

    public List<string> AvailableModels { get; set; } = new List<string>();

    public List<ConversationDTO> History { get; set; } = new List<ConversationDTO>();

    public Guid? ConversationId { get; set; }

    public List<Message> Messages = new List<Message>();



    public async Task LoadModels()
    {
        AvailableModels = await _api.ListModels();
    }

    public async Task ReloadConversations()
    {
        History = await _api.ListConversations();
    }

    public async Task<MessageDTO> PostMessage(string message)
    {
        bool historyNeedsRefresh=false;
        if (ConversationId == null)
        {
            historyNeedsRefresh = true;
            var conversation = await _api.CreateConversation(SelectedModel);
            ConversationId = conversation.ConversationId;
        }
        
        var answer = await _api.ProcessMessage(new PostMessageDTO() { Content = message }, ConversationId.Value);

        if(historyNeedsRefresh)
            await ReloadConversations();
        return answer;
    }

    public async Task ReloadMessages()
    {
        
        if (ConversationId == null)
        {
            return;
        }
        var messagesDTO = await _api.ListMessages(ConversationId.Value);
        Messages = messagesDTO.Select(x => new Message {DisplayName = x.Author, Content = x.Content}).ToList();
        
    }




}