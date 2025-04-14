using System.Net;
using ServerCore;

namespace PotionHeroServer.Session;

public class ClientSession : PacketSession
{
    public int SessionId { get; set; }
    public GameRoom Room { get; set; }
    
    public int HP { get; set; }
    public int gainedPower  { get; set; }
    
    public override void OnConnected(EndPoint endPoint)
    {
        Console.WriteLine($"Connected to {endPoint}");
        
        Program.Room.
    }

    public override void OnSend(int numOfBytes)
    {
        throw new NotImplementedException();
    }

    public override void OnDisconnected(EndPoint endPoint)
    {
        throw new NotImplementedException();
    }

    public override void OnRecvPacket(ArraySegment<byte> arraySegment)
    {
        throw new NotImplementedException();
    }
}