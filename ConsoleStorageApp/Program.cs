using ConsoleStorageApp.Common;
using ConsoleStorageApp.Services;

ILogger logger = new ConsoleLogger();
StorageDataLoader loader = new StorageDataLoader(logger);
Storage storage = new Storage(loader, logger);

await storage.Fill();

// Задание 1
storage.ShowStorageInformation();
// Задание 2
storage.ShowPallets(3);

Console.WriteLine("Для выхода нажмите любую клавишу");
Console.ReadKey();