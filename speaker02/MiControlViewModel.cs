using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace speaker02
{
    class MiControlViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        string _textoPantalla = String.Empty;

        public MiControlViewModel(MediaElement mediaElement)
        {

        }

        public string TextoPantalla {
            get => _textoPantalla;
            set
            {
                _textoPantalla = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TextoPantalla)));
            }        
        }

        public async Task MuestraMensaje(string mensaje) => await Despachador.Ejecuta(() =>
            TextoPantalla += $"{mensaje}\n");

    }
}
