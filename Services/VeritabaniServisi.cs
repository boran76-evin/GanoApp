using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using Gano.Models;
using Microsoft.Maui.Storage;

namespace Gano.Services;

public class VeritabaniServisi
{
    private SQLiteAsyncConnection db;

    private async Task Init()
    {
        if (db != null)
            return;

        var yol = Path.Combine(FileSystem.AppDataDirectory, "gano.db");
        db = new SQLiteAsyncConnection(yol);

        await db.CreateTableAsync<Ders>();
    }

    public async Task DersEkle(Ders ders)
    {
        await Init();
        await db.InsertAsync(ders);
    }

    public async Task<List<Ders>> DersleriGetir()
    {
        await Init();
        return await db.Table<Ders>().ToListAsync();
    }
    public async Task<Ders> DersGetir(int id)
    {
        await Init();
        return await db.Table<Ders>().Where(x => x.Id == id).FirstOrDefaultAsync();
    }
    public async Task DersGuncelle(Ders ders)
    {
        await Init();
        await db.UpdateAsync(ders);
    }

    public async Task DersSil(Ders ders)
    {
        await Init();
        await db.DeleteAsync(ders);
    }
}
