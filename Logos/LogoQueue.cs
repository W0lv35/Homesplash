using System.Collections.Concurrent;

namespace Homesplash.Logos;

public interface ILogoQueue {
    void Enqueue(string url);
    bool TryDequeue(out string? url);
}

public class LogoQueue : ILogoQueue {
    private readonly ConcurrentQueue<string> _urls = new();
    public void Enqueue(string url) => _urls.Enqueue(url);
    public bool TryDequeue(out string? url) => _urls.TryDequeue(out url);
}