using System;

namespace zoyobar.game
{

	internal class ButtonEventArgs
		: EventArgs
	{
		internal readonly string Command;

		internal ButtonEventArgs ( string command )
		{ this.Command = command; }

	}

}
