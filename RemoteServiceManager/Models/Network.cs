﻿using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace RemoteServiceManager.Models
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
			try
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
			catch (InvalidOperationException)
			{
				return false;
			}
		}

		public IEnumerable<string> GetMachineNames()
			=> _machineNames;

		public IEnumerable<string> GetServiceNames()
			=> _serviceNames;

		public IEnumerable<Tuple<string, string>> GetServiceStatuses(string machineName)
		{
			var statuses = new List<Tuple<string, string>>();
			Parallel.ForEach(_serviceNames, (serviceName) =>
				statuses.Add(Tuple.Create(serviceName, GetServiceStatus(machineName, serviceName))));
			return statuses;
		}

		public bool RestartService(string servicename, string machineName)
		{
			throw new NotImplementedException();
		}

		public bool StartService(string servicename, string machineName)
		{
			throw new NotImplementedException();
		}

		public bool StopService(string servicename, string machineName)
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
