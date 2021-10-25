using Sandbox;
using Sandbox.UI;

namespace WorldCraft
{
	public partial class BaseTool : BaseNetworkable
	{
		public virtual void Simulate( Client cl ) { }
		public virtual void FrameSimulate( Client cl ) { }
		public virtual void BuildOptionsSheet( Panel panel ) { }
	}
}
