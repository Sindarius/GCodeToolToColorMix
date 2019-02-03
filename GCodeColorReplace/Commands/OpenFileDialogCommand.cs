using GCodeColorReplace.ViewModel;
using Microsoft.Win32;
using System;
using System.Windows.Input;

namespace GCodeColorReplace.Commands
{

    public class OpenFileDialogCommand : ICommand
    {
        private ColorEditorVM _vm;

        public OpenFileDialogCommand(ColorEditorVM vm)
        {
            _vm = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "GCODE|*.gcode"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                _vm.FileName = openFileDialog.FileName;
            }


        }

        public event EventHandler CanExecuteChanged;
    }
}
