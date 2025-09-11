using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AppBibleNew.Model
{
    public class RosaryRoot
    {
        [JsonPropertyName("rosary")]
        public Rosary Rosary { get; set; }

        [JsonPropertyName("rosary_flow")]
        public RosaryFlow RosaryFlow { get; set; }

        [JsonPropertyName("schedule")]
        public List<ScheduleItem> Schedule { get; set; }
    }

    // ================== Rosary ==================
    public class Rosary
    {
        [JsonPropertyName("intro")]
        public List<RosaryPrayerItem> Intro { get; set; }

        [JsonPropertyName("mysteries")]
        public Mysteries Mysteries { get; set; }

        [JsonPropertyName("conclusion")]
        public List<RosaryPrayerItem> Conclusion { get; set; }
    }

    public class RosaryPrayerItem
    {
        [JsonPropertyName("prayer")]
        public string Prayer { get; set; }
    }


    public class Mysteries
    {
        [JsonPropertyName("Vui")]
        public List<MysteryItem> Vui { get; set; }

        [JsonPropertyName("Thương")]
        public List<MysteryItem> Thuong { get; set; }

        [JsonPropertyName("Mừng")]
        public List<MysteryItem> Mung { get; set; }

        [JsonPropertyName("Sáng")]
        public List<MysteryItem> Sang { get; set; }
    }

    public class MysteryItem
    {
        [JsonPropertyName("number")]
        public int Number { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("reference")]
        public string Reference { get; set; }
    }

    // ================== Flow ==================
    public class RosaryFlow
    {
        [JsonPropertyName("intro")]
        public List<FlowStep> Intro { get; set; }

        [JsonPropertyName("decade")]
        public List<FlowStep> Decade { get; set; }

        [JsonPropertyName("conclusion")]
        public List<FlowStep> Conclusion { get; set; }
    }

    public class FlowStep
    {
        [JsonPropertyName("step")]
        public int Step { get; set; }

        [JsonPropertyName("prayer")]
        public string Prayer { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; } // Nội dung kinh
    }

    // ================== Schedule ==================
    public class ScheduleItem
    {
        [JsonPropertyName("day")]
        public string Day { get; set; }

        [JsonPropertyName("mystery")]
        public string Mystery { get; set; }
    }
}
