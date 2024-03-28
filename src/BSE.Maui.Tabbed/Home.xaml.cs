namespace BSE.Maui.Tabbed;

public partial class Home : ContentPage
{
    int count = 0;
    public Home()
	{
		InitializeComponent();
	}

    private void OnCounterClicked(object sender, EventArgs e)
    {
        count++;

        if (count == 1)
            CounterBtn.Text = $"Clicked {count} time";
        else
            CounterBtn.Text = $"Clicked {count} times";

        SemanticScreenReader.Announce(CounterBtn.Text);
    }

    private async void OnNavigationClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Home_Sub1());
    }
}