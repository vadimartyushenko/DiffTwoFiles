// See https://aka.ms/new-console-template for more information


var filePath1 = args[0];
var filePath2 = args[1];
if (!(File.Exists(filePath1) && File.Exists(filePath2))) {
    Console.WriteLine($"Not found input file!");
}

var text1 = File.ReadAllLines(filePath1);
var text2 = File.ReadAllLines(filePath2);

var pt_numbers = text1.Select(str => str.Trim()).ToArray();

var stats_numbers = text2.Select(str => str.Trim()).ToArray();

var uniq_numbers = pt_numbers.Except(stats_numbers).ToArray();

foreach (var num in uniq_numbers)
{
    Console.WriteLine(num);
}
Console.ReadLine();