using AndroidX.Fragment.App;
using AndroidX.ViewPager2.Adapter;

namespace BSE.Maui.Tabbed.Platforms
{
    internal class MultiPageFragmentStateAdapter<T> : FragmentStateAdapter where T : Page
    {
        private readonly MultiPage<T> _page;
        private readonly IMauiContext _context;
        List<AdapterItemKey> keys = new List<AdapterItemKey>();

        public int CountOverride { get; set; }
        public override int ItemCount => CountOverride;

        public MultiPageFragmentStateAdapter(
            MultiPage<T> page, FragmentManager fragmentManager, IMauiContext context)
            : base(fragmentManager, context.GetActivity().Lifecycle)
        {
            _page = page;
            _context = context;
        }

        public override Fragment CreateFragment(int position)
        {
            var fragment = FragmentContainer.CreateInstance(GetItemIdByPosition(position), _context);
            return fragment;
        }

        AdapterItemKey GetItemIdByPosition(int position)
        {
            CheckItemKeys();
            var page = _page.Children[position];
            for (var i = 0; i < keys.Count; i++)
            {
                var item = keys[i];
                if (item.Page == page)
                {
                    return item;
                }
            }

            var itemKey = new AdapterItemKey(page, (ik) => keys.Remove(ik));
            keys.Add(itemKey);

            return itemKey;
        }

        private void CheckItemKeys()
        {
            for (var i = keys.Count - 1; i >= 0; i--)
            {
                var item = keys[i];

                if (!_page.Children.Contains(item.Page))
                {
                    // Disconnect will remove the ItemKey from the keys list
                    item.Disconnect();
                }
            }
        }
    }
}
