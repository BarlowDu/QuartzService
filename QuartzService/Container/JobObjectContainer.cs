using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzService.Container
{
    public class JobObjectContainer
    {
        static Hashtable hashTable = new Hashtable();
        public static void Set(string key, object value)
        {
            hashTable[key] = value;
        }

        public static object Get(string key)
        {
            return hashTable[key];
        }

        public static void Clear(Action<DictionaryEntry> itemAction)
        {
            foreach (DictionaryEntry kv in hashTable)
            {
                if (itemAction != null)
                {
                    itemAction(kv);
                }
                hashTable.Remove(kv.Key);

            }
        }
    }
}
