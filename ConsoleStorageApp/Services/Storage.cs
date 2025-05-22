using ConsoleStorageApp.Common;
using ConsoleStorageApp.Entities;
using System.Text;

namespace ConsoleStorageApp.Services;

public class Storage
{
    private List<Pallet> _pallets = [];
    private readonly StorageDataLoader _dataLoader;
    private readonly ILogger _logger;

    public Storage(StorageDataLoader dataLoader, ILogger logger)
    {
        _dataLoader = dataLoader;
        _logger = logger;
    }

    public async Task Fill()
    {
		_pallets.Clear();
		_pallets.TrimExcess();

        var palletsTask = _dataLoader.LoadPalletsAsync();
        var boxesTask = _dataLoader.LoadBoxesAsync();

        await Task.WhenAll(palletsTask, boxesTask);
        _pallets.AddRange(palletsTask.Result);

        foreach (var pallet in _pallets)
        {
            var boxes = boxesTask.Result.Where(box => box.PalletId == pallet.Id).ToList();
            pallet.Put(boxes);
        }

        _logger.Log(_pallets.Count != 0 ? $"Данные загружены: паллетов - {_pallets.Count}" : "Паллеты не были загружены",
            LogType.Information);
    }

    public void ShowStorageInformation()
    {
        if (_pallets.Count == 0)
        {
            _logger.Log("Паллеты не загружены", LogType.Error);
            return;
        }

        var grouppedPallets = _pallets
            .OrderBy(pl => pl.ExpirationDate)
            .GroupBy(pl => pl.ExpirationDate)
            .Select(item => new
            {
                ExpirationDate = item.Key,
                Pallets = item.OrderBy(pl => pl.Weight).ToList()
            });

        StringBuilder builder = new StringBuilder();
        builder.AppendLine("\nЗАДАНИЕ 1. Сгруппировать все паллеты по сроку годности, отсортировать по возрастанию срока годности, в каждой группе отсортировать паллеты по весу");

        foreach (var group in grouppedPallets)
        {
            builder.AppendLine();
            foreach (var pallet in group.Pallets)
                builder.AppendLine(pallet.ToString());
        }

        _logger.Log(builder.ToString(), LogType.Information);
    }

    public void ShowPallets(int amount)
    {
        if (_pallets.Count == 0)
        {
            _logger.Log("Паллеты не загружены", LogType.Error);
            return;
        }

        var pallets = _pallets
            .Select(pl => new
            {
                Pallet = pl,
                MaxExpDate = pl.Boxes.Max(box => box.ExpirationDate),
            })
            .OrderBy(box => box.MaxExpDate)
            .ThenBy(box => box.Pallet.Boxes.Sum(box => box.Volume))
            .Take(3)
            .Select(item => item.Pallet)
            .ToList();

        StringBuilder builder = new StringBuilder();
        builder.AppendLine("\nЗАДАНИЕ 2. 3 паллеты, которые содержат коробки с наибольшим сроком годности, отсортированные по возрастанию объема");

        foreach (var pl in pallets)
            builder.AppendLine($"Id: {pl.Id} Максимальный срок годности: {pl.Boxes.Max(box => box.ExpirationDate)} Объём: {pl.Volume.ToString("##.##")} м3");

        _logger.Log(builder.ToString(), LogType.Information);
    }
}
