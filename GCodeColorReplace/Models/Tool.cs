using GCodeColorReplace.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace GCodeColorReplace.Models
{
    public class Tool : INotifyPropertyChanged
    {
        public ObservableCollection<ExtrusionValue> ExtruderColorPercentages { get; set; }
        public string ToolName { get; set; }
        public bool IsValid => IsColorValid();

        public List<int> LinesToChange = new List<int>();

        public ObservableCollection<Color> ExtruderColors { get; set; }

        public string GCodeLineColor()
        {
            string result = $"M567 P0 E";
            for (int index = 0; index < ExtruderColorPercentages.Count; index++)
            {
                result = index == 0 ? $"{result}{ExtruderColorPercentages[index]:0.00}" : $"{result}:{ExtruderColorPercentages[index]:0.00}";
            }
            return result;
        }

        private void Init()
        {
            ExtruderColorPercentages = new ObservableCollection<ExtrusionValue>() { new ExtrusionValue(100), new ExtrusionValue(), new ExtrusionValue(), new ExtrusionValue() };

            ExtruderColors = new ObservableCollection<Color>();
            ExtruderColors.CollectionChanged += (sender, args) =>
            {
                ComputeToolColor();
                OnPropertyChanged(nameof(ToolColor));
            };
            foreach (var extrusionValue in ExtruderColorPercentages)
            {
                extrusionValue.PropertyChanged += (sender, args) =>
                {
                    OnPropertyChanged(nameof(ToolColor));
                };
            }
            ExtruderColorPercentages.CollectionChanged += (sender, args) =>
            {
                OnPropertyChanged(nameof(ToolColor));
            };
        }

        public Tool()
        {
            ToolName = "Unset";
            Init();
        }
        public Tool(string name)
        {
            ToolName = name;
            Init();
        }

        public Color ToolColor
        {
            get => ComputeToolColor();
            set { }
        }

        private Color ComputeToolColor()
        {
            if (ExtruderColors.Count < 4)
            {
                return Colors.Black;
            }

            if (!IsValid)
            {
                return Colors.Black;
            }

            byte R = 255;
            byte G = 255;
            byte B = 255;

            for (var index = 0; index < ExtruderColorPercentages.Count; index++)
            {
                var extruderRatio = ExtruderColorPercentages[index];
                Color color = ExtruderColors[index];
                R -= (byte)(color.R * extruderRatio.GetDecimal);
                G -= (byte)(color.G * extruderRatio.GetDecimal);
                B -= (byte)(color.B * extruderRatio.GetDecimal);
            }

            return Color.FromRgb((byte)(255 - R), (byte)(255 - G), (byte)(255 - B));
        }

        private bool IsColorValid()
        {
            double total = 0;
            ExtruderColorPercentages.ToList().ForEach(c => total += c.Value);
            return Math.Abs(total - 100) < 0.0001;
        }

        public override int GetHashCode()
        {
            return ToolName.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Tool t)
            {
                return t.ToolName == ToolName;
            }

            return false;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
