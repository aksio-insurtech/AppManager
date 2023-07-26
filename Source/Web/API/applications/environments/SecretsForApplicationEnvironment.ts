/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { field } from '@aksio/fundamentals';

import { Secret } from './Secret';

export class SecretsForApplicationEnvironment {

    @field(String)
    id!: string;

    @field(Secret, true)
    secrets!: Secret[];
}
