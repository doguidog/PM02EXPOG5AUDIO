using Plugin.AudioRecorder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PM02EXPOG5AUDIO
{
    [DesignTimeVisible(true)]
    public partial class MainPage : ContentPage
    {
        AudioRecorderService gravador;
        AudioPlayer reprodutor;
        public MainPage()
        {
            InitializeComponent();

            gravador = new AudioRecorderService
            {
                StopRecordingAfterTimeout = true,
                TotalAudioTimeout = TimeSpan.FromSeconds(15),
                AudioSilenceTimeout = TimeSpan.FromSeconds(15)
            };

            reprodutor = new AudioPlayer();
            reprodutor.FinishedPlaying += Finaliza_Reproducao;

        }

        private async void GravarButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!gravador.IsRecording)
                {
                    gravador.StopRecordingOnSilence = TimeoutSwitch.IsToggled;

                    GravarButton.IsEnabled = false;
                    ReproduzirButton.IsEnabled = false;

                    //Começa gravação
                    var audioRecordTask = await gravador.StartRecording();

                    GravarButton.Text = "Parar de grabar";
                    GravarButton.IsEnabled = true;

                    await audioRecordTask;

                    GravarButton.Text = "Grabar Audio";
                    ReproduzirButton.IsEnabled = true;
                }
                else
                {
                    GravarButton.IsEnabled = false;

                    //parar a gravação...
                    await gravador.StopRecording();

                    GravarButton.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                //blow up the app!
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        private async void ReproduzirButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var filePath = gravador.GetAudioFilePath();

                if (filePath != null)
                {
                    ReproduzirButton.IsEnabled = false;
                    GravarButton.IsEnabled = false;

                    reprodutor.Play(filePath);
                }
            }
            catch (Exception ex)
            {
                //blow up the app!
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }


        void Finaliza_Reproducao(object sender, EventArgs e)
        {
            ReproduzirButton.IsEnabled = true;
            GravarButton.IsEnabled = true;
        }


    }
}
