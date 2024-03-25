using Plugin.Maui.Audio;

namespace UserUI.Data.Service
{
    public class SpeechtoText
    {
        readonly IAudioManager audioManager;
        readonly IAudioRecorder audioRecorder;
        private byte[] data = new byte[6400];
        public string base64Str;
        public string RegText;
        public SpeechtoText(IAudioManager audioManager) 
        { 
            this.audioManager = audioManager;
            audioRecorder = audioManager.CreateRecorder();            
        }       
        public async void StartListen()
        {
            if (await Permissions.RequestAsync<Permissions.Microphone>() != PermissionStatus.Granted)
            {
                return;
            }
            if (!audioRecorder.IsRecording)
            {
                await audioRecorder.StartAsync();
            }
        }
        public async void StopListen()
        {
            if(audioRecorder.IsRecording)
            {
                
                var recordedaudio = await audioRecorder.StopAsync();
                var play = audioManager.CreatePlayer(recordedaudio.GetAudioStream());
                play.Play();
                var stream = recordedaudio.GetAudioStream();
                using (var binaryReader = new BinaryReader(stream))
                {
                    data = binaryReader.ReadBytes((int)stream.Length);
                    base64Str = Convert.ToBase64String(data);
                }
            }
        }        

    }
}
