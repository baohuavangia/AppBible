using AppBibleNew.Model;

namespace AppBibleNew.Service
{
    public interface IBibleService
    {
        Task<List<BibleRoot>> LoadBibleAsync();
        Task<List<BibleBook>> LoadBibleBooksAsync(string bibleName);
        Task<List<Chapter>> LoadBibleChaptersAsync(string bibleName, string bookName);
        Task<List<Section>> LoadBibleSectionsAsync(string bibleName, string bookName, int chapterNumber);
        Task<List<SubSection>> LoadBibleSubSectionsAsync(string bibleName, string bookName, int chapterNumber, string sectionTitle);
        Task<List<Verse>> LoadBibleVersesAsync(string bibleName, string bookName, int chapterNumber, string sectionTitle, string subsectionTitle);
    }
}