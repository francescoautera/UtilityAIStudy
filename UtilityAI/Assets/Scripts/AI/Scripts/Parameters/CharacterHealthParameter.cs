using CharacterData;
using UtilityAI;

public class CharacterHealthParameter : Parameter
{
    public override float GetValue(Thinker thinker)
    {
        var character = thinker.GetComponent<Character>();
        if(character == null)
        {
            return 0;
        }
        return character.Health;
    }
}