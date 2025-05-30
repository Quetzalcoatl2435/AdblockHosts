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

Console.WriteLine("Sources:");
Console.WriteLine("1. Mullvad (ads, trackers, malware)");
Console.WriteLine("2. Mullvad (ads, trackers, malware, gambling)");
Console.WriteLine("3. Steven Black's unified hosts (ads, malware)");
Console.WriteLine("4. Steven Black's unified hosts (ads, malware, gambling)");
Console.WriteLine("5. Mullvad (ads, trackers, malware) + Steven Black's unified hosts list (ads, malware)");
Console.WriteLine("6. Mullvad (ads, trackers, malware) + Steven Black's unified hosts list (ads, malware, gambling)");
Console.WriteLine("_. Customise");
Console.Write("Select source: ");
var opt = Console.ReadLine();
Console.WriteLine();
string domains;
List<string> domainList;
var trimmedDomains = new List<string>();

async Task Mullvad()
{
    Console.WriteLine("Get adblock list.");
    domains = await new HttpClient().GetStringAsync("https://github.com/mullvad/dns-blocklists/raw/main/output/doh/doh_adblock.txt");
    domainList = [.. domains.Split("\n")];
    AppendRecords(domainList);

    Console.WriteLine("Get privacy list.");
    domains = await new HttpClient().GetStringAsync("https://github.com/mullvad/dns-blocklists/raw/main/output/doh/doh_privacy.txt");
    domainList = [.. domains.Split("\n")];
    AppendRecords(domainList);
}

async Task MullvadAdult()
{
    Console.WriteLine("Get adult list.");
    domains = await new HttpClient().GetStringAsync("https://github.com/mullvad/dns-blocklists/raw/main/output/doh/doh_adult.txt");
    domainList = [.. domains.Split("\n")];
    AppendRecords(domainList);
}

async Task MullvadGambling()
{
    Console.WriteLine("Get gambling list.");
    domains = await new HttpClient().GetStringAsync("https://github.com/mullvad/dns-blocklists/raw/main/output/doh/doh_gambling.txt");
    domainList = [.. domains.Split("\n")];
    AppendRecords(domainList);
}

async Task MullvadSocial()
{
    Console.WriteLine("Get social list.");
    domains = await new HttpClient().GetStringAsync("https://github.com/mullvad/dns-blocklists/raw/main/output/doh/doh_social.txt");
    domainList = [.. domains.Split("\n")];
    AppendRecords(domainList);
}

async Task SteveBlack()
{
    Console.WriteLine("Get Steven Black's unified hosts list.");
    domains = await new HttpClient().GetStringAsync("https://raw.githubusercontent.com/StevenBlack/hosts/master/hosts");
    domainList = [.. domains.Split("\n")];
    foreach (var domain in domainList)
        if (domain.Length > 8 && domain[..8] == "0.0.0.0 " && domain.Split(' ')[1] != "0.0.0.0")
            trimmedDomains.Add(domain.Split(' ')[1]);
    AppendRecords(trimmedDomains);
}

async Task SteveBlackFakeNews()
{
    Console.WriteLine("Get Steven Black's fakenews hosts list.");
    domains = await new HttpClient().GetStringAsync("https://raw.githubusercontent.com/StevenBlack/hosts/master/alternates/fakenews-only/hosts");
    domainList = [.. domains.Split("\n")];
    foreach (var domain in domainList)
        if (domain.Length > 8 && domain[..8] == "0.0.0.0 " && domain.Split(' ')[1] != "0.0.0.0")
            trimmedDomains.Add(domain.Split(' ')[1]);
    AppendRecords(trimmedDomains);
}

async Task SteveBlackGambling()
{
    Console.WriteLine("Get Steven Black's gambling hosts list.");
    domains = await new HttpClient().GetStringAsync("https://raw.githubusercontent.com/StevenBlack/hosts/master/alternates/gambling-only/hosts");
    domainList = [.. domains.Split("\n")];
    foreach (var domain in domainList)
        if (domain.Length > 8 && domain[..8] == "0.0.0.0 " && domain.Split(' ')[1] != "0.0.0.0")
            trimmedDomains.Add(domain.Split(' ')[1]);
    AppendRecords(trimmedDomains);
}

async Task SteveBlackAdult()
{
    Console.WriteLine("Get Steven Black's adult hosts list.");
    domains = await new HttpClient().GetStringAsync("https://raw.githubusercontent.com/StevenBlack/hosts/master/alternates/porn-only/hosts");
    domainList = [.. domains.Split("\n")];
    foreach (var domain in domainList)
        if (domain.Length > 8 && domain[..8] == "0.0.0.0 " && domain.Split(' ')[1] != "0.0.0.0")
            trimmedDomains.Add(domain.Split(' ')[1]);
    AppendRecords(trimmedDomains);
}

async Task SteveBlackSocial()
{
    Console.WriteLine("Get Steven Black's social hosts list.");
    domains = await new HttpClient().GetStringAsync("https://raw.githubusercontent.com/StevenBlack/hosts/master/alternates/social-only/hosts");
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
        await Mullvad();
        Console.WriteLine();
        await MullvadGambling();
        break;
    case "3":
        await SteveBlack();
        break;
    case "4":
        await SteveBlack();
        Console.WriteLine();
        await SteveBlackGambling();
        break;
    case "5":
        await Mullvad();
        Console.WriteLine();
        await SteveBlack();
        break;
    case "6":
        await Mullvad();
        Console.WriteLine();
        await SteveBlack();
        Console.WriteLine();
        await SteveBlackGambling();
        break;
    default:
        Console.WriteLine("Customise sources:");
        Console.WriteLine("1. Mullvad (ads, trackers, malware)");
        Console.WriteLine("2. Mullvad (adult)");
        Console.WriteLine("3. Mullvad (gambling)");
        Console.WriteLine("4. Mullvad (social)");
        Console.WriteLine("5. Steven Black's unified hosts (ads, malware)");
        Console.WriteLine("6. Steven Black's fakenews hosts");
        Console.WriteLine("7. Steven Black's gambling hosts");
        Console.WriteLine("8. Steven Black's adult hosts");
        Console.WriteLine("9. Steven Black's social hosts");
        Console.Write("Select source(s) [numbers, no spaces, e.g. 137]: ");
        opt = Console.ReadLine();
        foreach (char o in opt)
        {
            Console.WriteLine();
            switch (o)
            {
                case '1':
                    await Mullvad();
                    break;
                case '2':
                    await MullvadAdult();
                    break;
                case '3':
                    await MullvadGambling();
                    break;
                case '4':
                    await MullvadSocial();
                    break;
                case '5':
                    await SteveBlack();
                    break;
                case '6':
                    await SteveBlackFakeNews();
                    break;
                case '7':
                    await SteveBlackGambling();
                    break;
                case '8':
                    await SteveBlackAdult();
                    break;
                case '9':
                    await SteveBlackSocial();
                    break;
            }
        }
        break;
}

Console.WriteLine();
adBlockDomains.Remove("s.youtube.com"); //needed for youtube history
Console.WriteLine("Number of entries: " + adBlockDomains.Count.ToString("N0"));
Console.WriteLine("Number of entries (deduplicated): " + adBlockDomains.Distinct().Count().ToString("N0"));
Console.WriteLine("Writing domains to file...");

File.WriteAllText("pihole_domain_list.txt", string.Join("\n", adBlockDomains.Distinct()));

Console.WriteLine("Done!");
Console.ReadLine();