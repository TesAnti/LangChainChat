using LangChain.Memory;
using LangChain.Providers;
using LangChain.Serve;
using LangChain.Utilities.Classes.Repository;
using LangChainChat;
using LangChainChat.Components;

var builder = WebApplication.CreateBuilder(args);

// add LangChainServe
builder.Services.AddLangChainServe();
// configure conversation name generator
builder.Services.ConfigureNameGenerator();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();

// add models
app.UseLangChainServe(options=>options.ConfigureModels());

app.UseBlazorFrameworkFiles();
app.MapFallbackToFile("index.html");
app.Run();

