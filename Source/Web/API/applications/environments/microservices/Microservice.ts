/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { field } from '@aksio/cratis-fundamentals';

import { MicroserviceKey } from './MicroserviceKey';

export class Microservice {

    @field(MicroserviceKey)
    id!: MicroserviceKey;

    @field(String)
    name!: string;
}
