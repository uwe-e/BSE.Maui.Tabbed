using Microsoft.Maui.Controls;

namespace BSE.Tunes.Maui.Client.Controls
{
    public partial class TabbedPageContainer : Microsoft.Maui.Controls.TabbedPage, IElementConfiguration<TabbedPageContainer>
    {
        public static readonly BindableProperty BottomViewProperty
            = BindableProperty.Create(nameof(BottomView),
                                      typeof(ContentView),
                                      typeof(TabbedPageContainer),
                                      null,
                                      propertyChanged: OnBottomViewChanged);

        readonly Lazy<PlatformConfigurationRegistry<TabbedPageContainer>> _platformConfigurationRegistry;


        public ContentView BottomView
        {
            get { return (ContentView)GetValue(BottomViewProperty); }
            set { SetValue(BottomViewProperty, value); }
        }

        public TabbedPageContainer()
        {
            _platformConfigurationRegistry = new Lazy<PlatformConfigurationRegistry<TabbedPageContainer>>(() => new PlatformConfigurationRegistry<TabbedPageContainer>(this));
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            ContentView contentView = BottomView;
            if (contentView != null)
            {
                SetInheritedBindingContext(contentView, BindingContext);
            }
        }

        private static void OnBottomViewChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var newElement = (Element)newValue;
            if (newElement != null)
            {
                BindableObject.SetInheritedBindingContext(newElement, bindable.BindingContext);
            }
        }

        IPlatformElementConfiguration<T, TabbedPageContainer> IElementConfiguration<TabbedPageContainer>.On<T>()
        {
            return _platformConfigurationRegistry.Value.On<T>();
        }
    }
}
