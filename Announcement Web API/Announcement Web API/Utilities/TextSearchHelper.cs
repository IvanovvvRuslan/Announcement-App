namespace Announcement_Web_API.Utilities;

public static class TextSearchHelper
{
    public static List<string> PrepareSearchTerms (string text)
    {
        if (string.IsNullOrEmpty(text))
            return new List<string>();
        
        return text.ToLower()
            .Split(new[] {' ', ',', '.', '!', '?', ';', ':', '-' }, StringSplitOptions.RemoveEmptyEntries)
            .Where (word => word.Length >= 3)
            .Where(word => !IsStopWord(word))
            .Distinct()
            .Take(10)
            .ToList();
    }

    public static bool IsStopWord(string word)
    {
        var stopWords = new [] { "the", "and", "or", "but", "in", "on", "at", "to", "for", "of", "with", "by" };
        
        return stopWords.Contains(word);
    }
    
    public static int CountMatches(List<string> searchTerms, string text)
    {
        var textWords = text.ToLower()
            .Split(new[] { ' ', ',', '.', '!', '?', ';', ':', '-' }, StringSplitOptions.RemoveEmptyEntries)
            .ToHashSet();

        return searchTerms.Count(term => textWords.Contains(term));
    }
}