namespace WebConfigurator.Clients;

public record Camera(string Name, string Url, string Model);

public sealed class WebApiClient
{
    private readonly List<Camera> _cameras = new()
    {
        new Camera("Камера 1", "127.0.0.1:9090", "Virtual Camera"),
        new Camera("Камера 2", "127.0.0.1:9090", "Virtual Camera"),
        new Camera("Камера 3", "127.0.0.1:9090", "Virtual Camera")
    };

    public async Task<IReadOnlyCollection<Camera>> GetCamerasAsync()
    {
        return await Task.FromResult(_cameras.AsReadOnly());
    }

    public async Task<bool> AddCameraAsync(Camera camera)
    {
        _cameras.Add(camera);
        return await Task.FromResult(true);
    }
}