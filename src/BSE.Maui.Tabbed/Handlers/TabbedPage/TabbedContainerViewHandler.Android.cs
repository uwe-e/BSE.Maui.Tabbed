using Android.Views;
using AndroidX.Core.View;
using BSE.Maui.Tabbed.Platforms.Android;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;


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

        private static void MapCurrentPage(TabbedContainerViewHandler handler, TabbedContainer container)
        {
            if (handler.PlatformView is TabbedContainerView tabbedView)
            {
                tabbedView.ScrollToCurrentPage();
            }
        }

        private static void MapBottomView(TabbedContainerViewHandler handler, TabbedContainer container)
        {
            if (handler.PlatformView is TabbedContainerView tabbedView)
            {
                var contentControl = container.BottomView;
                if (contentControl != null)
                {
                    var view = contentControl.ToPlatform(handler.MauiContext);
                    view.Layout(0, 0, 0, (int)contentControl.HeightRequest);
                    tabbedView.SetBottomView(view);
                }

            }
        }

        private static void MapBarBackgroundColor(TabbedContainerViewHandler handler, TabbedContainer container)
        {
            if (handler.PlatformView is TabbedContainerView tabbedView)
            {
                tabbedView.UpdateBarBackgroundColor();
            }
        }

        private static void MapSelectedTabColor(TabbedContainerViewHandler handler, TabbedContainer container)
        {
            if (handler.PlatformView is TabbedContainerView tabbedView)
            {
                tabbedView.UpdateTabItemStyle();
            }
        }

        private static void MapUnselectedTabColor(TabbedContainerViewHandler handler, TabbedContainer container)
        {
            if (handler.PlatformView is TabbedContainerView tabbedView)
            {
                tabbedView.UpdateTabItemStyle();
            }
        }

        public static void MapIsSwipePagingEnabled(TabbedContainerViewHandler handler, TabbedContainer container)
        {
            if (handler.PlatformView is TabbedContainerView tabbedView)
            {
                tabbedView.UpdateSwipePaging();
            }
        }
    }
}
