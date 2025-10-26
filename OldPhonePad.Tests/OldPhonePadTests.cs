namespace OldPhonePad.Tests
{
    /// Test cases for the OldPhonePad class
    public class OldPhonePadTests
    {
        [Fact]
        public void OldPhonePad_WithValidInput_ReturnsCorrectOutput()
        {
            // Arrange & Act & Assert
            Assert.Equal("E", OldPhonePad.Process("33#"));
        }

        [Fact]
        public void OldPhonePad_WithBackspace_ReturnsCorrectOutput()
        {
            // Arrange & Act & Assert
            Assert.Equal("B", OldPhonePad.Process("227*#"));
        }

        [Fact]
        public void OldPhonePad_WithPause_ReturnsCorrectOutput()
        {
            // Arrange & Act & Assert
            Assert.Equal("HELLO", OldPhonePad.Process("4433555 555666#"));
        }

        [Fact]
        public void OldPhonePad_WithComplexInput_ReturnsCorrectOutput()
        {
            // Arrange & Act & Assert
            Assert.Equal("TURING", OldPhonePad.Process("8 88777444666*664#"));
        }

        [Theory]
        [InlineData("2#", "A")]
        [InlineData("22#", "B")]
        [InlineData("222#", "C")]
        [InlineData("2222#", "A")] // Cycles back to first letter
        [InlineData("3#", "D")]
        [InlineData("33#", "E")]
        [InlineData("333#", "F")]
        [InlineData("4#", "G")]
        [InlineData("44#", "H")]
        [InlineData("444#", "I")]
        [InlineData("5#", "J")]
        [InlineData("55#", "K")]
        [InlineData("555#", "L")]
        [InlineData("6#", "M")]
        [InlineData("66#", "N")]
        [InlineData("666#", "O")]
        [InlineData("7#", "P")]
        [InlineData("77#", "Q")]
        [InlineData("777#", "R")]
        [InlineData("7777#", "S")]
        [InlineData("8#", "T")]
        [InlineData("88#", "U")]
        [InlineData("888#", "V")]
        [InlineData("9#", "W")]
        [InlineData("99#", "X")]
        [InlineData("999#", "Y")]
        [InlineData("9999#", "Z")]
        public void OldPhonePad_WithSingleKeyPresses_ReturnsCorrectLetter(string input, string expected)
        {
            // Arrange & Act
            var result = OldPhonePad.Process(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("2 2#", "AA")]
        [InlineData("22 22#", "BB")]
        [InlineData("3 3#", "DD")]
        [InlineData("4 4#", "GG")]
        [InlineData("5 5#", "JJ")]
        [InlineData("6 6#", "MM")]
        [InlineData("7 7#", "PP")]
        [InlineData("8 8#", "TT")]
        [InlineData("9 9#", "WW")]
        public void OldPhonePad_WithPauseBetweenSameKeys_ReturnsCorrectLetters(string input, string expected)
        {
            // Arrange & Act
            var result = OldPhonePad.Process(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("2*#", "")]
        [InlineData("22*#", "")]
        [InlineData("23*#", "A")]
        [InlineData("2*2#", "A")]
        [InlineData("22*22#", "B")]
        [InlineData("23*23#", "AAD")] // 2→A, 3→D, *→removes D (result: A), 2→A, 3→D, result: AAD
        public void OldPhonePad_WithBackspaceTheory_ReturnsCorrectOutput(string input, string expected)
        {
            // Arrange & Act
            var result = OldPhonePad.Process(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("2 3#", "AD")]
        [InlineData("22 33#", "BE")]
        [InlineData("2 3 4#", "ADG")]
        [InlineData("22 33 44#", "BEH")]
        [InlineData("2 3 4 5#", "ADGJ")]
        [InlineData("22 33 44 55#", "BEHK")]
        public void OldPhonePad_WithDifferentKeys_ReturnsCorrectLetters(string input, string expected)
        {
            // Arrange & Act
            var result = OldPhonePad.Process(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("2*2*#", "")]
        [InlineData("22*22*#", "")]
        [InlineData("2*2*2#", "A")]
        [InlineData("22*22*22#", "B")]
        public void OldPhonePad_WithMultipleBackspaces_ReturnsCorrectOutput(string input, string expected)
        {
            // Arrange & Act
            var result = OldPhonePad.Process(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("2222#", "A")] // 4 presses on key 2 = A (cycles back)
        [InlineData("3333#", "D")] // 4 presses on key 3 = D (cycles back)
        [InlineData("4444#", "G")] // 4 presses on key 4 = G (cycles back)
        [InlineData("5555#", "J")] // 4 presses on key 5 = J (cycles back)
        [InlineData("6666#", "M")] // 4 presses on key 6 = M (cycles back)
        [InlineData("77777#", "P")] // 5 presses on key 7 = P (cycles back)
        [InlineData("8888#", "T")] // 4 presses on key 8 = T (cycles back)
        [InlineData("99999#", "W")] // 5 presses on key 9 = W (cycles back)
        public void OldPhonePad_WithKeyCycling_ReturnsCorrectLetter(string input, string expected)
        {
            // Arrange & Act
            var result = OldPhonePad.Process(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void OldPhonePad_WithEmptyString_ThrowsArgumentException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => OldPhonePad.Process(""));
        }

        [Fact]
        public void OldPhonePad_WithNullInput_ThrowsArgumentException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => OldPhonePad.Process(null!));
        }

        [Fact]
        public void OldPhonePad_WithoutEndingHash_ThrowsArgumentException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => OldPhonePad.Process("22"));
        }

        [Fact]
        public void OldPhonePad_WithOnlyHash_ReturnsEmptyString()
        {
            // Arrange & Act
            var result = OldPhonePad.Process("#");

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void OldPhonePad_WithOnlyBackspace_ReturnsEmptyString()
        {
            // Arrange & Act
            var result = OldPhonePad.Process("*#");

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void OldPhonePad_WithOnlySpaces_ReturnsEmptyString()
        {
            // Arrange & Act
            var result = OldPhonePad.Process("   #");

            // Assert
            Assert.Equal("", result);
        }

        [Theory]
        [InlineData("2 2 2#", "AAA")]
        [InlineData("22 22 22#", "BBB")]
        [InlineData("2 22 222#", "ABC")]
        [InlineData("22 2 22#", "BAB")]
        public void OldPhonePad_WithComplexPausePatterns_ReturnsCorrectOutput(string input, string expected)
        {
            // Arrange & Act
            var result = OldPhonePad.Process(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetKeyMapping_ReturnsCorrectMapping()
        {
            // Arrange & Act
            var mapping = OldPhonePad.GetKeyMapping();

            // Assert
            Assert.Equal("ABC", mapping['2']);
            Assert.Equal("DEF", mapping['3']);
            Assert.Equal("GHI", mapping['4']);
            Assert.Equal("JKL", mapping['5']);
            Assert.Equal("MNO", mapping['6']);
            Assert.Equal("PQRS", mapping['7']);
            Assert.Equal("TUV", mapping['8']);
            Assert.Equal("WXYZ", mapping['9']);
        }

        [Fact]
        public void OldPhonePad_WithRealWorldExample_ReturnsCorrectOutput()
        {
            // Test case: "HELLO WORLD" would be "4433555 555666 96667775553#"
            // But let's test a simpler version first
            Assert.Equal("HELLO", OldPhonePad.Process("4433555 555666#"));
        }

        [Fact]
        public void OldPhonePad_WithBackspaceAtBeginning_HandlesCorrectly()
        {
            // Arrange & Act
            var result = OldPhonePad.Process("*2#");

            // Assert
            Assert.Equal("A", result);
        }

        [Fact]
        public void OldPhonePad_WithBackspaceInMiddle_HandlesCorrectly()
        {
            // Arrange & Act
             var result = OldPhonePad.Process("22*33#");

            // Assert
            Assert.Equal("E", result); // 22 gives B, * removes it, 33 gives E
        }
        
        [Fact]
        public void OldPhonePad_WithInvalidKeyInSequence_IgnoresInvalidKey()
        {
            // Arrange & Act - Test the !KeyMapping.ContainsKey(c) branch in switch expression
            // This should trigger the yellow line: var c when !KeyMapping.ContainsKey(c) => ('\0', 0, true)
            var result = OldPhonePad.Process("1#");

            // Assert - Invalid key '1' should be ignored, resulting in empty string
            Assert.Equal("", result);
        }
        
        [Fact]
        public void GetLetterFromKey_WithInvalidKey_ReturnsNullCharacter()
        {
            // Arrange & Act - Directly test the return '\0' path in GetLetterFromKey
            var result = OldPhonePad.GetLetterFromKey('1', 1);

            // Assert - Invalid key '1' should return '\0'
            Assert.Equal('\0', result);
        }
    }
}
