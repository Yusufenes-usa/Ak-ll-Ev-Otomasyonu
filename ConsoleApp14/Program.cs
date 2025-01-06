using System;
using System.Collections.Generic;
using NAudio.Wave;
using System.Linq;
using static Oda;
using System.Text;

public class ElektronikEsya
{
    public string Ad { get; private set; }
    public bool IsikliMi { get; private set; }
    public bool AcikMi { get; private set; }

    public ElektronikEsya(string ad, bool isikliMi)
    {
        Ad = ad;
        IsikliMi = isikliMi;
        AcikMi = false;
    }

    public virtual void DurumGuncelle(bool durum)
    {
        AcikMi = durum;
        Console.WriteLine($"{Ad} {(AcikMi ? "açıldı." : "kapandı.")}");
        if (AcikMi)
        {
            Console.WriteLine($"{Ad} enerji tüketiyor.");
        }
        else
        {
            Console.WriteLine($"{Ad} enerji tüketmiyor.");
        }
    }

}

public class SicaklikKontrol
{
    // Odanın sıcaklık değeri  
    public double Sicaklik { get; private set; }

    // Sıcaklık değişimini kontrol et  
    public bool SicakligiDegistir(double yeniSicaklik)
    {
        if (yeniSicaklik < 15 || yeniSicaklik > 30)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Hatalı giriş: Sıcaklık değeri 15 ile 30 arasında olmalıdır.");
            Console.ResetColor();
            return false;
        }
        else
        {
            Sicaklik = yeniSicaklik; // Sıcaklık değeri güncellenir  
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Sıcaklık {Sicaklik}°C olarak güncellendi.");
            Console.ResetColor();
            return true;
        }
    }
}


class Oda : SicaklikKontrol // SicaklikKontrol sınıfından türetiyoruz  
{
    // Odanın adı  
    public string Ad { get; private set; }

    // Odadaki elektronik eşyaların listesi  
    private List<ElektronikEsya> Esyalar { get; set; }

    // Oda sınıfının kurucusu  
    public Oda(string ad, double baslangicSicaklik) : base() // SicaklikKontrol sınıfının kurucusunu çağır  
    {
        Ad = ad; // Oda adı atanır  
        SicakligiDegistir(baslangicSicaklik); // Başlangıç sıcaklığını ayarlamak  
        Esyalar = new List<ElektronikEsya>(); // Elektronik eşya listesi başlatılır  
    }

    // Odaya yeni bir eşya ekleyen metot  
    public void EsyaEkle(ElektronikEsya esya)
    {
        Esyalar.Add(esya); // Eşya listeye eklenir  
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"{esya.Ad} {Ad} odasına eklendi.");
        Console.ResetColor();
    }

    // Odadaki eşyaların durumlarını listeleyen metot  
    public void EsyaDurumlariniListele()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"\n{Ad} odası (Sıcaklık: {Sicaklik}°C):");
        foreach (var esya in Esyalar)
        {
            Console.WriteLine($"- {esya.Ad}: {(esya.AcikMi ? "Açık (Enerji tüketiyor)" : "Kapalı (Enerji tüketmiyor)")}");
        }
        Console.ResetColor();
    }
    // Odadaki bir eşyanın açık/kapalı durumunu güncelleyen metot  
    public void EsyaDurumGuncelle(int esyaIndex, bool durum)
    {
        // Geçerli bir eşya numarası girilip girilmediğini kontrol eder  
        if (esyaIndex >= 0 && esyaIndex < Esyalar.Count)
        {
            Esyalar[esyaIndex].DurumGuncelle(durum); // Durumu günceller  
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Hatalı seçim: Geçerli bir eşya numarası giriniz.");
            Console.ResetColor();
        }
    }

    // Odadaki eşyaların listesini döndüren metot  
    public List<ElektronikEsya> GetEsyalar()
    {
        return Esyalar; // Elektronik eşya listesini döndürür  
    }

    // Odadaki bir eşyayı silen metot  
    public void EsyaSil(ElektronikEsya esya)
    {
        Esyalar.Remove(esya);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"{esya.Ad} {Ad} odasından silindi.");
        Console.ResetColor();
    }
}


class Program
{
    // Kullanıcı bilgilerini tutan bir sözlük
    static Dictionary<string, string> kullanicilar = new Dictionary<string, string>(); // Kullanıcı adı ve şifrelerin tutulduğu sözlük
    static string kayitAnaSifresi = "Y1S2aZ"; // Yeni kullanıcı kayıtları için gerekli ürün anahtarı

    static void Main(string[] args)
    {
        while (true)
        {
            try
            {
                // Ekranı temizle ve başlık kısmını yazdır
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("=========================================");
                Console.ResetColor();
                Console.WriteLine("         \u001b[1;33m KULLANICI GİRİŞ EKRANI  \u001b[0m");
                Console.WriteLine("=========================================");

                // Menü seçeneklerini ekrana yazdır
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("1. \u001b[1;34mKayıt Ol\u001b[0m");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("2. \u001b[1;34mGiriş Yap\u001b[0m");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("3. \u001b[1;34mÇıkış\u001b[0m");
                Console.ResetColor();

                // Kullanıcıdan seçim girişi al
                Console.ResetColor();
                Console.Write("\nLütfen seçmek istediğiniz öğenin numarasını tuşlayınız: ");
                string secim = Console.ReadLine();

                // Kullanıcı seçimine göre işlemleri gerçekleştir
                switch (secim)
                {
                    case "1":
                        KayitOl(); // Yeni kullanıcı kaydı yap
                        break;
                    case "2":
                        if (GirisYap()) // Kullanıcı giriş yaparsa menüye yönlendir
                        {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Giriş başarılı! Menüye yönlendiriliyorsunuz...");
                            Console.ResetColor();
                            Console.ReadKey();
                            ProgramMenu(); // Kullanıcı menüsüne yönlendirme
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            HataMesaji("Giriş başarısız. Lütfen kullanıcı adı ve şifrenizi kontrol ediniz.");
                            Console.ResetColor();
                        }
                        break;
                    case "3":
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\u001b[1;33mÇıkış yapılıyor. Güle güle! \u001b[0m");
                        Console.ResetColor();
                        return; // Programdan çıkış
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        HataMesaji("Hatalı seçim: Lütfen 1, 2 veya 3 seçeneklerinden birini giriniz.");
                        Console.ResetColor();
                        break;
                }

                Console.WriteLine("\nDevam etmek için bir tuşa basınız...");
                Console.ReadKey(); // Kullanıcının mesajı görmesi için bekleme
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                HataMesaji($"Bir hata oluştu: {ex.Message}"); // Hata durumunda mesaj yazdır
                Console.ResetColor();
            }
        }
    }

    // Kullanıcı giriş işlemini gerçekleştiren metot
    static bool GirisYap()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=== Giriş Yap ===");
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("Kullanıcı adı: ");
        Console.ResetColor();
        string kullaniciAd = Console.ReadLine();
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("Şifre: ");
        Console.ResetColor();
        string sifre = Console.ReadLine();

        // Kullanıcı adı ve şifre kontrolü
        if (kullanicilar.TryGetValue(kullaniciAd, out var storedSifre) && storedSifre == sifre)
        {
            return true; // Giriş başarılı
        }

        return false; // Giriş başarısız
    }

    // Yeni kullanıcı kaydı işlemini gerçekleştiren metot
    static void KayitOl()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=== Kayıt Ol ===");
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("Lütfen kayıt olmak için size ait 6 haneli şifreyi giriniz: ");
        Console.ResetColor();
        string girilenAnaSifre = ReadPassword();

        if (girilenAnaSifre != kayitAnaSifresi) // Ürün anahtarı kontrolü
        {
            Console.ForegroundColor = ConsoleColor.Red;
            HataMesaji("Hatalı şifre. Kayıt işlemi iptal edildi.");
            Console.ResetColor();
            return;
        }

        string kullaniciAd;
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Kullanıcı adı belirleyin (en az bir harf içermelidir): ");
            Console.ResetColor();
            kullaniciAd = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(kullaniciAd) && kullaniciAd.Any(char.IsLetter)) // Kullanıcı adı geçerliliği kontrolü
            {
                if (kullanicilar.ContainsKey(kullaniciAd))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    HataMesaji("Bu kullanıcı adı zaten alınmış. Lütfen farklı bir kullanıcı adı belirleyiniz.");
                    Console.ResetColor();
                }
                else
                {
                    break; // Geçerli bir kullanıcı adı
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                HataMesaji("Geçersiz kullanıcı adı. Lütfen en az bir harf içeren bir kullanıcı adı giriniz.");
                Console.ResetColor();
            }
        }

        string sifre;
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Şifre belirleyin (en az 8 karakter, bir büyük harf, bir küçük harf ve bir rakam içermelidir): ");
            Console.ResetColor();
            sifre = Console.ReadLine();
            if (SifreKontrol(sifre)) // Şifre kriterlerini kontrol eden metodun çağrılması
                break;
            else
                Console.ForegroundColor = ConsoleColor.Red;
            HataMesaji("Şifre geçerli değil. Lütfen belirtilen kriterlere uygun bir şifre giriniz.");
            Console.ResetColor();

        }

        kullanicilar.Add(kullaniciAd, sifre); // Yeni kullanıcı eklenmesi
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Kayıt başarılı! Şimdi giriş yapabilirsiniz.");
        Console.ResetColor();
    }
    static string ReadPassword()
    {
        StringBuilder password = new StringBuilder();
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(intercept: true); // Tuşu oku, ekrana yazdırma  

            if (key.Key == ConsoleKey.Backspace) // Eğer backspace tuşuna basıldıysa  
            {
                if (password.Length > 0)
                {
                    password.Remove(password.Length - 1, 1); // Son karakteri sil  
                    Console.Write("\b \b"); // Konsolda bir karakter geri sil  
                }
            }
            else if (key.Key != ConsoleKey.Enter) // Enter tuşuna basılmadıysa  
            {
                password.Append(key.KeyChar); // Tuşu string içine ekle  
                Console.Write("*"); // Konsolda yıldız yazdır  
            }
        } while (key.Key != ConsoleKey.Enter); // Enter tuşuna kadar devam et  

        Console.WriteLine(); // Yeni satıra geç  
        return password.ToString(); // Şifreyi döndür  
    }
    // Şifre kriterlerini kontrol eden yardımcı metot
    static bool SifreKontrol(string sifre)
    {
        return sifre.Length >= 8 // En az 8 karakter
            && sifre.Any(char.IsUpper) // En az bir büyük harf
            && sifre.Any(char.IsLower) // En az bir küçük harf
            && sifre.Any(char.IsDigit); // En az bir rakam
    }

    // Hata mesajlarını ekrana yazdıran yardımcı metot
    static void HataMesaji(string mesaj)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(mesaj);
        Console.ResetColor();
    }


    // Kullanıcıdan string türünde bir girdi alan ve hatalı girişlerde uyarı mesajı gösteren fonksiyon
    static string StringGirdiAl(string mesaj)
    {
        while (true)
        {
            Console.Write(mesaj); // Kullanıcıya mesaj gösterilir
            string girdi = Console.ReadLine(); // Kullanıcıdan girdi alınır
            if (!double.TryParse(girdi, out _)) // Girdi yalnızca harflerden oluşuyorsa kabul edilir
                return girdi;
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Hatalı giriş: Lütfen yalnızca harflerden oluşan bir isim giriniz.");
                Console.ResetColor();
            }
        }
    }

    // Kullanıcıdan tam sayı türünde bir girdi alan ve belirli bir aralıkta olmasını kontrol eden fonksiyon
    static int IntGirdiAl(string mesaj, int minDeger = 0, int maxDeger = 100)
    {
        int deger;
        while (true)
        {
            Console.Write(mesaj); // Kullanıcıya mesaj gösterilir
            if (int.TryParse(Console.ReadLine(), out deger) && deger >= minDeger && deger <= maxDeger) // Girdi doğrulanır
                return deger;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Geçersiz değer. Lütfen {minDeger} ile {maxDeger} arasında bir tam sayı giriniz.");
            Console.ResetColor();
        }
    }

    // Kullanıcıdan double türünde bir girdi alan ve belirli bir aralıkta olmasını kontrol eden fonksiyon
    static double DoubleGirdiAl(string mesaj, double min, double max)
    {
        while (true)
        {
            Console.Write(mesaj); // Kullanıcıya mesaj gösterilir
            try
            {
                double sonuc = double.Parse(Console.ReadLine()); // Kullanıcıdan girdi alınır
                if (sonuc >= min && sonuc <= max) // Girdi doğrulanır
                    return sonuc;
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Hatalı giriş: Değer {min} ile {max} arasında olmalıdır.");
                    Console.ResetColor();
                }
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Hatalı giriş: Lütfen geçerli bir sayı giriniz.");
                Console.ResetColor();
            }
        }

    }


    // Kullanıcıdan eşya bilgilerini alarak mevcut bir odaya eşya ekleyen fonksiyon
    static void YeniEsyaEkle(List<Oda> odalar)
    {
        // Öncelikle odaları isme göre sıralama  
        odalar.Sort((o1, o2) => string.Compare(o1.Ad, o2.Ad, StringComparison.CurrentCulture));
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Mevcut odalar:");
        Console.ResetColor();
        for (int i = 0; i < odalar.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {odalar[i].Ad}");
        }
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Yellow;
        string odaAdi = StringGirdiAl("Eşya eklemek istediğiniz odanın adını giriniz: "); // Kullanıcıdan oda adı alınır  
        Console.ResetColor();
        var oda = odalar.Find(o => o.Ad == odaAdi); // Oda listesinde arama yapılır  

        Console.Clear();

        if (oda != null) // Oda bulunduysa  
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Seçilen oda: {odaAdi}");
            Console.WriteLine("===============================");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Cyan;
            int esyaSayisi = IntGirdiAl("Eklemek istediğiniz eşya sayısını giriniz (1-100): ", 1, 100); // Eklenecek eşya sayısı alınır  
            Console.ResetColor();
            Console.Clear();

            for (int i = 0; i < esyaSayisi; i++)
            {
                string yeniEsyaAdi;

                do
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    yeniEsyaAdi = StringGirdiAl($"Eşya {i + 1} adını giriniz: "); // Eşya adı alınır  
                    Console.ResetColor();

                    if (string.IsNullOrWhiteSpace(yeniEsyaAdi)) // Boş veya sadece boşluk olan girdi kontrolü  
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\u001b[1;31mEşya adı boş olamaz. Lütfen geçerli bir isim giriniz.\u001b[0m");
                        Console.ResetColor();
                    }

                } while (string.IsNullOrWhiteSpace(yeniEsyaAdi)); // Geçerli bir isim girilene kadar döngü devam eder  

                // Kullanıcıdan akıllı eşya olup olmadığını sor  
                bool akilliMi = false;
                Console.ForegroundColor = ConsoleColor.Yellow;
                string akilliCevap = StringGirdiAl($"{yeniEsyaAdi} akıllı bir eşya mı? (Evet/Hayır): ");
                Console.ResetColor();
                if (akilliCevap.ToLower() == "evet")
                {
                    akilliMi = true;
                }

                bool isikliMi = yeniEsyaAdi.ToLower() == "lamba" || yeniEsyaAdi.ToLower() == "televizyon"; // Varsayılan ışıklı eşya tanımı  
                ElektronikEsya yeniEsya;

                // Akıllı eşya oluşturma  


                yeniEsya = new ElektronikEsya(yeniEsyaAdi, isikliMi); // Normal eşya  


                oda.EsyaEkle(yeniEsya); // Odaya eşya eklenir  

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\u001b[1;32m{yeniEsyaAdi} başarıyla {odaAdi} odasına eklendi.\u001b[0m");
                Console.ResetColor();
            }
        }
        else // Oda bulunamadıysa  
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\u001b[1;31mBelirttiğiniz isimde bir oda bulunamadı.\u001b[0m");
            Console.ResetColor();
        }
        Console.ResetColor();
    }

    // Kullanıcıdan oda bilgilerini alarak yeni bir oda ekleyen fonksiyon
    static void YeniOdaEkle(List<Oda> odalar)
    {
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("===============================");
        Console.ResetColor();
        Console.WriteLine($"\u001b[1;33m Yeni Oda bilgilerini giriniz:  \u001b[0m");
        Console.WriteLine("===============================");

        // Oda adı kontrolü  
        string yeniOdaAdi;
        do
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            yeniOdaAdi = StringGirdiAl("Yeni odanın adını giriniz: "); // Oda adı alınır  
            Console.ResetColor();

            if (string.IsNullOrWhiteSpace(yeniOdaAdi)) // Boş girdi kontrolü  
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\u001b[1;31mOda adı boş olamaz. Lütfen geçerli bir isim giriniz.\u001b[0m");
                Console.ResetColor();
            }
        } while (string.IsNullOrWhiteSpace(yeniOdaAdi)); // Geçerli bir isim girilene kadar döngü devam eder  

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("===============================");
        Console.ResetColor();
        Console.WriteLine($"\u001b[1;33m Yeni Oda bilgilerini giriniz:  \u001b[0m");
        Console.WriteLine("===============================");

        // Oda sıcaklığını alma  
        Console.ForegroundColor = ConsoleColor.Yellow;
        double yeniSicaklik = DoubleGirdiAl("Yeni odanın sıcaklığını (15-30°C arası) giriniz: ", 15, 30); // Oda sıcaklığı alınır  
        Console.ResetColor();
        Console.Clear();

        Oda yeniOda = new Oda(yeniOdaAdi, yeniSicaklik); // Yeni oda oluşturulur  

        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("===============================");
        Console.ResetColor();
        Console.WriteLine($"\u001b[1;33m Yeni oda Eşya bilgileri  \u001b[0m");
        Console.WriteLine("===============================");

        // Eşya sayısını alma ve kontrol  
        Console.ForegroundColor = ConsoleColor.Yellow;
        int yeniEsyaSayisi = IntGirdiAl("Bu oda için kaç eşya eklemek istiyorsunuz? ", 1, 100); // Eklenecek eşya sayısı alınır  
        Console.ResetColor();
        Console.Clear();

        for (int i = 0; i < yeniEsyaSayisi; i++)
        {
            string yeniEsyaAdi;
            do
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("===============================");
                Console.ResetColor();
                Console.WriteLine($"\u001b[1;33m Yeni Eşya bilgileri  \u001b[0m");
                Console.WriteLine("===============================");
                Console.ForegroundColor = ConsoleColor.Yellow;

                yeniEsyaAdi = StringGirdiAl($"Eşya {i + 1} adını giriniz: "); // Eşya adı alınır  
                Console.ResetColor();

                if (string.IsNullOrWhiteSpace(yeniEsyaAdi)) // Boş girdi kontrolü  
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\u001b[1;31mEşya adı boş olamaz. Lütfen geçerli bir isim giriniz.\u001b[0m");
                    Console.ResetColor();
                }

            } while (string.IsNullOrWhiteSpace(yeniEsyaAdi)); // Geçerli bir isim girilene kadar döngü devam eder  

            bool isikliMi = yeniEsyaAdi.ToLower() == "lamba" || yeniEsyaAdi.ToLower() == "televizyon"; // Varsayılan ışıklı eşya tanımı  
            ElektronikEsya yeniEsya = new ElektronikEsya(yeniEsyaAdi, isikliMi); // Yeni eşya oluşturulur  
            yeniOda.EsyaEkle(yeniEsya); // Odaya eşya eklenir  

            // Odaya eşya eklendi başarı mesajı  
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\u001b[1;32m{yeniEsyaAdi} başarıyla {yeniOdaAdi} odasına eklendi.\u001b[0m");
            Console.ResetColor();
            Console.Clear();
        }

        odalar.Add(yeniOda); // Oda listeye eklenir  
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\u001b[1;32m{yeniOdaAdi} başarıyla eklendi.\u001b[0m");
        Console.ResetColor();
        Console.ResetColor();
    }


    // Programın ana ekranına dönmesini sağlayan fonksiyon
    static void GirisEkraninaDon()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\u001b[1;34mGiriş ekranına dönülüyor...\u001b[0m");
        Console.ResetColor();
        Main(null); // Programın ana ekranını yeniden çalıştırır
    }
    static void OdaSil(List<Oda> odalar)
    {
        // Kullanıcıya mevcut odaları listeler
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Mevcut odalar:");
        Console.ResetColor();
        for (int i = 0; i < odalar.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {odalar[i].Ad}");
        }
        Console.ResetColor();

        // Silinecek odanın adını sorar
        Console.ForegroundColor = ConsoleColor.Yellow;
        string odaAdi = StringGirdiAl("Silmek istediğiniz odanın adını giriniz: ");
        Console.ResetColor();

        // Girilen ad ile eşleşen oda var mı kontrol edilir
        var oda = odalar.Find(o => o.Ad == odaAdi);



        if (oda != null)
        {
            // Oda listeden kaldırılır
            odalar.Remove(oda);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\u001b[1;32m{odaAdi} başarıyla silindi.\u001b[0m");
            Console.ResetColor();
        }
        else
        {
            // Oda bulunamazsa hata mesajı
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\u001b[1;31mHata: Belirttiğiniz isimde bir oda bulunamadı.\u001b[0m");
            Console.ResetColor();
        }



    }
    static void EsyaSil(List<Oda> odalar)
    {
        // Kullanıcıya mevcut odaları listeler
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Mevcut odalar:");
        Console.ResetColor();
        for (int i = 0; i < odalar.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {odalar[i].Ad}");
        }
        Console.ResetColor();

        // Kullanıcıdan oda adı alınır
        Console.ForegroundColor = ConsoleColor.Yellow;
        string odaAdi = StringGirdiAl("Eşya silmek istediğiniz odanın adını giriniz: ");
        Console.ResetColor();

        // Girilen ada sahip oda bulunur
        var oda = odalar.Find(o => o.Ad == odaAdi);
        if (oda != null)
        {
            // Oda içindeki eşyalar listelenir
            var esyalar = oda.GetEsyalar();
            if (esyalar.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\u001b[1;31mBu odada hiç eşya yok.\u001b[0m");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{odaAdi} odasındaki mevcut eşyalar:");
            Console.ResetColor();
            for (int i = 0; i < esyalar.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {esyalar[i].Ad}");
            }
            Console.ResetColor();

            // Kullanıcıdan silinecek eşya seçimi alınır
            Console.ForegroundColor = ConsoleColor.Yellow;
            int esyaIndex = IntGirdiAl("Silmek istediğiniz eşyanın numarasını giriniz: ") - 1;
            Console.ResetColor();

            // Girilen numara doğrulanır
            if (esyaIndex >= 0 && esyaIndex < esyalar.Count)
            {
                var silinecekEsya = esyalar[esyaIndex];
                oda.EsyaSil(silinecekEsya);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\u001b[1;32m{silinecekEsya.Ad} başarıyla silindi.\u001b[0m");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\u001b[1;31mHata: Geçerli bir eşya numarası giriniz.\u001b[0m");
                Console.ResetColor();
            }
        }
        else
        {
            // Oda bulunamazsa hata mesajı
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\u001b[1;31mHata: Belirttiğiniz isimde bir oda bulunamadı.\u001b[0m");
            Console.ResetColor();
        }
    }




    static void ProgramMenu()
    {
        while (true)
        {
            // Başlangıç menüsünü göster
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("===============================");
            Console.ResetColor();
            Console.WriteLine("\u001b[1;33m Akıllı Ev Sistemine Hoş Geldiniz!  \u001b[0m");
            Console.WriteLine("===============================");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("1. Menüye Git");
            Console.WriteLine("2. Oda ve Eşya Bilgileri Girmek");
            Console.WriteLine("3. Çıkış");
            Console.ResetColor();

            // Kullanıcıdan seçim al
            int secim = IntGirdiAl("Lütfen bir seçim yapınız (1-3): ", 1, 3);

            switch (secim)
            {
                case 1:
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Menüye gidiliyor...");
                    Console.ResetColor();
                    Console.ReadKey();
                    menü(); // Ana menüye dönmek için metodu çağır
                    break;

                case 2:
                    OdaVeEsyaBilgisiGir(); // Oda ve eşya bilgileri girmek için metodu çağır
                    break;

                case 3:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Çıkış yapılıyor. Güle güle!");
                    Console.ResetColor();
                    Environment.Exit(0); // Programdan çıkış
                    break;





            }
        }
    }
    static List<Oda> odalar = new List<Oda>();
    static void OdaVeEsyaBilgisiGir()
    {
        // Odaları tutacak listeyi oluştur  


        // Kullanıcıdan eklemek istediği oda sayısını al  
        Console.ForegroundColor = ConsoleColor.Yellow;
        int odaSayisi = IntGirdiAl("Kaç oda eklemek istiyorsunuz? ");
        Console.ResetColor();

        // Kullanıcının belirttiği sayıda oda ekle  
        for (int i = 0; i < odaSayisi; i++)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("===============================");
            Console.ResetColor();
            Console.WriteLine($"\u001b[1;33m {i + 1}. Oda bilgilerini giriniz:  \u001b[0m");
            Console.WriteLine("===============================");

            // Kullanıcıdan oda adı alma ve kontrol etme  
            string odaAdi;
            do
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                odaAdi = StringGirdiAl($"Oda {i + 1} ismini giriniz: ");
                Console.ResetColor();

                if (string.IsNullOrWhiteSpace(odaAdi)) // Boş girdi kontrolü  
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\u001b[1;31mOda adı boş olamaz. Lütfen geçerli bir isim giriniz.\u001b[0m");
                    Console.ResetColor();
                }
            } while (string.IsNullOrWhiteSpace(odaAdi)); // Geçerli bir isim girilene kadar döngü devam eder  

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("===============================");
            Console.ResetColor();
            Console.WriteLine($"\u001b[1;33m {i + 1}. Oda bilgilerini giriniz:  \u001b[0m");
            Console.WriteLine("===============================");

            // Oda sıcaklığını alma  
            Console.ForegroundColor = ConsoleColor.Yellow;
            double baslangicSicaklik = DoubleGirdiAl($"{odaAdi} sıcaklığını (15-30°C arası) giriniz: ", 15, 30);
            Oda yeniOda = new Oda(odaAdi, baslangicSicaklik);
            Console.ResetColor();
            Console.Clear();

            // Kullanıcıdan oda için eşya sayısını alma  
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("===============================");
            Console.ResetColor();
            Console.WriteLine($"\u001b[1;33m {i + 1}. Oda bilgilerini giriniz:  \u001b[0m");
            Console.WriteLine("===============================");
            Console.ForegroundColor = ConsoleColor.Yellow;
            int esyaSayisi = IntGirdiAl($"{odaAdi} için kaç eşya eklemek istiyorsunuz? ");
            Console.ResetColor();
            Console.Clear();

            // Eşyaların adlarını alma ve kontrol etme  
            for (int j = 0; j < esyaSayisi; j++)
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("===============================");
                Console.ResetColor();
                Console.WriteLine($"\u001b[1;33m {i + 1}. Oda Eşya bilgilerini giriniz:  \u001b[0m");
                Console.WriteLine("===============================");
                Console.ForegroundColor = ConsoleColor.Yellow;

                string esyaAdi;
                do
                {
                    esyaAdi = StringGirdiAl($"Eşya {j + 1} adını giriniz: ");
                    Console.ResetColor();

                    if (string.IsNullOrWhiteSpace(esyaAdi)) // Boş girdi kontrolü  
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\u001b[1;31mEşya adı boş olamaz. Lütfen geçerli bir isim giriniz.\u001b[0m");
                        Console.ResetColor();
                    }

                } while (string.IsNullOrWhiteSpace(esyaAdi)); // Geçerli bir isim girilene kadar döngü devam eder  

                bool isikliMi = esyaAdi.ToLower() == "lamba" || esyaAdi.ToLower() == "televizyon"; // Varsayılan ışıklı eşya tanımı  
                ElektronikEsya yeniEsya = new ElektronikEsya(esyaAdi, isikliMi); // Yeni eşya oluşturulur  
                yeniOda.EsyaEkle(yeniEsya); // Odaya eşya eklenir  
                Console.ResetColor();
            }

            // Odayı listeye ekle  
            odalar.Add(yeniOda);
        }

        // Oda ekleme işlemi tamamlandıktan sonra bir mesaj gösterebilirsiniz  
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\u001b[1;32mTüm odalar ve eşyalar başarıyla eklendi!\u001b[0m");
        Console.ResetColor();
        Console.ReadLine();
    }

    static void menü()
    {
        // Ana menüyü döngü içinde göster
        Console.Clear();
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n=========================================");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("              MENÜ            ");
            Console.ResetColor();
            Console.WriteLine("=========================================");

            // Menü seçeneklerini göster  
            string[] menuItems = new string[]
            {
    "Oda sıcaklığını değiştir",
    "Eşyaların durumunu değiştir",
    "Eşyaları ve odaların sıcaklıklarını listele",
    "Yeni eşya ekleme",
    "Yeni oda ekleme",
    "Oda sil",
    "Eşya sil",
    "Müzik (Aç-Kapa)",
    "Giriş ekranına dön",
    "Çıkış"
            };

            for (int i = 0; i < menuItems.Length; i++)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"[{i + 1}] \u001b[1;29m {menuItems[i]} \u001b[0m");
                Console.ResetColor();
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("=========================================");
            Console.ResetColor();
            Console.WriteLine("Lütfen bir seçenek girin: ");






            string secim = Console.ReadLine(); // Kullanıcıdan bir seçim al  
            Console.Clear(); // Ekranı temizle  

            switch (secim) // Kullanıcının yaptığı seçime göre işlemleri belirle  
            {
                case "1":
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("Mevcut Odalar ve Sıcaklıkları:\n");
                    Console.ResetColor();

                    foreach (var o in odalar) // Tüm odalar için  
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine($"{o.Ad} odası (Sıcaklık: {o.Sicaklik}°C)");
                        Console.ResetColor();
                    }

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    string odaAdi = StringGirdiAl("Sıcaklığını değiştirmek istediğiniz odanın adını giriniz: "); // Odanın adını kullanıcıdan al  
                    Console.ResetColor();

                    var oda = odalar.Find(o => o.Ad == odaAdi); // Belirtilen adı taşıyan odayı bul  
                    Console.Clear(); // Ekranı temizle  

                    if (oda != null) // Oda bulunduysa  
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow; // Yazı rengini sarıya döndür  

                        // Yeni sıcaklığı kullanıcıdan al  
                        double yeniSicaklik = DoubleGirdiAl("Yeni sıcaklığı (15-30°C arası) giriniz: ", 15, 30);

                        // Sıcaklığı güncelle  
                        if (oda.SicakligiDegistir(yeniSicaklik)) // Eğer geçerli bir sıcaklıksa güncelleme başarılı olacak  
                        {

                        }

                        // Ekranı temizle  
                    }
                    else // Oda bulunamadıysa  
                    {
                        Console.ForegroundColor = ConsoleColor.Red; // Hata mesajı için kırmızı renk  
                        Console.WriteLine("Hatalı seçim: Belirttiğiniz isimde bir oda bulunamadı."); // Hata mesajı göster  
                        Console.ResetColor(); // Rengi sıfırla  
                    }
                    break;

                case "2":
                    // Mevcut odaları ve eşyaların durumlarını yazdır  
                    Console.ForegroundColor = ConsoleColor.Cyan; // Yazı rengini maviye döndür  
                    Console.WriteLine("Mevcut Odalar ve Eşyalar:\n");
                    Console.ResetColor();
                    foreach (var o in odalar) // Tüm odalar için  
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        o.EsyaDurumlariniListele();
                        Console.ResetColor();
                        Console.WriteLine(); // Oda arasına boşluk bırak  
                    }
                    // Rengi sıfırla  

                    // Kullanıcıdan odanın adını alma  
                    Console.ForegroundColor = ConsoleColor.Yellow; // Yazı rengini sarıya döndür  
                    odaAdi = StringGirdiAl("Durumunu değiştirmek istediğiniz odanın adını giriniz: "); // Odanın adını kullanıcıdan al  
                    oda = odalar.Find(o => o.Ad.Equals(odaAdi, StringComparison.OrdinalIgnoreCase)); // Odayı bul (büyük/küçük harf duyarsız)  

                    Console.ResetColor(); // Rengi sıfırla  

                    if (oda != null) // Oda bulunduysa  
                    {
                        var esyalar = oda.GetEsyalar(); // Odanın eşyalarını al  
                        Console.WriteLine($"{oda.Ad} odasındaki eşyalar:");
                        for (int i = 0; i < esyalar.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {esyalar[i].Ad} - Durum: {(esyalar[i].AcikMi ? "Açık" : "Kapalı")}"); // Eşyaların adını ve durumunu göster  
                        }
                        Console.ForegroundColor = ConsoleColor.Yellow; // Yazı rengini sarıya döndür  
                        int esyaIndex = IntGirdiAl("Durumunu değiştirmek istediğiniz eşyanın numarasını giriniz: ") - 1; // Kullanıcıdan eşya numarasını al  
                        Console.ResetColor(); // Rengi sıfırla  

                        if (esyaIndex >= 0 && esyaIndex < esyalar.Count) // Geçerli bir eşya numarasıysa  
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow; // Yazı rengini sarıya döndür  
                            string durum = StringGirdiAl("Eşya için işlem seçiniz (Aç/Kapat): ").ToLower();// Kullanıcıdan aç/kapat durumu al  
                            Console.ResetColor();
                            if (durum == "aç" || durum == "a") // "aç" veya "a" komutu için  
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                oda.EsyaDurumGuncelle(esyaIndex, true); // Eşya durumunu aç  
                                Console.WriteLine($"{esyalar[esyaIndex].Ad} artık açık."); // Kullanıcıya dönüş mesajı  
                                Console.ResetColor();
                            }
                            else if (durum == "kapat" || durum == "k") // "kapat" veya "k" komutu için  
                            {
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                oda.EsyaDurumGuncelle(esyaIndex, false); // Eşya durumunu kapat  
                                Console.WriteLine($"{esyalar[esyaIndex].Ad} artık kapalı."); // Kullanıcıya dönüş mesajı  
                                Console.ResetColor();
                            }
                            else // Hatalı giriş  
                            {
                                Console.ForegroundColor = ConsoleColor.Red; // Hata mesajı için kırmızı renk  
                                Console.WriteLine("Hatalı seçim: Lütfen 'Aç' veya 'Kapat' yazınız."); // Hata mesajı göster  
                            }
                        }
                        else // Geçersiz eşya numarası  
                        {
                            Console.ForegroundColor = ConsoleColor.Red; // Hata mesajı için kırmızı renk  
                            Console.WriteLine("Hatalı seçim: Geçerli bir eşya numarası giriniz."); // Hata mesajı göster  
                        }
                    }
                    else // Oda bulunamadıysa  
                    {
                        Console.ForegroundColor = ConsoleColor.Red; // Hata mesajı için kırmızı renk  
                        Console.WriteLine("Hatalı seçim: Belirttiğiniz isimde bir oda bulunamadı."); // Hata mesajı göster  
                    }
                    Console.ResetColor(); // Rengi sıfırla  
                    break;

                case "3":
                    if (odalar.Count == 0) // Oda olup olmadığını kontrol et  
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\u001b[1;31mHATA: Listeleyecek odanız yok. Lütfen önce oda ekleyin.\u001b[0m");
                        Console.ResetColor();
                    }
                    else
                    {
                        foreach (var o in odalar) // Tüm odalar için  
                        {
                            o.EsyaDurumlariniListele(); // Odanın eşya durumlarını listele  
                        }
                    }
                    break;
                case "4":
                    YeniEsyaEkle(odalar); // Yeni eşya ekleme fonksiyonu  
                    break;

                case "5":
                    YeniOdaEkle(odalar); // Yeni oda ekleme fonksiyonu  
                    break;
                case "6":
                    OdaSil(odalar);
                    break;

                case "7":
                    EsyaSil(odalar);
                    break;

                case "8":
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("//Ses Sistemi Kontrol Programı: 'aç' yazarak müziği başlatabilir, 'kapat' yazarak durdurabilirsiniz.");
                    Console.WriteLine("Programdan çıkmak için 'çıkış' yazabilirsiniz.//");
                    Console.ResetColor();

                    WaveOutEvent outputDevice = null;
                    AudioFileReader audioFile = null;
                    bool isPlaying = false;

                    try
                    {
                        // Çalışma dizininde "Muzik" klasörünü kontrol et ve oluştur
                        string musicFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Muzik");
                        if (!Directory.Exists(musicFolder))
                        {
                            Directory.CreateDirectory(musicFolder);
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"Müzik klasörü oluşturuldu: {musicFolder}");
                            Console.WriteLine("Lütfen müzik dosyalarınızı bu klasöre ekleyin ve programı tekrar başlatın.");
                            Console.ResetColor();
                            break;
                        }

                        // Klasördeki müzik dosyalarını listele
                        var musicFiles = Directory.GetFiles(musicFolder, "*.mp3");
                        if (musicFiles.Length == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Müzik klasöründe herhangi bir .mp3 dosyası bulunamadı. Lütfen dosya ekleyin.");
                            Console.ResetColor();
                            break;
                        }

                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("Müzik klasöründeki mevcut dosyalar:");
                        for (int i = 0; i < musicFiles.Length; i++)
                        {
                            Console.WriteLine($"{i + 1}. {Path.GetFileName(musicFiles[i])}");
                        }
                        Console.ResetColor();

                        // Kullanıcıdan bir dosya seçmesini iste
                        int selectedIndex = IntGirdiAl("Çalmak istediğiniz müzik dosyasının numarasını giriniz: ", 1, musicFiles.Length) - 1;
                        string selectedFile = musicFiles[selectedIndex];
                        audioFile = new AudioFileReader(selectedFile);
                        outputDevice = new WaveOutEvent();
                        outputDevice.Init(audioFile);

                        while (true)
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("\nKomut giriniz \u001b[1;33m (aç/kapat/çıkış):\u001b[0m ");
                            string command = Console.ReadLine().ToLower();
                            Console.ResetColor();

                            if (command == "aç")
                            {
                                if (!isPlaying)
                                {
                                    outputDevice.Play();
                                    isPlaying = true;
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("Müzik çalmaya başladı.");
                                    Console.ResetColor();
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Müzik zaten çalıyor.");
                                    Console.ResetColor();
                                }
                            }
                            else if (command == "kapat")
                            {
                                if (isPlaying)
                                {
                                    outputDevice.Stop();
                                    isPlaying = false;
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("Müzik durduruldu.");
                                    Console.ResetColor();
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Müzik zaten durdurulmuş.");
                                    Console.ResetColor();
                                }
                            }
                            else if (command == "çıkış")
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine("MP3 Kontrol Programından çıkılıyor...");
                                Console.ResetColor();
                                break;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Hatalı komut: Lütfen 'aç', 'kapat' veya 'çıkış' giriniz.");
                                Console.ResetColor();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Hata: {ex.Message}");
                        Console.ResetColor();
                    }
                    finally
                    {
                        outputDevice?.Dispose();
                        audioFile?.Dispose();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Müzik kaynakları temizlendi.");
                        Console.ResetColor();
                    }
                    break;




                case "9":
                    GirisEkraninaDon(); // Kullanıcı giriş ekranına geri dönmesini sağlayan fonksiyonu çağır  
                    break; // 'case "7"' bitişi  

                case "10":
                    Console.ForegroundColor = ConsoleColor.Yellow; // Yazı rengini sarıya döndür  
                    Console.WriteLine("Çıkış yapılıyor. Güle güle!"); // Çıkış mesajını göster  
                    Console.ResetColor(); // Rengi sıfırla  
                    return; // Programı sonlandırarak çıkış yap  

                default:
                    Console.ForegroundColor = ConsoleColor.Red; // Hata mesajı için kırmızı renk  
                    Console.WriteLine("Hatalı giriş: Lütfen geçerli bir seçim yapınız."); // Hatalı giriş mesajını göster  
                    Console.ResetColor(); // Rengi sıfırla  
                    break; // 'default' kısmının sonu  







            }
        }




    }
}