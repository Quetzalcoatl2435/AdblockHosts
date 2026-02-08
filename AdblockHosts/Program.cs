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
Console.WriteLine("4. Mullvad (ads, trackers, malware) + Steven Black's unified hosts list (ads, malware)");
Console.WriteLine("5. Mullvad (ads, trackers, malware) + Steven Black's unified hosts list (ads, malware, fakenews, gambling)");
Console.WriteLine("6. Mullvad (ads, trackers, malware) + Steven Black's unified hosts list (ads, malware, fakenews, gambling) + NoTrack (tracker, malware)");
Console.WriteLine("_. Customise");
Console.Write("Select source: ");
var opt = Console.ReadLine();
Console.WriteLine();
string domains;
List<string> domainList;

async Task Mullvad()
{
    Console.WriteLine("Get adblock list.");
    domains = await new HttpClient().GetStringAsync("https://github.com/mullvad/dns-blocklists/raw/main/output/doh/doh_adblock.txt");
    domainList = [.. domains.Split("\n")];
    AppendRecords(domainList);
}

async Task MullvadTrackers()
{
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
    var trimmedDomains = new List<string>();
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
    var trimmedDomains = new List<string>();
    Console.WriteLine("Get Steven Black's fakenews hosts list.");
    domains = await new HttpClient().GetStringAsync("https://raw.githubusercontent.com/StevenBlack/hosts/master/alternates/fakenews-only/hosts");
    domainList = [.. domains.Split("\n")];
    foreach (var domain in domainList)
        if (domain.Length > 8 && domain[..8] == "0.0.0.0 ")
            trimmedDomains.Add(domain.Split(' ')[1]);
    AppendRecords(trimmedDomains);
}

async Task SteveBlackGambling()
{
    var trimmedDomains = new List<string>();
    Console.WriteLine("Get Steven Black's gambling hosts list.");
    domains = await new HttpClient().GetStringAsync("https://raw.githubusercontent.com/StevenBlack/hosts/master/alternates/gambling-only/hosts");
    domainList = [.. domains.Split("\n")];
    foreach (var domain in domainList)
        if (domain.Length > 8 && domain[..8] == "0.0.0.0 ")
            trimmedDomains.Add(domain.Split(' ')[1]);
    AppendRecords(trimmedDomains);
}

async Task SteveBlackAdult()
{
    var trimmedDomains = new List<string>();
    Console.WriteLine("Get Steven Black's adult hosts list.");
    domains = await new HttpClient().GetStringAsync("https://raw.githubusercontent.com/StevenBlack/hosts/master/alternates/porn-only/hosts");
    domainList = [.. domains.Split("\n")];
    foreach (var domain in domainList)
        if (domain.Length > 8 && domain[..8] == "0.0.0.0 ")
            trimmedDomains.Add(domain.Split(' ')[1]);
    AppendRecords(trimmedDomains);
}

async Task SteveBlackSocial()
{
    var trimmedDomains = new List<string>();
    Console.WriteLine("Get Steven Black's social hosts list.");
    domains = await new HttpClient().GetStringAsync("https://raw.githubusercontent.com/StevenBlack/hosts/master/alternates/social-only/hosts");
    domainList = [.. domains.Split("\n")];
    foreach (var domain in domainList)
        if (domain.Length > 8 && domain[..8] == "0.0.0.0 ")
            trimmedDomains.Add(domain.Split(' ')[1]);
    AppendRecords(trimmedDomains);
}

async Task SteveBlackAdHoc()
{
    var trimmedDomains = new List<string>();
    Console.WriteLine("Get Steven Black's ad-hoc hosts list.");
    domains = await new HttpClient().GetStringAsync("https://raw.githubusercontent.com/StevenBlack/hosts/master/data/StevenBlack/hosts");
    domainList = [.. domains.Split("\n")];
    foreach (var domain in domainList)
        if (domain.Length > 8 && domain[..8] == "0.0.0.0 ")
            trimmedDomains.Add(domain.Split(' ')[1]);
    AppendRecords(trimmedDomains);
}

async Task AdAway()
{
    var trimmedDomains = new List<string>();
    Console.WriteLine("Get AdAway hosts list.");
    domains = await new HttpClient().GetStringAsync("https://raw.githubusercontent.com/AdAway/adaway.github.io/master/hosts.txt");
    domainList = [.. domains.Split("\n")];
    foreach (var domain in domainList)
        if (domain.Length > 10 && domain[..10] == "127.0.0.1 " && !string.IsNullOrEmpty(domain.Split(' ')[1]) && domain.Split(' ')[1] != "localhost")
            trimmedDomains.Add(domain.Split(' ')[1]);
    AppendRecords(trimmedDomains);
}

async Task BaddBoyz()
{
    var trimmedDomains = new List<string>();
    Console.WriteLine("Get Mitchell Krog's Badd Boyz hosts list.");
    domains = await new HttpClient().GetStringAsync("https://raw.githubusercontent.com/mitchellkrogza/Badd-Boyz-Hosts/master/hosts");
    domainList = [.. domains.Split("\n")];
    foreach (var domain in domainList)
        if (domain.Length > 8 && domain[..8] == "0.0.0.0 ")
            trimmedDomains.Add(domain.Split(' ')[1]);
    AppendRecords(trimmedDomains);
}

async Task KADHosts()
{
    var trimmedDomains = new List<string>();
    Console.WriteLine("Get KADHosts list.");
    domains = await new HttpClient().GetStringAsync("https://raw.githubusercontent.com/FiltersHeroes/KADhosts/master/KADhosts.txt");
    domainList = [.. domains.Split("\n")];
    foreach (var domain in domainList)
        if (domain.Length > 8 && domain[..8] == "0.0.0.0 ")
            trimmedDomains.Add(domain.Split(' ')[1]);
    AppendRecords(trimmedDomains);
}

async Task MVPS()
{
    var trimmedDomains = new List<string>();
    Console.WriteLine("Get MVPS hosts list.");
    domains = await new HttpClient().GetStringAsync("https://winhelp2002.mvps.org/hosts.txt");
    domainList = [.. domains.Split("\n")];
    foreach (var domain in domainList)
        if (domain.Length > 8 && domain[..8] == "0.0.0.0 ")
            trimmedDomains.Add(domain.Split(' ')[1]);
    AppendRecords(trimmedDomains);
}

async Task someonewhocares()
{
    var trimmedDomains = new List<string>();
    Console.WriteLine("Get Dan Pollock – someonewhocares hosts list.");
    domains = await new HttpClient().GetStringAsync("https://someonewhocares.org/hosts/zero/hosts");
    domainList = [.. domains.Split("\n")];
    foreach (var domain in domainList)
        if (domain.Length > 8 && domain[..8] == "0.0.0.0 ")
            trimmedDomains.Add(domain.Split(' ')[1]);
    AppendRecords(trimmedDomains);
}

async Task TiuxoAds()
{
    var trimmedDomains = new List<string>();
    Console.WriteLine("Get Tiuxo hostlist - ads list.");
    domains = await new HttpClient().GetStringAsync("https://raw.githubusercontent.com/tiuxo/hosts/master/ads");
    domainList = [.. domains.Split("\n")];
    foreach (var domain in domainList)
        if (domain.Length > 8 && domain[..8] == "0.0.0.0 ")
            trimmedDomains.Add(domain.Split(' ')[1]);
    AppendRecords(trimmedDomains);
}

async Task UncheckyAds()
{
    var trimmedDomains = new List<string>();
    Console.WriteLine("Get UncheckyAds hosts list.");
    domains = await new HttpClient().GetStringAsync("https://raw.githubusercontent.com/FadeMind/hosts.extras/master/UncheckyAds/hosts");
    domainList = [.. domains.Split("\n")];
    foreach (var domain in domainList)
        if (domain.Length > 8 && domain[..8] == "0.0.0.0 ")
            trimmedDomains.Add(domain.Split(' ')[1]);
    AppendRecords(trimmedDomains);
}

async Task URLHaus()
{
    var trimmedDomains = new List<string>();
    Console.WriteLine("Get URLHaus hosts list.");
    domains = await new HttpClient().GetStringAsync("https://urlhaus.abuse.ch/downloads/hostfile/");
    domainList = [.. domains.Split("\n")];
    foreach (var domain in domainList)
        if (domain.Length > 9 && domain[..9] == "127.0.0.1")
            trimmedDomains.Add(domain.Split('\t')[1]);
    AppendRecords(trimmedDomains);
}

async Task yoyo()
{
    var trimmedDomains = new List<string>();
    Console.WriteLine("Get yoyo.org hosts list.");
    domains = await new HttpClient().GetStringAsync("https://pgl.yoyo.org/adservers/serverlist.php?hostformat=hosts&mimetype=plaintext&useip=0.0.0.0");
    domainList = [.. domains.Split("\n")];
    foreach (var domain in domainList)
        if (domain.Length > 8 && domain[..8] == "0.0.0.0 ")
            trimmedDomains.Add(domain.Split(' ')[1]);
    AppendRecords(trimmedDomains);
}

async Task NoTrackTracker()
{
    var trimmedDomains = new List<string>();
    Console.WriteLine("Get NoTrack Tracker Blocklist.");
    domains = await new HttpClient().GetStringAsync("https://gitlab.com/quidsup/notrack-blocklists/-/raw/master/trackers.list");
    domainList = [.. domains.Split("\n")];
    foreach (var domain in domainList)
    {
        if (domain.Length > 2 && domain[0] != '#')
            trimmedDomains.Add(domain);
    }
    AppendRecords(trimmedDomains);
}

async Task NoTrackMalware()
{
    var trimmedDomains = new List<string>();
    Console.WriteLine("Get NoTrack Malware Blocklist.");
    domains = await new HttpClient().GetStringAsync("https://gitlab.com/quidsup/notrack-blocklists/-/raw/master/malware.list");
    domainList = [.. domains.Split("\n")];
    foreach (var domain in domainList)
    {
        if (domain.Length > 2 && domain[0] != '#')
            trimmedDomains.Add(domain);
    }
    AppendRecords(trimmedDomains);
}

switch (opt)
{
    case "1":
        await Mullvad();
        Console.WriteLine();
        await MullvadTrackers();
        break;
    case "2":
        await Mullvad();
        Console.WriteLine();
        await MullvadTrackers();
        Console.WriteLine();
        await MullvadGambling();
        break;
    case "3":
        await SteveBlack();
        break;
    case "4":
        await Mullvad();
        Console.WriteLine();
        await MullvadTrackers();
        Console.WriteLine();
        await SteveBlack();
        break;
    case "5":
        await Mullvad();
        Console.WriteLine();
        await MullvadTrackers();
        Console.WriteLine();
        await SteveBlack();
        Console.WriteLine();
        await SteveBlackFakeNews();
        Console.WriteLine();
        await SteveBlackGambling();
        break;
    case "6":
        await Mullvad();
        Console.WriteLine();
        await MullvadTrackers();
        Console.WriteLine();
        await SteveBlack();
        Console.WriteLine();
        await SteveBlackFakeNews();
        Console.WriteLine();
        await SteveBlackGambling();
        Console.WriteLine();
        await NoTrackTracker();
        Console.WriteLine();
        await NoTrackMalware();
        break;
    default:
        Console.WriteLine("Customise sources:");
        Console.WriteLine("1. Mullvad (ads, malware)");
        Console.WriteLine("2. Mullvad (trackers)");
        Console.WriteLine("3. Mullvad (adult)");
        Console.WriteLine("4. Mullvad (gambling)");
        Console.WriteLine("5. Mullvad (social)");
        Console.WriteLine("6. Steven Black's unified hosts (ads, malware)");
        Console.WriteLine("7. Steven Black's fakenews hosts");
        Console.WriteLine("8. Steven Black's gambling hosts");
        Console.WriteLine("9. Steven Black's adult hosts");
        Console.WriteLine("0. Steven Black's social hosts");
        Console.WriteLine("a. NoTrack Tracker Blocklist");
        Console.WriteLine("b. NoTrack Malware Blocklist");
        Console.WriteLine();
        Console.WriteLine("Pick from Steven Black's sources:");
        Console.WriteLine("c. Steven Black's ad-hoc list");
        Console.WriteLine("d. AdAway");
        Console.WriteLine("e. Mitchell Krog's Badd Boyz Hosts");
        Console.WriteLine("f. KADHosts");
        Console.WriteLine("g. MVPS");
        Console.WriteLine("h. Dan Pollock – someonewhocares");
        Console.WriteLine("i. Tiuxo hostlist - ads");
        Console.WriteLine("j. UncheckyAds");
        Console.WriteLine("k. URLHaus");
        Console.WriteLine("l. yoyo.org");
        Console.Write("Select source(s) [numbers, no spaces, e.g. 137b]: ");
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
                    await MullvadTrackers();
                    break;
                case '3':
                    await MullvadAdult();
                    break;
                case '4':
                    await MullvadGambling();
                    break;
                case '5':
                    await MullvadSocial();
                    break;
                case '6':
                    await SteveBlack();
                    break;
                case '7':
                    await SteveBlackFakeNews();
                    break;
                case '8':
                    await SteveBlackGambling();
                    break;
                case '9':
                    await SteveBlackAdult();
                    break;
                case '0':
                    await SteveBlackSocial();
                    break;
                case 'a':
                    await NoTrackTracker();
                    break;
                case 'b':
                    await NoTrackMalware();
                    break;
                case 'c':
                    await SteveBlackAdHoc();
                    break;
                case 'd':
                    await AdAway();
                    break;
                case 'e':
                    await BaddBoyz();
                    break;
                case 'f':
                    await KADHosts();
                    break;
                case 'g':
                    await MVPS();
                    break;
                case 'h':
                    await someonewhocares();
                    break;
                case 'i':
                    await TiuxoAds();
                    break;
                case 'j':
                    await UncheckyAds();
                    break;
                case 'k':
                    await URLHaus();
                    break;
                case 'l':
                    await yoyo();
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