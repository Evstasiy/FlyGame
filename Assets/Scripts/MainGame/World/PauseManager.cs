using System.Collections.Generic;

public class PauseManager : IPauseHandler
{
    private readonly List<IPauseHandler> handlers = new List<IPauseHandler>();
    public bool IsPause {  get; private set; }
    public void Register(IPauseHandler handler)
    {
        handlers.Add(handler);
    }
    public void UnRegister(IPauseHandler handler)
    {
        handlers.Remove(handler);
    }
    public void SetPause(bool isPause)
    {
        IsPause = isPause;
        foreach (IPauseHandler handler in handlers)
        {
            handler.SetPause(isPause);
        }
    }
}
