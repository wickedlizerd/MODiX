using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

using Modix.Web.Protocol.Diagnostics;

namespace Modix.Web.Client.Diagnostics
{
    public class DiagnosticsViewModel
    {
        public DiagnosticsViewModel(IDiagnosticsContract diagnosticsContract)
        {
            _pingTestStartRequested = new Subject<Unit>();

            _pingTestStates = _pingTestStartRequested
                .Select(_ => diagnosticsContract.PerformPingTest()
                    .ToObservable())
                .Switch()
                .Scan(ImmutableList<PingTestState>.Empty, (states, response) => response switch
                {
                    PingTestDefinitions definitions => definitions.EndpointNames
                        .Select(endpointName => new PingTestState(
                            endpointName:   endpointName,
                            hasCompleted:   false,
                            latency:        null,
                            status:         EndpointStatus.Unknown))
                        .ToImmutableList(),
                    PingTestOutcome     outcome     => states.SetItem(
                        index:  states.FindIndex(0, state => state.EndpointName == outcome.EndpointName),
                        value:  new PingTestState(
                            endpointName:   outcome.EndpointName,
                            hasCompleted:   true,
                            latency:        outcome.Latency,
                            status:         outcome.Status)),
                    _                               => states
                })
                .Share();

            _isPingTestRunning = _pingTestStates
                .Select(states => states.Any(state => !state.HasCompleted));
        }

        public IObservable<bool> IsPingTestRunning
            => _isPingTestRunning;

        public IObservable<ImmutableList<PingTestState>> PingTestStates
            => _pingTestStates;

        public void StartPingTest()
            => _pingTestStartRequested.OnNext(Unit.Default);

        private readonly IObservable<bool> _isPingTestRunning;
        private readonly IObservable<ImmutableList<PingTestState>> _pingTestStates;
        private readonly Subject<Unit> _pingTestStartRequested;
    }
}
