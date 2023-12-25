namespace MusicApp;

public class SongInfo : IEquatable<SongInfo>
{
    public required string Name { get; init; }
    public required string Album { get; init; }
    public required string Author { get; init; }
    public required string AlbumCoverUrl { get; init; }

    public static SongInfo Empty => new()
        { Name = string.Empty, Album = string.Empty, Author = string.Empty, AlbumCoverUrl = string.Empty };

    public bool Equals(SongInfo? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Name == other.Name && Album == other.Album && Author == other.Author;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj is SongInfo info && Equals(info);
    }

    public override int GetHashCode() => HashCode.Combine(Name, Album, Author);

    public static bool operator ==(SongInfo? left, SongInfo? right) => Equals(left, right);

    public static bool operator !=(SongInfo? left, SongInfo? right) => !Equals(left, right);
}