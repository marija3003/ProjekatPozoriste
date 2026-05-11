using Microsoft.EntityFrameworkCore;
using ProjekatPozoriste.Data;
using ProjekatPozoriste.Services; 

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); 
//builder.Services.AddOpenApi();


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));


builder.Services.AddScoped<PredstavaService>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200") 
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
   // app.MapOpenApi(); 
    app.UseSwagger(); 
    app.UseSwaggerUI(); 
}

app.UseHttpsRedirection();

app.UseCors(options =>
{
    options.AllowAnyOrigin();
    options.AllowAnyMethod();
    options.AllowAnyHeader();
});

app.UseAuthorization();

app.MapControllers();

// Seed
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Došlo je do greške prilikom seed-ovanja baze.");
    }
}

app.UseStaticFiles(); 
app.UseCors("AllowAll");
app.Run();