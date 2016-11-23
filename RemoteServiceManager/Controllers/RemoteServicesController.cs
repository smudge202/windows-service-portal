﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RemoteServiceManager.Models;

namespace RemoteServiceManager.Controllers
{
	[Route("api/[controller]")]
	public class RemoteServicesController : Controller
	{
		private readonly INetwork _network;
		public RemoteServicesController(INetwork network)
		{
			_network = network;
		}

		[HttpGet("machineNames")]
		public IActionResult GetMachineNames()
			=> Json(_network.GetMachineNames());

		[HttpGet("serviceNames")]
		public IActionResult GetServiceNames()
			=> Json(_network.GetServiceNames());

		[HttpGet("status/{machineName}")]
		public IActionResult GetServiceStatuses(string machineName)
			=> Json(_network.GetServiceStatuses(machineName));

		[HttpGet("stop/{machineName}")]
		public IActionResult StopAllServices(string machineName)
			=> Json(_network.GetServiceNames().Select(s => StopService(machineName, s)));

		[HttpGet("start/{machineName}")]
		public IActionResult StartAllServices(string machineName)
			=> Json(_network.GetServiceNames().Select(s => StartService(machineName, s)));

		[HttpGet("restart/{machineName}")]
		public IActionResult RestartAllServices(string machineName)
			=> Json(_network.GetServiceNames().Select(s => RestartService(machineName, s)));

		[HttpGet("stop/{machineName}/{serviceName}")]
		public IActionResult StopService(string machineName, string serviceName)
			=> Json(_network.ChangeServiceStatus(machineName, serviceName, ServiceAction.Stop));

		[HttpGet("start/{machineName}/{serviceName}")]
		public IActionResult StartService(string machineName, string serviceName)
			=> Json(_network.ChangeServiceStatus(machineName, serviceName, ServiceAction.Start));

		[HttpGet("restart/{machineName}/{serviceName}")]
		public IActionResult RestartService(string machineName, string serviceName)
			=> Json(_network.ChangeServiceStatus(machineName, serviceName, ServiceAction.Restart));
	}
}