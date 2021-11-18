using Plugin.AudioRecorder;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace RecordPlayAudio.ViewModels
{
    public class RecordViewModel : ObservableObject
    {

        #region [ Attributes ]
        bool _isToggled;
        bool _isEnabledPlayButton;
        bool _isEnabledRecordButton;
        string _recordButtonText;
        AudioRecorderService recorder;
        AudioPlayer player;
        private ICommand _playCommand;
        private ICommand _recordCommand;
        #endregion [ Attributes ]

        #region [ Constructor ]
        public RecordViewModel()
        {
            RecordButtonText = "Record";
            IsToggled = false;
            IsEnabledPlayButton = false;

            recorder = new AudioRecorderService
            {
                StopRecordingAfterTimeout = true,
                TotalAudioTimeout = TimeSpan.FromSeconds(15),
                AudioSilenceTimeout = TimeSpan.FromSeconds(2)
            };

            player = new AudioPlayer();
            player.FinishedPlaying += Player_FinishedPlaying;
        }


        #endregion [ Constructor ]

        #region [ Properties ]
        public bool IsToggled
        {
            get => _isToggled;
            set => SetProperty(ref _isToggled, value);
        }

        public bool IsEnabledPlayButton
        {
            get => _isEnabledPlayButton;
            set => SetProperty(ref _isEnabledPlayButton, value);
        }

        public bool IsEnabledRecordButton
        {
            get => _isEnabledRecordButton;
            set => SetProperty(ref _isEnabledRecordButton, value);
        }

        public string RecordButtonText 
        { 
            get => _recordButtonText;
            set => SetProperty(ref _recordButtonText, value);
        }
        #endregion [ Properties ]

        #region [ Comands ]
        public ICommand RecordCommand => _recordCommand ?? (_recordCommand = new Command(async () => await Record()));
        public ICommand PlayCommand => _playCommand ?? (_playCommand = new Command(Play));
        #endregion [ Comands ]

        #region [ Methods ]
        async Task Record()
        {
            try
            {
                if (!recorder.IsRecording) //Record button clicked
                {
                    recorder.StopRecordingOnSilence = IsToggled;

                    IsEnabledRecordButton = false;
                    IsEnabledPlayButton = false;

                    //start recording audio
                    var audioRecordTask = await recorder.StartRecording();

                    RecordButtonText = "Stop Recording";
                    IsEnabledRecordButton = true;

                    await audioRecordTask;

                    RecordButtonText = "Record";
                    IsEnabledPlayButton = true;
                }
                else //Stop button clicked
                {
                    IsEnabledRecordButton = false;

                    //stop the recording...
                    await recorder.StopRecording();
                    IsEnabledRecordButton = true;
                }
            }
            catch (Exception ex)
            {
                //blow up the app!
                throw ex;
            }
        }

        void Play()
        {
            try
            {
                var filePath = recorder.GetAudioFilePath();

                if (filePath != null)
                {
                    IsEnabledPlayButton = false;
                    IsEnabledRecordButton = false;

                    player.Play(filePath);
                }
            }
            catch (Exception ex)
            {
                //blow up the app!
                throw ex;
            }
        }

        private void Player_FinishedPlaying(object sender, EventArgs e)
        {
            IsEnabledPlayButton = true;
            IsEnabledRecordButton= true;
        }
        #endregion [ Methods ]

    }
}
