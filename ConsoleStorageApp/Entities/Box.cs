namespace ConsoleStorageApp.Entities;

public class Box
{
	private int _id;
	private int _width;
	private int _height;
	private int _depth;
	private double _volume;
	private int _weight;
	private int _palletId;
	private DateOnly _expirationDate;

	public int Id => _id;
	public int Width => _width;
	public int Depth => _depth;
	public double Volume => _volume;
	public int Weight => _weight;
	public int PalletId => _palletId;
	public DateOnly ExpirationDate => _expirationDate; 

    public Box(int id, int width, int height, int depth, int weight, int palletId, DateOnly? expirationDate, DateOnly? creationDate)
    {
		_id = id;
		_width = width;
		_height = height;
		_depth = depth;
		_volume = width * height * depth;
		_weight = weight;
		_palletId = palletId;
		_expirationDate = expirationDate is null ? creationDate.Value.AddDays(100) : expirationDate.Value;
    }

	public bool IsValid()
	{
		return _id > 0 && _width > 0 && _height > 0 && _depth > 0 && _weight > 0;
	}
}