using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace zoyobar.game
{

	internal sealed class Controller
		: IDisposable
	{
		private GamePadState currentGamePadState;
		private bool gamePadConnected;

		internal readonly List<GestureSample> Gestures = new List<GestureSample> ( );
		private GamePadState lastGamePadState;
		internal readonly List<Motion> Motions = new List<Motion> ( );

		internal bool IsGestureEmpty
		{
			get { return this.Gestures.Count == 0; }
		}

		internal bool IsMotionEmpty
		{
			get { return this.Motions.Count == 0; }
		}

		internal Controller ( )
		{ this.gamePadConnected = false; }

		internal void Update ( )
		{
			this.lastGamePadState = this.currentGamePadState;
			this.currentGamePadState = GamePad.GetState ( PlayerIndex.One );
			this.gamePadConnected = this.currentGamePadState.IsConnected;

			this.Motions.Clear ( );

			foreach ( TouchLocation location in TouchPanel.GetState ( ) )
				this.Motions.Add ( Motion.Create ( location ) );

			this.Gestures.Clear ( );

			while ( TouchPanel.IsGestureAvailable )
				this.Gestures.Add ( TouchPanel.ReadGesture ( ) );

		}

		internal bool IsMotionExist ( MotionType type )
		{

			foreach ( Motion motion in this.Motions )
				if ( motion.Type == type )
					return true;

			return false;
		}

		internal bool IsButtonPress ( Buttons button )
		{ return null != this.currentGamePadState && this.currentGamePadState.IsButtonDown ( button ) && this.lastGamePadState.IsButtonUp ( button ); }

		public void Dispose ( )
		{
			this.Gestures.Clear ( );
			this.Motions.Clear ( );
		}

	}

}
