using AppBibleNew.Model;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace AppBibleNew.Service
{
    public class BibleService : IBibleService
    {
        private readonly string _filename = "kinh_thanh_clean.json";

        private async Task<string> LoadRawJsonAsync()
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync(_filename);
            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }

        // Load full Bible structure
        public async Task<List<BibleRoot>> LoadBibleAsync()
        {
            var root = await LoadRawJsonAsync();
            return JsonSerializer.Deserialize<List<BibleRoot>>(root, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<BibleRoot>();
        }

        // Load all books in a bible
        public async Task<List<BibleBook>> LoadBibleBooksAsync(string bibleName)
        {
            var bibles = await LoadBibleAsync();
            var bible = bibles.Find(b => b.Bible.Equals(bibleName, StringComparison.OrdinalIgnoreCase));
            return bible?.BibleBook ?? new List<BibleBook>();
        }

        // Load all chapters in a book
        public async Task<List<Chapter>> LoadBibleChaptersAsync(string bibleName, string bookName)
        {
            var books = await LoadBibleBooksAsync(bibleName);
            var book = books.Find(b => b.Book.Equals(bookName, StringComparison.OrdinalIgnoreCase));
            return book?.Chapters ?? new List<Chapter>();
        }

        // Load all sections in a chapter
        public async Task<List<Section>> LoadBibleSectionsAsync(string bibleName, string bookName, int chapterNumber)
        {
            var chapters = await LoadBibleChaptersAsync(bibleName, bookName);
            var chapter = chapters.Find(c => c.ChapterNumber == chapterNumber);
            return chapter?.Sections ?? new List<Section>();
        }

        // Load all subsections in a section
        public async Task<List<SubSection>> LoadBibleSubSectionsAsync(string bibleName, string bookName, int chapterNumber, string sectionTitle)
        {
            var sections = await LoadBibleSectionsAsync(bibleName, bookName, chapterNumber);
            var section = sections.Find(s => s.Title.Equals(sectionTitle, StringComparison.OrdinalIgnoreCase));
            return section?.Subsections ?? new List<SubSection>();
        }

        // Load verses
        public async Task<List<Verse>> LoadBibleVersesAsync(string bibleName, string bookName, int chapterNumber, string sectionTitle, string subsectionTitle)
        {
            var subsections = await LoadBibleSubSectionsAsync(bibleName, bookName, chapterNumber, sectionTitle);
            var subsection = subsections.Find(ss => ss.Title.Equals(subsectionTitle, StringComparison.OrdinalIgnoreCase));
            return subsection?.Content ?? new List<Verse>();
        }
    }
}
