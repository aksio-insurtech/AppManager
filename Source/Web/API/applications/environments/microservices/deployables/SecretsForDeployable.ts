/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { field } from '@aksio/cratis-fundamentals';

import { Secret } from './Secret';

export class SecretsForDeployable {

    @field(String)
    id!: string;

    @field(Secret, true)
    secrets!: Secret[];
}
