var banknotes = InitBanknotes();

while (true)
{
    var userMessage = Console.ReadLine();
    if(userMessage == "exit" || string.IsNullOrEmpty(userMessage))
    {
        break;
    }

    var input = int.Parse(userMessage);
    var tempBanknotes = new Dictionary<int, int>();
    var resultBanknotes = new Dictionary<int, int>();

    foreach (var banknote in banknotes)
    {
        if (input < banknote.Key)
        {
            tempBanknotes[banknote.Key] = banknote.Value;
            continue;
        }

        var sum = banknote.Key * banknote.Value;

        if (sum == input)
        {
            resultBanknotes[banknote.Key] = banknote.Value;
            input = 0;
            break;
        }

        if (sum > input)
        {
            var rest = input % banknote.Key;
            var needBanknotes = (input - rest) / banknote.Key;
            tempBanknotes[banknote.Key] = banknote.Value - needBanknotes;
            resultBanknotes[banknote.Key] = needBanknotes;
            input = rest;
        }
        else
        {
            var rest = input - sum;
            resultBanknotes[banknote.Key] = banknote.Value;
            input = rest;
        }
    }

    if (input == 0)
    {
        banknotes = tempBanknotes;

        Console.WriteLine("Your money:");

        foreach (var banknote in resultBanknotes)
        {
            Console.WriteLine($"{banknote.Key} {banknote.Value}");
        }
    }
    else
    {
        Console.WriteLine("Insufficient funds to issue");
    }
}

IDictionary<int, int> InitBanknotes()
{
    var lines = File.ReadAllLines(@"C:\OA\banknotes.txt");

    return lines.Select(p => p.Split().Select(x => int.Parse(x)))
        .OrderByDescending(p => p.First())
        .ToDictionary(k => k.First(), v => v.Last());
}
