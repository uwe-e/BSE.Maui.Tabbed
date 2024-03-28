#nullable disable

using Microsoft.Maui.Handlers;
using TabbedContainer = BSE.Tunes.Maui.Client.Controls.TabbedPageContainer;

namespace BSE.Maui.Tabbed.Handlers
{
    public partial class TabbedContainerViewHandler
    {
        public static PropertyMapper<TabbedContainer, TabbedContainerViewHandler> Mapper =
            new PropertyMapper<TabbedContainer, TabbedContainerViewHandler>(ElementMapper)
            {
#if ANDROID
                [nameof(TabbedContainer.CurrentPage)] = MapCurrentPage,
                [nameof(TabbedContainer.BottomView)] = MapBottomView,
                [nameof(TabbedContainer.BarBackgroundColor)] = MapBarBackgroundColor,
                [nameof(TabbedContainer.UnselectedTabColor)] = MapUnselectedTabColor,
                [nameof(TabbedContainer.SelectedTabColor)] = MapSelectedTabColor,

                //[nameof(Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific.TabbedPage.IsSwipePagingEnabledProperty.PropertyName)] = MapIsSwipePagingEnabled
                [nameof(BSE.Maui.Tabbed.Platforms.AndroidSpecific.TabbedPageContainer.IsSwipePagingEnabledProperty.PropertyName)] = MapIsSwipePagingEnabled
#endif
            };

        public static CommandMapper<TabbedContainer, TabbedContainerViewHandler> CommandMapper =
           new CommandMapper<TabbedContainer, TabbedContainerViewHandler>(ViewHandler.ElementCommandMapper);

        public TabbedContainerViewHandler()
            : base(Mapper, CommandMapper)
        {
        }
    }
}
