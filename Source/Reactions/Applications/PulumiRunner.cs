// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Common;
using Concepts.Pulumi;
using Pulumi.Automation;

namespace Reactions.Applications
{
    /// <summary>
    /// Represents an implementation of <see cref="IPulumiRunner"/>.
    /// </summary>
    public class PulumiRunner : IPulumiRunner
    {
        readonly IApplicationSettings _applicationSettings;

        public PulumiRunner(IApplicationSettings applicationSettings) => _applicationSettings = applicationSettings;

        /// <inheritdoc/>
        public void Up(WorkspaceStack stack)
        {
            _ = Task.Run(async () =>
            {
                var accessToken = await _applicationSettings.GetPulumiAccessToken();
                Environment.SetEnvironmentVariable("PULUMI_ACCESS_TOKEN", accessToken.ToString());
                await stack.UpAsync(new UpOptions { OnStandardOutput = Console.WriteLine });
            });
        }

        /// <inheritdoc/>
        public void Down(WorkspaceStack stack)
        {
            _ = Task.Run(async () =>
            {
                var accessToken = await _applicationSettings.GetPulumiAccessToken();
                Environment.SetEnvironmentVariable("PULUMI_ACCESS_TOKEN", accessToken.ToString());
                await stack.DestroyAsync(new DestroyOptions { OnStandardOutput = Console.WriteLine });
            });
        }
    }
}
