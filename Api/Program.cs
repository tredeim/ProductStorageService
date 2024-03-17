using FluentValidation;
using Api.Interceptors;
using Api.Services;
using Infrastructure;
using Api.Middlewares;
using Api.Application;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc(o =>
{    
    o.Interceptors.Add<LoggingInterceptor>();
}).AddJsonTranscoding();

builder.Services.AddGrpcSwagger();
builder.Services.AddSwaggerGen();
builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));
builder.Services.AddRepositories();
builder.Services.AddServices();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// Configure the HTTP request pipeline.
app.UseMiddleware<LoggingMiddleware>();
app.MapGrpcService<ProductService>();

app.Run();