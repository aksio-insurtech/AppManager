// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.Applications.Environments;
using Pulumi.Automation;

namespace Reactions.Applications.Pulumi;

/// <summary>
/// Defines a system for running Pulumi jobs.
/// </summary>
public interface IPulumiOperations
{
    /// <summary>
    /// Up a stack.
    /// </summary>
    /// <param name="application">Application stack belongs to.</param>
    /// <param name="definition">The configuration of a stack to up.</param>
    /// <param name="environment">The <see cref="ApplicationEnvironmentWithArtifacts"/>.</param>
    /// <param name="microservice">Optional <see cref="Microservice"/>.</param>
    /// <param name="upOptions">Optional <see cref="UpOptions"/>.</param>
    /// <param name="refreshOptions">Optional <see cref="RefreshOptions"/>.</param>
    /// <returns>Awaitable task.</returns>
    Task Up(Application application, Func<Task> definition, ApplicationEnvironmentWithArtifacts environment, Microservice? microservice = default, UpOptions? upOptions = default, RefreshOptions? refreshOptions = default);

    /// <summary>
    /// Down a stack.
    /// </summary>
    /// <param name="application">Application stack belongs to.</param>
    /// <param name="definition">The configuration of a stack to down.</param>
    /// <param name="environment">The <see cref="ApplicationEnvironmentWithArtifacts"/>.</param>
    /// <param name="microservice">Optional <see cref="Microservice"/>.</param>
    /// <returns>Awaitable task.</returns>
    Task Down(Application application, Func<Task> definition, ApplicationEnvironmentWithArtifacts environment, Microservice? microservice = default);

    /// <summary>
    /// Completely remove a stack and its history.
    /// </summary>
    /// <param name="application">Application stack belongs to.</param>
    /// <param name="definition">The configuration of a stack to down.</param>
    /// <param name="environment">The <see cref="ApplicationEnvironmentWithArtifacts"/>.</param>
    /// <param name="microservice">Optional <see cref="Microservice"/>.</param>
    /// <returns>Awaitable task.</returns>
    Task Remove(Application application, Func<Task> definition, ApplicationEnvironmentWithArtifacts environment, Microservice? microservice = default);

    /// <summary>
    /// Set a tag for a stack in a project.
    /// </summary>
    /// <param name="application">Application stack belongs to.</param>
    /// <param name="environment">The <see cref="ApplicationEnvironmentWithArtifacts"/>.</param>
    /// <param name="tagName">Tag to set.</param>
    /// <param name="value">Value of the tag.</param>
    /// <returns>Awaitable task.</returns>
    Task SetTag(Application application, ApplicationEnvironmentWithArtifacts environment, string tagName, string value);

    /// <summary>
    /// Consolidate an <see cref="ApplicationEnvironmentWithArtifacts"/>.
    /// </summary>
    /// <param name="application">Application the environment is for.</param>
    /// <param name="environment">The environment to consolidate.</param>
    /// <param name="consolidationId">The unique identifier of the consolidation.</param>
    /// <returns>Awaitable task.</returns>
    Task ConsolidateEnvironment(Application application, ApplicationEnvironmentWithArtifacts environment, ApplicationEnvironmentConsolidationId consolidationId);

    /// <summary>
    /// Set the image for a deployable.
    /// </summary>
    /// <param name="application">Application the environment is for.</param>
    /// <param name="environment">The environment to consolidate.</param>
    /// <param name="microservice">The microservice to set for.</param>
    /// <param name="consolidationId">The unique identifier of the consolidation.</param>
    /// <param name="deployable">The deployable on the microservice to set for.</param>
    /// <param name="image">The image to set.</param>
    /// <returns>Awaitable task.</returns>
    Task SetImageForDeployable(Application application, ApplicationEnvironmentWithArtifacts environment, Microservice microservice, ApplicationEnvironmentConsolidationId consolidationId, Deployable deployable, DeployableImageName image);
}
