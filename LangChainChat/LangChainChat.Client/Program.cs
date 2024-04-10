using LangChainChat.Client;
using LangChainChat.Client.Data;
using LangChainChat.Client.Model;
using Markdig;
using Markdown.ColorCode;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var pipeline = new MarkdownPipelineBuilder()
    .UseAdvancedExtensions()
    .UseColorCode()
    .Build();

builder.Services.AddSingleton(pipeline);
builder.Services.AddSingleton<AppState>();
builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton<ChainAPI>();

await builder.Build().RunAsync();
