using GCodeColorReplace.Annotations;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace GCodeColorReplace.ViewModel
{
    public class ExtruderSettingsVM : INotifyPropertyChanged
    {
        private ObservableCollection<Color> _extruderColors;

        public ObservableCollection<Color> ExtruderColors
        {
            get => _extruderColors;
            set
            {
                if (Equals(value, _extruderColors))
                {
                    return;
                }

                _extruderColors = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Extruder0_Color));
                OnPropertyChanged(nameof(Extruder1_Color));
                OnPropertyChanged(nameof(Extruder2_Color));
                OnPropertyChanged(nameof(Extruder3_Color));
            }
        }

        public ExtruderSettingsVM()
        {
            ExtruderColors = new ObservableCollection<Color> { Colors.Cyan, Colors.Magenta, Colors.Yellow, Colors.Black };
        }

        public Color Extruder0_Color
        {
            get => ExtruderColors[0];
            set
            {
                if (value.Equals(ExtruderColors[0]))
                {
                    return;
                }

                ExtruderColors[0] = value;
                OnPropertyChanged(nameof(Extruder0_Color));
                OnPropertyChanged(nameof(ExtruderColors));
                OnPropertyChanged();
            }
        }

        public Color Extruder1_Color
        {
            get => ExtruderColors[1];
            set
            {
                if (value.Equals(ExtruderColors[1]))
                {
                    return;
                }

                ExtruderColors[1] = value;
                OnPropertyChanged(nameof(Extruder1_Color));
                OnPropertyChanged(nameof(ExtruderColors));
            }
        }

        public Color Extruder2_Color
        {
            get => ExtruderColors[2];
            set
            {
                if (value.Equals(ExtruderColors[2]))
                {
                    return;
                }

                ExtruderColors[2] = value;
                OnPropertyChanged(nameof(Extruder2_Color));
                OnPropertyChanged(nameof(ExtruderColors));
            }
        }

        public Color Extruder3_Color
        {
            get => ExtruderColors[3];
            set
            {
                if (value.Equals(ExtruderColors[3]))
                {
                    return;
                }

                ExtruderColors[3] = value;
                OnPropertyChanged(nameof(Extruder3_Color));

            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
