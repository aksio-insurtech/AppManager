/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { field } from '@aksio/cratis-fundamentals';

import { DeployableKey } from './DeployableKey';

export class Deployable {

    @field(DeployableKey)
    id!: DeployableKey;

    @field(String)
    name!: string;

    @field(String)
    image!: string;
}
