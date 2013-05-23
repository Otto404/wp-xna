using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace zoyobar.game
{

	internal sealed class ResourceManager
		: IMountable
	{

		internal static IList<Resource> Combine ( IList<Resource> resourceIs, IList<Resource> resourceIIs )
		{
			List<Resource> resources = new List<Resource> ( );

			if ( null != resourceIs )
				resources.AddRange ( resourceIs );

			if ( null != resourceIIs )
				resources.AddRange ( resourceIIs );

			return resources;
		}

		private static string contentDirectory = "Content";
		internal static string ContentDirectory
		{
			get { return contentDirectory; }
			set { contentDirectory = string.IsNullOrEmpty ( value ) ? "Content" : value; }
		}

		private static string resolution = string.Empty;
		internal static string Resolution
		{
			set { resolution = string.IsNullOrEmpty ( value ) ? string.Empty : @"/" + value; }
		}

		private ContentManager contentManager;
		internal readonly IList<Resource> Resources;
		private readonly Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D> ( );
		private readonly Dictionary<string, SpriteFont> fonts = new Dictionary<string, SpriteFont> ( );
		private readonly Dictionary<string, SoundEffectInstance> sounds = new Dictionary<string, SoundEffectInstance> ( );
		private readonly Dictionary<string, Song> music = new Dictionary<string, Song> ( );
		internal World World;

		internal ResourceManager ( IList<Resource> resources )
		{ this.Resources = null == resources ? new Resource[] { } : resources; }

		public void LoadContent ( )
		{

			if ( null == this.contentManager )
				this.contentManager = new ContentManager ( this.World.Services, contentDirectory );

			try
			{

				foreach ( Resource resource in this.Resources )
					switch ( resource.Type )
					{
						case ResourceType.Image:
							this.textures.Add ( resource.Name, this.contentManager.Load<Texture2D> ( resolution + resource.Path ) );
							break;

						case ResourceType.Font:
							this.fonts.Add ( resource.Name, this.contentManager.Load<SpriteFont> ( resource.Path ) );
							break;

						case ResourceType.Sound:
							this.sounds.Add ( resource.Name, this.contentManager.Load<SoundEffect> ( resource.Path ).CreateInstance ( ) );
							break;

						case ResourceType.Music:
							this.music.Add ( resource.Name, this.contentManager.Load<Song> ( resource.Path ) );
							break;
					}

			}
			catch { }

		}

		public void UnloadContent ( )
		{

			foreach ( Texture2D texture in this.textures.Values )
				texture.Dispose ( );

			foreach ( SoundEffectInstance sound in this.sounds.Values )
				sound.Dispose ( );

			foreach ( Song song in this.music.Values )
				song.Dispose ( );

			this.textures.Clear ( );
			this.fonts.Clear ( );
			this.sounds.Clear ( );
			this.music.Clear ( );

			if ( !this.Resources.IsReadOnly )
				this.Resources.Clear ( );

			if ( null != this.contentManager )
				this.contentManager.Unload ( );

		}

		internal Texture2D GetTexture ( string name )
		{

			if ( string.IsNullOrEmpty ( name ) || !this.textures.ContainsKey ( name ) )
				throw new NullReferenceException ( string.Format ( "Texture2D <{0}> is not exist", name ) );

			return this.textures[ name ];
		}

		internal SpriteFont GetFont ( string name )
		{

			if ( string.IsNullOrEmpty ( name ) || !this.fonts.ContainsKey ( name ) )
				throw new NullReferenceException ( string.Format ( "SpriteFont <{0}> is not exist", name ) );

			return this.fonts[ name ];
		}

		internal SoundEffectInstance GetSound ( string name )
		{

			if ( string.IsNullOrEmpty ( name ) || !this.sounds.ContainsKey ( name ) )
				throw new NullReferenceException ( string.Format ( "SoundEffectInstance <{0}> is not exist", name ) );

			return this.sounds[ name ];
		}

		internal Song GetMusic ( string name )
		{

			if ( string.IsNullOrEmpty ( name ) || !this.music.ContainsKey ( name ) )
				throw new NullReferenceException ( string.Format ( "Song <{0}> is not exist", name ) );

			return this.music[ name ];
		}

	}

}
