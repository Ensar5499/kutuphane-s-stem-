using System;
using System.Collections.Generic;

class Program
{
    class Kitap
    {
        public string Ad { get; set; }
        public string Yazar { get; set; }
        public int YayınYılı { get; set; }
        public int Stok { get; set; }
        public int GünlükKira { get; set; } 
    }

    class Kiralama
    {
        public string KullanıcıAdı { get; set; }
        public string KitapAdı { get; set; }
        public DateTime İadeTarihi { get; set; }
    }

    static List<Kitap> kitaplar = new List<Kitap>();
    static List<Kiralama> kiralamalar = new List<Kiralama>();

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\nKütüphane Menüsü");
            Console.WriteLine("1. Kitap Ekle");
            Console.WriteLine("2. Kitap Kirala");
            Console.WriteLine("3. Kitap İade");
            Console.WriteLine("4. Kitap Ara");
            Console.WriteLine("5. Raporlama");
            Console.WriteLine("0. Çıkış");
            Console.Write("Seçiminiz: ");
            string secim = Console.ReadLine();

            if (secim == "0")
                break;

            switch (secim)
            {
                case "1":
                    KitapEkle();
                    break;
                case "2":
                    KitapKirala();
                    break;
                case "3":
                    KitapIade();
                    break;
                case "4":
                    KitapAra();
                    break;
                case "5":
                    Raporlama();
                    break;
                default:
                    Console.WriteLine("Geçersiz seçim, tekrar deneyin.");
                    break;
            }
        }
    }

    static void KitapEkle()
    {
        Console.Write("Kitap Adı: ");
        string ad = Console.ReadLine();
        Console.Write("Yazar Adı: ");
        string yazar = Console.ReadLine();
        Console.Write("Yayın Yılı: ");
        int yıl = int.Parse(Console.ReadLine());
        Console.Write("Adet: ");
        int adet = int.Parse(Console.ReadLine());
        Console.Write("Günlük Kira Ücreti (TL): ");
        int günlükKira = int.Parse(Console.ReadLine());

        foreach (var kitap in kitaplar)
        {
            if (kitap.Ad == ad && kitap.Yazar == yazar)
            {
                kitap.Stok += adet;
                Console.WriteLine("Kitap stok miktarı artırıldı.");
                return;
            }
        }

        kitaplar.Add(new Kitap { Ad = ad, Yazar = yazar, YayınYılı = yıl, Stok = adet, GünlükKira = günlükKira });
        Console.WriteLine("Yeni kitap eklendi.");
    }

    static void KitapKirala()
    {
        Console.WriteLine("Mevcut Kitaplar:");
        foreach (var kitap in kitaplar)
        {
            Console.WriteLine($"{kitap.Ad} - {kitap.Yazar} (Stok: {kitap.Stok}, Günlük Kira: {kitap.GünlükKira} TL)");
        }

        Console.Write("Kiralamak istediğiniz kitabın adı: ");
        string ad = Console.ReadLine();
        Kitap secilenKitap = kitaplar.Find(k => k.Ad == ad);

        if (secilenKitap == null || secilenKitap.Stok <= 0)
        {
            Console.WriteLine("Stokta yeterli kitap yok.");
            return;
        }

        Console.Write("Kiralamak istediğiniz gün sayısı: ");
        int gün = int.Parse(Console.ReadLine());
        Console.Write("Bütçeniz (TL): ");
        int bütçe = int.Parse(Console.ReadLine());
        int kiraBedeli = gün * secilenKitap.GünlükKira;

        if (bütçe < kiraBedeli)
        {
            Console.WriteLine("Bütçeniz yeterli değil.");
            return;
        }

        Console.Write("Adınız: ");
        string kullanıcıAdı = Console.ReadLine();

        secilenKitap.Stok--;
        kiralamalar.Add(new Kiralama
        {
            KullanıcıAdı = kullanıcıAdı,
            KitapAdı = ad,
            İadeTarihi = DateTime.Now.AddDays(gün)
        });

        Console.WriteLine($"Kiralama başarılı. İade tarihi: {DateTime.Now.AddDays(gün):dd/MM/yyyy}");
    }

    static void KitapIade()
    {
        Console.Write("İade edeceğiniz kitabın adı: ");
        string ad = Console.ReadLine();

        Kiralama kiralama = kiralamalar.Find(k => k.KitapAdı == ad);
        if (kiralama == null)
        {
            Console.WriteLine("Bu kitap için kiralama kaydı bulunamadı.");
            return;
        }

        Kitap iadeKitap = kitaplar.Find(k => k.Ad == ad);
        if (iadeKitap != null)
        {
            iadeKitap.Stok++;
            kiralamalar.Remove(kiralama);
            Console.WriteLine("Kitap başarıyla iade edildi.");
        }
        else
        {
            Console.WriteLine("İade edilecek kitap kütüphanede bulunamadı.");
        }
    }

    static void KitapAra()
    {
        Console.WriteLine("1. Kitap adına göre ara");
        Console.WriteLine("2. Yazar adına göre ara");
        Console.Write("Seçiminiz: ");
        string secim = Console.ReadLine();

        Console.Write("Arama terimi: ");
        string terim = Console.ReadLine();

        List<Kitap> sonuc = secim == "1"
            ? kitaplar.FindAll(k => k.Ad.ToLower().Contains(terim.ToLower()))
            : kitaplar.FindAll(k => k.Yazar.ToLower().Contains(terim.ToLower()));

        if (sonuc.Count == 0)
        {
            Console.WriteLine("Eşleşen kitap bulunamadı.");
            return;
        }

        foreach (var kitap in sonuc)
        {
            Console.WriteLine($"{kitap.Ad} - {kitap.Yazar} ({kitap.YayınYılı}) - Stok: {kitap.Stok}, Günlük Kira: {kitap.GünlükKira} TL");
        }
    }

    static void Raporlama()
    {
        Console.WriteLine("1. Tüm kitapları listele");
        Console.WriteLine("2. Kirada olan kitapları listele");
        Console.Write("Seçiminiz: ");
        string secim = Console.ReadLine();

        if (secim == "1")
        {
            foreach (var kitap in kitaplar)
            {
                Console.WriteLine($"{kitap.Ad} - {kitap.Yazar} ({kitap.YayınYılı}) - Stok: {kitap.Stok}, Günlük Kira: {kitap.GünlükKira} TL");
            }
        }
        else if (secim == "2")
        {
            foreach (var kiralama in kiralamalar)
            {
                Console.WriteLine($"{kiralama.KitapAdı} - {kiralama.KullanıcıAdı} - İade Tarihi: {kiralama.İadeTarihi:dd/MM/yyyy}");
            }
        }
        else
        {
            Console.WriteLine("Geçersiz seçim.");
        }
    }
}
