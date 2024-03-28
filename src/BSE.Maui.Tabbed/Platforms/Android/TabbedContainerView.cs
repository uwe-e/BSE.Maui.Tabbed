#nullable disable
using Android.Content;
using Android.Content.Res;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using AndroidX.ViewPager2.Widget;
using BSE.Maui.Tabbed.Platforms.AndroidSpecific;
using Google.Android.Material.BottomNavigation;
using Google.Android.Material.Navigation;
using Google.Android.Material.Tabs;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Platform;
using System.Collections.Specialized;

using AView = Android.Views.View;
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
        private bool _tabItemStyleLoaded;
        private Color _currentBarTextColor;
        private Color _currentBarItemColor;
        private Color _currentBarSelectedItemColor;
        private ColorStateList _originalTabTextColors;
        private ColorStateList _orignalTabIconColors;
        private ColorStateList _newTabTextColors;
        private ColorStateList _newTabIconColors;
        private ColorStateList _currentBarTextColorStateList;
        private int[] _emptyStateSet;
        private int[] _checkedStateSet;
        

        FragmentManager FragmentManager => _fragmentManager ?? (_fragmentManager = _mauiContext.GetFragmentManager());
        TabbedPageContainer Element { get; set; }

        public Color BarItemColor
        {
            get
            {
                if (Element != null)
                {
                    if (Element.IsSet(TabbedPage.UnselectedTabColorProperty))
                    {
                        return Element.UnselectedTabColor;
                    }
                }

                return null;
            }
        }

        public Color BarSelectedItemColor
        {
            get
            {
                if (Element != null)
                {
                    if (Element.IsSet(TabbedPage.SelectedTabColorProperty))
                    {
                        return Element.SelectedTabColor;
                    }
                }

                return null;
            }
        }

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

            Element = (TabbedPageContainer)element;

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

                _previousPage = Element.CurrentPage;

                ((IPageController)element).InternalChildren.CollectionChanged += OnChildrenCollectionChanged;
            }
        }

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
            _viewPager.SetCurrentItem(Element.Children.IndexOf(Element.CurrentPage), Element.OnThisPlatform().IsSwipePagingEnabled());
        }

        internal void SetBottomView(AView view)
        {
            if (view != null)
            {
                int viewHeight = (view.Height == -1) ? LayoutParams.WrapContent : view.Height;
                var layoutParams = new RelativeLayout.LayoutParams(LayoutParams.MatchParent, viewHeight);
                layoutParams.AddRule(LayoutRules.Above, _bottomNavigationView.Id);

                AddView(view, layoutParams);
            }
        }

        internal void UpdateSwipePaging()
        {
            _viewPager.UserInputEnabled = Element.OnThisPlatform().IsSwipePagingEnabled();
        }

        internal void UpdateTabItemStyle()
        {
            Color barItemColor = BarItemColor;
            Color barTextColor = Element.BarTextColor;
            Color barSelectedItemColor = BarSelectedItemColor;

            if (_tabItemStyleLoaded &&
                _currentBarItemColor == barItemColor &&
                _currentBarTextColor == barTextColor &&
                _currentBarSelectedItemColor == barSelectedItemColor)
            {
                return;
            }

            _tabItemStyleLoaded = true;
            _currentBarItemColor = BarItemColor;
            _currentBarTextColor = Element.BarTextColor;
            _currentBarSelectedItemColor = BarSelectedItemColor;

            UpdateBarTextColor();
            UpdateItemIconColor();
        }

        internal void UpdateBarBackgroundColor()
        {
            if (Element.BarBackground != null)
            {
                return;
            }
            
            Color tintColor = Element.BarBackgroundColor;

            if (tintColor == null)
            {
                _bottomNavigationView.SetBackground(null);
            }
            else if (tintColor != null)
            {
                _bottomNavigationView.SetBackgroundColor(tintColor.ToPlatform());
            }
        }

        private void UpdateBarTextColor()
        {
            _newTabTextColors = null;

            _currentBarTextColorStateList = GetItemTextColorStates() ?? _originalTabTextColors;
            _bottomNavigationView.ItemTextColor = _currentBarTextColorStateList;
        }

        private void UpdateItemIconColor()
        {
            _newTabIconColors = null;
            _bottomNavigationView.ItemIconTintList = GetItemIconTintColorState() ?? _orignalTabIconColors;
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

        ColorStateList GetItemTextColorStates()
        {
            if (_originalTabTextColors is null)
            {
                _originalTabTextColors = _bottomNavigationView.ItemTextColor;
            }

            Color barItemColor = BarItemColor;
            Color barTextColor = Element.BarTextColor;
            Color barSelectedItemColor = BarSelectedItemColor;

            if (barItemColor is null && barTextColor is null && barSelectedItemColor is null)
            {
                return _originalTabTextColors;
            }
            if (_newTabTextColors is not null)
            {
                return _newTabTextColors;
            }

            int checkedColor;

            // The new default color to use may have a color if BarItemColor is not null or the original colors for text
            // are not null either. If it does not happens, this variable will be null and the ColorStateList of the
            // original colors is used.
            int? defaultColor = null;

            if (barTextColor is not null)
            {
                checkedColor = barTextColor.ToPlatform().ToArgb();
                defaultColor = checkedColor;
            }
            else
            {
                if (barItemColor is not null)
                {
                    defaultColor = barItemColor.ToPlatform().ToArgb();
                }

                if (barItemColor is null && _originalTabTextColors is not null)
                {
                    defaultColor = _originalTabTextColors.DefaultColor;
                }

                if (!defaultColor.HasValue)
                {
                    return _originalTabTextColors;
                }
                else
                {
                    checkedColor = defaultColor.Value;
                }

                if (barSelectedItemColor is not null)
                {
                    checkedColor = barSelectedItemColor.ToPlatform().ToArgb();
                }
            }

            _newTabTextColors = GetColorStateList(defaultColor.Value, checkedColor);

            return _newTabTextColors;
        }

        private ColorStateList GetItemIconTintColorState()
        {
            if (_orignalTabIconColors == null)
            {
                _orignalTabIconColors = _bottomNavigationView.ItemIconTintList;
            }

            Color barItemColor = BarItemColor;
            Color barSelectedItemColor = BarSelectedItemColor;

            if (barItemColor == null && barSelectedItemColor == null)
            {
                return _orignalTabIconColors;
            }

            if (_newTabIconColors != null)
            {
                return _newTabIconColors;
            }

            int defaultColor = barItemColor.ToPlatform().ToArgb();

            if (barItemColor == null && _orignalTabIconColors != null)
            {
                defaultColor = _orignalTabIconColors.DefaultColor;
            }

            int checkedColor = defaultColor;

            if (barSelectedItemColor != null)
            {
                checkedColor = barSelectedItemColor.ToPlatform().ToArgb();
            }

            _newTabIconColors = GetColorStateList(defaultColor, checkedColor);
            return _newTabIconColors;
        }

        ColorStateList GetColorStateList(int defaultColor, int checkedColor)
        {
            int[][] states = new int[2][];
            int[] colors = new int[2];

            states[0] = GetSelectedStateSet();
            colors[0] = checkedColor;
            states[1] = GetEmptyStateSet();
            colors[1] = defaultColor;

            return new ColorStateList(states, colors);
        }

        int[] GetSelectedStateSet()
        {
            if (_checkedStateSet == null)
            {
                _checkedStateSet = new int[] { global::Android.Resource.Attribute.StateChecked };
            }

            return _checkedStateSet;
        }

        int[] GetEmptyStateSet()
        {
            if (_emptyStateSet == null)
            {
                _emptyStateSet = GetStateSet(AView.EmptyStateSet);
            }

            return _emptyStateSet;
        }

        int[] GetStateSet(IList<int> stateSet)
        {
            var results = new int[stateSet.Count];
            for (int i = 0; i < results.Length; i++)
                results[i] = stateSet[i];

            return results;
        }

        private class Listeners : ViewPager2.OnPageChangeCallback,
#pragma warning disable CS0618
            NavigationBarView.IOnItemSelectedListener

        {
            private readonly TabbedContainerView _tabbedContainerView;

            public Listeners(TabbedContainerView tabbedContainerView)
            {
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
