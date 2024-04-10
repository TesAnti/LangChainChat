using LangChain.Memory;
using LangChain.Providers;
using LangChain.Serve;
using LangChain.Utilities.Classes.Repository;
using LangChainChat.Components;
using static LangChain.Chains.Chain;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.



#region langchain serve services

var model = new OllamaLanguageModelInstruction("mistral:latest", options: new OllamaLanguageModelOptions()
{
    Temperature = 0,
    Stop = new[] { "User:" },
}).UseConsoleForDebug();
builder.Services.AddLangChainServe();

builder.Services.AddCustomNameGenerator(async messages =>
{
    var template =
        @"You will be given conversation between User and Assistant. Your task is to give a name to this conversation using maximum 3 words
Conversation:
{chat_history}
Conversation name: ";
    var conversationBufferMemory = await ConvertToConversationBuffer(messages);
    var chain = LoadMemory(conversationBufferMemory, "chat_history")
                | Template(template)
                | LLM(model);
    return await chain.Run("text");

});

#endregion




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();


#region langchain serve app


app.UseLangChainServe(options =>
{
    options.RegisterModel("Test model", async (messages) =>
    {
        var template = @"You are helpful assistant. Continue conversation with user. Keep your answers short.
{chat_history}
Assistant:";
        var conversationBufferMemory = await ConvertToConversationBuffer(messages);
        var chain = LoadMemory(conversationBufferMemory, "chat_history")
                    | Template(template)
                    | LLM(model);

        var response = await chain.Run("text");
        return new StoredMessage()
        {
            Author = MessageAuthor.AI,
            Content = response
        };
    });
});

#endregion

app.UseBlazorFrameworkFiles();
app.MapFallbackToFile("index.html");
app.Run();

async Task<ConversationBufferMemory> ConvertToConversationBuffer(List<StoredMessage> list)
{
    var conversationBufferMemory = new ConversationBufferMemory();
    conversationBufferMemory.Formatter.HumanPrefix = "User";
    conversationBufferMemory.Formatter.AiPrefix = "Assistant";
    List<Message> converted = list
        .Select(x => new Message(x.Content, x.Author == MessageAuthor.User ? MessageRole.Human : MessageRole.Ai)).ToList();
    await conversationBufferMemory.ChatHistory.AddMessages(converted);
    return conversationBufferMemory;
}