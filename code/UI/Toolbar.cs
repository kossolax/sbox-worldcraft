using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace WorldCraft
{
	public class ToolbarPanel : Panel
	{
		public class ToolPanel : Panel
		{
			public string ToolName;
			public ToolPanel( string name, string title, string icon )
			{
				ToolName = name;
				Add.Image( $"/UI/{icon}" );
				Add.Label( title );
				AddClass( "tool" );
			}

			public override void Tick()
			{
				if ( Local.Pawn is not EditorPawn pawn ) return;
				SetClass( "active", pawn.UserToolCurrent == ToolName );
			}

			protected override void OnClick( MousePanelEvent e )
			{
				if ( Local.Pawn is not EditorPawn pawn ) return;
				pawn.UserToolCurrent = ToolName;
			}
		}

		public ToolbarPanel()
		{
			StyleSheet.Load( "/UI/Toolbar.scss" );

			foreach ( var tool in Library.GetAllAttributes<BaseTool>() )
			{
				if ( tool.Title == "BaseTool" )	continue;

				var pnl = new ToolPanel( tool.Name, tool.Title, tool.Icon );
				pnl.Parent = this;
			}
		}
	}
}
