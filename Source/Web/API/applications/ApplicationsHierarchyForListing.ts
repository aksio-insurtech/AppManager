/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { field } from '@aksio/cratis-fundamentals';

import { MicroserviceOnApplication } from './MicroserviceOnApplication';

export class ApplicationsHierarchyForListing {

    @field(String)
    id!: string;

    @field(String)
    name!: string;

    @field(MicroserviceOnApplication, true)
    microservices!: MicroserviceOnApplication[];
}
