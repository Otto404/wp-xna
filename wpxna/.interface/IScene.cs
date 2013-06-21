using System.Collections.Generic;

namespace zoyobar.game
{

	internal interface IScene
	{
		World World
		{
			get;
			set;
		}

		Dictionary<string, Making> Makings
		{
			get;
		}

		AudioManager AudioManager
		{
			get;
		}

		bool IsEnabled
		{
			get;
		}

		bool IsClosed
		{
			get;
		}

		void Close ( );

	}

}
