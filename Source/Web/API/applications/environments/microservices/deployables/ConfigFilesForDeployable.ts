/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { field } from '@aksio/cratis-fundamentals';

import { DeployableOnEnvironmentKey } from './DeployableOnEnvironmentKey';
import { ConfigFile } from './ConfigFile';

export class ConfigFilesForDeployable {

    @field(DeployableOnEnvironmentKey)
    id!: DeployableOnEnvironmentKey;

    @field(ConfigFile, true)
    files!: ConfigFile[];
}
