namespace MusicApp;

public class SongInfoContainer(SongInfo? info = null)
{
    public SongInfo SongInfo { get; private set; } = info ?? SongInfo.Empty;

    public void Update(SongInfo newInfo)
    {
        SongInfo = newInfo;
        OnInfoUpdate?.Invoke();
    }

    public event Action? OnInfoUpdate;
}