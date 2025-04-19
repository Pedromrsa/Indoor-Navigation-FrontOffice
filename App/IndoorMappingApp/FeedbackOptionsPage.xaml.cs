namespace IndoorMappingApp;

public partial class FeedbackOptionsPage : ContentPage
{
	public FeedbackOptionsPage()
	{
		InitializeComponent();
	}

    private void OnSendFeedbackClicked(object sender, EventArgs e)
    {
        var selectedFeedback = new List<string>();

        if (Option1.IsChecked) selectedFeedback.Add("The path x seems very optimal.");
        if (Option2.IsChecked) selectedFeedback.Add("The path x is always full of people.");
        if (Option3.IsChecked) selectedFeedback.Add("This app is too slow to work on a daily basis.");
        if (Option4.IsChecked) selectedFeedback.Add("The neurolinc is not working properly.");
        if (Option5.IsChecked) selectedFeedback.Add("My position on the map is inaccurate.");

        string result = string.Join("\n", selectedFeedback);
        DisplayAlert("Feedback Submitted", result, "OK");
    }

}