using Microsoft.Maui.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            //var v = new Android.Views.View(MauiContext);
            var v = new Platforms.Android.TabbedContainerView(Context);

            return v;

            throw new NotImplementedException();
        }
    }
}
