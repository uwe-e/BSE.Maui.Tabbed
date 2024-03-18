using Microsoft.Maui.Handlers;

#if ANDROID
using PlatformView = BSE.Maui.Tabbed.Platforms.Android.TabbedContainerView;// Android.Views.View;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID && !TIZEN)
using PlatformView = System.Object;
#endif
using TabbedContainer = BSE.Tunes.Maui.Client.Controls.TabbedPageContainer;

namespace BSE.Maui.Tabbed.Handlers
{
    public partial class TabbedContainerViewHandler : ViewHandler<TabbedContainer, PlatformView>
    {
        protected override PlatformView CreatePlatformView()
        {
            var tabbedView = new Platforms.Android.TabbedContainerView(MauiContext);
            tabbedView.SetElement(VirtualView);
            return tabbedView;
        }
    }
}
