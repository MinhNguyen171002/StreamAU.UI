using CommunityToolkit.Maui.Core.Primitives;

namespace UserUI;

public partial class Player : ContentPage
{
	public Player()
	{
		InitializeComponent();
        //MainPage = new NavigationPage(new MainPage());
	}
    void OnPlayPauseButtonClicked(object sender, EventArgs e)
    {
        if(mediaElement.CurrentState == MediaElementState.Stopped || mediaElement.CurrentState == MediaElementState.Paused)
        {
            mediaElement.Play();
        }
        else if (mediaElement.CurrentState == MediaElementState.Playing)
        {
            mediaElement.Pause();
        }
    }
    void OnStopButtonClicked(object sender, EventArgs args)
    {
        mediaElement.Stop();
    }
}