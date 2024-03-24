using Android.OS;
using Android.Views;
using AndroidX.Fragment.App;

using AView = Android.Views.View;

namespace BSE.Maui.Tabbed.Platforms
{
    internal class FragmentContainer : Fragment
    {
        private AdapterItemKey _adapterItemKey;
        private IMauiContext _mauiContext;
        ViewGroup? _parent;
        AView? _pageContainer;
        //Action<AView>? _onCreateCallback;

        public Page Page => _adapterItemKey.Page;

        public FragmentContainer(AdapterItemKey adapterItemKey, IMauiContext mauiContext)
        {
            _adapterItemKey = adapterItemKey;
            _mauiContext = mauiContext;
        }

        public static FragmentContainer CreateInstance(AdapterItemKey adapterItemKey, IMauiContext mauiContext)
        {
            return new FragmentContainer(adapterItemKey, mauiContext) { Arguments = new Bundle() };
        }

        //public void SetOnCreateCallback(Action<AView> callback)
        //{
        //    _onCreateCallback = callback;
        //}

        public override AView OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
        {
            _parent = container ?? _parent;

            _pageContainer = Page.ToPlatform(_mauiContext, RequireContext(), inflater, ChildFragmentManager);
            _adapterItemKey.SetToStableView();
            _parent = _parent ?? (_pageContainer.Parent as ViewGroup);
            //_onCreateCallback?.Invoke(_pageContainer);

            return _pageContainer;
        }
    }
}
