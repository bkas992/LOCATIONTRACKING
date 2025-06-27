namespace LocationHeatmapApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState) // Fix: Add nullable annotation to match overridden member
    {
        return new Window(new MainPage());
    }
}