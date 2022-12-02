using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ToDoList.App.Utilities;

public static class EmojiCollection
{
    public static HashSet<string> All => _all.Value;
    private static readonly Lazy<HashSet<string>> _all = new Lazy<HashSet<string>>(GetAllEmojis);

    private static HashSet<string> GetAllEmojis()
    {
        using var source =
            typeof(EmojiCollection).Assembly.GetManifestResourceStream("ToDoList.App.Resources.emoji-test.txt")!;

        var reader = new StreamReader(source);

        var entryParsingRegex = new Regex("^(?<emoji>([0-9a-fA-F]+ )+)\\s+;.*# (?<description>.+)$", RegexOptions.Compiled);

        string? line;
        var emojis = new HashSet<string>();

        while ((line = reader.ReadLine()) != null)
        {
            if (line.StartsWith('#'))
                continue;

            if (!entryParsingRegex.IsMatch(line))
                continue;

            var match = entryParsingRegex.Match(line);

            var codePointSequence = string.Concat(match.Groups["emoji"].Value
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(codePoint => char.ConvertFromUtf32(int.Parse(codePoint, NumberStyles.HexNumber))));

            emojis.Add(codePointSequence);
        }

        return emojis;
    }
}