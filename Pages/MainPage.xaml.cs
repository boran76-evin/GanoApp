using Gano.Models;
using Gano.Services;

namespace Gano.Pages;

public partial class MainPage : ContentPage
{
    VeritabaniServisi? db = new VeritabaniServisi();
    public MainPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var dersler = await db.DersleriGetir();
        dersListesi.ItemsSource = dersler;
        GanoHesapla(dersler);
    }

    double HarfPuani(string? harf)
    {
        return harf switch
        {
            "AA" => 4.0,
            "BA" => 3.5,
            "BB" => 3.0,
            "CB" => 2.5,
            "CC" => 2.0,
            "DC" => 1.5,
            "DD" => 1.0,
            "FF" => 0.0,
            _ => 0.0
        };
    }

    async Task GanoAnimasyon()
    {
        await ganoLabel.ScaleToAsync(1.15, 100);
        await ganoLabel.ScaleToAsync(1.0, 100);
    }

    void GanoHesapla(List<Ders> dersler)
    {
        if (dersler == null || dersler.Count == 0)
        {
            ganoLabel.Text = "GANO: 0.00";
            return;
        }

        double toplamPuan = 0;
        double toplamAkts = 0;

        foreach (var ders in dersler)
        {
            if (!ders.OrtalamayaKatiliyormu)
                continue;

            double puan = HarfPuani(ders.BasariNotu);

            toplamPuan += puan * ders.AKTS;
            toplamAkts += ders.AKTS;
        }

        double gano = toplamAkts > 0 ? toplamPuan / toplamAkts : 0;

        ganoLabel.Text = $"GANO: {gano:F2}";
        ganoLabel.TextColor =
            gano >= 3.0 ? Colors.DodgerBlue :
            gano >= 2.0 ? Colors.Orange :
            Colors.Red;
        ganoLabel.Text = $"GANO: {gano:F2}";
        _ = GanoAnimasyon();
    }

    private async void btnDuzenle_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;

        var ders = button?.BindingContext as Ders;

        if (ders == null)
            return;

        await Shell.Current.GoToAsync($"{nameof(DersDuzenlePage)}?id={ders.Id}");
    }

    private async void btnSil_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var ders = button?.BindingContext as Ders;
        if (ders == null)
            return;
        bool onay = await DisplayAlertAsync("Sil","Bu ders silinsin mi?","Evet","Hayır");

        if (!onay)
            return;
        await db.DersSil(ders);

        var dersler = await db.DersleriGetir();
        dersListesi.ItemsSource = dersler;
        GanoHesapla(dersler);
    }
}