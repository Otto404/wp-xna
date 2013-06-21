using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace zoyobar.game
{

	internal abstract class Scene
		: IScene, IMountable, IHandleable, IDisposable
	{

		protected static IList<Making> combine ( IList<Making> makingIs, IList<Making> makingIIs )
		{
			List<Making> makings = new List<Making> ( );

			if ( null != makingIs )
				makings.AddRange ( makingIs );

			if ( null != makingIIs )
				makings.AddRange ( makingIIs );

			return makings;
		}


		internal event EventHandler<SceneEventArgs> Closing;
		internal event EventHandler<SceneEventArgs> Closed;

		internal readonly GestureType GestureType;
		private bool isClosed;
		public bool IsClosed
		{
			get { return this.isClosed; }
			set { this.isClosed = value; }
		}
		protected bool isEnabled = true;
		public bool IsEnabled
		{
			get { return this.isEnabled; }
		}

		internal readonly bool IsBroken;

		internal readonly Vector2 Location;
		protected World world;
		public World World
		{
			get { return this.world; }
			set
			{
				this.resourceManager.World = value;
				this.world = value;
			}
		}

		private SpriteBatch spiritBatch;
		private readonly ResourceManager resourceManager;
		protected AudioManager audioManager;
		public AudioManager AudioManager
		{
			get { return this.audioManager; }
		}
		private readonly bool isBackgroundMusicLoop;

		protected readonly Dictionary<string, Making> makings = new Dictionary<string, Making> ( );
		public Dictionary<string, Making> Makings
		{
			get { return this.makings; }
		}

		protected Scene ( Vector2 location, GestureType gestureType )
			: this ( location, gestureType,
			null,
			null,
			false,
			true
			)
		{ }
		protected Scene ( Vector2 location, GestureType gestureType, IList<Resource> resources, IList<Making> makings )
			: this ( location, gestureType, resources, makings,
			false,
			true
			)
		{ }
		protected Scene ( Vector2 location, GestureType gestureType, IList<Resource> resources, IList<Making> makings, bool isBroken )
			: this ( location, gestureType, resources, makings, isBroken,
			true
			)
		{ }
		protected Scene ( Vector2 location, GestureType gestureType, IList<Resource> resources, IList<Making> makings, bool isBroken, bool isBackgroundMusicLoop )
		{
			this.Location = location;
			this.GestureType = gestureType;

			this.resourceManager = new ResourceManager ( resources );
			this.audioManager = new AudioManager ( );

			if ( null != makings )
				foreach ( Making making in makings )
					if ( null != making )
						this.makings.Add ( making.Name, making );

			foreach ( Making making in this.makings.Values )
				making.Init ( this );

			this.IsBroken = isBroken;
			this.isBackgroundMusicLoop = isBackgroundMusicLoop;
		}

		public virtual void LoadContent ( )
		{

			this.spiritBatch = this.world.Services.GetService ( typeof ( SpriteBatch ) ) as SpriteBatch;

			this.resourceManager.LoadContent ( );

			foreach ( Making making in this.makings.Values )
				making.InitResource ( this.resourceManager );

			this.audioManager.LoadContent ( this.resourceManager );

			this.audioManager.PlayMusic ( "scene.sound", this.isBackgroundMusicLoop );
		}

		public virtual void UnloadContent ( )
		{
			this.audioManager.UnloadContent ( );

			this.resourceManager.UnloadContent ( );
		}

		public virtual void Dispose ( )
		{

			foreach ( Making making in this.makings.Values )
				making.Dispose ( );

			this.UnloadContent ( );
		}

		protected virtual void updating ( GameTime time )
		{ }

		public void Update ( GameTime time )
		{

			if ( this.isClosed || !this.isEnabled )
				return;

			this.updating ( time );
		}

		protected virtual void drawing ( GameTime time, SpriteBatch batch )
		{ }

		public void Draw ( GameTime time )
		{

			if ( this.isClosed )
				return;

			this.spiritBatch.Begin ( );
			this.drawing ( time, this.spiritBatch );
			this.spiritBatch.End ( );
		}

		protected virtual void inputing ( Controller controller )
		{ }

		public bool Input ( Controller controller )
		{

			if ( this.isClosed || !this.isEnabled )
				return false;

			this.inputing ( controller );
			return true;
		}

		public void Close ( )
		{

			if ( this.isClosed )
				return;

			if ( null == this.world )
				throw new NullReferenceException ( "world can't be null when closing scene" );

			if ( null != this.Closing )
			{
				SceneEventArgs closingArg = new SceneEventArgs ( );

				this.Closing ( this, closingArg );

				if ( closingArg.IsCancel )
					return;

			}

			if ( null != this.Closed )
				this.Closed ( this, new SceneEventArgs ( ) );

			//this.world.RemoveScene ( new SceneUnloader ( this.world, this ) );
			this.world.RemoveScene ( this );
		}

	}

}
