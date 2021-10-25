using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace WorldCraft
{
	[Library( "tool_select", Title = "selection", Icon = "SelectionTool.png" )]
	public partial class SelectionTool : BaseTool
	{
		// TODO: [Net] on this kills it.
		public PrimitiveEntity SelectedEntity { get; set; }

		public override void Simulate( Client cl )
		{
			var tr = Trace.Ray( Input.Cursor, 5000.0f ).Run();

			if ( Input.Pressed( InputButton.Attack1 ) )
			{
				SelectedEntity = tr.Entity as PrimitiveEntity;
			}

			if ( Input.Pressed( InputButton.Flashlight ) && Host.IsServer )
			{
				SelectedEntity?.Delete();
			}
		}

		public override void FrameSimulate( Client cl )
		{
			if ( SelectedEntity.IsValid() )
			{
				var bbox = SelectedEntity.CollisionBounds;
				DebugOverlay.Box( 0, SelectedEntity.Position, SelectedEntity.Rotation, bbox.Mins, bbox.Maxs, Color.Yellow );
				DebugOverlay.Axis( SelectedEntity.Position, SelectedEntity.Rotation, 16, depthTest: false );
			}
		}

		public override void BuildOptionsSheet( Panel panel )
		{
			base.BuildOptionsSheet( panel );

			panel.Add.Label( "Select Mode", "heading" );
			panel.Add.Button( "object", "active" );
			//panel.Add.Button( "Vertex" );
			//panel.Add.Button( "Face" );
			//panel.Add.Button( "Edge" );
		}

	}
}
