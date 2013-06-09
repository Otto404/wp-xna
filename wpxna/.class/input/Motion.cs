using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace zoyobar.game
{

	internal sealed class Motion
	{

		internal static Motion Create ( TouchLocation location )
		{ return Create ( location, false ); }
		internal static Motion Create ( TouchLocation location, bool is8D )
		{
			Motion motion = new Motion ( );

			switch ( location.State )
			{
				case TouchLocationState.Invalid:
					motion.Type = MotionType.None;
					break;

				case TouchLocationState.Pressed:
					motion.Type = MotionType.Down;
					break;

				case TouchLocationState.Moved:
					motion.Type = MotionType.Press;
					break;

				case TouchLocationState.Released:
					motion.Type = MotionType.Up;
					break;
			}

			motion.Position = location.Position / World.Scale;

			TouchLocation prevLocation;

			if ( location.TryGetPreviousLocation ( out prevLocation ) && prevLocation.State != TouchLocationState.Invalid )
				motion.Offset = ( location.Position - prevLocation.Position ) / World.Scale;

			return motion;
		}

		internal MotionType Type;
		internal Vector2 Position;

		internal Vector2 Offset;

		internal Motion ( )
		{ }

	}

}
