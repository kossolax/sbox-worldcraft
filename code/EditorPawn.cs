using Sandbox;

namespace WorldCraft
{
	public partial class EditorPawn : ModelEntity
	{
		public override void Simulate( Client cl )
		{
			UpdatePosition( cl );
			if ( IsClient )
			{
		}

			if ( Input.Down( InputButton.Attack2 ) )
				return;

			UpdateTool();
			CurrentTool?.Simulate( cl );
		}
		public override void FrameSimulate( Client cl )
		{
			UpdatePosition( cl );
			CurrentTool?.FrameSimulate( cl );
		}
	}
}
