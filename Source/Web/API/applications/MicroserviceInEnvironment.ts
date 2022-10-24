/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { field } from '@aksio/cratis-fundamentals';

import { Deployable } from './Deployable';

export class MicroserviceInEnvironment {

    @field(String)
    microserviceId!: string;

    @field(String)
    name!: string;

    @field(Deployable, true)
    deployables!: Deployable[];
}
