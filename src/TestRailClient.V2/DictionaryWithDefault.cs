using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ycode.TestRailClient.V2
{
	internal abstract class DictionaryWithDefault<T> : IReadOnlyDictionary<string, T>
    {
    	private T _defaultValue;

    	IDictionary<string, T> _values;
    	public T this[string key]
        {
        	get
            {
            	if (!_values.TryGetValue(key, out var value))
                {
                	value = _defaultValue;
                }
            	return value;
            }
        }

    	public IEnumerable<string> Keys => _values.Keys;
    	public IEnumerable<T> Values => _values.Values;
    	public int Count => _values.Count;

    	public DictionaryWithDefault()
        {
        }

    	public DictionaryWithDefault(T[] values, T defaultValue, params Func<T, string>[] keyFactories)
        {
        	_values = values
                .SelectMany<T, (string key, T value)>(v => keyFactories.Select(f => (f(v), v)))
                .Distinct(new KeyEqualityComparer<string, T>())
                .ToDictionary<(string key, T value), string, T>(pair => pair.key, pair => pair.value);
        	_defaultValue = defaultValue;
        }

    	public bool ContainsKey(string key)
            => _values.ContainsKey(key);
    	public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
            => _values.GetEnumerator();
    	public bool TryGetValue(string key, out T value)
            => _values.TryGetValue(key, out value);
    	IEnumerator IEnumerable.GetEnumerator()
            => _values.GetEnumerator();

        public abstract IReadOnlyDictionary<int, T> ToDictionaryById();
    }

    class KeyEqualityComparer<K, V> : IEqualityComparer<(K key, V value)>
    {
    	public bool Equals((K key, V value) x, (K key, V value) y)
        {
        	return x.key.Equals(y.key);
        }

        public int GetHashCode((K key, V value) obj)
        {
        	return obj.key.GetHashCode();
        }
    }

    /// <summary>
    /// Dictionary of TestRailStatus by both Name and Label
    /// </summary>
	internal class TestRailStatusDictionary : DictionaryWithDefault<TestRailStatus>
    {
        /// <summary>
        /// Constructor of <see cref="TestRailStatusDictionary"/>
        /// </summary>
        /// <param name="values">An array of <see cref="TestRailStatus" /></param>
        /// <remarks>
        /// Both Name and Label of <see cref="TestRailStatus" /> are used as key.
        ///	It is assumed that every names and labels are unique among the values.
        ///	If the name of one value and the labels of another value,
        ///	no exception will be thrown and either of the values will be regarded as the value for the name.
        /// </remarks>
    	public TestRailStatusDictionary(params TestRailStatus[] values)
            : base(values, TestRailStatus.Dummy, value => value.Name, value => value.Label)
        {
        }

        public override IReadOnlyDictionary<int, TestRailStatus> ToDictionaryById()
            => Values
            .Select<TestRailStatus, (int id, TestRailStatus s)>(s => (s.Id, s))
            .Distinct(new KeyEqualityComparer<int, TestRailStatus>())
            .ToDictionary<(int id, TestRailStatus s), int, TestRailStatus>(pair => pair.id, pair => pair.s);
    }

    /// <summary>
    /// Dictionary of TestRailPriority by both Name and ShortName
    /// </summary>
    /// <remarks>
    /// Both Name and ShortName of <see cref="TestRailPriority" /> are used as key.
    ///	It is assumed that every names and short names are unique among the values.
    ///	If the name of one value and the short name of another value,
    ///	no exception will be thrown and either of the values will be regarded as the value for the name.
    /// </remarks>
	internal class TestRailPriorityDictionary : DictionaryWithDefault<TestRailPriority>
    {
    	public TestRailPriorityDictionary(params TestRailPriority[] values)
            : base(values, TestRailPriority.Dummy, value => value.Name, value => value.ShortName)
        {
        }

        public override IReadOnlyDictionary<int, TestRailPriority> ToDictionaryById()
            => Values
            .Select<TestRailPriority, (int id, TestRailPriority p)>(p => (p.Id, p))
            .Distinct(new KeyEqualityComparer<int, TestRailPriority>())
            .ToDictionary<(int id, TestRailPriority p), int, TestRailPriority>(pair => pair.id, pair => pair.p);
    }
}
