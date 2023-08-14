using CharacterData;
using UtilityAI;

public class CharacterFunParameter : Parameter {

	public override float GetValue(Thinker thinker) {
		var character = thinker.GetComponent<Character>();
		if(character == null)
		{
			return 0;
		}
		return character.Fun;
	}
}