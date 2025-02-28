namespace BlazorWebApp.Components.Pages.ExamplePages
{
    public partial class DataEntry
    {




















        // The following block was included in order to provide one small demonstration of how
        // a Dictionary can be used.  In this case it was used to categorize the words in a string
        // by their length in letters.
        Random rng = new Random();
        Dictionary<int, List<string>> categorizedWords = new Dictionary<int, List<string>>();
        string input = "The quick brown fox jumped over the lazy dog";

        private void ProcessString()
        {
            string[] words = input.Split(' ');  // In order to "step through" the code like we did in 
                                                // class, place a breakpoint in the far left column of 
                                                // the editor using a left-click.
            foreach (string word in words)
            {
                if (!categorizedWords.ContainsKey(word.Length))
                {
                    categorizedWords.Add(word.Length, new List<string>());
                }

                categorizedWords[word.Length].Add(word);
            }
        }
    }
}
