using System;
using System.IO;
using Cosmos.System;
using Cosmos.System.FileSystem;
using Cosmos.System.Graphics;
using Sys = Cosmos.System;

namespace IronOS
{
    public class Kernel : Sys.Kernel
    {
        private CosmosVFS fs;
        private bool isGuiMode = false;
        private DesktopUI desktop;

        protected override void BeforeRun()
        {
            // Boot splash screen
            System.Console.Clear();
            System.Console.ForegroundColor = System.ConsoleColor.Yellow;
            System.Console.WriteLine("╔══════════════════════════╗");
            System.Console.WriteLine("║      Booting Iron OS     ║");
            System.Console.WriteLine("╚══════════════════════════╝");
            System.Console.WriteLine("Initializing file system...");
            //fs = new CosmosVFS(); Leaving Blank for now as doesn't work on hardware so can't test GUI
            //Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);

            System.Console.ForegroundColor = System.ConsoleColor.Green;
            System.Console.WriteLine("File system mounted.");
            System.Console.WriteLine("Type 'help' for available commands.");
            System.Console.WriteLine();
        }

        protected override void Run()
        {
            if (!isGuiMode)
            {
                System.Console.Write("IronOS> ");
                string input = System.Console.ReadLine();
                HandleCommand(input);
            }
            else
            {
                desktop?.Update(); // GUI mode update loop
            }
        }

        private void HandleCommand(string input)
        {
            switch (input)
            {
                case "help":
                    System.Console.WriteLine(" ");
                    System.Console.WriteLine("All commands are case specific.");
                    System.Console.WriteLine("_______________________________");
                    System.Console.WriteLine("about    :     Shows information about the OS");
                    System.Console.WriteLine("help     :     Shows commands");
                    System.Console.WriteLine("clear    :     Clears terminal output");
                    System.Console.WriteLine("shutdown :     Shuts down the machine");
                    System.Console.WriteLine("restart  :     Restarts The Machine");
                    System.Console.WriteLine("set background (colour)     : Sets the background colour");
                    System.Console.WriteLine("set text (colour)           : Sets the Text colour");
                    System.Console.WriteLine("time     :     Displays Time");
                    System.Console.WriteLine("whoami   :     Shows logged in user");
                    System.Console.WriteLine("ip       :     Shows your IP address (stub)");
                    System.Console.WriteLine("disk space  :     Shows available space (stub)");
                    System.Console.WriteLine("mkdir    :     Creates Directory");
                    System.Console.WriteLine("deldir   :     Deletes Directory");
                    System.Console.WriteLine("meow     :     Writes to file");
                    System.Console.WriteLine("read     :     Reads file content");
                    System.Console.WriteLine("dir      :     Lists all files");
                    System.Console.WriteLine("gui      :     Enters GUI mode");
                    break;

                case "about":
                    System.Console.WriteLine("Iron OS v1.0");
                    break;

                case "clear":
                    System.Console.Clear();
                    break;

                case "shutdown":
                    System.Console.WriteLine("Shutting down...");
                    Sys.Power.Shutdown();
                    break;

                case "restart":
                    System.Console.WriteLine("Restarting...");
                    Sys.Power.Reboot();
                    break;

                case "time":
                    System.Console.WriteLine("Current Time: " + DateTime.Now);
                    break;

                case "whoami":
                    System.Console.WriteLine("User0");
                    break;

                case "ip":
                    System.Console.WriteLine("IP address not available in text mode.");
                    break;

                case "disk space":
                    System.Console.WriteLine("Disk space info not implemented.");
                    break;

                case "mkdir":
                    System.Console.Write("Directory name: ");
                    string dirName = System.Console.ReadLine();
                    try
                    {
                        Directory.CreateDirectory(@"0:\" + dirName);
                        System.Console.WriteLine("Directory created.");
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine("Error: " + ex.Message);
                    }
                    break;

                case "deldir":
                    System.Console.Write("Directory to delete: ");
                    string delDir = System.Console.ReadLine();
                    try
                    {
                        Directory.Delete(@"0:\" + delDir);
                        System.Console.WriteLine("Directory deleted.");
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine("Error: " + ex.Message);
                    }
                    break;

                case "meow":
                    System.Console.Write("Filename: ");
                    string filename = System.Console.ReadLine();
                    System.Console.Write("Content: ");
                    string content = System.Console.ReadLine();
                    try
                    {
                        File.WriteAllText(@"0:\" + filename, content);
                        System.Console.WriteLine("File written.");
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine("Error: " + ex.Message);
                    }
                    break;

                case "read":
                    System.Console.Write("Filename: ");
                    string readFile = System.Console.ReadLine();
                    try
                    {
                        System.Console.WriteLine(File.ReadAllText(@"0:\" + readFile));
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine("Error: " + ex.Message);
                    }
                    break;

                case "dir":
                    try
                    {
                        var files = Directory.GetFiles(@"0:\");
                        foreach (var file in files)
                        {
                            System.Console.WriteLine(file);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine("Error: " + ex.Message);
                    }
                    break;

                // Background colors
                case "set background blue": System.Console.BackgroundColor = System.ConsoleColor.DarkBlue; System.Console.Clear(); break;
                case "set background red": System.Console.BackgroundColor = System.ConsoleColor.DarkRed; System.Console.Clear(); break;
                case "set background black": System.Console.BackgroundColor = System.ConsoleColor.Black; System.Console.Clear(); break;
                case "set background yellow": System.Console.BackgroundColor = System.ConsoleColor.Yellow; System.Console.Clear(); break;

                // Text colors
                case "set text green": System.Console.ForegroundColor = System.ConsoleColor.Green; break;
                case "set text blue": System.Console.ForegroundColor = System.ConsoleColor.Blue; break;
                case "set text white": System.Console.ForegroundColor = System.ConsoleColor.White; break;
                case "set text black": System.Console.ForegroundColor = System.ConsoleColor.Black; break;

                case "gui":
                    System.Console.Clear();
                    System.Console.WriteLine("Entering GUI mode...");
                    isGuiMode = true;
                    desktop = new DesktopUI();
                    desktop.Start(); // assumes you have this method
                    break;

                default:
                    System.Console.WriteLine("Unknown command. Type 'help' for a list.");
                    break;
            }

            System.Console.WriteLine();
        }
    }
}
