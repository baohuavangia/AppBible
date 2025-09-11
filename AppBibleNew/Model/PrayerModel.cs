using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AppBibleNew.Model
{
    public class PrayerRoot
    {
        [JsonPropertyName("partsname")]
        public string PartName { get; set; }
        [JsonPropertyName("parts")]
        public List<PrayerPart> PrayerPart { get; set; } 
    }

    public class PrayerPart
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("prayers")]
        public List<PrayerItem> PrayerItem { get; set; }
    }

    public class PrayerItem
    {
        [JsonPropertyName("name")]
        public string ItemName { get; set; } 

        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}
