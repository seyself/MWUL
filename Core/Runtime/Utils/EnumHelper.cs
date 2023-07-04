using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App 
{
    public static class EnumHelper
    {
        public static void ForEach<T>(System.Action<T> action) where T : struct, System.IConvertible
        {
            var type = typeof(T);
            foreach(T v in System.Enum.GetValues(type))
            {
                action((T)v);
            }
        }
    }
}