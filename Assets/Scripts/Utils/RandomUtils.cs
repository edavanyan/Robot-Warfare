using System;
using Random = UnityEngine.Random;

namespace Utils
{
    public static class RandomUtils
    {
        public static T GetRandomValue<T>() where T : Enum
        {
            var values = Enum.GetValues(typeof(T));
            return (T)values.GetValue(Random.Range(0, values.Length));
        }
    }
}
