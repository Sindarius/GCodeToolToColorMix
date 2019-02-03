using GCodeColorReplace.Annotations;
using GCodeColorReplace.Commands;
using GCodeColorReplace.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace GCodeColorReplace.ViewModel
{
    public class ColorEditorVM : INotifyPropertyChanged
    {
        private string _fileName;
        private GCodeFile _gCodeFile;


        private ExtruderSettingsVM _extruderColors;


        public ExtruderSettingsVM ExtruderColors
        {
            get => _extruderColors;
            set
            {
                if (Equals(value, _extruderColors))
                {
                    return;
                }

                _extruderColors = value;
                UpdateExtruderColorsOnTools();
                OnPropertyChanged();
            }
        }



        public ColorEditorVM()
        {
            OpenFileCommand = new OpenFileDialogCommand(this);
            SaveFileCommand = new SaveFileCommand();
            _fileName = "No File Loaded.";
            ExtruderColors = new ExtruderSettingsVM();
            ExtruderColors.PropertyChanged += (sender, args) =>
            {
                UpdateExtruderColorsOnTools();
                OnPropertyChanged();
            };
        }

        public string FileName
        {
            get => _fileName;
            set
            {
                _fileName = value;
                GCodeFile = new GCodeFile();
                GCodeFile.Open(_fileName);

                UpdateExtruderColorsOnTools();

                OnPropertyChanged(nameof(FileName));
            }
        }

        public void UpdateExtruderColorsOnTools()
        {

            if (ExtruderColors != null && GCodeFile != null)
            {
                foreach (var tool in GCodeFile.Tools)
                {

                    tool.ExtruderColors.Clear();
                    foreach (var color in ExtruderColors.ExtruderColors)
                    {
                        tool.ExtruderColors.Add(color);
                    }
                }
            }
        }

        public GCodeFile GCodeFile
        {
            get => _gCodeFile;
            set
            {
                if (Equals(value, _gCodeFile))
                {
                    return;
                }

                _gCodeFile = value;

                OnPropertyChanged(nameof(GCodeFile));
                CommandManager.InvalidateRequerySuggested();

            }
        }




        public ICommand OpenFileCommand { get; set; }
        public ICommand SaveFileCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
