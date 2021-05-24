using BetaSlicer.GUI;
using OpenTK.Windowing.Desktop;
using System;

namespace BetaSlicer
{
    class Program
    {
        const string WINDOW_TITLE = "BetaSlicer V0.01";

        static void Main(string[] args)
        {
            GameWindowSettings gameWindowSettings = GameWindowSettings.Default;

            NativeWindowSettings nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new OpenTK.Mathematics.Vector2i(800, 600),
                APIVersion = new Version(4, 5),
                //StartFocused,
                //StartVisible,
                Title = WINDOW_TITLE,
                //WindowState = OpenTK.Windowing.Common.WindowState.Normal
            };

            // GameWindowSettings.Default, new NativeWindowSettings() { Size = new Vector2i(1600, 900), APIVersion = new Version(4, 5) 
            MainWindow window = new MainWindow(gameWindowSettings, nativeWindowSettings);
            window.Run();
        }
    }
}
