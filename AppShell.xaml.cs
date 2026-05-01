using Gano.Pages;

namespace Gano
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(DersDuzenlePage),typeof(DersDuzenlePage));
        }
    }
}
