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
        /// <param name="application">Application stack belongs to.</param>
        /// <param name="name">Name of stack.</param>
        /// <param name="definition">The configuration of a stack to up.</param>
        /// <param name="environment">The <see cref="RuntimeEnvironment"/>.</param>
        void Up(Application application, string name, PulumiFn definition, RuntimeEnvironment environment);

        /// <summary>
        /// Down a stack.
        /// </summary>
        /// <param name="application">Application stack belongs to.</param>
        /// <param name="name">Name of stack.</param>
        /// <param name="definition">The configuration of a stack to down.</param>
        /// <param name="environment">The <see cref="RuntimeEnvironment"/>.</param>
        void Down(Application application, string name, PulumiFn definition, RuntimeEnvironment environment);

        /// <summary>
        /// Completely remove a stack and its history.
        /// </summary>
        /// <param name="application">Application stack belongs to.</param>
        /// <param name="name">Name of stack.</param>
        /// <param name="definition">The configuration of a stack to down.</param>
        /// <param name="environment">The <see cref="RuntimeEnvironment"/>.</param>
        void Remove(Application application, string name, PulumiFn definition, RuntimeEnvironment environment);
    }
}
