namespace bombali.runners
{
	using System.Collections.Generic;
	using domain;
	using orm;

	public class BombaliServiceRunner : IRunner
	{
		private readonly IList<IPersistenceStore> persistence_stores;

		public BombaliServiceRunner(IEnumerable<IPersistenceStore> persistence_stores)
		{
			this.persistence_stores = new List<IPersistenceStore>(persistence_stores);
		}

		public void run_the_application()
		{
			IList<IMonitor> monitors = new List<IMonitor>();

			foreach (IPersistenceStore persistence_store in persistence_stores)
			{
				foreach (IMonitor monitor in persistence_store.map_to_monitors()) monitors.Add(monitor);
			}

			foreach (IMonitor monitor in monitors)
			{
				//new Thread(() => 
				//    { 
				if (monitor != null) monitor.start_monitoring();
				//}
				//).Start();
			}
		}
	}
}