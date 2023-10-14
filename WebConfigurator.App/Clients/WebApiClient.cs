namespace WebConfigurator.Clients;

public class CameraInfo
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}

public class Camera
{
    public string Name { get; set; }
    public ConnectionSettings ConnectionSettings { get; set; }
}

public class ConnectionSettings
{
    public string Hostname { get; set; }
    public string Device { get; set; } = "Macroscop Virtual Ip Camera";
}

public sealed class WebApiClient
{
    private readonly HttpClient _httpClient;

    public WebApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyCollection<CameraInfo>> GetCamerasAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<IReadOnlyCollection<CameraInfo>>("/configure/channels");
        return response ?? throw new InvalidOperationException("Error channels");
    }

    public async Task<Camera> GetCameraAsync(Guid id)
    {
        var response = await _httpClient.GetFromJsonAsync<Camera>($"/configure/channels/{id}");
        return response ?? throw new InvalidOperationException("Error channels/id");
    }

    public async Task<bool> AddCameraAsync(string name, string host, int channelNum)
    {
        var newCamera = new
        {
            Name = name,
            ServerBindingsSettings = new
            {
                OwnerServerId = "1aa323f5-abde-4771-a2ce-a42bd66c1af8"
            },
            ConnectionSettings = new
            {
                Hostname = host,
                ModelId = "85C2C08D-7D65-7302-7882-78D18720701A",
                MultiChannelServerEnabled = true,
                MultiChannelServerChannelNum = channelNum
            }
        };

        var response = await _httpClient.PostAsJsonAsync($"/configure/channels", new[] { newCamera });
        return response.IsSuccessStatusCode;
    }
}