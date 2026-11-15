using System.Text.Json.Serialization;
using CounterStrikeSharp.API.Core;

namespace CscoWeapons;

public enum VipUspMode
{
    Off,
    All,
    Choice,
}

public class CscoWeaponsConfig : BasePluginConfig
{
    /// <summary>Per-weapon switch. false keeps the stock CS2 model.</summary>
    [JsonPropertyName("Weapons")]
    public Dictionary<string, bool> Weapons { get; set; } = WeaponModels.All.ToDictionary(w => w.Name, _ => true);

    /// <summary>"off", "all" or "choice" (players toggle it with !vipusp).</summary>
    [JsonPropertyName("VipUsp")]
    public string VipUsp { get; set; } = "off";

    /// <summary>FOV used by the AUG and SG 553 classic zoom.</summary>
    [JsonPropertyName("ClassicZoomFov")]
    public int ClassicZoomFov { get; set; } = 55;

    [JsonPropertyName("ConfigVersion")]
    public override int Version { get; set; } = 2;

    public bool IsEnabled(string weapon) => Weapons.TryGetValue(weapon, out var enabled) && enabled;

    public VipUspMode VipUspMode => VipUsp?.Trim().ToLowerInvariant() switch
    {
        "all" => VipUspMode.All,
        "choice" => VipUspMode.Choice,
        _ => VipUspMode.Off,
    };
}
