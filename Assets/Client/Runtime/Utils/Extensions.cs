using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Random = UnityEngine.Random;

namespace Client.Runtime.Utils
{
    public static class Extensions
    {
        public static void Shuffle<T>(this T[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                T temp = array[i];
                int random = Random.Range(i, array.Length);
                array[i] = array[random];
                array[random] = temp;
            }
        }

        public static void Shuffle<T>(this List<T> array)
        {
            for (int i = 0; i < array.Count; i++)
            {
                T temp = array[i];
                int random = Random.Range(i, array.Count);
                array[i] = array[random];
                array[random] = temp;
            }
        }

        public static T GetRandomElement<T>(this T[] source)
        {
            source.Shuffle();
            return source[0];
        }

        public static T GetRandomElement<T>(this List<T> source)
        {
            source.Shuffle();
            return source[0];
        }

        public static T GetRandomElement<T>(this IEnumerable<T> source)
        {
            var newSource = source.ToArray();
            newSource.Shuffle();
            return newSource[0];
        }

        public static IEnumerable<T> GetRandomElements<T>(this IEnumerable<T> source, int count)
        {
            var newSource = source.ToArray();
            newSource.Shuffle();
            return newSource.Take(count);
        }

        public static void ForEach<T>(this List<T> source, Action<T> action)
        {
            foreach (var i in source)
            {
                action.Invoke(i);
            }
        }

        public static void ForEach<T>(this T[] source, Action<T> action)
        {
            foreach (var i in source)
            {
                action.Invoke(i);
            }
        }

        public static T GetUniqueByIndex<T>(ref int index, T[] array)
        {
            if (index <= array.Length - 1)
                return array[index++];

            index = 0;
            array.Shuffle();

            return array[index++];
        }

        public static T GetUniqueByIndex<T>(ref int index, List<T> array)
        {
            if (index <= array.Count - 1)
                return array[index++];

            index = 0;
            array.Shuffle();

            return array[index++];
        }

        public static void EnqueueRange<T>(this Queue<T> queue, IEnumerable<T> enumerable)
        {
            foreach (var item in enumerable)
                queue.Enqueue(item);
        }

        public static T GetElementWithMinValue<T>(this List<T> list, Func<T, float> predicate)
        {
            var min = list[0];

            for (int i = 1, len = list.Count; i < len; i++)
            {
                var v1 = predicate.Invoke(min);
                var v2 = predicate.Invoke(list[i]);

                if (v2 < v1)
                {
                    min = list[i];
                }
            }

            return min;
        }

        public static List<T> WhereCached<T>(this List<T> list, List<T> cacheList, Predicate<T> predicate)
        {
            cacheList.Clear();

            for (int i = 0, len = list.Count; i < len; i++)
            {
                var item = list[i];

                if (predicate.Invoke(item))
                {
                    cacheList.Add(item);
                }
            }

            return cacheList;
        }

        public static void SetLayerRecursively(this Transform parent, int layer)
        {
            parent.gameObject.layer = layer;

            for (int i = 0, count = parent.childCount; i < count; i++)
            {
                parent.GetChild(i).SetLayerRecursively(layer);
            }
        }

        public static T FirstNonAlloc<T>(this List<T> collection, Func<T, bool> predicate)
        {
            for (int i = 0, len = collection.Count; i < len; i++)
            {
                if (predicate.Invoke(collection[i]))
                {
                    return collection[i];
                }
            }

            throw new Exception();
        }

        public static T FirstNonAlloc<T>(this T[] collection, Func<T, bool> predicate)
        {
            for (int i = 0, len = collection.Length; i < len; i++)
            {
                if (predicate.Invoke(collection[i]))
                {
                    return collection[i];
                }
            }

            throw new Exception();
        }

        public static T FirstOrDefaultNonAlloc<T>(this List<T> collection, Func<T, bool> predicate)
        {
            for (int i = 0, len = collection.Count; i < len; i++)
            {
                if (predicate.Invoke(collection[i]))
                {
                    return collection[i];
                }
            }

            return default;
        }

        public static Vector2 ToVector2(this Vector2Int vector)
        {
            return new Vector2(vector.x, vector.y);
        }

        public static Vector2 ToVector2(this Vector3Int vector)
        {
            return new Vector2(vector.x, vector.y);
        }

        public static T WrapAndGet<T>(this T[] array, int index)
        {
            return array[(index % array.Length + array.Length) % array.Length];
        }

        public static T WrapAndGet<T>(this List<T> list, int index)
        {
            return list[(index % list.Count + list.Count) % list.Count];
        }
    }
}