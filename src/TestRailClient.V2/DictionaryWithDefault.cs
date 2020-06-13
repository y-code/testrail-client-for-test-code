using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ycode.TestRailClient.V2
{
	internal class DictionaryWithDefault<K, V> : IReadOnlyDictionary<K, V>
    {
    	private V _defaultValue;

    	IDictionary<K, V> _values;
    	public V this[K key]
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

    	public IEnumerable<K> Keys => _values.Keys;
    	public IEnumerable<V> Values => _values.Values;
    	public int Count => _values.Count;

    	public DictionaryWithDefault()
        {
        }

    	public DictionaryWithDefault(V[] values, V defaultValue, params Func<V, K>[] keyFactories)
        {
        	_values = values
                .SelectMany<V, (K key, V value)>(v => keyFactories.Select(f => (f(v), v)))
                .Distinct(new KeyEqualityComparer<K, V>())
                .ToDictionary<(K key, V value), K, V>(pair => pair.key, pair => pair.value);
        	_defaultValue = defaultValue;
        }

    	public bool ContainsKey(K key)
            => _values.ContainsKey(key);
    	public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
            => _values.GetEnumerator();
    	public bool TryGetValue(K key, out V value)
            => _values.TryGetValue(key, out value);
    	IEnumerator IEnumerable.GetEnumerator()
            => _values.GetEnumerator();
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

    internal class DictionaryByIdAndName<V>
    {
        public V[] Values { get; }
        public Lazy<DictionaryWithDefault<int, V>> _byId { get; }
        public DictionaryWithDefault<int, V> ById => _byId.Value;
        public Lazy<DictionaryWithDefault<string, V>> _byName { get; }
        public DictionaryWithDefault<string, V> ByName => _byName.Value;
        public DictionaryByIdAndName(
            V[] values,
            V defaultValue,
            Func<V, int> idFactory,
            params Func<V, string>[] nameFactories)
        {
            Values = values;
            _byId = new Lazy<DictionaryWithDefault<int, V>>(
                () => new DictionaryWithDefault<int, V>(Values, defaultValue, idFactory));
            _byName = new Lazy<DictionaryWithDefault<string, V>>(
                () => new DictionaryWithDefault<string, V>(Values, defaultValue, nameFactories));
        }
    }

    /// <summary>
    /// Dictionary of TestRailStatus by both Name and Label
    /// </summary>
	internal class TestRailStatusDictionary : DictionaryByIdAndName<TestRailStatus>
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
            : base(values, TestRailStatus.Dummy, value => value.Id, value => value.Name, value => value.Label)
        {
        }
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
	internal class TestRailPriorityDictionary : DictionaryByIdAndName<TestRailPriority>
    {
    	public TestRailPriorityDictionary(params TestRailPriority[] values)
            : base(values, TestRailPriority.Dummy, value => value.Id, value => value.Name, value => value.ShortName)
        {
        }
    }
}
