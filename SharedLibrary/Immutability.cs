using System;
using System.Linq;

namespace SharedLibrary
{
    public static class Immutability
    {
        public static T Copy<T>(this T original) where T : new()
        {

            T duplicate = new T();

            foreach (var prop in original.GetType().GetProperties())
                prop.SetValue(duplicate, prop.GetValue(original));

            return duplicate;
        }

        public static void SetValuesFrom<T>(this T original, T newObj) where T : class
        {
            foreach (var prop in original.GetType().GetProperties().Where(p => p.CanWrite))
                prop.SetValue(original, prop.GetValue(newObj));
        }

        public static T With<T>(this T original,
            params Action<T>[] actions) where T : new()
        {

            T duplicate = original.Copy();

            foreach (var action in actions)
                action(duplicate);

            return duplicate;
        }
    }
}
