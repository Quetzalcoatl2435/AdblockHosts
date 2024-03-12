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
    Console.WriteLine("Get oisd-big from https://big.oisd.nl");
    domains = await new HttpClient().GetStringAsync("https://big.oisd.nl");
    domains = domains.Replace("||", "").Replace("^", "");
    domainList = domains.Split("\n").ToList();
    AppendRecords(domainList);

    Console.WriteLine();
    Console.WriteLine("Get frellwits-swedish-hosts-file from https://raw.githubusercontent.com/lassekongo83/Frellwits-filter-lists/master/Frellwits-Swedish-Hosts-File.txt");
    domains = await new HttpClient().GetStringAsync("https://raw.githubusercontent.com/lassekongo83/Frellwits-filter-lists/master/Frellwits-Swedish-Hosts-File.txt");
    domains = domains.Replace("127.0.0.1 ", "");
    domainList = domains.Split("\n").ToList();
    AppendRecords(domainList);

    Console.WriteLine("Get AdguardDNS from https://v.firebog.net/hosts/AdguardDNS.txt");
    domains = await new HttpClient().GetStringAsync("https://v.firebog.net/hosts/AdguardDNS.txt");
    domainList = domains.Split("\n").ToList();
    AppendRecords(domainList);

    Console.WriteLine("Get gambling blocklists from https://raw.githubusercontent.com/blocklistproject/Lists/master/alt-version/gambling-nl.txt");
    domains = await new HttpClient().GetStringAsync("https://raw.githubusercontent.com/blocklistproject/Lists/master/alt-version/gambling-nl.txt");
    domainList = domains.Split("\n").ToList();
    AppendRecords(domainList);

    Console.WriteLine();
    Console.WriteLine("Get firebog-easylist-privacy from https://v.firebog.net/hosts/Easyprivacy.txt");
    domains = await new HttpClient().GetStringAsync("https://v.firebog.net/hosts/Easyprivacy.txt");
    domainList = domains.Split("\n").ToList();
    AppendRecords(domainList);

    Console.WriteLine();
    Console.WriteLine("Get windows-spy-blocker-spy from https://raw.githubusercontent.com/crazy-max/WindowsSpyBlocker/master/data/hosts/spy.txt");
    domains = await new HttpClient().GetStringAsync("https://raw.githubusercontent.com/crazy-max/WindowsSpyBlocker/master/data/hosts/spy.txt");
    domains = domains.Replace("0.0.0.0 ", "");
    domainList = domains.Split("\n").ToList();
    AppendRecords(domainList);

    Console.WriteLine();
    Console.WriteLine("Get perflyst-android-tracking from https://raw.githubusercontent.com/Perflyst/PiHoleBlocklist/master/android-tracking.txt");
    domains = await new HttpClient().GetStringAsync("https://raw.githubusercontent.com/Perflyst/PiHoleBlocklist/master/android-tracking.txt");
    domainList = domains.Split("\n").ToList();
    AppendRecords(domainList);

    Console.WriteLine();
    Console.WriteLine("Get telemetry-alexa from https://raw.githubusercontent.com/nextdns/native-tracking-domains/main/domains/alexa");
    domains = await new HttpClient().GetStringAsync("https://raw.githubusercontent.com/nextdns/native-tracking-domains/main/domains/alexa");
    domainList = domains.Split("\n").ToList();
    AppendRecords(domainList);

    Console.WriteLine();
    Console.WriteLine("Get telemetry-apple from https://raw.githubusercontent.com/nextdns/native-tracking-domains/main/domains/apple");
    domains = await new HttpClient().GetStringAsync("https://raw.githubusercontent.com/nextdns/native-tracking-domains/main/domains/apple");
    domainList = domains.Split("\n").ToList();
    AppendRecords(domainList);

    Console.WriteLine();
    Console.WriteLine("Get telemetry-huawei from https://raw.githubusercontent.com/nextdns/native-tracking-domains/main/domains/huawei");
    domains = await new HttpClient().GetStringAsync("https://raw.githubusercontent.com/nextdns/native-tracking-domains/main/domains/huawei");
    domainList = domains.Split("\n").ToList();
    AppendRecords(domainList);

    Console.WriteLine();
    Console.WriteLine("Get telemetry-samsung from https://raw.githubusercontent.com/nextdns/native-tracking-domains/main/domains/samsung");
    domains = await new HttpClient().GetStringAsync("https://raw.githubusercontent.com/nextdns/native-tracking-domains/main/domains/samsung");
    domainList = domains.Split("\n").ToList();
    AppendRecords(domainList);

    Console.WriteLine();
    Console.WriteLine("Get telemetry-sonos from https://raw.githubusercontent.com/nextdns/native-tracking-domains/main/domains/sonos");
    domains = await new HttpClient().GetStringAsync("https://raw.githubusercontent.com/nextdns/native-tracking-domains/main/domains/sonos");
    domainList = domains.Split("\n").ToList();
    AppendRecords(domainList);

    Console.WriteLine();
    Console.WriteLine("Get telemetry-windows from https://raw.githubusercontent.com/nextdns/native-tracking-domains/main/domains/windows");
    domains = await new HttpClient().GetStringAsync("https://raw.githubusercontent.com/nextdns/native-tracking-domains/main/domains/windows");
    domainList = domains.Split("\n").ToList();
    AppendRecords(domainList);

    Console.WriteLine();
    Console.WriteLine("Get telemetry-xiaomi from https://raw.githubusercontent.com/nextdns/native-tracking-domains/main/domains/xiaomi");
    domains = await new HttpClient().GetStringAsync("https://raw.githubusercontent.com/nextdns/native-tracking-domains/main/domains/xiaomi");
    domainList = domains.Split("\n").ToList();
    AppendRecords(domainList);

    Console.WriteLine();
    Console.WriteLine("Get malware content blocklist from https://urlhaus.abuse.ch/downloads/hostfile");
    domains = await new HttpClient().GetStringAsync("https://urlhaus.abuse.ch/downloads/hostfile");
    domains = domains.Replace("127.0.0.1\t", "").Replace("\r", "\n");
    domainList = domains.Split("\n").ToList();
    AppendRecords(domainList);
}

async Task SteveBlack()
{
    Console.WriteLine("Get Steven Black's unified hosts list + gambling from https://raw.githubusercontent.com/StevenBlack/hosts/master/alternates/gambling/hosts");
    domains = await new HttpClient().GetStringAsync("https://raw.githubusercontent.com/StevenBlack/hosts/master/alternates/gambling/hosts");
    domainList = domains.Split("\n").ToList();
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
        domainList = domains.Split("\n").ToList();
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