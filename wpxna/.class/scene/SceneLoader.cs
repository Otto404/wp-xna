using System;
using System.Collections.Generic;
using System.Threading;

namespace zoyobar.game
{

	internal sealed class SceneLoader
		: IDisposable
	{
		internal event EventHandler<SceneLoaderEventArgs> Loaded;

		private readonly List<Scene> scenes = new List<Scene> ( );
		private readonly Type[] afterSceneTypes;

		internal SceneLoader ( World world, Scene[] scenes, Type[] afterSceneTypes )
		{

			if ( null == world )
				throw new ArgumentNullException ( "world", "world can't be null" );

			if ( null != scenes )
				foreach ( Scene scene in scenes )
					if ( null != scene )
					{
						scene.World = world;
						scene.IsClosed = false;

						this.scenes.Add ( scene );
					}

			this.afterSceneTypes = afterSceneTypes;
		}

		internal void LoadResource ( )
		{ this.loadContent ( ); }

		private void loadContent ( )
		{

			foreach ( Scene scene in this.scenes )
				scene.LoadContent ( );

			if ( null != this.Loaded )
			{
				SceneLoaderEventArgs loadedArg = new SceneLoaderEventArgs ( this.scenes, this.afterSceneTypes );

				this.Loaded ( this, loadedArg );
				loadedArg.Dispose ( );
			}

		}

		public void Dispose ( )
		{ this.scenes.Clear ( ); }

	}

}
