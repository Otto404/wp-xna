using System.Collections.Generic;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace zoyobar.game
{

	internal sealed class AudioManager
	{

		internal static bool IsSoundOn = true;
		internal static bool IsMusicOn = true;

		internal static void Stop ( )
		{

			if ( MediaPlayer.GameHasControl )
				MediaPlayer.Stop ( );

		}

		private readonly Dictionary<string, SoundEffectInstance> sounds = new Dictionary<string, SoundEffectInstance> ( );
		private readonly Dictionary<string, Song> music = new Dictionary<string, Song> ( );
		private bool isMusicPlaying = false;

		internal AudioManager ( )
		{ }

		internal void LoadContent ( ResourceManager resourceManager )
		{

			if ( null == resourceManager )
				return;

			foreach ( Resource resource in resourceManager.Resources )
				if ( resource.Type == ResourceType.Sound )
					this.sounds.Add ( resource.Name, resourceManager.GetSound ( resource.Name ) );
				else if ( resource.Type == ResourceType.Music )
					this.music.Add ( resource.Name, resourceManager.GetMusic ( resource.Name ) );

		}

		internal void UnloadContent ( )
		{

			this.StopAllSound ( );

			this.StopMusic ( );

			this.sounds.Clear ( );
			this.music.Clear ( );
		}

		internal void PlaySound ( string name )
		{

			if ( !IsSoundOn || string.IsNullOrEmpty ( name ) || !this.sounds.ContainsKey ( name ) )
				return;

			if ( this.sounds[name].State != SoundState.Playing )
				this.sounds[name].Play ( );

		}

		internal void PlayMusic ( string name )
		{ this.PlayMusic ( name, true ); }
		internal void PlayMusic ( string name, bool isLoop )
		{

			if ( !IsMusicOn || !MediaPlayer.GameHasControl || string.IsNullOrEmpty ( name ) || !this.music.ContainsKey ( name ) )
				return;

			if ( MediaPlayer.State != MediaState.Stopped )
				MediaPlayer.Stop ( );

			try
			{
				MediaPlayer.Play ( this.music[name] );

				MediaPlayer.IsRepeating = isLoop;
				this.isMusicPlaying = true;
			}
			catch { }

		}

		internal void StopAllSound ( )
		{

			foreach ( SoundEffectInstance sound in this.sounds.Values )
				sound.Stop ( );

		}

		internal void ResumeMusic ( )
		{

			if ( !MediaPlayer.GameHasControl || this.isMusicPlaying )
				return;

			MediaPlayer.Resume ( );
			this.isMusicPlaying = true;
		}

		internal void PauseMusic ( )
		{

			if ( !MediaPlayer.GameHasControl || !this.isMusicPlaying )
				return;

			MediaPlayer.Pause ( );
			this.isMusicPlaying = false;
		}

		internal void StopMusic ( )
		{

			if ( !MediaPlayer.GameHasControl || !this.isMusicPlaying )
				return;

			MediaPlayer.Stop ( );
			this.isMusicPlaying = false;
		}


	}

}
