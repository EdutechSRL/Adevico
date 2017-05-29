using System;
namespace lm.Comol.Core.DomainModel.Common
{
	[Serializable(), CLSCompliant(true)]
	public class DomainPresenter : iDomainPresenter
	{

		private iDomainView _View;
		private iApplicationContext _AppContext;

		protected iDomainManager _CurrentManager;
		#region "Public Person"
		public iApplicationContext AppContext {
			get { return _AppContext; }
		}
		public iDataContext DataContext {
			get { return _AppContext.DataContext; }
		}
		public iUserContext UserContext {
			get { return _AppContext.UserContext; }
		}
		public iDomainView View {
			get { return _View; }
		}
		public iDomainManager CurrentManager {
			get { return _CurrentManager; }
			set { _CurrentManager = value; }
		}
		#endregion
		public DomainPresenter(iApplicationContext oContext)
		{
			this._AppContext = oContext;
		}
		public DomainPresenter(iApplicationContext oContext, iDomainView view)
		{
			this._View = view;
			this._AppContext = oContext;
		}

	}
}