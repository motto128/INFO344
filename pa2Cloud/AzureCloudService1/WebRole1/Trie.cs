using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole1
{
    public class Trie
    {
        public Node root { get; private set; }

        public Trie()
        {
            root = new Node();
        }

        /// <summary>
        /// Adds given title to the Trie structure
        /// </summary>
        /// <param name="line"></param>
        public void AddTitle(string line)
        {
            Node current = root;
            // Loops through title to add characters into appropriate dictionaries
            for (int i = 0; i < line.Length; i++)
            {
                if (current.dictionary == null)
                {
                    current.dictionary = new Dictionary<char, Node>();
                }

                if (!current.dictionary.ContainsKey(line[i]))
                {
                    current.dictionary.Add(line[i], new Node());
                }
                current = current.dictionary[line[i]];
            }
            // Sets the last character of the title to a leaf node
            current.end = true;
        }

        /// <summary>
        /// Searches trie structure to see if any words match
        /// </summary>
        /// <param name="word"></param>
        /// <returns>a list of up to 10 search results</returns>
        public List<string> SearchForPrefix(string word)
        {
            Node current = root;
            string result = "";
            List<string> searchResults = new List<string>();
            for (int i = 0; i < word.Length; i++)
            {
                if (current.dictionary.ContainsKey(word[i]))
                {
                    current = current.dictionary[word[i]];
                    result += word[i];
                }
                else
                {
                    break;
                }
            }

            if (word == result && word != "")
            {
                searchResults = this.GetWords(current, word, searchResults);
            }
            return searchResults;
        }

        /// <summary>
        /// Recursive function traverse the trie node
        /// </summary>
        /// <param name="temp"></param>
        /// <param name="word"></param>
        /// <param name="searchResults"></param>
        /// <returns>a list of matching words</returns>
        public List<string> GetWords(Node temp, string word, List<string> searchResults)
        {
            if (searchResults.Count < 10)
            { 
                if (temp.end == true)
                {
                    searchResults.Add(word);
                }

                if (temp.dictionary != null)
                {
                    foreach (var dictionary in temp.dictionary)
                    {
                        if (searchResults.Count >= 10)
                        {
                            break;
                        }
                        word += dictionary.Key;
                        Node newTemp = dictionary.Value;
                        this.GetWords(newTemp, word, searchResults);
                        word = word.Substring(0, word.Length - 1);
                    }
                }
            }
            return searchResults;
        }

    }
}