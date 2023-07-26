/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { field } from '@aksio/fundamentals';

import { MicroserviceOnEnvironmentKey } from './MicroserviceOnEnvironmentKey';
import { EnvironmentVariable } from './EnvironmentVariable';

export class EnvironmentVariablesForMicroservice {

    @field(MicroserviceOnEnvironmentKey)
    id!: MicroserviceOnEnvironmentKey;

    @field(EnvironmentVariable, true)
    variables!: EnvironmentVariable[];
}
