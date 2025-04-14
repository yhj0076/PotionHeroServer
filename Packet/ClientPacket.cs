using ServerCore;

namespace PotionHeroServer.Packet;

public enum PacketType
{
    S_BroadcastGainedDmg = 1,
    S_BroadcastAttack = 2,
    C_GainedDmg = 3,
    S_BroadcastTime = 4,
}

public interface IPacket
{
    ushort Protocol { get; }
    void Read(ArraySegment<byte> segment);
    ArraySegment<byte> Write();
}

public class S_BroadcastGainedDmg : IPacket
{
    public int hostGainedDmg { get; set; }
    public int guestGainedDmg { get; set; }
    
    public ushort Protocol { get {return (ushort)PacketType.S_BroadcastGainedDmg; } }
    
    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        ReadOnlySpan<byte> buffer = new ReadOnlySpan<byte>(segment.Array, segment.Offset, segment.Count);
        count += sizeof(ushort);
        count += sizeof(ushort);
        this.hostGainedDmg = BitConverter.ToInt32(buffer.Slice(count, buffer.Length - count));
        count += sizeof(int);
        this.guestGainedDmg = BitConverter.ToInt32(buffer.Slice(count, buffer.Length - count));
        count += sizeof(int);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(8192);

        ushort count = 0;
        bool success = true;

        Span<byte> buffer = new Span<byte>(segment.Array, segment.Offset, segment.Count);
        
        count += sizeof(ushort);
        success &= BitConverter.TryWriteBytes(buffer.Slice(count,buffer.Length - count), (ushort)PacketType.S_BroadcastGainedDmg);
        count += sizeof(ushort);
        
        success &= BitConverter.TryWriteBytes(buffer.Slice(count,buffer.Length - count), hostGainedDmg);
        count += sizeof(int);
        success &= BitConverter.TryWriteBytes(buffer.Slice(count,buffer.Length - count), guestGainedDmg);
        count += sizeof(int);
        
        success &= BitConverter.TryWriteBytes(buffer, count);
        
        if (success == false)
            return null;

        return SendBufferHelper.Close(count);
    }
}