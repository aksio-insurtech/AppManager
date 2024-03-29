/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { field } from '@aksio/fundamentals';

import { ApplicationEnvironmentDeploymentKey } from './ApplicationEnvironmentDeploymentKey';
import { ApplicationEnvironmentDeploymentStatus } from './ApplicationEnvironmentDeploymentStatus';

export class ApplicationEnvironmentDeployment {

    @field(ApplicationEnvironmentDeploymentKey)
    id!: ApplicationEnvironmentDeploymentKey;

    @field(Date)
    started!: Date;

    @field(Date)
    completedOrFailed!: Date;

    @field(Number)
    status!: ApplicationEnvironmentDeploymentStatus;

    @field(String, true)
    errors!: string[];

    @field(String)
    stackTrace!: string;

    @field(String)
    log!: string;
}
