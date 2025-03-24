
using System;
using Cosmos.System;
using Cosmos.System.Graphics;
using Sys = Cosmos.System;

namespace IronOS
{
    public class Kernel : Sys.Kernel
    {
        private DesktopUI desktop;

        protected override void BeforeRun()
        {
            System.Console.WriteLine("TEST.");
            System.Console.WriteLine("Type 'gui' to enter graphical mode.");
        }

        protected override void Run()
        {
            string input = System.Console.ReadLine();
            if (input == "gui")
            {
                desktop = new DesktopUI();
                desktop.Start();
            }
            else
            {
                System.Console.WriteLine("Unknown command.");
            }
        }
    }
}
