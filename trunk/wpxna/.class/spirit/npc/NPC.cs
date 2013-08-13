using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace zoyobar.game
{

	internal abstract class NPC
		: Spirit, IAssailable
	{

		internal static string InjuredSoundName;
		internal static string DeadSoundName;

		internal event EventHandler<NPCEventArgs> Injured;

		private long frameCount = 0;

		protected readonly List<NPCAction> actions = new List<NPCAction> ( );
		protected readonly List<NPCAction> loopActions = new List<NPCAction> ( );
		private int currentActionIndex;
		protected int life;
		protected int protoLife;
		internal bool IsDied = false;

		internal NPCManager Manager;

		protected NPC ( IPlayScene scene, int type, Vector2 location, string movieName, string extendMovieName, float speed, int angle, HitArea hitArea, int width, int height, int life, IList<NPCAction> actions, double destroySecond, bool isMovieRotable, bool isAreaLimited, bool isAreaEntered, double areaSecond )
			: base ( scene, type, location, movieName, extendMovieName, speed, angle, hitArea, width, height, destroySecond, isMovieRotable, isAreaLimited, isAreaEntered, areaSecond )
		{

			this.life = life <= 0 ? 1 : life;
			this.protoLife = this.life;

			if ( null != actions )
				this.actions.AddRange ( actions );

			this.currentActionIndex = 0;
		}

		protected virtual void execute ( NPCAction action )
		{ }

		protected override void updating ( GameTime time )
		{
			this.frameCount++;

			foreach ( NPCAction action in this.loopActions.ToArray() )
				if ( action.FrameIndex <= this.frameCount )
				{
					this.execute ( action );

					if ( !action.Next ( ) )
						this.loopActions.Remove ( action );

				}

			for ( int index = this.currentActionIndex; index < this.actions.Count; index++ )
			{
				NPCAction action = this.actions[ index ];

				if ( action.FrameIndex > this.frameCount )
					break;
				else
				{
					this.execute ( action );

					if ( action.IsLoop )
					{
						this.loopActions.Add ( action );
						action.Next ( );
					}

					this.currentActionIndex++;
				}

			}

			base.updating ( time );
		}

		protected override void movieEnded ( object sender, MovieEventArgs e )
		{

			if ( e.SequenceName == "dead" )
				this.Destroy ( );

		}

		protected virtual void dying ( )
		{ }

		public virtual bool Injure ( int life, int type )
		{

			if ( this.IsDied )
				return true;

			this.scene.AudioManager.PlaySound ( InjuredSoundName );
			
			int injuredLife;

			if ( this.life < life )
				injuredLife = this.life;
			else
				injuredLife = life;

			this.life -= injuredLife;

			if ( null != this.Injured )
				this.Injured ( this, new NPCEventArgs ( this, injuredLife, type ) );

			if ( this.life <= 0 )
			{
				this.scene.AudioManager.PlaySound ( DeadSoundName );
				
				this.IsDied = true;
				this.PlayMovie ( "dead" );

				this.dying ( );
			}
			else
				this.PlayExtendMovie ( "flash" );

			return this.life <= 0;
		}

		protected override void Dispose ( bool disposing )
		{

			try
			{

				if ( disposing )
				{
					this.actions.Clear ( );

					this.loopActions.Clear ( );
				}

			}
			catch { }

			base.Dispose ( disposing );
		}

	}

}