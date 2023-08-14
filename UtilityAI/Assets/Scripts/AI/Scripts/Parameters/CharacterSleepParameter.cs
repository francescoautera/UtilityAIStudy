using CharacterData;
using UtilityAI;

public class CharacterSleepParameter : Parameter {

	public override float GetValue(Thinker thinker) {
		var character = thinker.GetComponent<Character>();
		if(character == null)
		{
			return 0;
		}
		return character.Sleepy;
	}
}