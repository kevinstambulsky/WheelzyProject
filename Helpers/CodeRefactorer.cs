using System.Text.RegularExpressions;

public static class CodeRefactorer
{
    private static readonly Regex AsyncMethodRegex = new(
        @"\basync\s+(?:[\w<>\[\]\.?\s]+?)\s+(?<name>[A-Za-z_]\w*)\s*\(",
        RegexOptions.Compiled | RegexOptions.Multiline);

    private static readonly Regex SuffixRegex = new(
        @"\b(\w+?)(Vm|Vms|Dto|Dtos)\b",
        RegexOptions.Compiled);

    private static readonly Regex MethodSpacingRegex = new(
        @"\}\r?\n(?!\r?\n)([ \t]*)
          (?=                                   
            (?:\[[^\]]+\]\s*)*                  
            (?:(?:public|private|protected|internal|static|async|sealed|virtual|override|new|unsafe)\s+)*  
            [^\s\(\{\};]+                       
            \s+[A-Za-z_]\w*\s*\(                
          )",
        RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);

    public static void ProcessFolder(string root)
    {
        foreach (var path in Directory.EnumerateFiles(root, "*.cs", SearchOption.AllDirectories))
        {
            if (path.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}") ||
                path.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}"))
                continue;

            var text = File.ReadAllText(path);
            var newText = ProcessText(text);

            if (!string.Equals(text, newText, StringComparison.Ordinal))
                File.WriteAllText(path, newText);
        }
    }

    public static string ProcessText(string source)
    {
        source = AsyncMethodRegex.Replace(source, m =>
        {
            var name = m.Groups["name"].Value;
            if (name.EndsWith("Async")) return m.Value;
            return m.Value.Replace(name + "(", name + "Async(");
        });

        source = SuffixRegex.Replace(source, m =>
        {
            var core = m.Groups[1].Value;
            var suf = m.Groups[2].Value;
            return core + TransformSuffix(suf);
        });

        source = MethodSpacingRegex.Replace(source, "}\n\n$1");

        return source;
    }

    private static string TransformSuffix(string suf) => suf switch
    {
        "Vm" => "VM",
        "Vms" => "VMs",
        "Dto" => "DTO",
        "Dtos" => "DTOs",
        _ => suf
    };
}
