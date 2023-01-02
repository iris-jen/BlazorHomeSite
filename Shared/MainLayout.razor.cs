namespace BlazorHomeSite.Shared;

public partial class MainLayout
{
    public MainLayout()
    {
        DrawerOpen = true;
    }

    public bool DrawerOpen { get; private set; }

    private void DrawerToggle()
    {
        DrawerOpen = !DrawerOpen;
    }
}