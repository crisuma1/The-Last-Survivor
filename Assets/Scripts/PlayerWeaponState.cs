using System;

public enum WeaponMode
{
    Gun,
    Throw
}


public static class PlayerWeaponState
{
    public static WeaponMode CurrentMode { get; private set; } = WeaponMode.Gun;
    public static event Action<WeaponMode> OnWeaponModeChanged;

    public static void SetMode(WeaponMode mode)
    {
        if (CurrentMode == mode) return;
        CurrentMode = mode;
        OnWeaponModeChanged?.Invoke(mode);
    }
}
