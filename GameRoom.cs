using PotionHeroServer.Session;
using ServerCore;

namespace PotionHeroServer;

public class GameRoom : IJobQueue
{
    List<ClientSession> _clientSessions = new List<ClientSession>();
    JobQueue _jobQueue = new JobQueue();
    List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();
    
    public void Push(Action job)
    {
        _jobQueue.Push(job);
    }

    public void Flush()
    {
        foreach (var session in _clientSessions)
        {
            session.Send(_pendingList);
        }
        
        _pendingList.Clear();
    }

    public void Broadcast(ArraySegment<byte> segment)
    {
        _pendingList.Add(segment);
    }

    public void Enter(ClientSession clientSession)
    {
        // 플레이어 추가
        _clientSessions.Add(clientSession);
        clientSession.Room = this;
        
        // 상대방 도착 정보 전송
        // clientSession.Send();
        
        
    }
}