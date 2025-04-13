namespace ServerCore;

public class SendBufferHelper
{
    public static ThreadLocal<SendBuffer> CurrentBuffer = new ThreadLocal<SendBuffer>(() => { return null;});

    public static int ChunkSize { get; set; } = 65535;

    public static ArraySegment<byte> Open(int reserverdSize)
    {
        if (CurrentBuffer.Value == null)
            CurrentBuffer.Value = new SendBuffer(ChunkSize);
        
        if(CurrentBuffer.Value.FreeSize < reserverdSize)
            CurrentBuffer.Value = new SendBuffer(ChunkSize);
        
        return CurrentBuffer.Value.Open(reserverdSize);
    }

    public static ArraySegment<byte> Close(int usedSize)
    {
        return CurrentBuffer.Value.Close(usedSize);
    }
}

public class SendBuffer
{
    byte[] _buffer;
    private int _usedSize = 0;

    public SendBuffer(int chunkSize)
    {
        _buffer = new byte[chunkSize];
    }

    public int FreeSize
    {
        get { return _buffer.Length - _usedSize; } 
    }

    public ArraySegment<byte> Open(int reserverdSize)
    {
        if (reserverdSize > FreeSize)
            return null;
        
        return new ArraySegment<byte>(_buffer, _usedSize, reserverdSize);
    }

    public ArraySegment<byte> Close(int usedSize)
    {
        ArraySegment<byte> segment = new ArraySegment<byte>(_buffer, _usedSize, usedSize);
        _usedSize += usedSize;
        return segment;
    }
}