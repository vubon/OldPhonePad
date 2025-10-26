using System.Text;

namespace OldPhonePad
{
    /// <summary>
    /// Represents an old phone keypad with text input functionality.
    /// Each number key (2-9) contains multiple letters, and pressing a key multiple times
    /// cycles through the letters. A pause (space) is required to type consecutive letters
    /// from the same key. The '#' key sends the message.
    /// </summary>
    public static class OldPhonePad
    {
        
        /// <summary>
        /// Maps each number key(2-9) to its corresponding letters
        /// </summary>
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

        /// <summary>
        /// Gets the letter corresponding to a key press sequence.
        /// </summary>
        /// <param name="key">The number key pressed</param>
        /// <param name="pressCount">Number of times the key was pressed</param>
        /// <returns>The corresponding letter or `\0`</returns> 
        public static char GetLetterFromKey(char key, int pressCount)
        {
            // Use TryGetValue to avoid double lookup
            if (!KeyMapping.TryGetValue(key, out var letters))
                return '\0';
            
            // Get key letters (e.g. 2 = ABC)
            // var letters = KeyMapping[key];
            
            // Press count is 1-based, but array is 0-based
            var index = (pressCount - 1) % letters.Length;
            
            return letters[index];
        }
        
        /// <summary>
        /// Gets the key mapping for testing purposes.
        /// </summary>
        /// <returns>A read-only dictionary of key mappings</returns>
        public static IReadOnlyDictionary<char, string> GetKeyMapping()
        {
            return KeyMapping;
        }
        
        /// <summary>
        /// Converts a sequence of key presses into the corresponding text message.
        /// 
        /// Rules:
        /// - Each number key (2-9) represents multiple letters
        /// - Pressing a key multiple times cycles through its letters
        /// - A space character represents a pause, allowing consecutive letters from the same key
        /// - The '*' key represents backspace
        /// - The '#' key sends the message (always present at the end)
        /// 
        /// Examples:
        /// - "33#" -> "E" (3 pressed twice and since there is no space it means the letter should be 'E')
        /// - "227*#" -> "B" (2 pressed twice = B, then next 7 once = P. Next '*' So it will remove the letter 'P' so final output 'B')
        /// - "4433555 555666#" -> "HELLO"
        /// </summary>
        /// <param name="input">The sequence of key presses ending with '#'</param>
        /// <returns>The decoded text message</returns>
        /// <exception cref="ArgumentException">Thrown when input is null, empty, or doesn't end with '#'</exception>
        public static string Process(string input)
        {
            // Check the income data, if null or empty throw error 
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("Input cannot be empty or null", nameof(input));
            
            // input data should have `#` end of the string. If not raise the error 
            if (!input.EndsWith("#"))
                throw new ArgumentException("Input must end with '#'", nameof(input));
            
            // State variable. This variables will reuse of each step except the result one
            var result = new StringBuilder();
            // Using '\0' (null character) as a sentinel value to indicate "no key currently pressed"
            // This allows us to distinguish between "no key" and any valid key character
            var currentKey = '\0'; // Indicates "no key currently pressed"
            var pressCount = 0;
            
            foreach (var currentChar in input)
            {

                // Clean switch expression - much more readable than multiple if statements
                var (newCurrentKey, newPressCount, shouldContinue) = currentChar switch
                {
                    '#' => ProcessSendKey(result, currentKey, pressCount),
                    '*' => ProcessBackspace(result, currentKey, pressCount),
                    ' ' => ProcessPause(result, currentKey, pressCount),
                    var c when !KeyMapping.ContainsKey(c) => ('\0', 0, true), // Skip invalid keys
                    var c => ProcessNumberKey(result, c, currentKey, pressCount)
                };

                currentKey = newCurrentKey;
                pressCount = newPressCount;

                if (!shouldContinue) break; // Exit loop on send key
            }
            
            return result.ToString();
        }
        
        /// <summary>
        /// Processes the send key (#) - modifies result StringBuilder and returns (newKey, newCount, shouldContinue)
        /// </summary>
        private static (char newKey, int newCount, bool shouldContinue) ProcessSendKey(StringBuilder result, char currentKey, int pressCount)
        {
            // Process any current key before sending
            if (currentKey != '\0')
            {
                result.Append(GetLetterFromKey(currentKey, pressCount));
            }
            // Reset state and exit loop
            return ('\0', 0, false);
        }

        /// <summary>
        /// Processes the backspace key (*) - returns (newKey, newCount, shouldContinue)
        /// </summary>
        private static (char newKey, int newCount, bool shouldContinue) ProcessBackspace(StringBuilder result, char currentKey, int pressCount)
        {
            if (currentKey != '\0')
            {
                result.Append(GetLetterFromKey(currentKey, pressCount));
            }
            
            if (result.Length > 0)
            {
                result.Length--; // Remove last character
            }
            
            return ('\0', 0, true); // Continue loop
        }

        /// <summary>
        /// Processes the pause key (space) - returns (newKey, newCount, shouldContinue)
        /// </summary>
        private static (char newKey, int newCount, bool shouldContinue) ProcessPause(StringBuilder result, char currentKey, int pressCount)
        {
            if (currentKey != '\0')
            {
                result.Append(GetLetterFromKey(currentKey, pressCount));
            }
            
            return ('\0', 0, true); // Continue loop
        }

        /// <summary>
        /// Processes number keys - returns (newKey, newCount, shouldContinue)
        /// </summary>
        private static (char newKey, int newCount, bool shouldContinue) ProcessNumberKey(StringBuilder result, char currentChar, char currentKey, int pressCount)
        {
            if (currentKey == currentChar)
            {
                // Same key pressed again
                return (currentKey, pressCount + 1, true);
            }
            // Different key or first key - process current key if it exists
            if (currentKey != '\0')
            {
                result.Append(GetLetterFromKey(currentKey, pressCount));
            }
            return (currentChar, 1, true);
        }
    }
}