using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace GCodeColorReplace.Models
{
    public class GCodeFile
    {
        private List<string> _fileLines = new List<string>();
        public ObservableCollection<Tool> Tools { get; set; } = new ObservableCollection<Tool>();

        private Regex toolRegex = new Regex("^T(?<Tool>[0-9]*)");
        private Regex tempRegex = new Regex("(?<TempCommand>(M104|M109)).*[sS](?<Temp>[0-9\\.]*).*[tT](?<Tool>[0-9]*)");
        private Regex curaTempRegex = new Regex("(?<TempCommand>(M104|M109)).*[tT](?<Tool>[0-9\\.]*).*[sS](?<Temp>[0-9]*)");
        private string _filePath;
        private Dictionary<string, double> MaxToolTemp = new Dictionary<string, double>();


        public GCodeFile()
        {
        }


        //Search for M104
        //M104 S240 T4

        public void Open(string filePath)
        {

            if (!System.IO.File.Exists(filePath))
            {
                throw new FileNotFoundException("GCode file not found");
            }
            _filePath = filePath;
            using (var fileStream = new System.IO.StreamReader(_filePath))
            {
                var lineIndex = 0;
                while (!fileStream.EndOfStream)
                {
                    var line = fileStream.ReadLine();

                    _fileLines.Add(line);

                    if (line.Trim().StartsWith(";"))
                    {
                        lineIndex++;
                        continue;
                    }

                    if (line.Contains(';'))
                    {
                      line = line.Substring(0, line.IndexOf(';') - 1);
                    }


                    var toolMatch = toolRegex.Match(line);
                    if (toolMatch.Success)
                    {
                        var toolName = toolMatch.Groups["Tool"].Value;
                        var tool = Tools.FirstOrDefault(t => t.ToolName == toolName);
                        if (tool == null)
                        {
                            tool = new Tool(toolName);
                            Tools.Add(tool);
                        }
                        tool.LinesToChange.Add(lineIndex);
                        lineIndex++;
                        continue;
                    }

                    var tempMatch = tempRegex.Match(line);

                    if (!tempMatch.Success)
                    {
                        tempMatch = curaTempRegex.Match(line);
                    }

                    if (tempMatch.Success)
                    {
                        var tool = tempMatch.Groups["Tool"].Value;
                        var temp = double.Parse(tempMatch.Groups["Temp"].Value);
                        var tempCommand = tempMatch.Groups["TempCommand"].Value;
                        if (MaxToolTemp.ContainsKey(tool))
                        {
                            if (MaxToolTemp[tool] < temp)
                            {
                                MaxToolTemp[tool] = temp;
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(tool))
                            {
                                MaxToolTemp.Add(tool, temp);
                            }
                        }
                        //We'll change the tool to T0 and use the temp the tool is currently set to.
                        _fileLines[lineIndex] = $"{tempCommand} S{temp} T0";
                    }
         
                    



        lineIndex++;
                }
            }
        }


        public void Write(string savePath)
        {
            //var path = System.IO.Path.GetDirectoryName(savePath);
            //var filename = System.IO.Path.GetFileName(savePath);

            foreach (var tool in Tools)
            {
                var gcodeLineColor = tool.GCodeLineColor();
                foreach (var lineIdx in tool.LinesToChange)
                {
                    _fileLines[lineIdx] = gcodeLineColor;
                }
            }

            using (var file = new System.IO.StreamWriter(savePath))
            {
                file.WriteLine("T0"); //Set the default tool to T0 right from the get go since we'll be targeting tool 0 for all the actions. May need to make this configurable.
                foreach (var line in _fileLines)
                {
                    file.WriteLine(line);
                }
            }
        }

        public string Path => _filePath;
    }
}

