using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace CscoWeapons;

/// <summary>
/// Gives CS2 weapons their Counter-Strike: Classic Offensive models. The models come from the
/// "CS:CO Weapons" Workshop addon, which the server has to mount (e.g. with MultiAddonManager).
/// </summary>
public partial class CscoWeapons : BasePlugin, IPluginConfig<CscoWeaponsConfig>
{
    public override string ModuleName => "CS:CO Weapons";
    public override string ModuleVersion => "1.0.0";
    public override string ModuleAuthor => "Gh4zi";
    public override string ModuleDescription => "Counter-Strike: Classic Offensive weapon models for CS2";

    public CscoWeaponsConfig Config { get; set; } = new();

    private readonly HashSet<ulong> _vipUspPlayers = new();

    public void OnConfigParsed(CscoWeaponsConfig config) => Config = config;

    public override void Load(bool hotReload)
    {
        RegisterListener<Listeners.OnServerPrecacheResources>(Precache);
        RegisterListener<Listeners.OnEntityCreated>(OnEntityCreated);
        RegisterListener<Listeners.OnTick>(OnTick);
    }

    // Anything that isn't precached shows up as ERROR on clients, including the magazine
    // model the reload animation drops.
    private static void Precache(ResourceManifest manifest)
    {
        foreach (var weapon in WeaponModels.All)
        {
            manifest.AddResource(weapon.Model);
            if (weapon.HasMagazine)
                manifest.AddResource(weapon.MagazineModel);
        }

        manifest.AddResource(WeaponModels.VipUsp);
        manifest.AddResource(WeaponModels.VipUsp.Replace(".vmdl", "_mag.vmdl"));
    }

    private void OnEntityCreated(CEntityInstance entity)
    {
        if (!entity.DesignerName.StartsWith("weapon_"))
            return;

        // The item index isn't set yet when the entity is created.
        var weapon = new CBasePlayerWeapon(entity.Handle);
        Server.NextFrame(() => ApplyModel(weapon));
    }

    [GameEventHandler]
    public HookResult OnItemEquip(EventItemEquip @event, GameEventInfo info)
    {
        var weapon = @event.Userid?.PlayerPawn.Value?.WeaponServices?.ActiveWeapon.Value;
        if (weapon is { IsValid: true })
            Server.NextFrame(() => ApplyModel(weapon));

        return HookResult.Continue;
    }

    private void ApplyModel(CBasePlayerWeapon weapon)
    {
        if (!weapon.IsValid)
            return;

        var index = weapon.AttributeManager.Item.ItemDefinitionIndex;
        var model = WeaponModels.Find(index);
        if (model == null || !Config.IsEnabled(model.Name))
            return;

        var path = index == WeaponModels.UspSilencer && UsesVipUsp(weapon) ? WeaponModels.VipUsp : model.Model;
        weapon.SetModel(path);
    }

    private bool UsesVipUsp(CBasePlayerWeapon weapon)
    {
        switch (Config.VipUspMode)
        {
            case VipUspMode.All:
                return true;
            case VipUspMode.Choice:
                var owner = weapon.OwnerEntity.Value?.As<CCSPlayerPawn>().OriginalController.Value;
                return owner is { IsValid: true } && _vipUspPlayers.Contains(owner.SteamID);
            default:
                return false;
        }
    }

    [ConsoleCommand("css_vipusp", "Switch between the CS:CO USP-S and the VIP USP")]
    [CommandHelper(whoCanExecute: CommandUsage.CLIENT_ONLY)]
    public void OnVipUspCommand(CCSPlayerController? player, CommandInfo command)
    {
        if (player is not { IsValid: true })
            return;

        if (Config.VipUspMode != VipUspMode.Choice)
        {
            command.ReplyToCommand(" This server doesn't allow choosing the USP model.");
            return;
        }

        var enabled = _vipUspPlayers.Add(player.SteamID);
        if (!enabled)
            _vipUspPlayers.Remove(player.SteamID);

        command.ReplyToCommand(enabled ? " VIP USP enabled." : " VIP USP disabled.");

        var active = player.PlayerPawn.Value?.WeaponServices?.ActiveWeapon.Value;
        if (active is { IsValid: true })
            ApplyModel(active);
    }
}
