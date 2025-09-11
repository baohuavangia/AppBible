using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBibleNew.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class BibleRoot
    {
        [JsonPropertyName("bible")]
        public string Bible { get; set; }

        [JsonPropertyName("biblebook")]
        public List<BibleBook> BibleBook { get; set; }
    }

    public class BibleBook
    {
        [JsonPropertyName("book")]
        public string Book { get; set; }

        [JsonPropertyName("chapters")]
        public List<Chapter> Chapters { get; set; }
    }

    public class Chapter
    {
        [JsonPropertyName("chapter")]
        public int ChapterNumber { get; set; }

        [JsonPropertyName("sections")]
        public List<Section> Sections { get; set; }
    }

    public class Section
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("subsections")]
        public List<SubSection> Subsections { get; set; }
    }

    public class SubSection
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("content")]
        public List<Verse> Content { get; set; }
    }

    public class Verse
    {
        [JsonPropertyName("verse")]
        public int VerseNumber { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }
    }

}
