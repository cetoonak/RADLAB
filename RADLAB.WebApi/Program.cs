using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RADLAB.Business.Abstract;
using RADLAB.Business.Concrete;
using RADLAB.Data.Context;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<RADLABDBContext>(config =>
{
    config.UseSqlServer(builder.Configuration.GetConnectionString(builder.Environment.IsDevelopment() ? "Debug" : "Release"));
    config.EnableSensitiveDataLogging();
});

builder.Services.AddScoped<ILoginManager, LoginManager>();
builder.Services.AddScoped<IRolManager, RolManager>();
builder.Services.AddScoped<IKullaniciManager, KullaniciManager>();
builder.Services.AddScoped<ILookupManager, LookupManager>();
builder.Services.AddScoped<ITanimKodluManager, TanimKodluManager>();
builder.Services.AddScoped<ITanimBasicManager, TanimBasicManager>();
builder.Services.AddScoped<ITanimSayiliManager, TanimSayiliManager>();
builder.Services.AddScoped<IIlIlceManager, IlIlceManager>();
builder.Services.AddScoped<ICihazManager, CihazManager>();
builder.Services.AddScoped<IAyarManager, AyarManager>();
builder.Services.AddScoped<IDuyuruManager, DuyuruManager>();
builder.Services.AddScoped<IDashboardManager, DashboardManager>();
builder.Services.AddScoped<IRaporManager, RaporManager>();
builder.Services.AddScoped<IMesajManager, MesajManager>();
builder.Services.AddScoped<ISMSMailManager, SMSMailManager>();
builder.Services.AddScoped<IKalibrasyonManager, KalibrasyonManager>();
builder.Services.AddScoped<IEgitimManager, EgitimManager>();
builder.Services.AddScoped<IKursManager, KursManager>();
builder.Services.AddScoped<IIndirimKoduManager, IndirimKoduManager>();
builder.Services.AddScoped<ISiparisManager, SiparisManager>();
builder.Services.AddScoped<IOdemeManager, OdemeManager>();
builder.Services.AddScoped<IStokHareketManager, StokHareketManager>();
builder.Services.AddScoped<IVakifBankManager, VakifBankManager>();
builder.Services.AddScoped<ISoruManager, SoruManager>();
builder.Services.AddScoped<ITestManager, TestManager>();
builder.Services.AddScoped<IVideoManager, VideoManager>();
builder.Services.AddScoped<IOgrenciManager, OgrenciManager>();
builder.Services.AddScoped<IOnlineEgitimManager, OnlineEgitimManager>();
builder.Services.AddScoped<IDosyaManager, DosyaManager>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "RADLAB", Version = "v1.0.0" });

    var securitySchema = new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    c.AddSecurityDefinition("Bearer", securitySchema);

    var securityRequirement = new OpenApiSecurityRequirement
                {
                    { securitySchema, new[] { "Bearer" } }
                };

    c.AddSecurityRequirement(securityRequirement);
});







builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtIssuer"],
            ValidAudience = builder.Configuration["JwtAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSecurityKey"])),
            ClockSkew = TimeSpan.Zero
        };
    });





var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();