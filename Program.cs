using ObituariosWeb;

var builder = WebApplication.CreateBuilder(args);

// 🔥 CORS (IMPORTANTE)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<FirebirdService>();

var app = builder.Build();

// 🔥 ACTIVAR CORS (ANTES DE MAPCONTROLLERS)
app.UseCors("AllowAll");

app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();