using APILayer.Extensions;
using BusinessLogicLayer.Extensions;
using DataAccessLayer.Database;
using DataAccessLayer.Extensions;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using VisualFXVault.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddProblemDetails();

builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddBusinessLogicLayer();

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication();

builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:4200") // Replace with frontend URL
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});


var app = builder.Build();

app.UseExceptionHandling();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.MapOpenApi();

    app.MapScalarApiReference();
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    // dbContext.Database.EnsureCreated();
    dbContext.Database.Migrate();
}

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapProducts();

app.Run();

