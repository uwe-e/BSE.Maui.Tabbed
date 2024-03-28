using FormsElement = BSE.Tunes.Maui.Client.Controls.TabbedPageContainer;

namespace BSE.Maui.Tabbed.Platforms.AndroidSpecific
{
    internal static class TabbedPageContainer
    {
        public static readonly BindableProperty IsSwipePagingEnabledProperty =
            BindableProperty.Create("IsSwipePagingEnabled", typeof(bool),
            typeof(TabbedPageContainer), true);

        public static bool GetIsSwipePagingEnabled(BindableObject element)
        {
            return (bool)element.GetValue(IsSwipePagingEnabledProperty);
        }

        public static bool IsSwipePagingEnabled(this IPlatformElementConfiguration<Microsoft.Maui.Controls.PlatformConfiguration.Android, FormsElement> config)
        {
            return GetIsSwipePagingEnabled(config.Element);
        }
    }
}
