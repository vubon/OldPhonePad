# Old Phone Pad
A C# implementation of an old phone keypad text input system that converts key press sequences into text messages

## Overview Problem Statement
Here is an Old phone keypad with alphabetical letters, a backspace key, and a send button.

Each button has a number to identify it and pressing a button multiple times will cycle through the letters on it 
allowing each button to represent more than one letter. <br/>

For example, pressing 2 once will return `A` but pressing twice in succession will retunr `B`

you must pause for a second in order to type two characters from the same button after each other: "222 2 22" -> "CAB"

Assume that a send "#" will always be included at the end of every input.

Examples:
```csharp
OldPhonePad("33#") => output: E
OldPhonePad("227*#) => output: B
OldPhonePad("4433555 555666#") => output: HELLO
OldPhonePad("8 88777444666*664#") => output: ????? 
```
## Solution Overview
Steps: 
- Each number key (2-9) contains multiple letters
- Pressing a key multiple times cycles through its letters
- A pause (space) is required to type consecutive letters from the same key
- The `*` key represents backspace
- The `#` key sends the message

## Key Mapping

| Key | Letters |
|-----|---------|
| 2   | ABC     |
| 3   | DEF     |
| 4   | GHI     |
| 5   | JKL     |
| 6   | MNO     |
| 7   | PQRS    |
| 8   | TUV     |
| 9   | WXYZ    |

### Rules

1. **Single Press**: Pressing a key once gives the first letter
    - `2#` → `A`
    - `3#` → `D`

2. **Multiple Presses**: Pressing a key multiple times cycles through its letters
    - `22#` → `B` (second letter)
    - `222#` → `C` (third letter)
    - `2222#` → `A` (cycles back to first letter)

3. **Pause (Space)**: A space allows typing consecutive letters from the same key
    - `2 2#` → `AA`
    - `22 22#` → `BB`

4. **Backspace**: The `*` key removes the last typed character
    - `22*#` → `` (removes the second `B` and return empty string)
    - `22*22#` → `B` (removes `B`, then adds `B` again)

5. **Send**: The `#` key always ends the input and sends the message


## Core Logic Flow
State Variables:
- currentKey: Which number key (2-9) is currently beining pressed or null
- pressCount: How many times the current key has been pressed 
- result: The final text message

### Character Processing Rules:
1. `#`(Send Key)
```text
- Proces any remaining key presses 
- Append to result 
- Break the loop
```
2. `*` (Backspace)
```text
- Process current key first (if any)
- Remove last character from result
- Reset state (currentKey = `\0` and pressCount = 0)
- Continue the loop 
```

3. ` `(Space/Pause)
```text
- Process current key (if any)
- Reset state (currentKey = `\0` and pressCount = 0)
- Continue the loop
```
4. Number Keys (2 - 9)
```text
- Skip if not a valid number key
- If same key: Increment pressCount
- If differnt key:
    - Process previous key (if any)
    - Start new key
    - Set pressCount = 1
- Continue the loop 
```

### Design Decision: Using `'\0'` as Sentinel Value

The algorithm uses `'\0'` (null character) as a sentinel value to indicate "no key currently pressed". 
This design choice was made for several important reasons:

#### Why `'\0'`?

1. **Type Safety**: The method returns `char`, not `string`, so we need a character value to represent "no key"
2. **Memory Efficiency**: `'\0'` is a single character (2 bytes) vs alternatives that would require more memory
3. **Standard Convention**: `'\0'` is the standard "null character" in programming, widely recognized
4. **Clear Semantics**: `'\0'` clearly means "no value" or "invalid character"
5. **Performance**: No string comparisons needed - just simple character equality checks

#### Alternative Approaches Considered:

| Approach                    | Why Not Used |
|-----------------------------|--------------|
| **Empty String `""`**       | ❌ Would require changing return type from `char` to `string`, breaking the API |
| **Special Character `'?'`** | ❌ Could be confusing - is `'?'` a valid result or error indicator? |
| **Nullable `char?`**        | ❌ Adds complexity with null checking throughout the code |
| **Exception Throwing**      | ❌ Overkill for this simple case - not an exceptional condition |
| **Boolean Flag**            | ❌ Would require additional state variables, making code more complex |

#### Code Example:
```csharp
// Using '\0' as sentinel value
var currentKey = '\0';  // Indicates "no key currently pressed"

// Easy to check
if (currentKey != '\0')  // Simple character comparison
{
    result.Append(GetLetterFromKey(currentKey, pressCount));
}

// vs alternatives that would be more complex:
// if (currentKey != null)           // Nullable approach
// if (currentKey != '?')            // Special character approach  
// if (!string.IsNullOrEmpty(currentKey))  // String approach
```

This design choice makes the code **simpler, faster, and more maintainable** while following established programming conventions.

## Algorithm Details

The algorithm processes the input character by character:

1. **State Tracking**: Maintains current key and press count
2. **Character Processing**:
    - Numbers (2-9): Update state or process previous key
    - Space: Process current key and reset state
    - `*`: Remove last character and reset state
    - `#`: Process final key and return result

3. **Letter Selection**: Uses modulo arithmetic to cycle through letters
4. **Backspace Handling**: Maintains a StringBuilder for efficient character removal


## Project Structure

```
OldPhonePad/
├── OldPhonePad.sln                # Solution file
├── OldPhonePad/                   # Main library project
│   ├── OldPhonePad.csproj         # Project file
│   └── OldPhonePad.cs             # Main implementation
├── OldPhonePad.Console/           # Console demo project
│   ├── OldPhonePad.Console.csproj # Console project file
│   └── Program.cs                 # Console application
├── OldPhonePad.Tests/             # Test project
│   ├── OldPhonePad.Tests.csproj   # Test project file
│   └── OldPhonePadTests.cs        # Test cases
└── README.md                      # This file
```

### Prerequisites
- .NET 9.0 SDK
  - Download .NET 9.0 SDK from [Microsoft's website](https://dotnet.microsoft.com/download)
  - Run the installer and follow the instructions
  - Verify installation: `dotnet --version`
- Windows, macOS, or Linux
- Any text editor

## Command List
```bash
# Clone or download the project
cd OldPhonePad

# Restore packages
dotnet restore

# Build the solution
dotnet build

# Run the console 
dotnet run --project OldPhonePad.Console
```

### Running Tests

```bash
# Run all tests
dotnet test

# Run tests with detailed output
dotnet test --verbosity normal

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Generate HTML coverage report (requires ReportGenerator tool)
# Install ReportGenerator: dotnet tool install -g dotnet-reportgenerator-globaltool
~/.dotnet/tools/reportgenerator -reports:"./OldPhonePad.Tests/TestResults/**/coverage.cobertura.xml" -targetdir:"./OldPhonePad.Tests/CoverageReport" -reporttypes:Html
```

**Note**: The HTML coverage report requires the `dotnet-reportgenerator-globaltool` to be installed. If you haven't installed it yet, run:
```bash
dotnet tool install -g dotnet-reportgenerator-globaltool
```

# Docker Setup for OldPhonePad
## Prerequisites
- Docker installed on your system

```bash
# Build
docker build -t oldphonepad:latest .

# Run
docker run --rm oldphonepad:latest

# With input
echo "8 88777444666*664#" | docker run -i --rm oldphonepad:latest
```