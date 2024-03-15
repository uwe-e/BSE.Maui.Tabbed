namespace BSE.Tunes.Maui.Client.Controls
{
    public partial class TabbedPageContainer : Microsoft.Maui.Controls.TabbedPage
    {
        public static readonly BindableProperty BottomViewProperty
            = BindableProperty.Create(nameof(BottomView),
                                      typeof(ContentView),
                                      typeof(TabbedPageContainer),
                                      null,
                                      propertyChanged: OnBottomViewChanged);

        public ContentView BottomView
        {
            get { return (ContentView)GetValue(BottomViewProperty); }
            set { SetValue(BottomViewProperty, value); }
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
    }
}
