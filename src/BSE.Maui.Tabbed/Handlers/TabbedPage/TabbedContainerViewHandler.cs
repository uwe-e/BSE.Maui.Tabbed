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

            };

        public static CommandMapper<TabbedContainer, TabbedContainerViewHandler> CommandMapper =
           new CommandMapper<TabbedContainer, TabbedContainerViewHandler>(ViewHandler.ElementCommandMapper);

        public TabbedContainerViewHandler()
            : base(Mapper, CommandMapper)
        {
        }
    }
}
