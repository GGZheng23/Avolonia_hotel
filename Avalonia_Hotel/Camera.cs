namespace Avalonia_Hotel;

public class Camera
{
    public int id { get; set; }
    public string tipo { get; set; }
    public double costo { get; set; }

    public override string ToString()
    {
        return id+ " " + tipo +" " + costo + "/g";
    }
}