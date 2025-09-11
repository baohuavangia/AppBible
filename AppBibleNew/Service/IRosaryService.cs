using AppBibleNew.Model;

namespace AppBibleNew.Service
{
    public interface IRosaryService
    {
        Task<string?> GetMysteryByDayAsync(string day);
        Task<List<MysteryItem>> LoadMysteriesAsync(string type);
        Task<MysteryItem?> LoadMysteryDetailAsync(string type, int number);
        Task<RosaryRoot> LoadRosaryAsync();
        Task<RosaryFlow> LoadRosaryFlowAsync();
        Task<List<ScheduleItem>> LoadScheduleAsync();
    }
}