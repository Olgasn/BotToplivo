using System.Collections.Generic;

namespace BotToplivo.Speller
{
    public class SpellResult
	{
		public List<Error> Errors
		{
			get;
			set;
		}

		public SpellResult()
		{
		}
	}
}
