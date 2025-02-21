
using DiffTwoFiles.Processing;

if (args.Length != 2) {
    Console.WriteLine("Only two paths required!");
    return;
}

var fi1 = new FileInfo(args[0]);
var fi2 = new FileInfo(args[1]);
if (!(fi1.Exists && fi2.Exists)) {
    Console.WriteLine($"Not found input file!");
}

var processor = new WarningProcessor();

var text1 = File.ReadAllLines(fi1.FullName);
var text2 = File.ReadAllLines(fi2.FullName);

var pt_numbers = text1.OrderDescending().Select(str => processor.Process(str).Trim()).ToArray();

var stats_numbers = text2.OrderDescending().Select(str => processor.Process(str).Trim()).ToArray();

var f1_uniq_numbers = pt_numbers.Except(stats_numbers).ToArray();

if (f1_uniq_numbers.Length > 0)
{
    Console.WriteLine($"Uniq strings for file \"{fi1.Name}\"");
    var idx = 0;
    foreach (var num in f1_uniq_numbers)
    {
        Console.Write($"{idx++}:\t");
        Console.WriteLine(num);
    }
} else {
    var f2_uniq = stats_numbers.Except(pt_numbers).ToArray();
    if (f2_uniq.Length > 0) {
        Console.WriteLine($"Uniq strings for file \"{fi2.Name}\"");
        foreach (var num in f2_uniq) {
            Console.WriteLine(num);
        }
    }
}

Console.ReadLine();