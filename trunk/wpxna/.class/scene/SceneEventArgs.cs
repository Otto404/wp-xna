using System;

namespace zoyobar.game
{

	internal class SceneEventArgs
		: EventArgs
	{
		internal bool IsCancel;
		internal string Command;

		internal SceneEventArgs ( )
			: this ( false, null )
		{ }
		internal SceneEventArgs ( string command )
			: this ( false, command )
		{ }
		internal SceneEventArgs ( bool isCancel )
			: this ( isCancel, null )
		{ }
		internal SceneEventArgs ( bool isCancel, string command )
		{
			this.IsCancel = isCancel;
			this.Command = command;
		}

	}

}
