namespace BlazorHomeSite.Shared;

public partial class MainLayout
{
    public bool DrawerOpen { get; private set; }

    public MainLayout()
    {
        DrawerOpen = true;
    }

    private void DrawerToggle()
    {
        DrawerOpen = !DrawerOpen;
    }
}