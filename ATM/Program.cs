var banknotes = InitBanknotes();

while (true)
{
    var userMessage = Console.ReadLine();
    if (userMessage == "exit" || string.IsNullOrEmpty(userMessage))
    {
        break;
    }

    var input = int.Parse(userMessage);
    var result = GetMoney(input, banknotes);

    if(result.Item3 == 0)
    {
        foreach (var banknote in result.Item2)
        {
            banknotes[banknote.Key] = banknote.Value;
        }

        foreach (var banknote in result.Item1)
        {
            Console.WriteLine($"{banknote.Key} {banknote.Value}");
        }
    }
    else
    {
        Console.WriteLine("Insufficient funds to issue");
    }
}

(IDictionary<int, int>, IDictionary<int,int>, int) GetMoney(int input, IDictionary<int, int> banknotes)
{
    var tempBanknotes = new Dictionary<int, int>();
    var resultBanknotes = new Dictionary<int, int>();
    var tempInput = input;

    foreach (var banknote in banknotes)
    {
        if (tempInput < banknote.Key)
        {
            tempBanknotes[banknote.Key] = banknote.Value;
            continue;
        }

        var sum = banknote.Key * banknote.Value;

        if (sum == tempInput)
        {
            resultBanknotes[banknote.Key] = banknote.Value;
            tempInput = 0;
            break;
        }

        if (sum > tempInput)
        {
            var rest = tempInput % banknote.Key;
            var needBanknotes = (tempInput - rest) / banknote.Key;
            tempBanknotes[banknote.Key] = banknote.Value - needBanknotes;
            resultBanknotes[banknote.Key] = needBanknotes;
            tempInput = rest;
        }
        else
        {
            var rest = tempInput - sum;
            resultBanknotes[banknote.Key] = banknote.Value;
            tempInput = rest;
        }
    }

    if(tempInput == 0)
    {
        return (resultBanknotes, tempBanknotes, tempInput);
    }

    if(tempInput != 0 && banknotes.Count == 1)
    {
        return (null, null, tempInput);
    }

    return GetMoney(input, banknotes.Skip(1).ToDictionary(k => k.Key, v => v.Value));
}

IDictionary<int, int> InitBanknotes()
{
    var lines = File.ReadAllLines(@"C:\OA\banknotes.txt");

    return lines.Select(p => p.Split().Select(x => int.Parse(x)))
        .OrderByDescending(p => p.First())
        .ToDictionary(k => k.First(), v => v.Last());
}
