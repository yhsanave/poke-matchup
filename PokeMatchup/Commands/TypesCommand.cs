using System.ComponentModel;
using PokeMatchup;
using Spectre.Console;
using Spectre.Console.Cli;

[Description("Print a formatted list of types.")]
internal class TypesCommand : Command<TypesCommand.Settings>
{
    public class Settings : CommandSettings { }

    public override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var types = Enum.GetNames<PokeTypes>();
        List<string> formattedTypes = new List<string>();
        foreach (var t in types)
        {
            var type = MatchupCommand.MarkupType(t);
            formattedTypes.Add(type.PadRight(22));
        }

        for (var i = 0; i < 20; i += 5)
        {
            AnsiConsole.MarkupLine(string.Join("", formattedTypes.Slice(i, 5)));
        }

        return 0;
    }
}