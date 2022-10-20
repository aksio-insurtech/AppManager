/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { field } from '@aksio/cratis-fundamentals';

import { DeployableOnMicroservice } from './DeployableOnMicroservice';

export class MicroserviceInEnvironment {

    @field(String)
    microserviceId!: string;

    @field(String)
    name!: string;

    @field(DeployableOnMicroservice, true)
    deployables!: DeployableOnMicroservice[];
}
