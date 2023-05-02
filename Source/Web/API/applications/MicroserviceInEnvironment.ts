/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { field } from '@aksio/cratis-fundamentals';

import { ModuleInEnvironment } from './ModuleInEnvironment';

export class MicroserviceInEnvironment {

    @field(String)
    microserviceId!: string;

    @field(String)
    name!: string;

    @field(ModuleInEnvironment, true)
    modules!: ModuleInEnvironment[];
}
