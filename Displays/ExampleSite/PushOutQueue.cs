using System.Collections;
using System.Diagnostics.Contracts;

namespace ExampleSite;

/// <summary>
/// Represents a first-in, first-out collection that removes the items from the front of the queue to add new items
/// to the back when the specified item capacity is reached.
/// </summary>
public class PushOutQueue<T> : ICollection<T>, IReadOnlyCollection<T>
{
    public int MaxItems { get; }
    private T[] _items;
    
    /// <summary>
    /// Creates a first-in, first-out collection that removes the items from the front of the queue to add new items
    /// to the back when the specified item capacity is reached.
    /// </summary>
    /// <param name="maxItems">The maximum capacity of items allowed in the queue. Must be at least 1.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="maxItems"/> is less than 1.</exception>
    public PushOutQueue(int maxItems)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(maxItems, 1);
        MaxItems = maxItems;
        _items = new T[maxItems];
    }

    public IEnumerator<T> GetEnumerator() => new Enumerator(this);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Enqueue(T item)
    {
        if (Count >= MaxItems) RemoveCount(1);
        _items[Count] = item;
        Count += 1;
    }

    public void Add(T item) => Enqueue(item);

    public void Clear() => _items = [];

    [Pure]
    public bool Contains(T item) => _items.Contains(item);

    public void CopyTo(T[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);

    public bool Remove(T item)
    {
        int i = Array.IndexOf(_items, item);
        if (i < 0) return false;
        return RemoveAt(i);
    }

    public T Dequeue()
    {
        ThrowIfEmptyQueue("Cannot dequeue an element from an empty queue.");
        T item = _items[0];
        RemoveCount(1);
        return item;
    }
    
    public void RemoveCount(int count)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(count);
        if (count is 0) return;
        
        if (count < MaxItems)
        {
            T[] source = _items[count..];
            _items = new T[MaxItems];
            Count -= count;
            Array.Copy(source, _items, Count);
        }
        else
        {
            _items = new T[MaxItems];
            Count = 0;
        }
    }

    public bool RemoveAt(int i)
    {
        if (i >= Count || i < 0) return false;

        _items = [.._items[..i], .._items[(i + 1)..]];
        Count -= 1;
        return true;
    }

    public int Count { get; private set; }
    public bool IsReadOnly => false;

    private void ThrowIfEmptyQueue(string? errorMessage = null)
    {
        if (Count is 0) throw new InvalidOperationException($"Queue empty. {errorMessage}");
    }
    
    public PushOutQueue<T> Clone() => new(MaxItems) { _items = _items };

#nullable disable
    private class Enumerator(PushOutQueue<T> queue) : IEnumerator<T>
    {
        private PushOutQueue<T> Queue { get; set; } = queue;
        private PushOutQueue<T> BackupQueue { get; } = queue.Clone();

        public bool MoveNext()
        {
            if (Queue.Count is 0) return false;
            Current = Queue.Dequeue();
            return true;
        }

        public void Reset()
        {
            Current = default;
            Queue = BackupQueue.Clone();
        }

        public T Current { get; private set; }

        object IEnumerator.Current => Current!;

        public void Dispose()
        {
            Array.Clear(BackupQueue._items);
            Array.Clear(Queue._items);
        }
    }
}