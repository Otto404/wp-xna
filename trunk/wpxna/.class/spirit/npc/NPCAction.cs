
namespace zoyobar.game
{

    internal abstract class NPCAction
    {

		internal readonly int Type;

		private long frameIndex;
		internal long FrameIndex
		{
			get { return this.frameIndex; }
		}

		internal readonly bool IsLoop;

		private readonly int loopCount;

		private int loopedCount = 0;
		internal int LoopedCount
		{
			get { return this.loopedCount; }
		}


		private readonly long intervalFrameCount;

		protected NPCAction ( NPCAction action )
		{
			this.Type = action.Type;

			this.frameIndex = action.frameIndex;

			this.IsLoop = action.IsLoop;
			this.loopCount = action.loopCount;
			this.intervalFrameCount = action.intervalFrameCount;
		}
		protected NPCAction ( int type, float second, float intervalSecond, int loopCount )
		{
			this.Type = type;

			this.frameIndex = World.ToFrameCount ( second );

			this.IsLoop = intervalSecond > 0;
			this.loopCount = loopCount;
			this.intervalFrameCount = World.ToFrameCount ( intervalSecond );
		}

		internal virtual bool Next ( )
		{

			if ( !this.IsLoop || ( this.loopCount > 0 && this.loopedCount >= this.loopCount ) )
				return false;

			this.frameIndex += this.intervalFrameCount;
			this.loopedCount++;
			return true;
		}

		internal abstract NPCAction Clone ( );

    }

}
