using System.Collections.Generic;

namespace Gilmond.WindowsService.Portal.Models
{
	public interface INetwork
	{
		IDictionary<string, string> GetServiceStatuses(string machineName);
		IEnumerable<string> GetMachineNames();
		IEnumerable<string> GetServiceNames();
		bool ChangeServiceStatus(string machineName, string serviceName, ServiceAction serviceAction);
	}
}