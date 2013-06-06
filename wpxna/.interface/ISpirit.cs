
namespace zoyobar.game
{

	internal interface ISpirit
	{

		float Speed
		{
			get;
			set;
		}

		void PlayMovie ( string sequenceName );
		void PlayMovie ( string sequenceName, bool isRecord );
		void PlayMovie ( string sequenceName, bool isRecord, bool isReplay );
		void PlayExtendMovie ( string sequenceName );
		void PlayExtendMovie ( string sequenceName, bool isRecord );
		void PlayExtendMovie ( string sequenceName, bool isRecord, bool isReplay );
		void BackMovie ( string sequenceName );
		void BackExtendMovie ( string sequenceName );
	}

}
