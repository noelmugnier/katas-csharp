namespace anagram;

public class Kata06Tests
{
    [Test]
    public void CountAnagramsWithLinq()
    {
        var expectedNumberOfAnagrams = 20683;
        var expectedNumberOfWords = 48162;
        var words = File.ReadAllLines("words_list.txt");

        var groupedWords = words.GroupBy(w => new string(w.ToCharArray().OrderBy(c => c).ToArray()))
            .Where(g => g.Count() > 1)
            .Select(g => g.ToList());

        Assert.AreEqual(expectedNumberOfAnagrams, groupedWords.Count());
        Assert.AreEqual(expectedNumberOfWords, groupedWords.Sum(c => c.Count()));
    }

    [Test]
    public void CountAnagramsWithDictionary()
    {
        var expectedNumberOfAnagrams = 20683;
        var expectedNumberOfWords = 48162;
        var words = File.ReadAllLines("words_list.txt");

        var groupedWords = new Dictionary<string, List<string>>();
        foreach (var word in words)
        {
            var orderedWordCharacters = new string(word.ToCharArray().OrderBy(c => c).ToArray());
            if (groupedWords.ContainsKey(orderedWordCharacters))
                groupedWords[orderedWordCharacters].Add(word);
            else
                groupedWords.Add(orderedWordCharacters, new List<string> { word });
        }

        var foundAnagrams = groupedWords.Count(c => c.Value.Count > 1);
        var totalWords = groupedWords.Where(c => c.Value.Count > 1).Sum(c => c.Value.Count);

        Assert.AreEqual(expectedNumberOfAnagrams, foundAnagrams);
        Assert.AreEqual(expectedNumberOfWords, totalWords);
    }
}