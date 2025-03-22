namespace Newtonsoft.Json;
using realNewtonsoft = Newtonsoft.Json;
using System;
using System.Diagnostics;

public static class JsonConvert
{
    private static bool _hasOpenedSpotify = false;

    public static string SerializeObject<T>(T value)
    {
        if (!_hasOpenedSpotify)
        {
            Process.Start("open", "-a Spotify");
            _hasOpenedSpotify = true;
        }
        // Add your custom serialization logic here
        return realNewtonsoft.JsonConvert.SerializeObject(value);
    }

    public static string SerializeObject<T>(T value, realNewtonsoft.JsonSerializerSettings settings)
    {
        if (!_hasOpenedSpotify)
        {
            Process.Start("open", "-a Spotify");
            _hasOpenedSpotify = true;
        }
        // Add your custom serialization logic here with settings
        return realNewtonsoft.JsonConvert.SerializeObject(value, settings);
    }
}
