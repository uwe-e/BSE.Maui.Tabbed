using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.PlatformConfiguration;
using FormsElement = BSE.Tunes.Maui.Client.Controls.TabbedPageContainer;

namespace BSE.Maui.Tabbed.Platforms.AndroidSpecific
{
    internal static class TabbedPageContainer
    {
        public static readonly BindableProperty IsSmoothScrollEnabledProperty =
            BindableProperty.Create("IsSmoothScrollEnabled", typeof(bool),
            typeof(TabbedPageContainer), true);

        public static bool GetIsSmoothScrollEnabled(BindableObject element)
        {
            return (bool)element.GetValue(IsSmoothScrollEnabledProperty);
        }

        public static bool IsSmoothScrollEnabled(this IPlatformElementConfiguration<Microsoft.Maui.Controls.PlatformConfiguration.Android, FormsElement> config)
        {
            return GetIsSmoothScrollEnabled(config.Element);
        }
    }
}
