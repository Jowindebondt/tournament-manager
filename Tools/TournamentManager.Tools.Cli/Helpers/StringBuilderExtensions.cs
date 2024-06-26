﻿using System.Text;

namespace TournamentManager.Tools.Cli;

public static class StringBuilderExtensions
{
    public static void AddGeneratorInformation(this StringBuilder sb)
    {
        var text = $"// This file is generated by {nameof(TournamentManager)}.{nameof(TournamentManager.Tools)}.{nameof(TournamentManager.Tools.Cli)} //";
        sb.AppendLine(new string('/', text.Length));
        sb.AppendLine(text);
        sb.AppendLine(new string('/', text.Length));
        sb.AppendLine();
    }
}
