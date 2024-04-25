using Jachas_Lo_Fi_.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jachas_Lo_Fi_.ViewModel
{
    class ViewModel : INotifyPropertyChanged
    {
        private Modelmvvm model = new Modelmvvm();
        public double Wartość
        {
            get
            {
                return model.Wartość;
            }
            set
            {
                model.Wartość = value;
                onPropertyChanged(nameof(Wartość));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void onPropertyChanged(string nazwaWłasności)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(nazwaWłasności));
        }
    }
}
