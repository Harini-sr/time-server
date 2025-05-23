var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowDevClients", policy =>
    {
        policy.WithOrigins(
                "http://localhost:3000",  // React
                "http://localhost:4200",  // Angular
                "http://localhost:55206"  // Your actual frontend if needed
            )
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use the unified CORS policy once
app.UseCors("AllowDevClients");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
