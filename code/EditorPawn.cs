using Sandbox;

namespace WorldCraft
{
	public partial class EditorPawn : ModelEntity
	{
		public override void Simulate( Client cl )
		{
			UpdatePosition( cl );
		}

		public override void FrameSimulate( Client cl )
		{
			UpdatePosition( cl );
		}
	}
}
