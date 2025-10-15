using System.Text;

namespace OldPhonePad
{
    public static class OldPhonePad
    {
        private static readonly Dictionary<char, string> KeyMapping = new Dictionary<char, string>()
        {
            { '2', "ABC" },
            { '3', "DEF" },
            { '4', "GHI" },
            { '5', "JKL" },
            { '6', "MNO" },
            { '7', "PQRS" },
            { '8', "TUV" },
            { '9', "WXYZ" },
        };

        private static char GetLetterFromKey(char key, int pressCount)
        {
            if (!KeyMapping.ContainsKey(key))
                return '\0';
            var letters = KeyMapping[key];
            var index = (pressCount - 1) % letters.Length;
            return letters[index];
        }

        public static string Process(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("Input cannot be empty or null", nameof(input));
            if (!input.EndsWith("#"))
                throw new ArgumentException("Input must end with '#'", nameof(input));
            
            var result = new StringBuilder();
            var currentKey = '\0';
            var pressCount = 0;

            for (var i = 0; i < input.Length; i++)
            {
                var currentChar = input[i];
                if (currentChar == '#')
                {
                    if (currentKey != '\0')
                    {
                        result.Append(GetLetterFromKey(currentKey, pressCount));
                    }
                    break;
                }

                if (currentChar == '*')
                {
                    if (currentKey != '\0')
                    {
                        result.Append(GetLetterFromKey(currentKey, pressCount));
                    }

                    if (result.Length > 0)
                    {
                        result.Length--; // Remove last character
                    }
                    // Reset current key state 
                    currentKey = '\0';
                    pressCount = 0;
                    continue;
                }
                
                // Handle space (pause) early return 
                if (currentChar == ' ')
                {
                    if (currentKey != '\0')
                    {
                        result.Append(GetLetterFromKey(currentKey, pressCount));
                    }
                    // Reset for next key 
                    currentKey = '\0';
                    pressCount = 0;
                    continue;
                }
                // Skip non-number keys 
                if (!KeyMapping.ContainsKey(currentChar)) 
                    continue;
                
                // Handle number keys - main logic without nesting 
                if (currentKey == currentChar)
                {
                    // Same key pressed again
                    pressCount++;
                }
                else
                {
                    if (currentKey != '\0')
                    {
                        result.Append(GetLetterFromKey(currentKey, pressCount));
                    }
                    // start new key 
                    currentKey = currentChar; // Track which key
                    pressCount = 1; // Press Once 
                }
            }
            
            return result.ToString();
        }
    }
}