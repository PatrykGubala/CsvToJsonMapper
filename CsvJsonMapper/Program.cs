using CsvJsonMapper.Forms;
using System;
using System.Windows.Forms;

namespace CsvJsonMapper
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}