/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { field } from '@aksio/cratis-fundamentals';

import { DeployableOnEnvironmentKey } from './DeployableOnEnvironmentKey';
import { EnvironmentVariable } from './EnvironmentVariable';

export class EnvironmentVariablesForDeployable {

    @field(DeployableOnEnvironmentKey)
    id!: DeployableOnEnvironmentKey;

    @field(EnvironmentVariable, true)
    variables!: EnvironmentVariable[];
}
