/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { field } from '@aksio/cratis-fundamentals';

import { EnvironmentVariable } from './EnvironmentVariable';

export class EnvironmentVariablesForDeployable {

    @field(String)
    id!: string;

    @field(EnvironmentVariable, true)
    variables!: EnvironmentVariable[];
}