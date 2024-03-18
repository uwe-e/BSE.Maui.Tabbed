#nullable disable

using Android.Content;
using Android.Views;
using BSE.Maui.Tabbed.Platforms.Android.Extensions;
using Google.Android.Material.BottomNavigation;
using Microsoft.Maui.Controls.Platform;

namespace BSE.Maui.Tabbed.Platforms
{
    public static class BottomNavigationViewUtils
    {
        const int MoreTabId = 99;

        public static async void SetupMenu(
            IMenu menu,
            int maxBottomItems,
            List<(string title, ImageSource icon, bool tabEnabled)> items,
            int currentIndex,
            BottomNavigationView bottomView,
            IMauiContext mauiContext)
        {
            Context context = mauiContext.Context;

            while (items.Count < menu.Size())
            {
                menu.RemoveItem(menu.GetItem(menu.Size() - 1).ItemId);
            }

            int numberOfMenuItems = items.Count;
            bool showMore = numberOfMenuItems > maxBottomItems;
            int end = showMore ? maxBottomItems - 1 : numberOfMenuItems;

            List<IMenuItem> menuItems = new List<IMenuItem>();
            List<Task> loadTasks = new List<Task>();
            for (int i = 0; i < end; i++)
            {
                var item = items[i];

                IMenuItem menuItem;
                if (i >= menu.Size())
                    loadTasks.Add(SetupMenuItem(item, menu, i, currentIndex, bottomView, mauiContext, out menuItem));
                else
                {
                    menuItem = menu.GetItem(i);
                    SetMenuItemTitle(menuItem, item.title);
                    loadTasks.Add(SetMenuItemIcon(menuItem, item.icon, mauiContext));
                }

                menuItems.Add(menuItem);
            }

            if (showMore)
            {
                var moreString = context.Resources.GetText(_Microsoft.Android.Resource.Designer.ResourceConstant.String.overflow_tab_title);
                var menuItem = menu.Add(0, MoreTabId, 0, moreString);
                menuItems.Add(menuItem);

                menuItem.SetIcon(_Microsoft.Android.Resource.Designer.ResourceConstant.Drawable.abc_ic_menu_overflow_material);
                if (currentIndex >= maxBottomItems - 1)
                    menuItem.SetChecked(true);
            }

            bottomView.SetShiftMode(false, false);

            if (loadTasks.Count > 0)
                await Task.WhenAll(loadTasks);
        }

        static Task SetupMenuItem(
            (string title, ImageSource icon, bool tabEnabled) item,
            IMenu menu,
            int index,
            int currentIndex,
            BottomNavigationView bottomView,
            IMauiContext mauiContext,
            out IMenuItem menuItem)
        {
            Task returnValue;
            using var title = new Java.Lang.String(item.title);
            menuItem = menu.Add(0, index, 0, title);
            returnValue = SetMenuItemIcon(menuItem, item.icon, mauiContext);
            UpdateEnabled(item.tabEnabled, menuItem);
            if (index == currentIndex)
            {
                menuItem.SetChecked(true);
                bottomView.SelectedItemId = index;
            }

            return returnValue;
        }

        static void SetMenuItemTitle(IMenuItem menuItem, string title)
        {
            using var jTitle = new Java.Lang.String(title);
            menuItem.SetTitle(jTitle);
        }

        static async Task SetMenuItemIcon(IMenuItem menuItem, ImageSource source, IMauiContext context)
        {
            if (!menuItem.IsAlive())
                return;

            if (source is null)
                return;

            var services = context.Services;
            var provider = services.GetRequiredService<IImageSourceServiceProvider>();
            var imageSourceService = provider.GetRequiredImageSourceService(source);

            var result = await imageSourceService.GetDrawableAsync(
                source,
                context.Context);

            if (menuItem.IsAlive())
            {
                menuItem.SetIcon(result?.Value);
            }
        }

        static void UpdateEnabled(bool tabEnabled, IMenuItem menuItem)
        {
            if (menuItem.IsEnabled != tabEnabled)
                menuItem.SetEnabled(tabEnabled);
        }

    }
}
