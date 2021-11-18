using RecordPlayAudio.Views;
using Xamarin.Forms;

namespace RecordPlayAudio
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new RecordPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
