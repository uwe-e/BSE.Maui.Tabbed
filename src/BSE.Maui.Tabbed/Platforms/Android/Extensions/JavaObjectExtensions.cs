using System.Diagnostics.CodeAnalysis;

namespace BSE.Maui.Tabbed.Platforms.Android.Extensions
{
    static class JavaObjectExtensions
    {
        public static bool IsAlive([NotNullWhen(true)] this global::Android.Runtime.IJavaObject? obj)
        {
            if (obj == null)
                return false;

            return !obj.IsDisposed();
        }

        public static bool IsDisposed(this global::Android.Runtime.IJavaObject obj)
        {
            return obj.Handle == IntPtr.Zero;
        }
    }
}
