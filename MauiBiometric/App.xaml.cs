using Plugin.Maui.Biometric;

namespace MauiBiometric
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            //return new Window(new AppShell());
            return new Window(new BiometricAuthPage());
        }

        public static void SetShell()
        {
            App.Current.Windows[0].Page = new AppShell();
        }
    }
    class BiometricAuthPage : ContentPage
    {
        protected async override void OnAppearing()
        {
            var biometric = BiometricAuthenticationService.Default;

            if (biometric.IsPlatformSupported)
            {
                var authenticationRequest = new AuthenticationRequest
                {
                    Title = "Unlock to continue",
                    NegativeText = "Cancel",
                    Subtitle = "This is subtitle text",
                    Description = "This is description",
                    AllowPasswordAuth = true,
                };
                AuthenticationResponse response = await biometric.AuthenticateAsync(authenticationRequest, CancellationToken.None);

                if (response.Status == BiometricResponseStatus.Success)
                {
                    // Biometric auth success
                    //await DisplayAlert("Success", "Biometric auth success", "OK");
                    App.SetShell();
                    return;
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