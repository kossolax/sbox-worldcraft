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

		[Event.Hotload]
		private void OnHotload()
		{
			if ( IsClient )
			{
				// problem here.. on hotload ConVar.ClientData gets reset on server
				// but stays the same on client, doing this to remind the server
				UserToolCurrent = "junk"; 
				UserToolCurrent = previousTool;
			}
		}

	}
}
