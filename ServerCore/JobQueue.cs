using System.Runtime.Intrinsics.X86;

namespace ServerCore;

public interface IJobQueue
{
    void Push(Action job);
}

public class JobQueue : IJobQueue
{
    Queue<Action> _jobQueue = new Queue<Action>();
    object _lock = new object();
    private bool _flush = false;

    public void Push(Action job)
    {
        bool flush = false;
        lock (_lock)
        {
            _jobQueue.Enqueue(job);
            if(_flush == false)
                flush = _flush = true;
        }

        if (flush)
            Flush();
    }

    private void Flush()
    {
        while (true)
        {
            Action job = Pop();
            if (job == null)
                return;
            
            job.Invoke();
        }
    }

    private Action Pop()
    {
        lock (_lock)
        {
            if (_jobQueue.Count == 0)
            {
                _flush = false;
                return null;
            }
            return _jobQueue.Dequeue();
        }
    }
}