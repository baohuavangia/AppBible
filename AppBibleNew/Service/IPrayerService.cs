using AppBibleNew.Model;

namespace AppBibleNew.Service
{
    public interface IPrayerService
    {
        Task<List<PrayerRoot>> LoadPrayerAsync();
        Task<List<PrayerPart>> LoadPrayerNameAsync(string prayName);
        Task<List<PrayerItem>> LoadPrayerPartAsync(string prayItem, string prayName);
    }
}