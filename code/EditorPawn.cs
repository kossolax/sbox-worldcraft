using Sandbox;
using System;

namespace WorldCraft
{
	public partial class EditorPawn : ModelEntity
	{
		public override void Simulate( Client cl )
		{
			UpdatePosition( cl );

			if ( IsClient )
			{
				if ( Input.Pressed( InputButton.View ) )
					Game.GridSize = MathF.Max( 1.0f, Game.GridSize / 2.0f );
				if ( Input.Pressed( InputButton.Voice ) )
					Game.GridSize = MathF.Min( 64.0f, Game.GridSize * 2.0f );

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
