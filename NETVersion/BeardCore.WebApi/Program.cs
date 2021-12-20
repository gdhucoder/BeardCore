﻿// Copyright (c) HuGuodong 2022. All Rights Reserved.
using BeardCore.Commons.Log;
using log4net;
using log4net.Repository;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddLog4Net("log4net.config");
// ILoggerRepository LoggerRepository = LogManager.CreateRepository("NETCoreRepository");
// Log4netHelper.SetConfig(LoggerRepository, "log4net.config");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Memory Cache
builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowAnyOrigin());

app.Run();
