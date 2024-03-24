using System;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.Fragment.App;
using Microsoft.Maui.Platform;
using AView = Android.Views.View;
using AContext = Android.Content.Context;
using System.Collections.Concurrent;


namespace BSE.Maui.Tabbed.Platforms
{
    internal static class MauiContextExtensions
    {
        public static AppCompatActivity GetActivity(this IMauiContext mauiContext) =>
            (mauiContext.Context?.GetActivity() as AppCompatActivity)
            ?? throw new InvalidOperationException("AppCompatActivity Not Found");

        public static FragmentManager GetFragmentManager(this IMauiContext mauiContext)
        {
            var fragmentManager = mauiContext.Services.GetService<FragmentManager>();

            return fragmentManager
                ?? mauiContext.Context?.GetFragmentManager()
                ?? throw new InvalidOperationException("FragmentManager Not Found");
        }

        public static AView ToPlatform(
            this IView view,
            IMauiContext fragmentMauiContext,
            AContext context,
            LayoutInflater layoutInflater,
            FragmentManager childFragmentManager)
        {
            if (view.Handler?.MauiContext is MauiContext scopedMauiContext)
            {
                // If this handler belongs to a different activity then we need to 
                // recreate the view.
                // If it's the same activity we just update the layout inflater
                // and the fragment manager so that the platform view doesn't recreate
                // underneath the users feet
                if (scopedMauiContext.GetActivity() == context.GetActivity() &&
                    view.Handler.PlatformView is AView platformView)
                {
                    //scopedMauiContext.AddWeakSpecific(layoutInflater);
                    //scopedMauiContext.AddWeakSpecific(childFragmentManager);
                    return platformView;
                }
            }

            return view.ToPlatform(fragmentMauiContext.MakeScoped(layoutInflater: layoutInflater, fragmentManager: childFragmentManager));
        }

        public static IMauiContext MakeScoped(this IMauiContext mauiContext,
            LayoutInflater? layoutInflater = null,
            FragmentManager? fragmentManager = null,
            AContext? context = null,
            bool registerNewNavigationRoot = false)
        {
            var scopedContext = new MauiContextImpl(mauiContext.Services);

            if (layoutInflater != null)
            {
scopedContext.AddWeakSpecific(layoutInflater);
//scopedContext.Services.adds
            }
                

            if (fragmentManager != null)
            {
scopedContext.AddWeakSpecific(fragmentManager);
            }
                

            if (context != null)
            {
scopedContext.AddWeakSpecific(context);
            }
                

            if (registerNewNavigationRoot)
            {
                if (fragmentManager == null)
                {
throw new InvalidOperationException("If you're creating a new Navigation Root you need to use a new Fragment Manager");
                }
                    

               scopedContext.AddSpecific(new NavigationRootManager(scopedContext));
            }

            return scopedContext;
        }

        //class WrappedServiceProvider : IServiceProvider
        //{
        //    readonly ConcurrentDictionary<Type, (object, Func<object, object?>)> _scopeStatic = new();

        //    public WrappedServiceProvider(IServiceProvider serviceProvider)
        //    {
        //        Inner = serviceProvider;
        //    }

        //    public IServiceProvider Inner { get; }

        //    public object? GetService(Type serviceType)
        //    {
        //        if (_scopeStatic.TryGetValue(serviceType, out var scope))
        //        {
        //            var (state, getter) = scope;
        //            return getter.Invoke(state);
        //        }

        //        return Inner.GetService(serviceType);
        //    }

        //    public void AddSpecific(Type type, Func<object, object?> getter, object state)
        //    {
        //        _scopeStatic[type] = (state, getter);
        //    }
        //}
    }
}
