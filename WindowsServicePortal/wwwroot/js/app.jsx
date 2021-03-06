﻿var statusUrlRoot = "/api/windowsservice/status/";
var machineListUrl = "/api/windowsservice/machinenames";

var MachineList = React.createClass({
    getInitialState: function () {
        return { data: [] };
    },
    componentWillMount: function () {
        var xhr = new XMLHttpRequest();
        xhr.open('get', this.props.url, true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            this.setState({ data: data });
        }.bind(this);
        xhr.send();
    },
    render: function () {
        var machineNodes = this.state.data.map(function (machine) {
            return (
                <Machine name={machine.name}>
                </Machine>
                );
        })
        return (
            <div className="machineList">
                {machineNodes}
            </div>)
        ;
    }
});

var Machine = React.createClass({
    render: function () {
        return (
          <div className="machine">
            <h2 className="machineName">
                {this.props.name}
            </h2>
              <ServiceList machineName={this.props.name} pollInterval={2000}/>
          </div>
      );
    }
});

var ServiceList = React.createClass({
    loadServiceStatusFromServer: function () {
        var xhr = new XMLHttpRequest();
        xhr.open('get', statusUrlRoot + this.props.machineName, true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            this.setState({ data: data });
        }.bind(this);
        xhr.send();
    },
    getInitialState: function () {
        return { data: [] };
    },
    componentWillMount: function () {
        this.loadServiceStatusFromServer();
        window.setInterval(this.loadServiceStatusFromServer, this.props.pollInterval)
    },
    render: function () {
        var serviceNodes = this.state.data.map(function (service) {
            return (
                <Service name={service.name} status={service.status}/>
                );
        })
        return (
            <div className="serviceList">
                {serviceNodes}
            </div>)
        ;
    }
});

var Service = React.createClass({
    render: function () {
        return (
          <div className="service">
              {this.props.name} {this.props.status}
          </div>
      );
    }
});

ReactDOM.render(
  <MachineList url={machineListUrl} />,
  document.getElementById('content')
);