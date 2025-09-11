using AppBibleNew.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace AppBibleNew.Service
{
    public class PrayerService : IPrayerService
    {
        private readonly string _filename = "Prayer.json";

        private async Task<string> LoadRawJsonAsync()
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync(_filename);
            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }

        public async Task<List<PrayerRoot>> LoadPrayerAsync()
        {
            var root = await LoadRawJsonAsync();
            return JsonSerializer.Deserialize<List<PrayerRoot>>(root, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<PrayerRoot>();
        }

        public async Task<List<PrayerPart>> LoadPrayerNameAsync(string prayName)
        {
            var prayer = await LoadPrayerAsync();
            var pray = prayer.Find(b => b.PartName.Equals(prayName, StringComparison.OrdinalIgnoreCase));
            return pray?.PrayerPart ?? new List<PrayerPart>();
        }

        public async Task<List<PrayerItem>> LoadPrayerPartAsync(string prayItem, string prayName)
        {
            var prayer = await LoadPrayerNameAsync(prayName);
            var pray = prayer.Find(b => b.Name.Equals(prayItem, StringComparison.OrdinalIgnoreCase));
            return pray?.PrayerItem ?? new List<PrayerItem>();
        }




    }
}
