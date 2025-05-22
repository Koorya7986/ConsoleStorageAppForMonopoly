using ConsoleStorageApp.Common;
using ConsoleStorageApp.Entities;

namespace ConsoleStorageApp.Services;

public class StorageDataLoader
{
    private const string PALLET_FILENAME = "../../../Pallets.txt";
    private const string BOX_FILENAME = "../../../Boxes.txt";
    private const char SEPARATOR = ';';

    private readonly ILogger _logger;

    public StorageDataLoader(ILogger logger)
    {
        _logger = logger;
    }

    public async Task<List<Pallet>> LoadPalletsAsync()
    {
        List<Pallet> output = new List<Pallet>();

        try
        {
            using StreamReader reader = new StreamReader(PALLET_FILENAME);
            while (!reader.EndOfStream)
            {
                Pallet pallet = await ParsePalletAsync(reader);

                if (pallet.IsValid())
                    output.Add(pallet);
            }

            _logger.Log("Паллеты успешно загружены", LogType.Information);
        }
        catch (FileNotFoundException)
        {
            _logger.Log("Файл с паллетами не найден", LogType.Error);
        }
        catch (Exception ex)
        {
            _logger.Log(ex.Message, LogType.Error);
        }

        return output;
    }

    public async Task<List<Box>> LoadBoxesAsync()
    {
        List<Box> output = new List<Box>();

        try
        {
            using StreamReader reader = new StreamReader(BOX_FILENAME);
            while (!reader.EndOfStream)
            {
                Box box = await ParseBoxAsync(reader);

                if (box.IsValid())
                    output.Add(box);
            }

            _logger.Log("Коробки успешно загружены", LogType.Information);
        }
        catch (FileNotFoundException)
        {
            _logger.Log("Файл с коробками не найден", LogType.Error);
        }
        catch (Exception)
        {
            throw;
        }

        return output;
    }

    private async Task<Box> ParseBoxAsync(StreamReader reader)
    {
        string data = await reader.ReadLineAsync();
        string[] items = data.Split(SEPARATOR);
        return new Box(int.Parse(items[0]),
            int.Parse(items[1]),
            int.Parse(items[2]),
            int.Parse(items[3]),
            int.Parse(items[4]),
            int.Parse(items[5]),
            DateOnly.Parse(items[6]),
            DateOnly.TryParse(items[7], out var result) ? null : result);
    }

    private async Task<Pallet> ParsePalletAsync(StreamReader reader)
    {
        string data = await reader.ReadLineAsync();
        string[] items = data.Split(SEPARATOR);
        return new Pallet(int.Parse(items[0]),
            int.Parse(items[1]),
            int.Parse(items[2]),
            int.Parse(items[3]));
    }
}
