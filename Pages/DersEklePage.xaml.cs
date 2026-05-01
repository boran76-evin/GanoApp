using Gano.Models;
using Gano.Services;
using System.Globalization;

namespace Gano.Pages;

public partial class DersEklePage : ContentPage
{
    VeritabaniServisi db = new VeritabaniServisi();
    Ders? mevcutDers;
    private string? dersId;

    public string? DersId
    {
        get => dersId;
        set
        {
            dersId = value;
            _ = DersiYukle();
        }
    }
  public DersEklePage()
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
        }
    }
    private async void Button_Clicked(object sender, EventArgs e)
    {
        // 1. ZORUNLU ALAN KONTROLÜ
        if (string.IsNullOrWhiteSpace(codeEntry.Text) ||
            string.IsNullOrWhiteSpace(nameEntry.Text))
        {
            await DisplayAlertAsync("Hata", "Ders kodu ve adı boş olamaz", "OK");
            return;
        }

        // 2. SAYISAL KONTROL
        if (!double.TryParse(aktsEntry.Text, out _) ||
            !double.TryParse(saatEntry.Text, out _))
        {
            await DisplayAlertAsync("Hata", "AKTS ve saat sayısal olmalı", "OK");
            return;
        }

        // 3. HARF NOTU KONTROLÜ
        if (notPicker.SelectedItem == null)
        {
            await DisplayAlertAsync("Hata", "Harf notu seçmelisin", "OK");
            return;
        }
        try
        {
            double akts = double.Parse(aktsEntry.Text);
            double saat = double.Parse(saatEntry.Text);

            Ders ders = new Ders
            {
                DersKodu = codeEntry.Text,
                DersAdi = nameEntry.Text,
                AKTS = akts,
                HaftalikSaat = saat,
                Donem = donemPicker.SelectedItem?.ToString(),
                BasariNotu = notPicker.SelectedItem?.ToString(),
                OrtalamayaKatiliyormu = ortalamaSwitch.IsToggled
            };

            await db.DersEkle(ders);

            await DisplayAlertAsync("Başarılı", "Ders kaydedildi", "OK");

            // 4. TEMİZLEME
            codeEntry.Text = "";
            nameEntry.Text = "";
            aktsEntry.Text = "";
            saatEntry.Text = "";
            donemPicker.SelectedItem = null;
            notPicker.SelectedItem = null;
            ortalamaSwitch.IsToggled = false;

            // 5. GERİ DÖN
            await Shell.Current.GoToAsync("//MainPage");
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Hata", ex.Message, "OK");
        }

    }
    private async void Guncelle_Clicked(object sender, EventArgs e)
    {
        mevcutDers.DersKodu = codeEntry.Text;
        mevcutDers.DersAdi = nameEntry.Text;

        double akts = double.TryParse(aktsEntry.Text.Replace(',', '.'),NumberStyles.Any,CultureInfo.InvariantCulture,out var a) ? a : 0;
        double saat = double.TryParse(saatEntry.Text.Replace(',', '.'),NumberStyles.Any,CultureInfo.InvariantCulture,out var s) ? s : 0;
        mevcutDers.AKTS = akts;
        mevcutDers.HaftalikSaat = saat;

        mevcutDers.BasariNotu = notPicker.SelectedItem?.ToString();

        await db.DersGuncelle(mevcutDers);

        await DisplayAlertAsync("Başarılı","Ders güncellendi","OK");

        await Shell.Current.GoToAsync("..");
    }

    private async void Sil_Clicked(object sender, EventArgs e)
    {
        if (mevcutDers == null)
            return;

        bool onay = await DisplayAlertAsync("Sil","Bu dersi silmek istiyor musun?","Evet","Hayır");

        if (!onay)
            return;

        await db.DersSil(mevcutDers);

        await DisplayAlertAsync("Başarılı","Ders silindi","OK");

        await Shell.Current.GoToAsync("..");
    }
}