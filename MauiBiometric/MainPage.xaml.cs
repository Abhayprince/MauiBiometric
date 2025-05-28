using Plugin.Maui.Biometric;
using System.Threading.Tasks;

namespace MauiBiometric
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
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

        private async void UnlockBtn_Clicked(object sender, EventArgs e)
        {
            var biometric = BiometricAuthenticationService.Default;

            if (biometric.IsPlatformSupported)
            {
                var authenticationRequest = new AuthenticationRequest
                {
                    Title = "Unlock to continue",
                    NegativeText = "Cancel",
                    Subtitle = "This is subtitle text",
                    Description ="This is description",
                    AllowPasswordAuth = true,
                };
                AuthenticationResponse response = await biometric.AuthenticateAsync(authenticationRequest, CancellationToken.None);

                if(response.Status == BiometricResponseStatus.Success)
                {
                    // Biometric auth success
                    await DisplayAlert("Success", "Biometric auth success", "OK");
                }
                else
                {
                    // Error in biometric auth
                    // or User Cancelled
                    await DisplayAlert("Failure", response.ErrorMsg ?? "Unknow error in biometric auth", "OK");
                }
            }
        }
    }

}
