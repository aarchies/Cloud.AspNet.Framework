
string path = @"G:\desktop\pytorch\1800\";
var index = 0;
DirectoryInfo dir = new DirectoryInfo(path);
var dataset = new List<string>();
string dirname = string.Empty;
foreach (FileInfo file in dir.GetFiles("*.*", SearchOption.AllDirectories))
{
    if (index == 800000)
        return;
    Console.WriteLine(file.FullName);
    if (file.DirectoryName != dirname && dirname != string.Empty)
    {
        File.AppendAllLines(@"G:\desktop\pytorch" + "\\dataset.txt", dataset);
        index++;
        dataset.Clear();
    }
    var dirs = file.FullName.Replace(path, "");
    dataset.Add(dirs + "\t" + index);
    dirname = file.DirectoryName!;
}
