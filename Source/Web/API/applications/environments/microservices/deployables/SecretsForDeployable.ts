/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { field } from '@aksio/cratis-fundamentals';

import { DeployableOnEnvironmentKey } from './DeployableOnEnvironmentKey';
import { Secret } from './Secret';

export class SecretsForDeployable {

    @field(DeployableOnEnvironmentKey)
    id!: DeployableOnEnvironmentKey;

    @field(Secret, true)
    secrets!: Secret[];
}
