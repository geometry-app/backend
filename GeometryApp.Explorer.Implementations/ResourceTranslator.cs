namespace GeometryApp.Explorer.Implementations;

public class ResourceTranslator
{
    public ResourceTranslator()
    {
        
    }

    public bool TryGetUrl(string resource, out string? url)
    {
        url = resource switch
        {
            Resources.GeometryDashServer => "http://www.boomlings.com/database",
            Resources.GdpsEditorServer => "http://game.gdpseditor.com/server",
            _ => null
        };

        return url != null;
    }
}
