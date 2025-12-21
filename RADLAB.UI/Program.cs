using Blazored.LocalStorage;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using RADLAB.UI.Services.Infrastructure;
using RADLAB.UI.Services.Services;
using RADLAB.UI.Utils;
using Radzen;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddRazorPages();
//builder.Services.AddControllers();

builder.Services.AddServerSideBlazor().AddHubOptions(x => x.MaximumReceiveMessageSize = 102400000).AddCircuitOptions(options => { options.DetailedErrors = true; });

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<DialogService>();

string WebApiUrlConfigurationName = builder.Environment.IsDevelopment() ? "WebApiUrlDebug" : "WebApiUrlRelease";

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration[WebApiUrlConfigurationName]) });

builder.Services.AddScoped<ImageService>();
builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddAuthenticationCore();

builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IRolService, RolService>();
builder.Services.AddScoped<IKullaniciService, KullaniciService>();
builder.Services.AddScoped<ILookupService, LookupService>();
builder.Services.AddScoped<ITanimKodluService, TanimKodluService>();
builder.Services.AddScoped<ITanimBasicService, TanimBasicService>();
builder.Services.AddScoped<ITanimSayiliService, TanimSayiliService>();
builder.Services.AddScoped<IIlIlceService, IlIlceService>();
builder.Services.AddScoped<ICihazService, CihazService>();
builder.Services.AddScoped<IFRService, FRService>();
builder.Services.AddScoped<IAyarService, AyarService>();
builder.Services.AddScoped<IDuyuruService, DuyuruService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IRaporService, RaporService>();
builder.Services.AddScoped<IMesajService, MesajService>();
builder.Services.AddScoped<ISMSMailService, SMSMailService>();
builder.Services.AddScoped<IKalibrasyonService, KalibrasyonService>();
builder.Services.AddScoped<IEgitimService, EgitimService>();
builder.Services.AddScoped<IKursService, KursService>();
builder.Services.AddScoped<IIndirimKoduService, IndirimKoduService>();
builder.Services.AddScoped<ISiparisService, SiparisService>();
builder.Services.AddScoped<IOdemeService, OdemeService>();
builder.Services.AddScoped<IStokHareketService, StokHareketService>();
builder.Services.AddScoped<IVakifBankService, VakifBankService>();
builder.Services.AddScoped<ISoruService, SoruService>();
builder.Services.AddScoped<ITestService, TestService>();
builder.Services.AddScoped<IVideoService, VideoService>();
builder.Services.AddScoped<IOgrenciService, OgrenciService>();
builder.Services.AddScoped<IOnlineEgitimService, OnlineEgitimService>();
builder.Services.AddScoped<IDosyaService, DosyaService>();


//builder.Services.AddHttpClient<HttpClient>(ConfigureHttpClient);
//builder.Services.AddScoped<HttpClient>(serviceProvider => serviceProvider.GetService<IHttpClientFactory>().CreateClient());

//static void ConfigureHttpClient(HttpClient httpClient)
//{
//    httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
//};








var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapBlazorHub();
    endpoints.MapFallbackToPage("/_Host");
    endpoints.MapControllers();
});

//app.UsePathBase("/RADLABUI");

app.Run();