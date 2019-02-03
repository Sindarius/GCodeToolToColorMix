using GCodeColorReplace.Annotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GCodeColorReplace.Models
{
    public class ExtrusionValue : INotifyPropertyChanged
    {
        private double _value;


        public double Value
        {
            get => _value;
            set
            {
                if (value == _value)
                {
                    return;
                }

                _value = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Value));
            }
        }

        public double GetDecimal => (double) (_value / 100.0);

        public ExtrusionValue()
        {
            _value = 0;
        }

        public ExtrusionValue(double Value)
        {
            _value = Value;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        public override string ToString()
        {
            return $"{GetDecimal:0.##}";
        }
    }
}
