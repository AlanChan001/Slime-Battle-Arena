interface IWeapon {

    public void Attack();
    public float GetWeaponCooldown();

    public int GetWeaponDamage();

    public float GetWeaponRange();

    public float GetStaminaCost();

    public bool GetPlayingAttackAnim();
}
