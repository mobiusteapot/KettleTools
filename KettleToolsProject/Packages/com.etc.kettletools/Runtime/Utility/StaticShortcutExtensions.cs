using UnityEngine;
namespace ETC.KettleTools
{
    public static class StaticShortcutExtensions {
        public static T IfNullUse<T>(this T @this, T other) where T : class {
            if(@this == null) return other;
            return @this;
        }
    }
}