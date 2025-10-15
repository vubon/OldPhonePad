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
├── OldPhonePad.sln              # Solution file
├── OldPhonePad/                 # Main library project
│   ├── OldPhonePad.csproj       # Project file
│   └── OldPhonePad.cs           # Main implementation
└── README.md                    # This file
```

### Prerequisites
- .NET 9.0 SDK
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