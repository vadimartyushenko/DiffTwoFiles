﻿using System.Globalization;
using System.Text.RegularExpressions;

namespace DiffStatValues;

partial class Program
{
    [GeneratedRegex(@"^\[""(?<stat>[A-Za-z0-9 $.+&]+)""\] = (?<value>[0-9]*(?:\.[0-9]*))?$", RegexOptions.IgnoreCase)]
    private static partial Regex ColorStatsRx();

    [GeneratedRegex(@"^^""(?<stat>[A-Za-z0-9 $.+&]+)"":[ ]+{ isPercentageComparable: [10], isValueComparable: [10], refValue: (?<value>[0-9]*(?:\.[0-9]*))?[ ]+},$", RegexOptions.IgnoreCase)]
    private static partial Regex IndexStatsRx();
}

internal partial class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 2)
        {
            Console.WriteLine("Only two paths required!");
            return;
        }
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        var colorsFi = new FileInfo(args[0]);
        var indexFi = new FileInfo(args[1]);
        if (!(colorsFi.Exists && indexFi.Exists))
        {
            Console.WriteLine($"Not found input file!");
        }

        var colorRx = ColorStatsRx();
        var indexRx = IndexStatsRx();

        const double threshold = 1e-3;

        var colorsTxt = File.ReadAllLines(colorsFi.FullName);
        var indexTxt = File.ReadAllLines(indexFi.FullName);

        var colorsStats = new Dictionary<string, float>(50);
        var indexStats = new Dictionary<string, float>(50);


        var notValueColorsStats = new List<string>();
        var notValueIndexStats = new List<string>();


        foreach (var line in indexTxt)
        {
            var txt = line.Split("//")[0].Trim();
            if (txt.Contains('—'))
            {
                notValueIndexStats.Add(txt);
                continue;
            }
            var matches = indexRx.Matches(txt);
            if (matches.Count > 0)
            {
                var statName = matches[0].Groups.Values.First(x => x.Name == "stat").Value;
                var value = float.Parse(matches[0].Groups.Values.First(x => x.Name == "value").Value, NumberStyles.Any);
                indexStats.TryAdd(statName, value);
            }
            else
            {
                notValueIndexStats.Add(txt);
                //Console.WriteLine($"Not parsed: {txt}");
            }

        }


        foreach (var line in colorsTxt)
        {
            var txt = line.Split(',')[0];
            if (txt.Contains("All"))
                continue;
            if (txt.Contains("NaN"))
            {
                notValueColorsStats.Add(txt);
                continue;
            }

            var matches = colorRx.Matches(txt);
            if (matches.Count > 0)
            {
                var statName = matches[0].Groups.Values.First(x => x.Name == "stat").Value;
                var value = float.Parse(matches[0].Groups.Values.First(x => x.Name == "value").Value, NumberStyles.Any);
                colorsStats.TryAdd(statName, value);
            }
            else
            {
                Console.WriteLine($"Not parsed: {txt}");
            }

        }

        if (colorsStats.Count != indexStats.Count)
            throw new Exception("Invalid number of stats!");

        // compare number stats
        var num = 0;
        foreach (var (stat, value) in colorsStats)
        {
            if (!indexStats.TryGetValue(stat, out var indexStatValue))
            {
                throw new Exception("Invalid stat!");
            }
            Console.WriteLine($"{num++}. {stat}: {(Math.Abs(value - indexStatValue) < threshold ? "equal" : "not equal")}");
        }

        Console.WriteLine("Not value stats in \"HudStats.Colors.cs\"");
        num = 0;
        foreach (var stat in notValueColorsStats)
        {
            Console.WriteLine($"{num++}. {stat}");
        }

        Console.WriteLine("Not value stats in \"Index.cshtml\"");
        num = 0;
        foreach (var stat in notValueIndexStats)
        {
            Console.WriteLine($"{num++}. {stat}");
        }


        Console.ReadKey();
    }
}