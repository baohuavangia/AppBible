using AppBibleNew.Model;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace AppBibleNew.Service
{
    public class RosaryService : IRosaryService
    {
        private readonly string _filename = "RosaryAll.json";

        private async Task<string> LoadRawJsonAsync()
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync(_filename);
            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }

        // Load toàn bộ dữ liệu Rosary
        public async Task<RosaryRoot> LoadRosaryAsync()
        {
            var json = await LoadRawJsonAsync();
            return JsonSerializer.Deserialize<RosaryRoot>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new RosaryRoot();
        }

        // Flow (Intro, Decade, Conclusion)
        public async Task<RosaryFlow> LoadRosaryFlowAsync()
        {
            var root = await LoadRosaryAsync();
            return root.RosaryFlow ?? new RosaryFlow();
        }

        // Lịch đọc theo ngày
        public async Task<List<ScheduleItem>> LoadScheduleAsync()
        {
            var root = await LoadRosaryAsync();
            return root.Schedule ?? new List<ScheduleItem>();
        }

        // Các mầu nhiệm theo loại (Vui, Thương, Mừng, Sáng)
        public async Task<List<MysteryItem>> LoadMysteriesAsync(string type)
        {
            var root = await LoadRosaryAsync();
            return type switch
            {
                "Vui" => root.Rosary.Mysteries?.Vui ?? new List<MysteryItem>(),
                "Thương" => root.Rosary.Mysteries?.Thuong ?? new List<MysteryItem>(),
                "Mừng" => root.Rosary.Mysteries?.Mung ?? new List<MysteryItem>(),
                "Sáng" => root.Rosary.Mysteries?.Sang ?? new List<MysteryItem>(),
                _ => new List<MysteryItem>()
            };
        }

        // Chi tiết 1 mầu nhiệm
        public async Task<MysteryItem?> LoadMysteryDetailAsync(string type, int number)
        {
            var mysteries = await LoadMysteriesAsync(type);
            return mysteries.Find(m => m.Number == number);
        }

        // Trả về loại mầu nhiệm theo ngày (vd: "Thứ Năm" => "Sáng")
        public async Task<string?> GetMysteryByDayAsync(string day)
        {
            var schedule = await LoadScheduleAsync();
            var item = schedule.Find(s => s.Day.Equals(day, System.StringComparison.OrdinalIgnoreCase));
            return item?.Mystery;
        }
    }
}
