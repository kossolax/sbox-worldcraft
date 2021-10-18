using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace WorldCraft
{
	public partial class Hud : RootPanel
	{
		private bool _showMouse;
		public bool ShowMouse
		{
			set
			{
				if ( value != _showMouse )
				{
					SetClass( "accept-input", value );
					Style.PointerEvents = value ? "visible" : "none";
					Style.Dirty();
				}

				_showMouse = value;
			}
		}

		public Hud()
		{
			SetTemplate( "/UI/Hud.html" );
		}
	}
}
