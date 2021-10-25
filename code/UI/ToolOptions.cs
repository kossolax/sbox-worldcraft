using Sandbox;
using Sandbox.UI;

namespace WorldCraft
{
	class ToolOptionsPanel : Panel
	{

		private string _activeTool;

		public ToolOptionsPanel()
		{
			StyleSheet.Load( "/ui/tooloptions.scss" );
		}

		[Event.Frame]
		private void OnFrame()
		{
			if ( Local.Pawn is not EditorPawn pawn )
			{
				return;
			}

			if ( string.IsNullOrEmpty( _activeTool )
				|| _activeTool != pawn.UserToolCurrent )
			{
				BuildOptions( pawn.UserToolCurrent );
			}
		}

		private void BuildOptions( string toolName )
		{
			DeleteChildren( true );

			var tool = Library.Create<BaseTool>( toolName );

			if ( tool == null )
			{
				// something is wrong
				return;
			}

			_activeTool = toolName;

			tool.BuildOptionsSheet( this );
		}

	}
}
