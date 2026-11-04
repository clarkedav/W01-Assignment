using Newtonsoft.Json;

var currentDirectory = Directory.GetCurrentDirectory();
var storesDirectory = Path.Combine(currentDirectory, "stores");

var salesTotalDir = Path.Combine(currentDirectory, "salesTotalDir");
Directory.CreateDirectory(salesTotalDir);

var salesFiles = FindFiles(storesDirectory);

var salesTotal = CalculateSalesTotal(salesFiles);

File.AppendAllText(Path.Combine(salesTotalDir, "totals.txt"), $"{salesTotal}{Environment.NewLine}");

// NEW: generate the summary report
GenerateSalesSummary(salesFiles, storesDirectory, salesTotalDir);

Console.WriteLine($"Total sales: {salesTotal:C}");
Console.WriteLine($"Report saved to: {Path.Combine(salesTotalDir, "salesSummary.txt")}");

IEnumerable<string> FindFiles(string folderName)
{
    List<string> salesFiles = new List<string>();

    var foundFiles = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);

    foreach (var file in foundFiles)
    {
        if (Path.GetFileName(file) == "sales.json")
{
    salesFiles.Add(file);
}
    }

    return salesFiles;
}

double CalculateSalesTotal(IEnumerable<string> salesFiles)
{
    double salesTotal = 0;

    foreach (var file in salesFiles)
    {
        string salesJson = File.ReadAllText(file);
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(salesJson);
        salesTotal += data?.Total ?? 0;
    }

    return salesTotal;
}

// NEW FUNCTION
void GenerateSalesSummary(IEnumerable<string> salesFiles, string storesDirectory, string outputDirectory)
{
    var report = new System.Text.StringBuilder();
    double grandTotal = 0;
    var details = new System.Text.StringBuilder();

    foreach (var file in salesFiles)
    {
        string salesJson = File.ReadAllText(file);
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(salesJson);
        double fileTotal = data?.Total ?? 0;

        grandTotal += fileTotal;

        string fileName = Path.GetRelativePath(storesDirectory, file);
        details.AppendLine($"  {fileName}: {fileTotal:C}");
    }

    report.AppendLine("Sales Summary");
    report.AppendLine("----------------------------");
    report.AppendLine($" Total Sales: {grandTotal:C}");
    report.AppendLine();
    report.AppendLine(" Details:");
    report.Append(details);

    File.WriteAllText(Path.Combine(outputDirectory, "salesSummary.txt"), report.ToString());
}

record SalesData(double Total);
