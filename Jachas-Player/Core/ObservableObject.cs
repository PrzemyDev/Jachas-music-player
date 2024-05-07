using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Jachas_Lo_Fi_.Core
{
    internal class ObservableObject : INotifyPropertyChanged
    {
        //public event PropertyChangedEventHandler? PropertyChanged; //CS8370 --Feature 'global using directive' is not available in C# 7.3
        public event PropertyChangedEventHandler PropertyChanged = null; //fix
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
