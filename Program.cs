using Pinger;

try
{
    PrepareConsole();
    var app = new PingApp(args);
    app.Run();
    Console.ReadKey(true);
    app.Stop();
    Console.WriteLine("Application stopped. Press any key");
    Console.ReadKey(true);
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Fatal Error:");
    Console.WriteLine(ex.Message);
    Console.ResetColor();
}

static void PrepareConsole()
{

}