using Pinger;

Console.Title = $"Pinger (started: {DateTime.Now.ToLongTimeString()})";
Console.CursorVisible = false;

var app = new PingApp(args);
app.Run();
Console.ReadKey(true);
app.Stop();
Console.WriteLine("Application stopped. Press any key");
Console.ReadKey(true);