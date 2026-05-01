using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gano.Models;

public class Ders
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string? DersKodu { get; set; }
    public string? DersAdi { get; set; }
    public double HaftalikSaat { get; set; }
    public double AKTS { get; set; }
    public bool OrtalamayaKatiliyormu { get; set; }
    public string? Donem { get; set; }
    public string? BasariNotu { get; set; }
}
