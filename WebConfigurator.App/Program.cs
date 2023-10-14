using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using MudBlazor.Services;
using WebConfigurator.Clients;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:8060");

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();
builder.Services.AddSingleton<WebApiClient>();
builder.Services.AddHttpClient<WebApiClient>(httpClient =>
{
    const string username = "root";
    const string password = "";
    var credentialsBytes = Encoding.ASCII.GetBytes($"{username}:{password}");
    var base64Credentials = Convert.ToBase64String(credentialsBytes);
    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64Credentials);

    var uriBuilder = new UriBuilder
    {
        Host = "192.168.100.140",
        Port = 8080,
        Scheme = "http",
    };
    httpClient.BaseAddress = uriBuilder.Uri;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();