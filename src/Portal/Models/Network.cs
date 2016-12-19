using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using Microsoft.Extensions.Options;

namespace Gilmond.WindowsService.Portal.Models
{
	public class Network : INetwork
	{
		private readonly IEnumerable<string> _machineNames;
		private readonly IEnumerable<string> _serviceNames;

		public Network(IOptions<MyOptions> options)
		{
			_machineNames = options.Value.MachineNameList;
			_serviceNames = options.Value.ServiceNameList;
		}

		public bool ChangeServiceStatus(string machineName, string serviceName, ServiceAction serviceAction)
		{
			switch (serviceAction)
			{
				case ServiceAction.Start:
					return StartService(serviceName, machineName);
				case ServiceAction.Stop:
					return StopService(serviceName, machineName);
				case ServiceAction.Restart:
					return RestartService(serviceName, machineName);
				default:
					return false;
			}
		}

		public IEnumerable<string> GetMachineNames()
			=> _machineNames;

		public IEnumerable<string> GetServiceNames()
			=> _serviceNames;

		public IDictionary<string, string> GetServiceStatuses(string machineName)
		{
			var statuses = new ConcurrentDictionary<string, string>();
			_serviceNames.AsParallel().ForAll(s => statuses.TryAdd(s, GetServiceStatus(machineName, s)));
			return statuses;
		}

		private bool RestartService(string servicename, string machineName)
			=> StopService(servicename, machineName) && StartService(servicename, machineName);

		private bool StartService(string servicename, string machineName)
		{
			try
			{
				using (var service = new ServiceController(servicename, machineName))
				{
					service.Start();
					service.WaitForStatus(ServiceControllerStatus.Running);
					return true;
				}
			}
			catch (InvalidOperationException)
			{
				return false;
			}

		}
		private bool StopService(string servicename, string machineName)
		{
			try
			{
				using (var service = new ServiceController(servicename, machineName))
				{
					if (service.CanStop)
					{
						service.Stop();
						service.WaitForStatus(ServiceControllerStatus.Stopped);
						return true;
					}
					else
					{
						return false;
					}
				}
			}
			catch (InvalidOperationException)
			{
				return false;
			}
		}

		private string GetServiceStatus(string machineName, string serviceName)
		{
			var serviceStatus = string.Empty;
			try
			{
				using (var serviceController = new ServiceController(serviceName, machineName))
				{
					serviceStatus = serviceController.Status.ToString();
				}
			}
			catch (InvalidOperationException)
			{
				serviceStatus = "Not Installed";
			}
			return serviceStatus;
		}
	}
}
