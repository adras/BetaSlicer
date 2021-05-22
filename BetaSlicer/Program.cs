using BetaSlicer.GUI;
using System;
using Veldrid.NeoDemo;

namespace BetaSlicer
{
    class Program
    {
        static void Main(string[] args)
        {
            //OldMainWindow mainWindow = new OldMainWindow();
            //mainWindow.Show();

            MainWindow window = new MainWindow();
            window.Run();
        }
    }
}
