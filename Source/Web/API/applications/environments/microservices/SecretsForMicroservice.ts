/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { field } from '@aksio/fundamentals';

import { MicroserviceOnEnvironmentKey } from './MicroserviceOnEnvironmentKey';
import { Secret } from './Secret';

export class SecretsForMicroservice {

    @field(MicroserviceOnEnvironmentKey)
    id!: MicroserviceOnEnvironmentKey;

    @field(Secret, true)
    secrets!: Secret[];
}
