namespace CscoWeapons;

public record WeaponModel(ushort ItemIndex, string Name, string Model, bool HasMagazine = true)
{
    public string MagazineModel => Model.Replace(".vmdl", "_mag.vmdl");
}

public static class WeaponModels
{
    private const string Root = "models/weapons/csco";

    public const ushort Aug = 8;
    public const ushort Sg553 = 39;
    public const ushort UspSilencer = 61;

    public const string VipUsp = Root + "/usp_beta/weapon_pist_usp_silencer.vmdl";

    public static readonly WeaponModel[] All =
    {
        new(1, "deagle", Root + "/deagle/weapon_pist_deagle.vmdl"),
        new(2, "elite", Root + "/elite/weapon_pist_elite.vmdl"),
        new(3, "fiveseven", Root + "/fiveseven/weapon_pist_fiveseven.vmdl"),
        new(4, "glock", Root + "/glock18/weapon_pist_glock18.vmdl"),
        new(7, "ak47", Root + "/ak47/weapon_rif_ak47.vmdl"),
        new(Aug, "aug", Root + "/aug/weapon_rif_aug.vmdl"),
        new(9, "awp", Root + "/awp/weapon_snip_awp.vmdl"),
        new(10, "famas", Root + "/famas/weapon_rif_famas.vmdl"),
        new(11, "g3sg1", Root + "/g3sg1/weapon_snip_g3sg1.vmdl"),
        new(13, "galilar", Root + "/galil/weapon_rif_galilar.vmdl"),
        new(14, "m249", Root + "/m249/weapon_mach_m249.vmdl", HasMagazine: false),
        new(16, "m4a4", Root + "/m4a1_acog/weapon_rif_m4a4.vmdl"),
        new(17, "mac10", Root + "/mac10/weapon_smg_mac10.vmdl"),
        new(19, "p90", Root + "/p90/weapon_smg_p90.vmdl"),
        new(23, "mp5sd", Root + "/mp5/weapon_smg_mp5sd.vmdl"),
        new(24, "ump45", Root + "/ump45/weapon_smg_ump45.vmdl"),
        new(25, "xm1014", Root + "/xm1014/weapon_shot_xm1014.vmdl", HasMagazine: false),
        new(32, "hkp2000", Root + "/usp/weapon_pist_hkp2000.vmdl"),
        new(34, "mp9", Root + "/tmp/weapon_smg_mp9.vmdl"),
        new(35, "nova", Root + "/m3/weapon_shot_nova.vmdl", HasMagazine: false),
        new(36, "p250", Root + "/p228/weapon_pist_p250.vmdl"),
        new(38, "scar20", Root + "/sg550/weapon_snip_scar20.vmdl"),
        new(Sg553, "sg556", Root + "/sg552/weapon_rif_sg556.vmdl"),
        new(40, "ssg08", Root + "/scout/weapon_snip_ssg08.vmdl"),
        new(60, "m4a1_silencer", Root + "/m4a1/weapon_rif_m4a1_silencer.vmdl"),
        new(UspSilencer, "usp_silencer", Root + "/usp/weapon_pist_usp_silencer.vmdl"),
    };

    private static readonly Dictionary<ushort, WeaponModel> ByIndex = All.ToDictionary(w => w.ItemIndex);

    public static WeaponModel? Find(ushort itemIndex) => ByIndex.GetValueOrDefault(itemIndex);
}
