using Google.Protobuf;
using Grpc.Core;
using GrpcServiceCzat;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
using GrpcServiceCzat.Services;
using System.Collections.Concurrent;

namespace GrpcServiceCzat.Services
{
    public class CzatService : ChatService.ChatServiceBase
    {
       private List<string> usersInChat;
       private static List<IServerStreamWriter<ChatMessage>> ob = new List<IServerStreamWriter<ChatMessage>>();
        //ob = new List<IServerStreamWriter<ChatMessage>>();
        //public List<ChatMessage> observers=new List<ChatMessage>();
       /// List<IServerStreamWriter> ob= List<IServerStreamWriter>();
        // CodedOutputStream(byte[];
        private readonly ILogger<CzatService> _logger;
        public CzatService(ILogger<CzatService> logger)
        {
            _logger = logger;
            usersInChat = new List<string>();
            //ob = new List<IServerStreamWriter<ChatMessage>>();
        }

        public override Task<JoinResponse> join(User request, ServerCallContext context)
        {
            string user1 = request.Name;
            if (usersInChat.Contains(user1))
            {
                return Task.FromResult(new JoinResponse
                {
                    Error = 1,
                    Msg= "Uzytkownik istnieje"
                });
            }
            else
            {
                usersInChat.Add(user1);
                return Task.FromResult(new JoinResponse
                {
                    Error = 0,
                    Msg = "Dodano:" +user1
                });
            }

                
        }
        public override Task<Empty> sendMsg(ChatMessage request, ServerCallContext context)
        {
            var chat = new ChatMessage();
            chat.From = request.From;
            chat.Msg = request.Msg;
            chat.Time=request.Time;
            foreach (var observe in ob)
            {
                observe.WriteAsync(chat);
            }
            return Task.FromResult(new Empty
            {
              
            });
        }
        public override Task<UserList> getAllUsers(Empty request, ServerCallContext context)
        {

            return Task.FromResult(new UserList {});
    
        }
        //private static readonly ConcurrentDictionary<User, IServerStreamWriter<ChatMessage>> MessageSubscriptions = new Dictionary<User, IServerStreamWriter<ChatMessage>>();

        public override async Task receiveMsg(Empty request, IServerStreamWriter<ChatMessage> responseStream, ServerCallContext context)
        {
            
            //MessageSubscriptions.
            //Program.ob.Add(responseStream);
            // Dane1.ob.Add(responseStream);
            //return Task.CompletedTask;
            //ob=new List<IServerStreamWriter<ChatMessage>>();
            ob.Add(responseStream);
            
            while (!context.CancellationToken.IsCancellationRequested)
            {
                // Avoid pegging CPU
                await Task.Delay(1000);
            }
            ob.Remove(responseStream);
            //for (var i = 0; i < 500; i++)
            //{
               // await responseStream.WriteAsync(chat);
             //   await Task.Delay(TimeSpan.FromSeconds(1));
            //}
             
        }
    }
}
