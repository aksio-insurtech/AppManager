/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { field } from '@aksio/fundamentals';

import { MicroserviceOnEnvironmentKey } from './MicroserviceOnEnvironmentKey';
import { ConfigFile } from './ConfigFile';

export class ConfigFilesForMicroservice {

    @field(MicroserviceOnEnvironmentKey)
    id!: MicroserviceOnEnvironmentKey;

    @field(ConfigFile, true)
    files!: ConfigFile[];
}
