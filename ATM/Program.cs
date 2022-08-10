var ATMBanknotes = InitBanknotes();

while (true)
{
    var userMessage = Console.ReadLine();
    if (userMessage == "exit" || string.IsNullOrEmpty(userMessage))
    {
        break;
    }

    var input = int.Parse(userMessage);
    var possibleCombinations = CalculateCombinations(new List<int>(), ATMBanknotes, 0, 0, input);
    var bestCombination = possibleCombinations
        .OrderBy(p => p.Select(p => p.Value).Sum())
        .FirstOrDefault();

    if(bestCombination != null)
    {
        foreach(var combination in bestCombination)
        {
            ATMBanknotes[combination.Key] = ATMBanknotes[combination.Key] - combination.Value;

            Console.WriteLine($"{combination.Key} {combination.Value}");
        }
    }
    else
    {
        Console.WriteLine("YOU GET NOTHING");
    }
}

IEnumerable<IDictionary<int, int>> CalculateCombinations(
    List<int> tempBancnotes,
    IDictionary<int, int> ATMBanknotes,
    int highest,
    int sum,
    int input)
{
    var result = new List<IDictionary<int, int>>();

    if (sum == input)
    {
        var combination = CreateCombination(tempBancnotes, ATMBanknotes);

        if(combination != null && combination.Count != 0)
        {
            result.Add(combination);
        }
    }

    if (sum > input)
    {
        return result;
    }

    foreach (var value in ATMBanknotes)
    {
        if (value.Key >= highest)
        {
            var copy = new List<int>(tempBancnotes) { value.Key };
            var newSum = sum + value.Key;
            var combinations = CalculateCombinations(copy, ATMBanknotes, value.Key, newSum, input);
            
            result.AddRange(combinations);
        }
    }

    return result;
}

IDictionary<int, int>? CreateCombination(IReadOnlyCollection<int> tempBancnotes, IDictionary<int, int> ATMBanknotes)
{
    var combination = new Dictionary<int, int>();

    foreach(var bancnote in ATMBanknotes)
    {
        var numberOfBanknotes = tempBancnotes.Count(value => value == bancnote.Key);

        if(numberOfBanknotes > bancnote.Value)
        {
            return null;
        }

        if(numberOfBanknotes != 0)
        {
            combination[bancnote.Key] = numberOfBanknotes;
        }
    }

    return combination;
}

IDictionary<int, int> InitBanknotes()
{
    var lines = File.ReadAllLines(@"C:\OA\banknotes.txt");

    return lines.Select(p => p.Split().Select(x => int.Parse(x)))
        .OrderByDescending(p => p.First())
        .ToDictionary(k => k.First(), v => v.Last());
}