namespace Basic
{
    public enum CardActionType
    {
        Moving,
        Attack,
        Debuff
    }

    public enum CardElementType
    {
        Air,
        Water,
        Earth,
        Fire
    }

    public enum AimGroups
    {
        Enemies,
        Friends,
        Self,
        AllExeptSelf,
        FriendsExeptSelf,
        AllAndSelf,
        EnemiesAndSelf,
        FriendsAndSelf,
        All
    }

    public enum Amount
    {
        One,
        Two,
        All
    }

    public enum DebuffType
    {
        Any,
        None,

        Attacked,
        Healed,
        Moved,

        Binding,
        Disarming,
        Rooting,
        Slowing,
        Speeding,
        Shielding,
        Weakening,
        Empowering
    }
}