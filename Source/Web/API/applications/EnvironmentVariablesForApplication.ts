/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { field } from '@aksio/fundamentals';

import { EnvironmentVariable } from './EnvironmentVariable';

export class EnvironmentVariablesForApplication {

    @field(String)
    id!: string;

    @field(EnvironmentVariable, true)
    variables!: EnvironmentVariable[];
}
