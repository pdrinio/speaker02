using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace speaker02
{
    class InterfazTexto
    {
        SpeechSynthesizer _sintetizador;
        MediaElement _media;
        Task _inicio;

        public Func<string, Task> EntradaTexto { get; set; }
        public Func<string, Task> SalidaTexto { get; set; }

        public InterfazTexto(MediaElement media)
        {
            _media = media;

            _inicio = Task.Run(InitializeSynthetizerAsync);
        }

        async Task InitializeSynthetizerAsync()
        {
            if (!await AudioCapturePermissions.RequestMicrophonePermission())
                throw new UnauthorizedAccessException("No hay acceso a un micrófono");

            _sintetizador = new SpeechSynthesizer
            {
                Voice = SpeechSynthesizer.AllVoices.FirstOrDefault(v => v.Gender == VoiceGender.Female) ?? SpeechSynthesizer.DefaultVoice
            };
        }

        public async Task NotifyAsync(string message)
        {
            await _inicio;

            var synth = _sintetizador;
            var msg = message;

            var stream = await synth.SynthesizeTextToStreamAsync(msg);

            // Play no bloquea hasta terminar la reproducción: para evitar que un audio posterior pise este esperamos a que termine controlando el evento MediaEnded
            var tsc = new TaskCompletionSource<bool>();
            RoutedEventHandler onEnded = (s, e) => tsc.SetResult(true);

            await Despachador.Ejecuta(() =>
            {
                _media.AutoPlay = true;
                _media.SetSource(stream, stream.ContentType);
                _media.MediaEnded += onEnded;
                _media.Play();
            });

            await tsc.Task;

            await Despachador.Ejecuta(() =>
                _media.MediaEnded -= onEnded);

            if (SalidaTexto != null)
                await SalidaTexto(msg);
        }
    }
}
