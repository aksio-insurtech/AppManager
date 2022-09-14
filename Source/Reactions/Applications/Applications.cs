// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts;
using Pulumi.Automation;
using Reactions.Applications.Pulumi;

namespace Reactions.Applications;

[Route("/api/applications")]
public class Applications : Controller
{
    readonly IPulumiStackDefinitions _stackDefinitions;
    readonly IImmediateProjections _projections;
    readonly IPulumiOperations _pulumiOperations;
    readonly ExecutionContext _executionContext;

    public Applications(
        IPulumiStackDefinitions stackDefinitions,
        IImmediateProjections projections,
        IPulumiOperations pulumiOperations,
        ExecutionContext executionContext)
    {
        _stackDefinitions = stackDefinitions;
        _projections = projections;
        _pulumiOperations = pulumiOperations;
        _executionContext = executionContext;
    }

    [HttpPost("{applicationId}/consolidate")]
    public async Task Consolidate([FromRoute] ApplicationId applicationId)
    {
        var application = await _projections.GetInstanceById<Application>(applicationId);
        var definition = PulumiFn.Create(() => _stackDefinitions.Application(_executionContext, application, CloudRuntimeEnvironment.Development));
        await _pulumiOperations.Up(application, application.Name, definition, CloudRuntimeEnvironment.Development);
    }
}
