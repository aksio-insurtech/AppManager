/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { field } from '@aksio/cratis-fundamentals';

import { ApplicationEnvironmentKey } from './ApplicationEnvironmentKey';

export class ApplicationEnvironment {

    @field(ApplicationEnvironmentKey)
    id!: ApplicationEnvironmentKey;

    @field(String)
    name!: string;

    @field(String)
    displayName!: string;

    @field(String)
    shortName!: string;
}
