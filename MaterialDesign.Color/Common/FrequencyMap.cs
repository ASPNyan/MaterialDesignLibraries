using System.Collections;
using System.Diagnostics.Contracts;
using System.Numerics;

namespace MaterialDesign.Color.Common;

/// <summary>
/// Represents a map of frequencies for a given set of values.
/// </summary>
/// <typeparam name="TValue">The type of values in the map.</typeparam>
public class FrequencyMap<TValue> : ICollection<KeyValuePair<TValue, int>> where TValue : notnull
{
    private Dictionary<TValue, int> _map = new();

    /// <summary>
    /// Creates a FrequencyMap from an existing list of values.
    /// </summary>
    /// <param name="values">The list of values to use</param>
    /// <returns>The new FrequencyMap with populated frequencies</returns>
    public static FrequencyMap<TValue> From(IEnumerable<TValue> values)
    {
        FrequencyMap<TValue> output = new();
        foreach (TValue value in values) output.Add(value);
        return output;
    }

    /// <summary>
    /// Creates a FrequencyMap from an existing list of values and a method to get the frequency.
    /// </summary>
    /// <param name="values">The list of values to use</param>
    /// <param name="getFrequency">A method to get the frequency with, receiving the value and index position</param>
    /// <returns>The new FrequencyMap with populated frequencies</returns>
    public static FrequencyMap<TValue> From(IReadOnlyList<TValue> values, Func<TValue, int, int> getFrequency)
    {
        FrequencyMap<TValue> output = new();

        for (int i = 0; i < values.Count; i++)
        {
            TValue value = values[i];
            int frequency = getFrequency(value, i);

            output.Add(value, frequency);
        }

        return output;
    }
    
    /// <inheritdoc cref="From(Dictionary{TValue,int})"/>
    public static FrequencyMap<TValue> From(IEnumerable<KeyValuePair<TValue, int>> dictionaryFreqMap) =>
        From(dictionaryFreqMap.ToDictionary(x => x.Key, y => y.Value));

    /// <summary>
    /// Creates a FrequencyMap from an existing dictionary of values and frequencies.
    /// </summary>
    /// <param name="dictionaryFreqMap">A dictionary containing values and frequencies</param>
    /// <returns>The new translated frequency map</returns>
    public static FrequencyMap<TValue> From(Dictionary<TValue, int> dictionaryFreqMap)
    {
        FrequencyMap<TValue> output = new();

        foreach ((TValue value, int frequency) in dictionaryFreqMap) output.Add(value, frequency);

        return output;
    }

    /// <summary>
    /// Creates a FrequencyMap with updated values from an existing FrequencyMap
    /// </summary>
    /// <param name="existing">The existing frequency map</param>
    /// <param name="getValue">A method to get the value with</param>
    /// <typeparam name="TExisting">The typeof the existing FrequencyMap</typeparam>
    /// <returns>The new translated frequency map</returns>
    public static FrequencyMap<TValue> From<TExisting>(FrequencyMap<TExisting> existing,
        Func<TExisting, TValue> getValue)
        where TExisting : notnull
    {
        FrequencyMap<TValue> output = new();

        foreach ((TExisting existingValue, int frequency) in existing)
        {
            TValue value = getValue(existingValue);
            output.Add(value, frequency);
        }

        return output;
    }

    /// <summary>
    /// Creates a FrequencyMap with updated values from an existing FrequencyMap.
    /// </summary>
    /// <param name="existing">The existing frequency map</param>
    /// <param name="getValue">A method to get the value with, containing the existing item and its frequency</param>
    /// <typeparam name="TExisting">The typeof the existing FrequencyMap</typeparam>
    /// <returns>The new translated frequency map</returns>
    public static FrequencyMap<TValue> From<TExisting>(FrequencyMap<TExisting> existing,
        Func<TExisting, int, TValue> getValue)
        where TExisting : notnull
    {
        FrequencyMap<TValue> output = new();

        foreach ((TExisting existingValue, int frequency) in existing)
        {
            TValue value = getValue(existingValue, frequency);
            output.Add(value, frequency);
        }

        return output;
    }

    /// <summary>
    /// Creates a FrequencyMap with updated values from an existing FrequencyMap.
    /// </summary>
    /// <param name="existing">The existing frequency map</param>
    /// <param name="getValue">A method to get the value with, given the existing item and its frequency</param>
    /// <param name="getFrequency">A method to get the frequency with, given the existing frequency</param>
    /// <typeparam name="TExisting">The typeof the existing FrequencyMap</typeparam>
    /// <returns>The new translated frequency map</returns>
    public static FrequencyMap<TValue> From<TExisting>(FrequencyMap<TExisting> existing,
        Func<TExisting, int, TValue> getValue, Func<int, int> getFrequency) where TExisting : notnull
    {
        FrequencyMap<TValue> output = new();

        foreach ((TExisting existingValue, int existingFrequency) in existing)
        {
            TValue value = getValue(existingValue, existingFrequency);
            int frequency = getFrequency(existingFrequency);
            output.Add(value, frequency);
        }

        return output;
    }

    /// <summary>
    /// Creates a FrequencyMap with updated values from an existing FrequencyMap.
    /// </summary>
    /// <param name="existing">The existing frequency map</param>
    /// <param name="getValue">A method to get the value with, given the existing item and its frequency</param>
    /// <param name="getFrequency">A method to get the frequency with, given the existing item and its frequency</param>
    /// <typeparam name="TExisting">The typeof the existing FrequencyMap</typeparam>
    /// <returns>The new translated frequency map</returns>
    public static FrequencyMap<TValue> From<TExisting>(FrequencyMap<TExisting> existing,
        Func<TExisting, int, TValue> getValue, Func<TExisting, int, int> getFrequency) where TExisting : notnull
    {
        FrequencyMap<TValue> output = new();

        foreach ((TExisting existingValue, int existingFrequency) in existing)
        {
            TValue value = getValue(existingValue, existingFrequency);
            int frequency = getFrequency(existingValue, existingFrequency);
            output.Add(value, frequency);
        }

        return output;
    }

    /// <summary>
    /// Creates a FrequencyMap with updated values from an existing FrequencyMap with non-int frequencies.
    /// </summary>
    /// <param name="existing">The existing frequency map</param>
    /// <param name="getValue">A method to get the value with, given the existing item, its frequency, and its index</param>
    /// <param name="getFrequency">A method to get the frequency with, given the existing item, its frequency, and its index</param>
    /// <typeparam name="TExisting">The typeof the existing FrequencyMap</typeparam>
    /// <typeparam name="TFrequency">The typeof the existing FrequencyMap's frequency</typeparam>
    /// <returns>The new translated frequency map</returns>
    public static FrequencyMap<TValue> From<TExisting, TFrequency>(FrequencyMap<TExisting, TFrequency> existing,
        Func<TExisting, TFrequency, int, TValue> getValue, Func<TExisting, TFrequency, int, int> getFrequency) 
        where TExisting : notnull where TFrequency : struct, INumber<TFrequency>
    {
        FrequencyMap<TValue> output = new();

        using IEnumerator<KeyValuePair<TExisting, TFrequency>> enumerator = existing.GetEnumerator();
        for (int i = 0; enumerator.MoveNext(); i++)
        {
            (TExisting existingValue, TFrequency existingFrequency) = enumerator.Current;
            TValue value = getValue(existingValue, existingFrequency, i);
            int frequency = getFrequency(existingValue, existingFrequency, i);
            output.Add(value, frequency);
        }

        return output;
    }

    /// <summary> 
    /// The indexer for FrequencyMap should not be used as it implies that the indexer is get/set, 
    /// but is actually get-only. Use the <see cref="GetFrequency"/> method instead, since this is just an alias of it.
    /// </summary> 
    [Obsolete("The indexer for FrequencyMap should not be used as it implies that the indexer is get/set, " +
              "but is actually get-only. Use the GetFrequency method instead, since this is just an alias of it.")]
    public int this[TValue value] => GetFrequency(value);

    /// <summary>
    /// Adds the value to the map, and if it already exists increments the frequency.
    /// </summary>
    /// <returns>The new frequency of the value, or -1 if the method fails.</returns>
    public int Add(TValue value)
    {
        if (!_map.TryAdd(value, 1)) ++_map[value];

        return _map[value];
    }

    /// <summary>
    /// Increases the frequency of the value by the provided frequency, or adds it with the specified frequency if it
    /// doesn't exist already.
    /// </summary>
    /// <returns>The new frequency of the value, or -1 if the method fails.</returns>
    public int Add(TValue value, int addedFrequency)
    {
        if (addedFrequency is 0) return -1;
        
        if (!_map.TryAdd(value, addedFrequency)) _map[value] += addedFrequency;
            
        return _map[value];
    }

    /// <inheritdoc cref="Add(TValue,int)"/>
    public void Add(KeyValuePair<TValue, int> pair) => Add(pair.Key, pair.Value);

    /// <summary>
    /// Returns the frequency of the provided value in the map. Returns 0 if the value does not exist.
    /// </summary>
    /// <returns>The frequency of the provided value in the map, or 0 if the value does not exist.</returns>
    [Pure]
    public int GetFrequency(TValue value) => _map.GetValueOrDefault(value, 0);

    /// <summary>
    /// Removes the value entirely from the map. Use <see cref="Decrement"/> if you wish to only decrease the frequency.
    /// </summary>
    /// <returns>true if the element is successfully found and removed; otherwise, false.
    /// This method returns false if value is not found</returns>
    public bool Remove(TValue value) => _map.Remove(value, out _);

    public void CopyTo(KeyValuePair<TValue, int>[] array, int arrayIndex)
    {
        ICollection<KeyValuePair<TValue, int>> map = _map;
        map.CopyTo(array, arrayIndex);
        _map = map.ToDictionary();
    }

    /// <inheritdoc cref="Remove(TValue)"/>
    public bool Remove(KeyValuePair<TValue, int> pair) => Remove(pair.Key);

    /// <inheritdoc cref="IDictionary{TKey,TValue}.Clear"/>
    public void Clear() => _map.Clear();

    /// <summary>
    /// Decrements the frequency of the provided value in the map by one, or removing it if the end result is a
    /// frequency of 0.
    /// </summary>
    /// <returns>The resulting frequency, 0 if the value is removed from the map, and -1 if it doesn't exist.</returns>
    public int Decrement(TValue value)
    {
        int newFrequency = !_map.TryGetValue(value, out var frequency) ? 0 : Math.Max(0, frequency - 1);

        if (newFrequency > 0)
        {
            _map[value] = newFrequency;
        }
        else
        {
            _map.Remove(value);
        }

        return newFrequency;
    }

    /// <param name="value">The key to locate in the map</param>
    /// <returns>true if the map contains an element with the specified key; otherwise, false.</returns>
    public bool Contains(TValue value) => _map.ContainsKey(value);


    /// <summary>
    /// Shorthand of the <see cref="Contains(KeyValuePair{TValue,int},bool)"/> method with <see langword="false"/>
    /// as exact frequency.
    /// <inheritdoc cref="Contains(KeyValuePair{TValue,int},bool)"/>
    /// </summary>
    /// <param name="pair"><inheritdoc cref="Contains(KeyValuePair{TValue,int},bool)"/></param>
    /// <returns><inheritdoc cref="Contains(KeyValuePair{TValue,int},bool)"/></returns>
    public bool Contains(KeyValuePair<TValue, int> pair) => Contains(pair, false);
    
    /// <summary>
    /// Determines whether the dictionary contains the specified key-value pair.
    /// </summary>
    /// <param name="pair">The key-value pair to locate in the dictionary.</param>
    /// <param name="exactFrequency">True when checking for an exact frequency, otherwise checks for at least the frequency</param>
    /// <returns>
    /// <see langword="true"/> if the dictionary contains the specified key-value pair; otherwise, <see langword="false"/>.
    /// </returns>
    public bool Contains(KeyValuePair<TValue, int> pair, bool exactFrequency) =>
        exactFrequency ? _map.Contains(pair) : _map.TryGetValue(pair.Key, out int value) && value >= pair.Value;

    public List<TValue> Values => _map.OrderByDescending(kv => kv.Value).Select(kvp => kvp.Key).ToList();

    /// <summary>
    /// Returns the number of unique values in the map.
    /// </summary>
    public int Count => _map.Count;

    public bool IsReadOnly => false;

    /// <summary>
    /// Returns the sum of the frequencies.
    /// </summary>
    public int FrequencySum => _map.Values.Sum();

    public TValue GetMostFrequent() => _map.MaxBy(kv => kv.Value).Key;

    public TValue GetMostFrequent(out int frequency)
    {
        (TValue value, frequency) = _map.MaxBy(kv => kv.Value);
        return value;
    }

    public List<TValue> GetMostFrequent(int count) =>
        _map.OrderByDescending(kv => kv.Value).Take(count).Select(kv => kv.Key).ToList();

    public FrequencyMap<TValue> GetMostFrequentWithFrequencies(int count) =>
        From(_map.OrderByDescending(kv => kv.Value).Take(count));

    public IEnumerator<KeyValuePair<TValue, int>> GetEnumerator() => _map.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public override string ToString() => ToString(val => val.ToString()!);

    public string ToString(Func<TValue, string> getValueString)
    {
        string output = $"FrequencyMap[{Count}] {{ ({getValueString(_map.First().Key)} => {_map.First().Value})";
        return _map.Skip(1).Aggregate(output, (current, kvp) => 
                   current + $", ({getValueString(kvp.Key)} => {kvp.Value})") + "}";
    }
}

public class FrequencyMap<TValue, TFrequency> : ICollection<KeyValuePair<TValue, TFrequency>>
    where TValue : notnull where TFrequency : struct, INumber<TFrequency>
{
    private Dictionary<TValue, TFrequency> _map = new();

    /// <summary>
    /// Creates a FrequencyMap from an existing list of values.
    /// </summary>
    /// <param name="values">The list of values to use</param>
    /// <param name="getFrequency">The method to get the frequency with, given the existing item and its index</param>
    /// <returns>The new FrequencyMap with populated frequencies</returns>
    public static FrequencyMap<TValue, TFrequency> From(IReadOnlyList<TValue> values, Func<TValue, int, TFrequency> getFrequency)
    {
        FrequencyMap<TValue, TFrequency> output = new();

        for (int i = 0; i < values.Count; i++)
        {
            TValue value = values[i];
            TFrequency frequency = getFrequency(value, i);

            output.Add(value, frequency);
        }

        return output;
    }

    /// <inheritdoc cref="From(Dictionary{TValue,TFrequency})"/>
    public static FrequencyMap<TValue, TFrequency> From(IEnumerable<KeyValuePair<TValue, TFrequency>> dictionaryFreqMap) =>
        From(dictionaryFreqMap.ToDictionary(x => x.Key, y => y.Value));

    /// <summary>
    /// Creates a FrequencyMap from an existing dictionary of values and frequencies.
    /// </summary>
    /// <param name="dictionaryFreqMap">A dictionary containing values and frequencies</param>
    /// <returns>The new translated frequency map</returns>
    public static FrequencyMap<TValue, TFrequency> From(Dictionary<TValue, TFrequency> dictionaryFreqMap)
    {
        FrequencyMap<TValue, TFrequency> output = new();

        foreach ((TValue value, TFrequency frequency) in dictionaryFreqMap) output.Add(value, frequency);

        return output;
    }

    /// <summary>
    /// Creates a FrequencyMap with updated values from an existing FrequencyMap
    /// </summary>
    /// <param name="existing">The existing frequency map</param>
    /// <param name="getValue">A method to get the value with</param>
    /// <typeparam name="TExisting">The typeof the existing FrequencyMap</typeparam>
    /// <returns>The new translated frequency map</returns>
    public static FrequencyMap<TValue, TFrequency> From<TExisting>(FrequencyMap<TExisting, TFrequency> existing,
        Func<TExisting, TValue> getValue)
        where TExisting : notnull
    {
        FrequencyMap<TValue, TFrequency> output = new();

        foreach ((TExisting existingValue, TFrequency frequency) in existing)
        {
            TValue value = getValue(existingValue);
            output.Add(value, frequency);
        }

        return output;
    }

    /// <summary>
    /// Creates a FrequencyMap with updated values from an existing FrequencyMap.
    /// </summary>
    /// <param name="existing">The existing frequency map</param>
    /// <param name="getValue">A method to get the value with, containing the existing item and its frequency</param>
    /// <typeparam name="TExisting">The typeof the existing FrequencyMap</typeparam>
    /// <returns>The new translated frequency map</returns>
    public static FrequencyMap<TValue, TFrequency> From<TExisting>(FrequencyMap<TExisting, TFrequency> existing,
        Func<TExisting, TFrequency, TValue> getValue)
        where TExisting : notnull
    {
        FrequencyMap<TValue, TFrequency> output = new();

        foreach ((TExisting existingValue, TFrequency frequency) in existing)
        {
            TValue value = getValue(existingValue, frequency);
            output.Add(value, frequency);
        }

        return output;
    }

    /// <summary>
    /// Creates a FrequencyMap with updated values from an existing FrequencyMap.
    /// </summary>
    /// <param name="existing">The existing frequency map</param>
    /// <param name="getValue">A method to get the value with, given the existing item and its frequency</param>
    /// <param name="getFrequency">A method to get the frequency with, given the existing frequency</param>
    /// <typeparam name="TExisting">The typeof the existing FrequencyMap</typeparam>
    /// <returns>The new translated frequency map</returns>
    public static FrequencyMap<TValue, TFrequency> From<TExisting>(FrequencyMap<TExisting, TFrequency> existing,
        Func<TExisting, TValue> getValue, Func<TFrequency, TFrequency> getFrequency) where TExisting : notnull
    {
        FrequencyMap<TValue, TFrequency> output = new();

        foreach ((TExisting existingValue, TFrequency existingFrequency) in existing)
        {
            TValue value = getValue(existingValue);
            TFrequency frequency = getFrequency(existingFrequency);
            output.Add(value, frequency);
        }

        return output;
    }
    
    /// <summary>
    /// Creates a FrequencyMap with updated values from an existing FrequencyMap.
    /// </summary>
    /// <param name="existing">The existing frequency map</param>
    /// <param name="getValue">A method to get the value with, given the existing item and its frequency</param>
    /// <param name="getFrequency">A method to get the frequency with, given the existing item and its frequency</param>
    /// <typeparam name="TExisting">The typeof the existing FrequencyMap</typeparam>
    /// <returns>The new translated frequency map</returns>
    public static FrequencyMap<TValue, TFrequency> From<TExisting>(FrequencyMap<TExisting, TFrequency> existing,
        Func<TExisting, TFrequency, TValue> getValue, Func<TFrequency, TFrequency> getFrequency) where TExisting : notnull
    {
        FrequencyMap<TValue, TFrequency> output = new();

        foreach ((TExisting existingValue, TFrequency existingFrequency) in existing)
        {
            TValue value = getValue(existingValue, existingFrequency);
            TFrequency frequency = getFrequency(existingFrequency);
            output.Add(value, frequency);
        }

        return output;
    }

    /// <summary>
    /// Creates a FrequencyMap with updated values from an existing FrequencyMap with non-int frequencies.
    /// </summary>
    /// <param name="existing">The existing frequency map</param>
    /// <param name="getValue">A method to get the value with, given the existing item, its frequency, and its index</param>
    /// <param name="getFrequency">A method to get the frequency with, given the existing item, its frequency, and its index</param>
    /// <typeparam name="TExisting">The typeof the existing FrequencyMap</typeparam>
    /// <typeparam name="TFrequency">The typeof the existing FrequencyMap's frequency</typeparam>
    /// <returns>The new translated frequency map</returns>
    public static FrequencyMap<TValue, TFrequency> From<TExisting>(FrequencyMap<TExisting, TFrequency> existing,
        Func<TExisting, TFrequency, TValue> getValue, Func<TExisting, TFrequency, TFrequency> getFrequency) where TExisting : notnull
    {
        FrequencyMap<TValue, TFrequency> output = new();

        foreach ((TExisting existingValue, TFrequency existingFrequency) in existing)
        {
            TValue value = getValue(existingValue, existingFrequency);
            TFrequency frequency = getFrequency(existingValue, existingFrequency);
            output.Add(value, frequency);
        }

        return output;
    }

    /// <summary> 
    /// The indexer for FrequencyMap should not be used as it implies that the indexer is get/set, 
    /// but is actually get-only. Use the <see cref="GetFrequency"/> method instead, since this is just an alias of it.
    /// </summary> 
    [Obsolete("The indexer for FrequencyMap should not be used as it implies that the indexer is get/set, " +
              "but is actually get-only. Use the GetFrequency method instead, since this is just an alias of it.")]
    public TFrequency this[TValue value] => GetFrequency(value);

    /// <summary>
    /// Adds the value to the map, and if it already exists increments the frequency.
    /// </summary>
    /// <returns>The new frequency of the value, or -1 if the method fails.</returns>
    public TFrequency Add(TValue value)
    {
        if (!_map.TryAdd(value, TFrequency.One)) ++_map[value];

        return _map[value];
    }

    /// <summary>
    /// Increases the frequency of the value by the provided frequency, or adds it with the specified frequency if it
    /// doesn't exist already.
    /// </summary>
    /// <returns>The new frequency of the value, or -1 if the method fails.</returns>
    public TFrequency Add(TValue value, TFrequency addedFrequency)
    {
        if (addedFrequency is 0) return -TFrequency.One;
        
        if (!_map.TryAdd(value, addedFrequency)) _map[value] += addedFrequency;
            
        return _map[value];
    }

    /// <inheritdoc cref="Add(TValue,TFrequency)"/>
    public void Add(KeyValuePair<TValue, TFrequency> pair) => Add(pair.Key, pair.Value);

    /// <summary>
    /// Returns the frequency of the provided value in the map. Returns 0 if the value does not exist.
    /// </summary>
    /// <returns>The frequency of the provided value in the map, or 0 if the value does not exist.</returns>
    [Pure]
    public TFrequency GetFrequency(TValue value) => _map.GetValueOrDefault(value, TFrequency.Zero);

    /// <summary>
    /// Removes the value entirely from the map. Use <see cref="Decrement"/> if you wish to only decrease the frequency.
    /// </summary>
    /// <returns>true if the element is successfully found and removed; otherwise, false.
    /// This method returns false if value is not found</returns>
    public bool Remove(TValue value) => _map.Remove(value, out _);

    public void CopyTo(KeyValuePair<TValue, TFrequency>[] array, int arrayIndex)
    {
        ICollection<KeyValuePair<TValue, TFrequency>> map = _map;
        map.CopyTo(array, arrayIndex);
        _map = map.ToDictionary();
    }

    /// <inheritdoc cref="Remove(TValue)"/>
    public bool Remove(KeyValuePair<TValue, TFrequency> pair) => Remove(pair.Key);

    /// <inheritdoc cref="IDictionary{TKey,TValue}.Clear"/>
    public void Clear() => _map.Clear();

    /// <summary>
    /// Decrements the frequency of the provided value in the map by one, or removing it if the end result is a
    /// frequency of 0.
    /// </summary>
    /// <returns>The resulting frequency, 0 if the value is removed from the map, and -1 if it doesn't exist.</returns>
    public TFrequency Decrement(TValue value)
    {
        TFrequency newFrequency = !_map.TryGetValue(value, out var frequency)
            ? TFrequency.Zero
            : TFrequency.Zero > frequency - TFrequency.One ? TFrequency.Zero : frequency - TFrequency.One;

        if (newFrequency > TFrequency.Zero)
        {
            _map[value] = newFrequency;
        }
        else
        {
            _map.Remove(value);
        }

        return newFrequency;
    }

    /// <param name="value">The key to locate in the map</param>
    /// <returns>true if the map contains an element with the specified key; otherwise, false.</returns>
    public bool Contains(TValue value) => _map.ContainsKey(value);


    /// <summary>
    /// Shorthand of the <see cref="Contains(KeyValuePair{TValue,TFrequency},bool)"/> method with <see langword="false"/>
    /// as exact frequency.
    /// <inheritdoc cref="Contains(KeyValuePair{TValue,TFrequency},bool)"/>
    /// </summary>
    /// <param name="pair"><inheritdoc cref="Contains(KeyValuePair{TValue,TFrequency},bool)"/></param>
    /// <returns><inheritdoc cref="Contains(KeyValuePair{TValue,TFrequency},bool)"/></returns>
    public bool Contains(KeyValuePair<TValue, TFrequency> pair) => Contains(pair, false);
    
    /// <summary>
    /// Determines whether the dictionary contains the specified key-value pair.
    /// </summary>
    /// <param name="pair">The key-value pair to locate in the dictionary.</param>
    /// <param name="exactFrequency">True when checking for an exact frequency, otherwise checks for at least the frequency</param>
    /// <returns>
    /// <see langword="true"/> if the dictionary contains the specified key-value pair; otherwise, <see langword="false"/>.
    /// </returns>
    public bool Contains(KeyValuePair<TValue, TFrequency> pair, bool exactFrequency) =>
        exactFrequency ? _map.Contains(pair) : _map.TryGetValue(pair.Key, out TFrequency value) && value >= pair.Value;

    public List<TValue> Values => _map.OrderByDescending(kv => kv.Value).Select(kvp => kvp.Key).ToList();

    /// <summary>
    /// Returns the number of unique values in the map.
    /// </summary>
    public int Count => _map.Count;

    public bool IsReadOnly => false;

    /// <summary>
    /// Returns the sum of the frequencies.
    /// </summary>
    public TFrequency FrequencySum 
    {
        get
        {
            TFrequency sum = TFrequency.Zero;
            foreach (TFrequency value in _map.Values)
            {
                checked { sum += TFrequency.CreateChecked(value); }
            }

            return sum;
        }
    }

    public TValue GetMostFrequent() => _map.MaxBy(kv => kv.Value).Key;

    public TValue GetMostFrequent(out TFrequency frequency)
    {
        (TValue value, frequency) = _map.MaxBy(kv => kv.Value);
        return value;
    }

    public List<TValue> GetMostFrequent(int count) =>
        _map.OrderByDescending(kv => kv.Value).Take(count).Select(kv => kv.Key).ToList();

    public KeyValuePair<TValue, TFrequency> GetMostFrequentWithFrequencies() =>
        _map.MaxBy(kv => kv.Value);
    
    public FrequencyMap<TValue, TFrequency> GetMostFrequentWithFrequencies(int count) =>
        From(_map.OrderByDescending(kv => kv.Value).Take(count));

    public IEnumerator<KeyValuePair<TValue, TFrequency>> GetEnumerator() => _map.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public override string ToString() => ToString(val => val.ToString()!);

    public string ToString(Func<TValue, string> getValueString)
    {
        string output = $"FrequencyMap[{Count}] {{ ({getValueString(_map.First().Key)} => {_map.First().Value})";
        return _map.Skip(1).Aggregate(output, (current, kvp) => 
                   current + $", ({getValueString(kvp.Key)} => {kvp.Value})") + "}";
    }
}