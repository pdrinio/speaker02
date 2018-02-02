using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace speaker02
{
    class Despachador
    { 
        public static Task Ejecuta(Action accion)
        {
            return CoreApplication.MainView.CoreWindow.Dispatcher.
                    RunAsync(CoreDispatcherPriority.Normal, () => accion()).AsTask();

        }        


    }
}
