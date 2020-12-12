using Entitas;

public class CharacterComponent : IComponent
{
    public CharacterType type;
    public CharacterState state;
    public Weapon weapon;
}
