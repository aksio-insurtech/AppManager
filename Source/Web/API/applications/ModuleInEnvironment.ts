/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { field } from '@aksio/cratis-fundamentals';

import { DeployableOnMicroservice } from './DeployableOnMicroservice';

export class ModuleInEnvironment {

    @field(String)
    moduleId!: string;

    @field(String)
    name!: string;

    @field(DeployableOnMicroservice, true)
    deployables!: DeployableOnMicroservice[];
}
