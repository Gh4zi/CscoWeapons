using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;

namespace CscoWeapons;

/// <summary>
/// CS 1.6 style zoom for the AUG and SG 553. The stock scope animation doesn't line up with the
/// Classic Offensive scopes, so it's blocked and replaced with a plain FOV zoom.
/// </summary>
public partial class CscoWeapons
{
    private const uint DefaultFov = 90;

    private readonly Dictionary<int, ZoomState> _zoom = new();

    private sealed class ZoomState
    {
        public bool Zoomed;
        public bool Attack2Held;
        public uint ActiveWeapon;
    }

    private void OnTick()
    {
        foreach (var player in Utilities.GetPlayers())
        {
            if (!player.IsValid || player.IsBot)
                continue;

            var state = GetZoomState(player);
            var weapon = player.PawnIsAlive ? player.PlayerPawn.Value?.WeaponServices?.ActiveWeapon.Value : null;
            var weaponIndex = weapon is { IsValid: true } ? weapon.Index : 0u;

            if (state.ActiveWeapon != weaponIndex)
            {
                Unzoom(player);
                state.ActiveWeapon = weaponIndex;
            }

            if (weapon == null || !UsesClassicZoom(weapon))
            {
                state.Attack2Held = false;
                continue;
            }

            BlockStockScope(weapon);

            var attack2 = (player.Buttons & PlayerButtons.Attack2) != 0;
            var pressed = attack2 && !state.Attack2Held;
            state.Attack2Held = attack2;

            if (weapon.As<CCSWeaponBase>().InReload)
            {
                Unzoom(player);
                continue;
            }

            if (!pressed)
                continue;

            if (state.Zoomed)
                Unzoom(player);
            else
                SetFov(player, (uint)Config.ClassicZoomFov);
        }
    }

    private bool UsesClassicZoom(CBasePlayerWeapon weapon)
    {
        var index = weapon.AttributeManager.Item.ItemDefinitionIndex;
        if (index != WeaponModels.Aug && index != WeaponModels.Sg553)
            return false;

        var model = WeaponModels.Find(index);
        return model != null && Config.IsEnabled(model.Name);
    }

    private static void BlockStockScope(CBasePlayerWeapon weapon)
    {
        if (weapon.NextSecondaryAttackTick >= Server.TickCount + 64)
            return;

        weapon.NextSecondaryAttackTick = Server.TickCount + 100_000;
        Utilities.SetStateChanged(weapon, "CBasePlayerWeapon", "m_nNextSecondaryAttackTick");
    }

    private void SetFov(CCSPlayerController player, uint fov)
    {
        player.DesiredFOV = fov;
        Utilities.SetStateChanged(player, "CBasePlayerController", "m_iDesiredFOV");
        GetZoomState(player).Zoomed = fov != DefaultFov;
    }

    private void Unzoom(CCSPlayerController? player)
    {
        if (player is { IsValid: true } && GetZoomState(player).Zoomed)
            SetFov(player, DefaultFov);
    }

    private ZoomState GetZoomState(CCSPlayerController player)
    {
        if (!_zoom.TryGetValue(player.Slot, out var state))
        {
            state = new ZoomState();
            _zoom[player.Slot] = state;
        }

        return state;
    }

    [GameEventHandler]
    public HookResult OnPlayerDeath(EventPlayerDeath @event, GameEventInfo info)
    {
        Unzoom(@event.Userid);
        return HookResult.Continue;
    }

    [GameEventHandler]
    public HookResult OnPlayerDisconnect(EventPlayerDisconnect @event, GameEventInfo info)
    {
        if (@event.Userid is { IsValid: true } player)
            _zoom.Remove(player.Slot);

        return HookResult.Continue;
    }
}
