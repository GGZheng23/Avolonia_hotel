using System;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Avalonia_Hotel;

public class Prenotazione
{
    [ReadOnly(true)]
    public int id { get; set; }
    public string nomeCliente{ get; set; }
    public string cognomeCliente{ get; set; }
    public string sesso{ get; set; }
    
    [ReadOnly(true)]
    public DateOnly dataCheckIn { get; set; }
    [ReadOnly(true)]
    public DateOnly dataCheckOut { get; set; }
    public int numPersone { get; set; }
    public string telefono { get; set; }

    public override string ToString()
    {
        return "Prenotazione selezionata: " +id + " " + nomeCliente + " " + cognomeCliente;
    }
}