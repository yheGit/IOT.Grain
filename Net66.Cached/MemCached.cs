using Memcached.ClientLibrary;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Net66.Comm;

namespace Net66.Cached
{
    public class MemCached
    {
        public enum Stats
        {
            Default,
            Reset,
            Malloc,
            Maps,
            Sizes,
            Slabs,
            Items,
            CachedDump,
            Detail
        }
        private static MemcachedClient _MemClient;
        private static MemcachedClient MemClient
        {
            get
            {
                if (string.IsNullOrEmpty(Utils.GetMemCachedUrl))
                {
                    return null;
                }
                if (MemCached.MemClient1 == null)
                {
                    try
                    {
                        MemCached.MemClient1 = new MemcachedClient();
                        MemCached.MemClient1.PoolName=MemCached.GetMainPoolName(Utils.GetMemCachedUrl);
                        MemCached.MemClient1.EnableCompression=false;
                    }
                    catch
                    {
                        return null;
                    }
                }
                return MemCached.MemClient1;
            }
        }
        public static MemcachedClient MemClient1
        {
            get
            {
                return MemCached._MemClient;
            }
            set
            {
                MemCached._MemClient = value;
            }
        }
        

        private static string GetMainPoolName(string serverList)
        {
            if (string.IsNullOrEmpty(serverList))
            {
                return "";
            }
            string[] servers = serverList.Split(new char[]
            {
                ';'
            }).ToArray<string>();
            SockIOPool expr_2F = SockIOPool.GetInstance("mmcache");
            expr_2F.SetServers(servers);
            expr_2F.MaxConnections=10000;
            expr_2F.MinConnections=10;
            expr_2F.SocketConnectTimeout=1000;
            expr_2F.SocketTimeout=100;
            expr_2F.Initialize();
            return "mmcache";
        }


        public static bool IsExists(string key)
        {
            if (MemCached.MemClient == null)
                return false;
            return MemCached.MemClient.KeyExists(key);
        }


        public static bool AddCache(string key, string value, int minutes)
        {
            return MemCached.MemClient != null && MemCached.MemClient.Add(key, value, DateTime.Now.AddMinutes((double)minutes));
        }
        public static bool SetCacheByJson(string key, object value, int minutes)
        {
            if (MemCached.MemClient == null)
            {
                return false;
            }
            string value2 = JsonConvert.SerializeObject(value);
            return MemCached.SetCache(key, value2, minutes);
        }
        public static bool SetCacheByJsonBySecond(string key, object value, int Second)
        {
            if (MemCached.MemClient == null)
            {
                return false;
            }
            string text = JsonConvert.SerializeObject(value);
            return MemCached.MemClient.Set(key, text, DateTime.Now.AddSeconds((double)Second));
        }
        public static bool SetCache(string key, object value, int minutes)
        {
            return MemCached.MemClient != null && MemCached.MemClient.Set(key, value, DateTime.Now.AddMinutes((double)minutes));
        }
        public static bool SetCacheBySecond(string key, object value, int Second)
        {
            if (MemCached.MemClient == null)
            {
                return false;
            }
            return MemCached.MemClient.Set(key, value, DateTime.Now.AddSeconds((double)Second));
        }


        public static bool ReplaceCache(string key, string value, int minutes)
        {
            return MemCached.MemClient != null && MemCached.MemClient.Replace(key, value, DateTime.Now.AddMinutes((double)minutes));
        }
        public static T GetCacheByJson<T>(string key)
        {
            object cache = MemCached.GetCache(key);
            if (cache == null)
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(cache.ToString());
        }
        public static int GetCacheByInt(string key)
        {
            if (MemCached.MemClient == null)
            {
                return 0;
            }
            object obj = MemCached.MemClient.Get(key);
            if (obj == null)
            {
                return 0;
            }
            return TypeParse.StrToInt(obj, 0);
        }
        public static object GetCache(string key)
        {
            if (MemCached.MemClient == null)
            {
                return null;
            }
            return MemCached.MemClient.Get(key);
        }

        public static string GetCacheByString(string key)
        {
            if (MemCached.MemClient == null)
            {
                return null;
            }
            var obj=MemCached.MemClient.Get(key);
            if (obj == null)
            {
                return "";
            }
            return obj.ToString();
        }
        public static Hashtable GetCacheHt(string[] keys)
        {
            if (MemCached.MemClient == null)
            {
                return null;
            }
            return MemCached.MemClient.GetMultiple(keys);
        }
        public static object[] GetCacheList(string[] keys)
        {
            if (MemCached.MemClient == null)
            {
                return null;
            }
            return MemCached.MemClient.GetMultipleArray(keys);
        }
        public static bool DelCache(string key)
        {
            return MemCached.MemClient != null && MemCached.MemClient.Delete(key);
        }
        public static bool DelCacheStartsWithByKey(string key)
        {
            if (MemCached.MemClient == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
            List<string> list = (
                from p in MemCached.GetAllKeys()
                where p.StartsWith(key)
                select p).ToList<string>();
            if (list == null || list.Count<string>() <= 0)
            {
                return false;
            }
            foreach (string current in list)
            {
                MemCached.MemClient.Delete(current);
            }
            return true;
        }
        public static bool FlushAll()
        {
            return MemCached.MemClient != null && MemCached.MemClient.FlushAll();
        }
        public static IList<string> GetStats(IList<string> serverArrayList, MemCached.Stats statsCommand, string param)
        {
            if (MemCached.MemClient == null)
            {
                return null;
            }
            IList<string> list = new List<string>();
            if (param == null)
            {
                param = "";
            }
            else
            {
                param = param.Trim().ToLower();
            }
            string text = "stats";
            switch (statsCommand)
            {
                case MemCached.Stats.Reset:
                    text = "stats reset";
                    break;
                case MemCached.Stats.Malloc:
                    text = "stats malloc";
                    break;
                case MemCached.Stats.Maps:
                    text = "stats maps";
                    break;
                case MemCached.Stats.Sizes:
                    text = "stats sizes";
                    break;
                case MemCached.Stats.Slabs:
                    text = "stats slabs";
                    break;
                case MemCached.Stats.Items:
                    text = "stats items";
                    break;
                case MemCached.Stats.CachedDump:
                    {
                        string[] array = param.Split(new char[]
                        {
                    ' '
                        });
                        if (array.Length == 2 && MemCached.IsNumericArray(array))
                        {
                            text = "stats cachedump  " + param;
                        }
                        break;
                    }
                case MemCached.Stats.Detail:
                    if (string.Equals(param, "on") || string.Equals(param, "off") || string.Equals(param, "dump"))
                    {
                        text = "stats detail " + param.Trim();
                    }
                    break;
                default:
                    text = "stats";
                    break;
            }
            ArrayList arrayList = new ArrayList(serverArrayList.ToArray<string>());
            Hashtable hashtable = MemCached.MemClient.Stats(arrayList, text);
            foreach (string text2 in hashtable.Keys)
            {
                list.Add("server:__:" + text2);
                Hashtable hashtable2 = (Hashtable)hashtable[text2];
                foreach (string text3 in hashtable2.Keys)
                {
                    list.Add(text3 + ":" + hashtable2[text3]);
                }
            }
            return list;
        }
        public static bool IsNumericArray(string[] strNumber)
        {
            if (strNumber == null)
            {
                return false;
            }
            if (strNumber.Length < 1)
            {
                return false;
            }
            for (int i = 0; i < strNumber.Length; i++)
            {
                if (!MemCached.IsNumeric(strNumber[i]))
                {
                    return false;
                }
            }
            return true;
        }
        public static bool IsNumeric(string expression)
        {
            return expression != null && expression.Length > 0 && expression.Length <= 11 && Regex.IsMatch(expression, "^[-]?[0-9]*[.]?[0-9]*$") && (expression.Length < 10 || (expression.Length == 10 && expression[0] == '1') || (expression.Length == 11 && expression[0] == '-' && expression[1] == '1'));
        }
        public static IList<string> GetAllKeys()
        {
            List<string> list = new List<string>();
            if (string.IsNullOrEmpty(Utils.GetMemCachedUrl))
            {
                return null;
            }
            list.Add(Utils.GetMemCachedUrl);
            IList<int> list2 = new List<int>();
            using (IEnumerator<string> enumerator = MemCached.GetStats(list, MemCached.Stats.Items, null).GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    string[] array = enumerator.Current.Split(new char[]
                    {
                        ':'
                    });
                    if (array.Length > 1)
                    {
                        int num = 0;
                        if (!(array[1] == "__"))
                        {
                            int.TryParse(array[1], out num);
                            if (num > 0)
                            {
                                bool flag = false;
                                using (IEnumerator<int> enumerator2 = list2.GetEnumerator())
                                {
                                    while (enumerator2.MoveNext())
                                    {
                                        if (enumerator2.Current == num)
                                        {
                                            flag = true;
                                            break;
                                        }
                                    }
                                }
                                if (!flag)
                                {
                                    list2.Add(num);
                                }
                            }
                        }
                    }
                }
            }
            IList<string> list3 = new List<string>();
            foreach (int current in list2)
            {
                using (IEnumerator<string> enumerator = MemCached.GetStats(list, MemCached.Stats.CachedDump, current + " 0").GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        string[] array2 = enumerator.Current.Split(new char[]
                        {
                            ':'
                        });
                        if (array2.Length > 1 && !(array2[1] == "__"))
                        {
                            list3.Add(array2[0]);
                        }
                    }
                }
            }
            return list3;
        }
    }
}
