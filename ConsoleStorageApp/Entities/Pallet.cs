namespace ConsoleStorageApp.Entities;

public class Pallet
{
	private const int WEIGHT = 30;

	private int _id;
	private int _width;
	private int _height;
	private int _depth;
	private double _volume;
	private int _weight;
	private DateOnly _expirationDate;
	private List<Box> _boxes = [];

	public int Id => _id;
	public DateOnly ExpirationDate => _expirationDate;
	public int Weight => _weight;
	public double Volume => _volume;
	public IReadOnlyList<Box> Boxes => _boxes.AsReadOnly();

    public Pallet(int id, int width, int height, int depth)
    {
		_id = id;
		_width = width;
		_height = height;
		_depth = depth;
    }

	public bool IsValid() => _id > 0 && _width > 0 && _height > 0 && _depth > 0;

	public void Put(List<Box> boxes)
	{
		_boxes = boxes;
		_volume = (boxes.Sum(box => box.Volume) + _width * _height * _depth) / Math.Pow(10, 6);
		_weight = boxes.Sum(box => box.Weight) + WEIGHT;
		_expirationDate = boxes.Min(box => box.ExpirationDate);
	}

	public override string ToString() 
		=> $"Id: {_id} | Вес: {_weight} | Срок годности: {_expirationDate} | Суммарный объём: {_volume.ToString("##.##")} м3";
}
