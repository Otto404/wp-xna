using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace zoyobar.game
{

	internal class Label
		: Making, ILockable
	{

		internal static void Draw ( Label label, SpriteBatch batch )
		{

			if ( !label.isVisible )
				return;

			Color color = label.color;

			if ( label.blink != 0 )
			{
				label.Alpha += label.blink;

				if ( label.Alpha <= 0.5 || label.Alpha >= 1 )
					label.blink = -label.blink;

			}

			if ( label.Alpha != 1 )
				color = color * label.Alpha;

			batch.DrawString ( label.font, label.Text, label.location * World.Scale, color, label.Rotation, Vector2.Zero, label.FontScale * ( label.Rotation == 0 ? World.Scale : World.FlipScale ), SpriteEffects.None, 0 );
		}

		internal static void InitSize ( Label label )
		{ InitSize ( label, false ); }
		internal static void InitSize ( Label label, bool isForce )
		{

			if ( null == label )
				return;

			if ( label.Width == 0 || isForce )
				label.Width = ( int ) ( label.font.MeasureString ( label.Text ).X * label.FontScale );

			if ( label.Height == 0 || isForce )
				label.Height = ( int ) ( label.font.LineSpacing * label.FontScale );

		}

		private float blink;
		internal float Alpha = 1;
		internal readonly float Rotation;

		internal string Text;
		internal float FontScale;
		protected SpriteFont font;
		protected Color color;

		protected Vector2 location;
		public Vector2 Location
		{
			get { return this.location; }
			set { this.location = value; }
		}

		internal int Width;
		internal int Height;
		internal Rectangle Bound;

		internal Label ( string name, string text, float fontScale, float blink )
			: this ( name, "peg", text, Vector2.Zero, 0, 0, fontScale, Color.White, blink, 1, 0 )
		{ }
		internal Label ( string name, string text, float fontScale, int angle )
			: this ( name, "peg", text, Vector2.Zero, 0, 0, fontScale, Color.White, 0, 1, angle )
		{ }
		internal Label ( string name, string text, float fontScale )
			: this ( name, "peg", text, Vector2.Zero, 0, 0, fontScale, Color.White, 0, 1, 0 )
		{ }
		internal Label ( string name, string text, float fontScale, Color color )
			: this ( name, "peg", text, Vector2.Zero, 0, 0, fontScale, color, 0, 1, 0 )
		{ }
		internal Label ( string name, string text, float fontScale, Color color, int angle )
			: this ( name, "peg", text, Vector2.Zero, 0, 0, fontScale, color, 0, 1, angle )
		{ }
		internal Label ( string name, string text, Vector2 location, int width, int height, float fontScale )
			: this ( name, "peg", text, location, width, height, fontScale, Color.White, 0, 1, 0 )
		{ }
		internal Label ( string name, string text, Vector2 location, float fontScale )
			: this ( name, "peg", text, location, 0, 0, fontScale, Color.White, 0, 1, 0 )
		{ }
		internal Label ( string name, string text, Vector2 location, float fontScale, float blink, float alpha )
			: this ( name, "peg", text, location, 0, 0, fontScale, Color.White, blink, alpha, 0 )
		{ }
		internal Label ( string name, string text, Vector2 location, float fontScale, float blink )
			: this ( name, "peg", text, location, 0, 0, fontScale, Color.White, blink, 1, 0 )
		{ }
		internal Label ( string name, string text, float fontScale, Color color, float blink )
			: this ( name, "peg", text, Vector2.Zero, 0, 0, fontScale, color, blink, 1, 0 )
		{ }
		internal Label ( string name, string text, Vector2 location, float fontScale, Color color )
			: this ( name, "peg", text, location, 0, 0, fontScale, color, 0, 1, 0 )
		{ }
		internal Label ( string name, string text, Vector2 location, float fontScale, Color color, float blink )
			: this ( name, "peg", text, location, 0, 0, fontScale, color, blink, 1, 0 )
		{ }
		internal Label ( string name, string text, Vector2 location, float fontScale, Color color, float blink, int angle )
			: this ( name, "peg", text, location, 0, 0, fontScale, color, blink, 1, angle )
		{ }
		internal Label ( string name, string text, Vector2 location, float fontScale, Color color, float blink, float alpha, int angle )
			: this ( name, "peg", text, location, 0, 0, fontScale, color, blink, alpha, angle )
		{ }
		internal Label ( string name, string resourceName, string text, Vector2 location, Color color )
			: this ( name, resourceName, text, location, 0, 0, 1, color, 0, 1, 0 )
		{ }
		internal Label ( string name, string resourceName, string text, Vector2 location, int width, float fontScale, Color color, float blink )
			: this ( name, resourceName, text, location, width, 0, fontScale, color, blink, 1, 0 )
		{ }
		internal Label ( string name, string resourceName, string text, Vector2 location, int width, int height, float fontScale, Color color, float blink )
			: this ( name, resourceName, text, location, width, height, fontScale, color, blink, 1, 0 )
		{ }
		internal Label ( string name, string resourceName, string text, Vector2 location, int width, int height, float fontScale, Color color, float blink, float alpha )
			: this ( name, resourceName, text, location, width, height, fontScale, color, blink, alpha, 0 )
		{ }
		internal Label ( string name, string resourceName, string text, Vector2 location, int width, int height, float fontScale, Color color, float blink, float alpha, int angle )
			: base ( name, resourceName )
		{

			if ( null == text )
				throw new ArgumentNullException ( "text", "text can't be null" );

			if ( width > 0 )
				this.Width = width;

			if ( height > 0 )
				this.Height = height;

			this.Text = text;
			this.location = location;
			this.FontScale = fontScale <= 0 ? 1 : fontScale;
			this.color = color;
			this.blink = blink;
			this.Alpha = alpha < 0 || alpha > 1 ? 1 : alpha;
			this.Rotation = Calculator.Radian ( angle );
		}

		internal override void InitResource ( ResourceManager resourceManager )
		{ this.font = resourceManager.GetFont ( this.resourceName ); }

	}

}
