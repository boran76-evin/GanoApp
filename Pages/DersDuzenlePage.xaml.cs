using Gano.Models;
using Gano.Services;
using System.Globalization;

namespace Gano.Pages;

[QueryProperty(nameof(DersId), "id")]
public partial class DersDuzenlePage : ContentPage
{
    VeritabaniServisi? db = new();
    Ders? mevcutDers;

    private string? dersId = "";
    public string? DersId
    {
        get => dersId;
        set
        {
            dersId = value ?? "";
            _ = DersiYukle();
        }
    }
    public DersDuzenlePage()
    {
        InitializeComponent();
    }

    async Task DersiYukle()
    {
        if (int.TryParse(DersId, out int id))
        {
            mevcutDers = await db.DersGetir(id);

            if (mevcutDers == null)
                return;

            codeEntry.Text = mevcutDers.DersKodu;
            nameEntry.Text = mevcutDers.DersAdi;
            aktsEntry.Text = mevcutDers.AKTS.ToString();
            saatEntry.Text = mevcutDers.HaftalikSaat.ToString();

            notPicker.SelectedItem = mevcutDers.BasariNotu;
            donemPicker.SelectedItem = mevcutDers.Donem;
            ortalamaSwitch.IsToggled = mevcutDers.OrtalamayaKatiliyormu;
        }
    }

    private async void btnGuncelle_Clicked(object sender, EventArgs e)
    {
        try
        {
            double akts = double.TryParse(aktsEntry.Text.Replace(',', '.'),NumberStyles.Any,CultureInfo.InvariantCulture,out var a) ? a : 0;
            double saat = double.TryParse(saatEntry.Text.Replace(',', '.'),NumberStyles.Any,CultureInfo.InvariantCulture,out var s) ? s : 0;

            mevcutDers.DersKodu = codeEntry.Text;
            mevcutDers.DersAdi = nameEntry.Text;
            mevcutDers.AKTS = akts;
            mevcutDers.HaftalikSaat = saat;
            mevcutDers.Donem = donemPicker.SelectedItem?.ToString();
            mevcutDers.BasariNotu = notPicker.SelectedItem?.ToString();
            mevcutDers.OrtalamayaKatiliyormu = ortalamaSwitch.IsToggled;

            await db.DersGuncelle(mevcutDers);

            await DisplayAlertAsync("Başarılı","Ders güncellendi","OK");

            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Hata", ex.Message, "OK");
        }
    }
}