using System.Security.Principal;
using GD.Models.Commons;

namespace GoldDataWeb.Models
{
	public class UserIdentity : IPrincipal
	{
		public UserIdentity(IIdentity identity)
		{
			Identity = identity;
		}

		public IIdentity Identity{ get; }

		public Auth Auth { get; set; }

		public Rol Rol { get; set; }

		public bool IsInRole(string role)
		{
			return true;
		}

		public bool IsRole(Rol.Type type)
		{
			return Rol.Id == (int)type;
		}
	}
}
