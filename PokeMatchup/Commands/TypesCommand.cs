using System.ComponentModel;
using PokeMatchup;
using Spectre.Console;
using Spectre.Console.Cli;

namespace PokeMatchup;

[Description("Print a formatted list of types.")]
public class TypesCommand(IAnsiConsole console) : Command<TypesCommand.Settings>
{
    private readonly IAnsiConsole _console = console;

    public class Settings : CommandSettings { }

    public override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var types = Enum.GetNames<PokeTypes>();
        List<string> formattedTypes = [];
        foreach (var t in types)
        {
            var type = MatchupCommand.MarkupType(t);
            formattedTypes.Add(type.PadRight(22));
        }

        for (var i = 0; i < 20; i += 5)
        {
            _console.MarkupLine(string.Join("", formattedTypes.Slice(i, 5)));
        }

        return 0;
    }
}