using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSA.Dictionary
{
   /// <summary>
   /// A hash map/dictionary that uses an array as a backing store for 
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <typeparam name="K"></typeparam>
   public class ArrayDictionary<T, K> : IDictionary<T, K>
   {
      private sealed class Bucket
      {
         private readonly IList<KeyValuePair<T, K>> _chain;
         private readonly EqualityComparer<T> _keyEquator;
         private readonly EqualityComparer<K> _valueEquator;

         public Bucket(int keyCode)
         {
            KeyCode = keyCode;
            _chain = new List<KeyValuePair<T, K>>();
            _keyEquator = EqualityComparer<T>.Default;
            _valueEquator = EqualityComparer<K>.Default;
         }

         public IEnumerable<KeyValuePair<T, K>> Pairs
         {
            get { return _chain; }
         }

         public IEnumerable<T> Keys
         {
            get { return _chain.Select(t => t.Key); }
         }

         public IEnumerable<K> Values
         {
            get { return _chain.Select(t => t.Value); }
         }

         public int KeyCode { get; private set; }

         public int Count { get { return _chain.Count; } }

         public void Clear()
         {
            _chain.Clear();
         }

         private bool RemoveCore(Predicate<KeyValuePair<T, K>> shouldRemove)
         {
            bool mustRemove = false;
            int indexToRemoveAt = 0;
            foreach (var i in _chain)
            {
               if (shouldRemove.Invoke(i))
               {
                  mustRemove = true;
                  break;
               }

               indexToRemoveAt++;
            }

            if (mustRemove)
            {
               _chain.RemoveAt(indexToRemoveAt);
            }

            return mustRemove;
         }

         private bool CompareKeyOnly(KeyValuePair<T, K> firstKvp, KeyValuePair<T, K> secondKvp)
         {
            return _keyEquator.Equals(firstKvp.Key, secondKvp.Key);
         }

         public bool Remove(T key)
         {
            Predicate<KeyValuePair<T, K>> predicate = item => CompareKeyOnly(new KeyValuePair<T, K>(key, default(K)), item);
            return RemoveCore(predicate);
         }

         private bool CompareKeyAndValue(KeyValuePair<T, K> firstKvp, KeyValuePair<T, K> secondKvp)
         {
            return _keyEquator.Equals(firstKvp.Key, secondKvp.Key) && _valueEquator.Equals(firstKvp.Value, secondKvp.Value);
         }

         public bool Remove(KeyValuePair<T, K> kvp)
         {
            Predicate<KeyValuePair<T, K>> predicate = item => CompareKeyAndValue(kvp, item);
            return RemoveCore(predicate);
         }

         public bool Contains(T t)
         {
            return _chain.Any(i => _keyEquator.Equals(t, i.Key));
         }

         public void AddOrReplace(KeyValuePair<T, K> kvp)
         {
            bool needsDelete = false;
            int indexToRemove = -1;
            for (int i = 0; i < _chain.Count; i++)
            {
               var focusKey = _chain[i].Key;
               if (_keyEquator.Equals(kvp.Key, focusKey))
               {
                  indexToRemove = i;
                  needsDelete = true;
                  break;
               }
            }

            if (needsDelete)
            {
               _chain.RemoveAt(indexToRemove);
            }

            _chain.Add(kvp);
         }

         public bool TryGet(T t, out K k)
         {
            k = default(K);

            for (int i = 0; i < _chain.Count; i++)
            {
               var currentKvp = _chain[i];

               if (_keyEquator.Equals(t, currentKvp.Key))
               {
                  k = currentKvp.Value;
                  return true;
               }
            }

            return false;
         }

      }

      private readonly int _bucketCount;
      private readonly Bucket[] _buckets;

      /// <summary>
      /// Initializes a new instance of the <see cref="ArrayDictionary{T, K}"/> class.
      /// </summary>
      /// <param name="seed">The initial enumerable key-value pairs to add to the map.</param>
      /// <param name="bucketCount">The bucket count.</param>
      public ArrayDictionary(IEnumerable<KeyValuePair<T, K>> seed, int bucketCount)
      {
         _buckets = new Bucket[bucketCount];
         _bucketCount = bucketCount;

         foreach (var item in seed)
         {
            Add(item);
         }
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ArrayDictionary{T, K}"/> class.
      /// </summary>
      /// <param name="bucketCount">The bucket count.</param>
      public ArrayDictionary(int bucketCount)
      {
         _buckets = new Bucket[bucketCount];
         _bucketCount = bucketCount;
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ArrayDictionary{T, K}"/> class.
      /// </summary>
      public ArrayDictionary()
         : this(50)
      {

      }

      private Bucket GetOrCreateBucket(T t)
      {
         int hashCode = t.GetHashCode();
         int index = Math.Abs(hashCode % _bucketCount);
         Bucket bucket = _buckets[index];

         if (bucket == default(Bucket))
         {
            bucket = new Bucket(hashCode);
            _buckets[index] = bucket;
         }

         return bucket;
      }

      #region Implementation of IEnumerable

      /// <summary>
      /// Returns an enumerator that iterates through the collection.
      /// </summary>
      /// <returns>
      /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
      /// </returns>
      public IEnumerator<KeyValuePair<T, K>> GetEnumerator()
      {
         foreach(var bucket in _buckets)
         {
            if (bucket != null)
            {
               foreach(var kvp in bucket.Pairs)
               {
                  yield return kvp;
               }
            }
         }
      }

      /// <summary>
      /// Returns an enumerator that iterates through a collection.
      /// </summary>
      /// <returns>
      /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
      /// </returns>
      IEnumerator IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }

      #endregion

      #region Implementation of ICollection<KeyValuePair<T,K>>

      /// <summary>
      /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
      /// </summary>
      /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
      public void Add(KeyValuePair<T, K> item)
      {
         Bucket b = GetOrCreateBucket(item.Key);
         if (b.Contains(item.Key))
         {
            throw new ArgumentException("Key already exists in dictionary");
         }

         b.AddOrReplace(item);
      }

      /// <summary>
      /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
      /// </summary>
      /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. </exception>
      public void Clear()
      {
         foreach (var i in _buckets)
         {
            if (i != null)
            {
               i.Clear();
            }
         }
      }

      /// <summary>
      /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
      /// </summary>
      /// <returns>
      /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
      /// </returns>
      /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
      public bool Contains(KeyValuePair<T, K> item)
      {
         Bucket b = GetOrCreateBucket(item.Key);
         K k;
         if (b.TryGet(item.Key, out k))
         {
            return EqualityComparer<K>.Default.Equals(item.Value, k);
         }

         return false;
      }

      /// <summary>
      /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
      /// </summary>
      /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param><param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception><exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.</exception>
      public void CopyTo(KeyValuePair<T, K>[] array, int arrayIndex)
      {
         throw new System.NotImplementedException();
      }

      /// <summary>
      /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
      /// </summary>
      /// <returns>
      /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
      /// </returns>
      /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
      public bool Remove(KeyValuePair<T, K> item)
      {
         Bucket b = GetOrCreateBucket(item.Key);
         return b.Remove(item);
      }

      /// <summary>
      /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
      /// </summary>
      /// <returns>
      /// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
      /// </returns>
      public int Count
      {
         get 
         {
            int count = 0;
            foreach(var b in _buckets)
            {
               if(b != null)
               {
                  count += b.Count;
               }
            }

            return count;
         }
      }

      /// <summary>
      /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
      /// </summary>
      /// <returns>
      /// true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.
      /// </returns>
      public bool IsReadOnly
      {
         get { return false; }
      }

      #endregion

      #region Implementation of IDictionary<T,K>

      /// <summary>
      /// Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the specified key.
      /// </summary>
      /// <returns>
      /// true if the <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the key; otherwise, false.
      /// </returns>
      /// <param name="key">The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.</param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception>
      public bool ContainsKey(T key)
      {
         var bucket = GetOrCreateBucket(key);
         return bucket.Contains(key);
      }

      /// <summary>
      /// Adds an element with the provided key and value to the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
      /// </summary>
      /// <param name="key">The object to use as the key of the element to add.</param><param name="value">The object to use as the value of the element to add.</param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception><exception cref="T:System.ArgumentException">An element with the same key already exists in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.</exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IDictionary`2"/> is read-only.</exception>
      public void Add(T key, K value)
      {
         Add(new KeyValuePair<T, K>(key, value));
      }

      /// <summary>
      /// Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
      /// </summary>
      /// <returns>
      /// true if the element is successfully removed; otherwise, false.  This method also returns false if <paramref name="key"/> was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2"/>.
      /// </returns>
      /// <param name="key">The key of the element to remove.</param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IDictionary`2"/> is read-only.</exception>
      public bool Remove(T key)
      {
         var bucket = GetOrCreateBucket(key);
         return bucket.Remove(key);
      }

      /// <summary>
      /// Gets the value associated with the specified key.
      /// </summary>
      /// <returns>
      /// true if the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the specified key; otherwise, false.
      /// </returns>
      /// <param name="key">The key whose value to get.</param><param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value"/> parameter. This parameter is passed uninitialized.</param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception>
      public bool TryGetValue(T key, out K value)
      {
         var bucket = GetOrCreateBucket(key);
         return bucket.TryGet(key, out value);
      }

      /// <summary>
      /// Gets or sets the element with the specified key.
      /// </summary>
      /// <returns>
      /// The element with the specified key.
      /// </returns>
      /// <param name="key">The key of the element to get or set.</param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception><exception cref="T:System.Collections.Generic.KeyNotFoundException">The property is retrieved and <paramref name="key"/> is not found.</exception><exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IDictionary`2"/> is read-only.</exception>
      public K this[T key]
      {
         get 
         {
            var bucket = GetOrCreateBucket(key);
            K value;
            if (bucket.TryGet(key, out value))
            {
               return value;
            }

            throw new InvalidOperationException("Key not found in dictionary");
         }
         set
         {
            var bucket = GetOrCreateBucket(key);
            bucket.AddOrReplace(new KeyValuePair<T, K>(key, value));
         }
      }

      /// <summary>
      /// Gets an <see cref="T:System.Collections.Generic.ICollection`1"/> containing the keys of the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
      /// </summary>
      /// <returns>
      /// An <see cref="T:System.Collections.Generic.ICollection`1"/> containing the keys of the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/>.
      /// </returns>
      public ICollection<T> Keys
      {
         get { return new List<T>(_buckets.SelectMany(item => item.Keys)); }
      }

      /// <summary>
      /// Gets an <see cref="T:System.Collections.Generic.ICollection`1"/> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
      /// </summary>
      /// <returns>
      /// An <see cref="T:System.Collections.Generic.ICollection`1"/> containing the values in the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/>.
      /// </returns>
      public ICollection<K> Values
      {
         get { return new List<K>(_buckets.SelectMany(item => item.Values)); }
      }

      #endregion
   }
}
