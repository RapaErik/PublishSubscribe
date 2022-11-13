using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using PublishSubscribe.Extensions;
using PublishSubscribe.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//this part we need for FluentValidation
builder.Services.AddFluentValidationAutoValidation().AddValidatorsFromAssembly(typeof(Program).Assembly);


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//This part we need for our services used in the project
builder.Services.AddServices();
builder.Services.AddBackgroundWorkerServices();
builder.Services.AddHttpServices();


var app = builder.Build();


// Configure the HTTP request pipeline.

app.UseMiddleware<ExceptionHandlerMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
