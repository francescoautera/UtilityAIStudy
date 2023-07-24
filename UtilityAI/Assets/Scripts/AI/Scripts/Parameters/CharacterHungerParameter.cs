using UtilityAI;

public class CharacterHungerParameter : Parameter
{
    public override float GetValue(Thinker thinker)
    {
        var character = thinker.GetComponent<Character>();
        if(character == null)
        {
            return 0;
        }
        return character.Hunger;
    }
}
