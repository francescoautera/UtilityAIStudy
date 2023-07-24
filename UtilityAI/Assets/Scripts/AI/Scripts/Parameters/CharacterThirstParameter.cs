using UtilityAI;

public class CharacterThirstParameter : Parameter
{
    public override float GetValue(Thinker thinker)
    {
        var character = thinker.GetComponent<Character>();
        if(character == null)
        {
            return 0;
        }
        return character.Thirst;
    }
}
