using GCodeColorReplace.Models;
using Microsoft.Win32;
using System;
using System.Windows.Input;

namespace GCodeColorReplace.Commands
{
    public class SaveFileCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            //return parameter is GCodeFile;
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter is GCodeFile gcodeFile)
            {
                var saveFileDialog = new SaveFileDialog();
                var fileName = $"Color_{System.IO.Path.GetFileName(gcodeFile.Path)}";
                saveFileDialog.FileName = fileName; //System.IO.Path.GetFullPath(gcodeFile.Path) + fileName;
                if (saveFileDialog.ShowDialog() == true)
                {
                    gcodeFile.Write(saveFileDialog.FileName);
                }
            }

        }

        public event EventHandler CanExecuteChanged;
    }
}
