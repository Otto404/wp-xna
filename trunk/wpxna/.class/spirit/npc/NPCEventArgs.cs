using System;
using Microsoft.Xna.Framework;

namespace zoyobar.game
{

	internal class NPCEventArgs
		: SpiritEventArgs
	{
		internal readonly int InjuredLife;
		internal readonly int InjuredType;

		internal NPCEventArgs ( NPC npc )
			: this ( npc, 0, 0 )
		{ }
		internal NPCEventArgs ( NPC npc, int injuredLife, int injuredType )
			: base ( npc )
		{
			this.InjuredLife = injuredLife;
			this.InjuredType = injuredType;
		}

	}

}
