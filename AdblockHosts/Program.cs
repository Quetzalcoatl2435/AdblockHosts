Console.WriteLine("Update pihole adblock list.");

var adBlockDomains = new List<string>();

int count = 1;
void AppendRecords(List<string> lines)
{
    foreach (var line in lines)
        if (line.Length > 0 && line[0] != '#' && line.Trim()[^1] != '.')
            if (Uri.IsWellFormedUriString("https://" + line.Trim(), UriKind.Absolute))
            {
                Console.WriteLine(count++.ToString("N0") + " " + line.Trim());
                adBlockDomains.Add(line.Trim());
            }
}

Console.WriteLine("Select source:");
Console.WriteLine("1. Mullvad DNS");
Console.WriteLine("2. Steven Black's unified hosts list");
Console.WriteLine("3. Steven Black's unified hosts + gambling list");
Console.WriteLine("4. All");
var opt = Console.ReadLine();
string domains;
List<string> domainList;
var trimmedDomains = new List<string>();

async Task Mullvad()
{
    Console.WriteLine("Get adblock list.");
    domains = await new HttpClient().GetStringAsync("https://github.com/mullvad/dns-blocklists/raw/main/output/doh/doh_adblock.txt");
    domainList = [.. domains.Split("\n")];
    AppendRecords(domainList);

    Console.WriteLine("Get gambling list.");
    domains = await new HttpClient().GetStringAsync("https://github.com/mullvad/dns-blocklists/raw/main/output/doh/doh_gambling.txt");
    domainList = [.. domains.Split("\n")];
    AppendRecords(domainList);

    Console.WriteLine("Get privacy list.");
    domains = await new HttpClient().GetStringAsync("https://github.com/mullvad/dns-blocklists/raw/main/output/doh/doh_privacy.txt");
    domainList = [.. domains.Split("\n")];
    AppendRecords(domainList);
}

async Task SteveBlack()
{
    Console.WriteLine("Get Steven Black's unified hosts list + gambling from https://raw.githubusercontent.com/StevenBlack/hosts/master/alternates/gambling/hosts");
    domains = await new HttpClient().GetStringAsync("https://raw.githubusercontent.com/StevenBlack/hosts/master/alternates/gambling/hosts");
    domainList = [.. domains.Split("\n")];
    foreach (var domain in domainList)
        if (domain.Length > 8 && domain[..8] == "0.0.0.0 " && domain.Split(' ')[1] != "0.0.0.0")
            trimmedDomains.Add(domain.Split(' ')[1]);
    AppendRecords(trimmedDomains);
}

switch (opt)
{
    case "1":
        await Mullvad();
        break;
    case "2":
        Console.WriteLine("Get Steven Black's unified hosts list from https://raw.githubusercontent.com/StevenBlack/hosts/master/hosts");
        domains = await new HttpClient().GetStringAsync("https://raw.githubusercontent.com/StevenBlack/hosts/master/hosts");
        domainList = [.. domains.Split("\n")];
        foreach (var domain in domainList)
            if (domain.Length > 8 && domain[..8] == "0.0.0.0 " && domain.Split(' ')[1] != "0.0.0.0")
                trimmedDomains.Add(domain.Split(' ')[1]);
        AppendRecords(trimmedDomains);
        break;
    case "3":
        await SteveBlack();
        break;
    case "4":
        await Mullvad();
        Console.WriteLine();
        await SteveBlack();
        break;
}

Console.WriteLine("Number of entries: " + adBlockDomains.Count.ToString("N0"));
Console.WriteLine("Number of entries (deduplicated): " + adBlockDomains.Distinct().Count().ToString("N0"));
Console.WriteLine("Writing domains to file...");

File.WriteAllText("pihole_domain_list.txt", string.Join("\n", adBlockDomains.Distinct()));

Console.WriteLine("Done!");
Console.ReadLine();