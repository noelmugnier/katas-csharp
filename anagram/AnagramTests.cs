namespace anagram;

public class AnagramTests
{
    private string _word = "anagram";

    [Test]
    public void IsNotAnagram()
    {
        Assert.IsFalse("naga".IsAnagramFor(_word));
        Assert.IsFalse(" ".IsAnagramFor(_word));
        Assert.IsFalse(((string?)null).IsAnagramFor(_word));
        Assert.IsFalse("naga".IsAnagramFor(null));
    }

    [Test]
    public void IsAnagram()
    {
        Assert.IsTrue("nagaram".IsAnagramFor(_word));
        Assert.IsTrue(" nagaram ".IsAnagramFor(_word));
        Assert.IsTrue("AnaGraM".IsAnagramFor(_word));
    }

    [Test]
    public void FindAnagrams()
    {
        Assert.AreEqual(2, new List<string> { "nagaram", "margana", "azes" }.FindAnagramsFor(_word).Count);
    } 
}

public static class AnagramExtensions
{
    public static int CountAnagramsFor(this List<string> words, string anagram)
    {
        return words.FindAnagramsFor(anagram).Count;
    }

    public static IList<string> FindAnagramsFor(this List<string> words, string anagram)
    {
        return words.Where(word => word.IsAnagramFor(anagram)).ToList();
    }

    public static bool IsAnagramFor(this string? word, string? anagram)
    {
        word = word?.Trim().ToLowerInvariant() ?? string.Empty;
        anagram = anagram?.Trim().ToLowerInvariant() ?? string.Empty;

        if (word.Length != anagram?.Length)
            return false;

        return word.OrderBy(c => c).SequenceEqual(anagram.OrderBy(c => c));
    }
}
