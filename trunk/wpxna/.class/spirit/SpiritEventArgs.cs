using System;
using Microsoft.Xna.Framework;

namespace zoyobar.game
{

	internal class SpiritEventArgs
		: EventArgs
	{
		internal bool IsCancel;
		internal readonly int Type;
		internal readonly Vector2 Location;
		internal int Angle;
		internal readonly int Width;
		internal readonly int Height;

		internal SpiritEventArgs ( )
			: this ( null, false )
		{ }
		internal SpiritEventArgs ( Spirit spirit )
			: this ( spirit, false )
		{ }
		internal SpiritEventArgs ( bool isCancel )
			: this ( null, isCancel )
		{ }
		internal SpiritEventArgs ( Spirit spirit, bool isCancel )
		{

			if ( null != spirit )
			{
				this.Type = spirit.Type;
				this.Location = spirit.Location;
				this.Angle = spirit.Angle;
				this.Width = spirit.Width;
				this.Height = spirit.Height;
			}

			this.IsCancel = isCancel;
		}

	}

}
