#nullable disable
using Android.Content;
using Android.Views;
using Android.Widget;
using AndroidX.CoordinatorLayout.Widget;
using AndroidX.ViewPager2.Widget;
using BSE.Tunes.Maui.Client.Controls;
using Google.Android.Material.AppBar;
using Google.Android.Material.BottomNavigation;
using Microsoft.Maui.Controls.Platform;
using System.Collections.Specialized;

namespace BSE.Maui.Tabbed.Platforms.Android
{
    public class TabbedContainerView : RelativeLayout
    {
        BottomNavigationView _bottomNavigationView;
        ViewPager2 _viewPager;
        private readonly IMauiContext _mauiContext;

        TabbedPageContainer Element { get; set; }

        public TabbedContainerView(IMauiContext mauiContext) : base(mauiContext.Context)
        {
            _mauiContext = mauiContext;
        }

        public TabbedContainerView(Context? context) : base(context)
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
                
                AddView(_viewPager, viewPagerParams);
                AddView(_bottomNavigationView, bottomNavigationViewLayoutParams);

                OnChildrenCollectionChanged(null, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

                ((IPageController)element).InternalChildren.CollectionChanged += OnChildrenCollectionChanged;
            }
        }
        

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            var width = r - l;
            var height = b - t;

            if (width <= 0 || height <= 0)
            {
                return;
            }

            if (width > 0 && height > 0)
            {
                //_bottomNavigationView.seth.Height = h
            }

            base.OnLayout(changed, l, t, r, b);
        }

        private void OnChildrenCollectionChanged(object value, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            BottomNavigationView bottomNavigationView = _bottomNavigationView;
            
            if (Element.Children.Count == 0)
            {
                bottomNavigationView.Menu.Clear();
            }
            else
            {
                SetupBottomNavigationView();
                //bottomNavigationView.SetOnItemSelectedListener(_listeners);
            }
        }

        private void SetupBottomNavigationView()
        {
            var currentIndex = Element.Children.IndexOf(Element.CurrentPage);
            var items = CreateTabList();

            BottomNavigationViewUtils.SetupMenu(
                _bottomNavigationView.Menu,
                _bottomNavigationView.MaxItemCount,
                items,
                currentIndex,
                _bottomNavigationView,
                FindMauiContext());

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

        private IMauiContext? FindMauiContext()
        {
            if (Element is Microsoft.Maui.IElement fe && fe.Handler?.MauiContext != null)
            {
                return fe.Handler.MauiContext;
            }
            return null;
        }

    }
}
