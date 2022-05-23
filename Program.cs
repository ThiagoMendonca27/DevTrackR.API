using DevTrackR.API.Persistence;
using DevTrackR.API.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SendGrid.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

var sendGridApiKey = builder.Configuration.GetSection("SendGridApiKey").Value;
// Add services to the container.
builder.Services.AddSendGrid(o => o.ApiKey = sendGridApiKey);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o => {
    o.SwaggerDoc("v1", new OpenApiInfo{
        Title = "DevTrackR v1",
        Version = "V1",
        Contact = new OpenApiContact{
            Name = "Thiago Mendonça",
            Email = "thiago.a.mendonca89@gmail.com"
        }
    });

    var xmlPath = Path.Combine(AppContext.BaseDirectory, "DevTrackR.API.xml");
    o.IncludeXmlComments(xmlPath); 
});
//builder.Services.AddDbContext<DevTrackRContext>(); Utilizado com uma base externa
builder.Services.AddDbContext<DevTrackRContext>(o => o.UseInMemoryDatabase("DevTrackR"));

builder.Services.AddScoped<IPackageRepository, PackageRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
    app.UseSwagger();
    app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
