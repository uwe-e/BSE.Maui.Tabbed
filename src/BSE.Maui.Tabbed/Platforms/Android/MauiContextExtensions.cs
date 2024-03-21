using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.Fragment.App;
using Microsoft.Maui.Platform;


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

        //static Android.Views.View ToPlatform(
        //    this IView view,
        //    IMauiContext fragmentMauiContext,
        //    Android.Content.Context context,
        //    LayoutInflater layoutInflater,
        //    FragmentManager childFragmentManager)
        //{
        //    if (view.Handler?.MauiContext is MauiContext scopedMauiContext)
        //    {
        //        // If this handler belongs to a different activity then we need to 
        //        // recreate the view.
        //        // If it's the same activity we just update the layout inflater
        //        // and the fragment manager so that the platform view doesn't recreate
        //        // underneath the users feet
        //        if (scopedMauiContext.GetActivity() == context.GetActivity() &&
        //            view.Handler.PlatformView is View platformView)
        //        {
        //            scopedMauiContext.AddWeakSpecific(layoutInflater);
        //            scopedMauiContext.AddWeakSpecific(childFragmentManager);
        //            return platformView;
        //        }
        //    }

        //    return view.ToPlatform(fragmentMauiContext.MakeScoped(layoutInflater: layoutInflater, fragmentManager: childFragmentManager));
        //}
    }
}
