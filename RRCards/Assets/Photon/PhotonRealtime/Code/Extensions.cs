
#if UNITY_4_7 || UNITY_5 || UNITY_5_3_OR_NEWER
#define SUPPORTED_UNITY
#endif


namespace Photon.Realtime
{
    using System.Collections;
	using System.Collections.Generic;
    using ExitGames.Client.Photon;

    #if SUPPORTED_UNITY
    using UnityEngine;
    using Debug = UnityEngine.Debug;
    #endif
    #if SUPPORTED_UNITY || NETFX_CORE
    using Hashtable = ExitGames.Client.Photon.Hashtable;
    using SupportClass = ExitGames.Client.Photon.SupportClass;
    #endif
    public static class Extensions
    {
        public static void Merge(this IDictionary target, IDictionary addHash)
        {
            if (addHash == null || target.Equals(addHash))
            {
                return;
            }

            foreach (object key in addHash.Keys)
            {
                target[key] = addHash[key];
            }
        }
        public static void MergeStringKeys(this IDictionary target, IDictionary addHash)
        {
            if (addHash == null || target.Equals(addHash))
            {
                return;
            }

            foreach (object key in addHash.Keys)
            {
                if (key is string)
                {
                    target[key] = addHash[key];
                }
            }
        }
        public static string ToStringFull(this IDictionary origin)
        {
            return SupportClass.DictionaryToString(origin, false);
        }
		public static string ToStringFull<T>(this List<T> data)
		{
			if (data == null) return "null";

			string[] sb = new string[data.Count];
			for (int i = 0; i < data.Count; i++)
			{
				object o = data[i];
				sb[i] = (o != null) ? o.ToString() : "null";
			}

			return string.Join(", ", sb);
		}
        public static string ToStringFull(this object[] data)
        {
            if (data == null) return "null";

            string[] sb = new string[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                object o = data[i];
                sb[i] = (o != null) ? o.ToString() : "null";
            }

            return string.Join(", ", sb);
        }
        public static Hashtable StripToStringKeys(this IDictionary original)
        {
            Hashtable target = new Hashtable();
            if (original != null)
            {
                foreach (object key in original.Keys)
                {
                    if (key is string)
                    {
                        target[key] = original[key];
                    }
                }
            }

            return target;
        }
        public static Hashtable StripToStringKeys(this Hashtable original)
        {
            Hashtable target = new Hashtable();
            if (original != null)
            {
                foreach (DictionaryEntry entry in original)
                {
                    if (entry.Key is string)
                    {
                        target[entry.Key] = original[entry.Key];
                    }
                }
            }

            return target;
        }
        private static readonly List<object> keysWithNullValue = new List<object>();
        public static void StripKeysWithNullValues(this IDictionary original)
        {
            lock (keysWithNullValue)
            {
                keysWithNullValue.Clear();

                foreach (DictionaryEntry entry in original)
                {
                    if (entry.Value == null)
                    {
                        keysWithNullValue.Add(entry.Key);
                    }
                }

                for (int i = 0; i < keysWithNullValue.Count; i++)
                {
                    var key = keysWithNullValue[i];
                    original.Remove(key);
                }
            }
        }
        public static void StripKeysWithNullValues(this Hashtable original)
        {
            lock (keysWithNullValue)
            {
                keysWithNullValue.Clear();

                foreach (DictionaryEntry entry in original)
                {
                    if (entry.Value == null)
                    {
                        keysWithNullValue.Add(entry.Key);
                    }
                }

                for (int i = 0; i < keysWithNullValue.Count; i++)
                {
                    var key = keysWithNullValue[i];
                    original.Remove(key);
                }
            }
        }
        public static bool Contains(this int[] target, int nr)
        {
            if (target == null)
            {
                return false;
            }

            for (int index = 0; index < target.Length; index++)
            {
                if (target[index] == nr)
                {
                    return true;
                }
            }

            return false;
        }
    }
}

