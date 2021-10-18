using Sandbox;

namespace WorldCraft
{
	public partial class EditorPawn
	{
		[ConVar.ClientData( "tool_current" )]
		public string UserToolCurrent { get; set; } = "tool_select";
		[Net, Predicted] public BaseTool CurrentTool { get; set; }

		private string previousTool = "tool_select";

		// TODO: Is this really the best place for it?
		void UpdateTool()
		{
			if ( CurrentTool == null || UserToolCurrent != previousTool )
			{
				CurrentTool = Library.Create<BaseTool>( $"{UserToolCurrent}", false );
				previousTool = UserToolCurrent;
			}
		}
	}
}
