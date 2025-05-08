namespace IndoorMappingApp;

public partial class FeedbackOptionsPage : ContentPage
{
	public FeedbackOptionsPage()
	{
		InitializeComponent();
	}

    private void OnSendFeedbackClicked(object sender, EventArgs e)
    {
        var lrm = LocalizationResourceManager.Instance;

        var selectedFeedback = new List<string>();

        if (Option1.IsChecked) selectedFeedback.Add(lrm["Option1_Text"]);
        if (Option2.IsChecked) selectedFeedback.Add((lrm["Option2_Text"]));
        if (Option3.IsChecked) selectedFeedback.Add((lrm["Option3_Text"]));
        if (Option4.IsChecked) selectedFeedback.Add((lrm["Option4_Text"]));
        if (Option5.IsChecked) selectedFeedback.Add((lrm["Option5_Text"]));

        string result = string.Join("\n", selectedFeedback);
        //DisplayAlert("Feedback Submitted", result, "OK");
        DisplayAlert(
                    lrm["Feedback"],
                    lrm["FeedbackSubmitted"],
                    lrm["Button_OK"]);

    }

}