using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace zoyobar.game
{

    internal class Shape
		: Making
	{

		internal static void Draw ( Shape shape, GameTime time, SpriteBatch batch )
		{

			if ( !shape.isVisible )
				return;

			batch.Draw ( shape.Texture, shape.location * World.Scale, null, Color.White, 0, Vector2.Zero, World.TextureScale, SpriteEffects.None, shape.order );
		}

		internal Texture2D Texture;

		protected Vector2 location;
		public virtual Vector2 Location
		{
			get { return this.location; }
			set { this.location = value; }
		}

		protected readonly float order;

		internal Shape ( string name, string resourceName )
			: this ( name, resourceName,
			Vector2.Zero,
			0
			)
		{ }
		internal Shape ( string name, string resourceName, float order )
			: this ( name, resourceName,
			Vector2.Zero,
			order
			)
		{ }
		internal Shape ( string name, string resourceName, Vector2 location )
			: this ( name, resourceName, location,
			0
			)
		{ }
		internal Shape ( string name, string resourceName, Vector2 location, float order )
			: base ( name, resourceName )
		{
			this.location = location;
			this.order = order;
		}

		internal override void InitResource ( ResourceManager resourceManager )
		{ this.Texture = resourceManager.GetTexture ( this.resourceName ); }

    }

}
