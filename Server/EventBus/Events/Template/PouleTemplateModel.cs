namespace EventBus.Events.Template;

public class PouleTemplateModel
{
    public string Name { get; }
    public int TotalPlayers { get; }

    public PouleTemplateModel(string name, int totalPlayers)
    {
        Name = name;
        TotalPlayers = totalPlayers;
    }
}
