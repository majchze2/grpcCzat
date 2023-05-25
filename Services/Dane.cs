using Grpc.Core;

namespace GrpcServiceCzat.Services
{
    public class Dane1
    {
       public List<IServerStreamWriter<ChatMessage>> ob = new List<IServerStreamWriter<ChatMessage>>();
    }
}
