using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace zoyobar.game
{

	internal class NPCManager
		: SpiritManager<NPC>
	{
		protected long frameCount = 0;

		internal event EventHandler<SpiritEventArgs> Destroyed;

		internal event EventHandler<NPCEventArgs> Injured;

		internal NPCManager ( )
			: base ( )
		{ }

		protected override void spiritDestroyed ( object sender, SpiritEventArgs e )
		{

			if ( null != this.Destroyed )
				this.Destroyed ( sender, e );

			NPC npc = sender as NPC;
			npc.Injured -= this.Injured;
			npc.Manager = null;
			base.spiritDestroyed ( sender, e );
		}


		internal override void Append ( NPC spirit, int order )
		{

			spirit.Manager = this;
			spirit.Injured += this.Injured;
			base.Append ( spirit, order );
		}

		internal override void Update ( GameTime time )
		{
			this.frameCount++;
		}

		internal List<NPC> HitTest ( HitArea area )
		{
			List<NPC> npcs = new List<NPC> ( );

			foreach ( NPC npc in this.Spirits )
				if ( !npc.IsDied && area.HitTest ( npc.HitArea ) )
					npcs.Add ( npc );

			return npcs;
		}

		internal List<NPC[]> HitTest ( )
		{
			List<NPC[]> npcs = new List<NPC[]> ( );

			for ( int index1 = 0; index1 < this.Spirits.Count; index1++ )
			{
				NPC npc1 = this.Spirits[ index1 ];

				if ( npc1.IsDied )
					continue;

				for ( int index2 = index1 + 1; index2 < this.Spirits.Count; index2++ )
				{
					NPC npc2 = this.Spirits[ index2 ];

					if ( !npc2.IsDied && npc1.HitArea.HitTest ( npc2.HitArea ) )
						npcs.Add ( new NPC[] { npc1, npc2 } );

				}

			}

			return npcs;
		}

		internal void SetReelSpeed ( DirectionType direction, float speed )
		{

			foreach ( NPC npc in this.Spirits )
				npc.SetReelSpeed ( direction, speed );

		}

	}

}
