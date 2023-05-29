using Grpc.Core;
using GrpcServiceCzat;
using GrpcServiceCzat.Services;
using System;

 

var builder = WebApplication.CreateBuilder(args);

//var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
//builder.Services.AddGrpc(options =>
//{
//   options.EnableDetailedErrors = true;
//   options.MaxReceiveMessageSize = 2 * 1024 * 1024; // 2 MB
//   options.MaxSendMessageSize = 5 * 1024 * 1024; // 5 MB
//});

builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
{
   builder.AllowAnyOrigin()
           .AllowAnyMethod()
          .AllowAnyHeader()
         .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
}));
//const string corsPolicy = "_corsPolicy";
//builder.Services.AddCors(options =>
//{
//   options.AddPolicy(name: corsPolicy,
//                     policy =>
//                     {
//                          policy.WithOrigins("https://localhost:5001",
//                                             "http://localhost:5000")
//                                .AllowAnyMethod();
//                     });
//});
//WebApplication app = builder.Build();
//app.UseCors(corsPolicy);
builder.Services.AddGrpc();
//d.ob = "";
var app = builder.Build();
app.UseRouting();

app.UseCors();
//app.UseCors((CorsOptions.AllowAll));
app.UseGrpcWeb(new GrpcWebOptions
{
    DefaultEnabled = true
});
//app.UseEndpoints(endpoints => {
//endpoints.MapGrpcService<ChatMessage>().EnableGrpcWeb();
//});
//public List<IServerStreamWriter<ChatMessage>> ob = new List<IServerStreamWriter<ChatMessage>>();
app.MapGrpcService<CzatService>().EnableGrpcWeb();
//app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
