#nullable disable
using Android.Content;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using AndroidX.ViewPager2.Widget;
using BSE.Tunes.Maui.Client.Controls;
using Google.Android.Material.BottomNavigation;
using Google.Android.Material.Navigation;
using System.Collections.Specialized;
using Microsoft.Maui.Controls.Platform;
using BSE.Maui.Tabbed.Platforms.AndroidSpecific;
using TabbedPageContainer = BSE.Tunes.Maui.Client.Controls.TabbedPageContainer;

namespace BSE.Maui.Tabbed.Platforms.Android
{
    public class TabbedContainerView : RelativeLayout
    {
        BottomNavigationView _bottomNavigationView;
        ViewPager2 _viewPager;
        private FragmentManager _fragmentManager;
        private Page _previousPage;
        private readonly IMauiContext _mauiContext;
        readonly Listeners _listeners;

        FragmentManager FragmentManager => _fragmentManager ?? (_fragmentManager = _mauiContext.GetFragmentManager());
        TabbedPageContainer Element { get; set; }

        public TabbedContainerView(IMauiContext mauiContext) : base(mauiContext.Context)
        {
            _mauiContext = mauiContext;
            _listeners = new Listeners(this);
        }

        public TabbedContainerView(Context context) : base(context)
        {
        }

        internal void SetElement(VisualElement element)
        {
            if (Element is not null)
            {

            }

            Element = (Tunes.Maui.Client.Controls.TabbedPageContainer)element;

            if (Element is not null)
            {
                LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);

                var bottomNavigationViewLayoutParams = new RelativeLayout.LayoutParams(
                            LayoutParams.MatchParent,
                            LayoutParams.WrapContent);

                bottomNavigationViewLayoutParams.AddRule(LayoutRules.AlignParentBottom);

                _bottomNavigationView = new BottomNavigationView(_mauiContext.Context)
                {
                    LayoutParameters = bottomNavigationViewLayoutParams,
                    Id = Resource.Id.bottom_navigation
                };

                var viewPagerParams = new RelativeLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
                viewPagerParams.AddRule(LayoutRules.Above, _bottomNavigationView.Id);

                _viewPager = new ViewPager2(_mauiContext.Context)
                {
                    OverScrollMode = OverScrollMode.Never,
                    LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent)
                };
                _viewPager.RegisterOnPageChangeCallback(_listeners);
                _viewPager.Adapter = new MultiPageFragmentStateAdapter<Page>((MultiPage<Page>)element, FragmentManager, _mauiContext)
                {
                    CountOverride = ((MultiPage<Page>)element).Children.Count
                };

                AddView(_viewPager, viewPagerParams);
                AddView(_bottomNavigationView, bottomNavigationViewLayoutParams);

                OnChildrenCollectionChanged(null, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

                //ScrollToCurrentPage();

                _previousPage = Element.CurrentPage;

                ((IPageController)element).InternalChildren.CollectionChanged += OnChildrenCollectionChanged;
            }
        }
        

        //protected override void OnLayout(bool changed, int l, int t, int r, int b)
        //{
        //    var width = r - l;
        //    var height = b - t;

        //    if (width <= 0 || height <= 0)
        //    {
        //        return;
        //    }

        //    if (width > 0 && height > 0)
        //    {
        //        //_bottomNavigationView.seth.Height = h
        //    }

        //    base.OnLayout(changed, l, t, r, b);
        //}

        private void OnChildrenCollectionChanged(object value, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {

            //ViewPager2 pager = _viewPager;

            //if (pager.Adapter is MultiPageFragmentStateAdapter<Page> adapter)
            //{
            //    adapter.CountOverride = Element.Children.Count;
            //}

            BottomNavigationView bottomNavigationView = _bottomNavigationView;
            
            if (Element.Children.Count == 0)
            {
                bottomNavigationView.Menu.Clear();
            }
            else
            {
                SetupBottomNavigationView();
                bottomNavigationView.SetOnItemSelectedListener(_listeners);
            }
        }

        internal void ScrollToCurrentPage()
        {
            if (Element.CurrentPage == null)
            {
                return;
            }
            _viewPager.SetCurrentItem(Element.Children.IndexOf(Element.CurrentPage), Element.OnThisPlatform().IsSmoothScrollEnabled());
        }

        private void SetupBottomNavigationView()
        {
            var currentIndex = Element.Children.IndexOf(Element.CurrentPage);
            var items = CreateTabList();

            /*
             * a copy of the internal internal class Microsoft.Maui.Controls.Platform.BottomNavigationViewUtils
             */
            BottomNavigationViewUtils.SetupMenu(
                _bottomNavigationView.Menu,
                _bottomNavigationView.MaxItemCount,
                items,
                currentIndex,
                _bottomNavigationView,
                FindMauiContext());
            
            if (Element.CurrentPage == null && Element.Children.Count > 0)
            {
                Element.CurrentPage = Element.Children[0];
            }
        }

        private List<(string title, ImageSource icon, bool tabEnabled)> CreateTabList()
        {
            var items = new List<(string title, ImageSource icon, bool tabEnabled)>();

            for (int i = 0; i < Element.Children.Count; i++)
            {
                var item = Element.Children[i];
                items.Add((item.Title, item.IconImageSource, item.IsEnabled));
            }

            return items;
        }

        private IMauiContext FindMauiContext()
        {
            if (Element is Microsoft.Maui.IElement fe && fe.Handler?.MauiContext != null)
            {
                return fe.Handler.MauiContext;
            }
            return null;
        }

        private class Listeners : ViewPager2.OnPageChangeCallback,
#pragma warning disable CS0618
            NavigationBarView.IOnItemSelectedListener

        {
            private readonly TabbedContainerView _tabbedContainerView;

            public Listeners(TabbedContainerView tabbedContainerView) {
                _tabbedContainerView = tabbedContainerView;
            }

            public override void OnPageSelected(int position)
            {
                base.OnPageSelected(position);

                var _previousPage = _tabbedContainerView._previousPage;
                var element = _tabbedContainerView.Element;
                var _bottomNavigationView = _tabbedContainerView._bottomNavigationView;

                if (_previousPage != element.CurrentPage)
                {
                    _previousPage?.SendDisappearing();
                    _previousPage = element.CurrentPage;
                    _tabbedContainerView._previousPage = element.CurrentPage;
                }

                if (element.Children.Count > 0)
                {
                    element.CurrentPage = element.Children[position];
                    element.CurrentPage.SendAppearing();
                }

                _bottomNavigationView.SelectedItemId = position;
            }

            bool NavigationBarView.IOnItemSelectedListener.OnNavigationItemSelected(IMenuItem item)
            {
                var id = item.ItemId;
                
                if (_tabbedContainerView._bottomNavigationView.SelectedItemId != item.ItemId && _tabbedContainerView.Element.Children.Count > item.ItemId)
                {
                    _tabbedContainerView.Element.CurrentPage = _tabbedContainerView.Element.Children[item.ItemId];
                }

                return true;
            }
        }

    }
}
