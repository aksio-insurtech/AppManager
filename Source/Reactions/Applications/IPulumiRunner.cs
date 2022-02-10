// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Pulumi.Automation;

namespace Reactions.Applications
{
    /// <summary>
    /// Defines a system for running Pulumi jobs.
    /// </summary>
    public interface IPulumiRunner
    {
        /// <summary>
        /// Up a stack.
        /// </summary>
        /// <param name="stack">The stack to up.</param>
        void Up(WorkspaceStack stack);

        /// <summary>
        /// Down a stack.
        /// </summary>
        /// <param name="stack">The stack to down.</param>
        void Down(WorkspaceStack stack);
    }
}
