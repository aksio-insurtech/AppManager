/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { field } from '@aksio/cratis-fundamentals';

import { ConfigFile } from './ConfigFile';

export class ConfigFilesForApplication {

    @field(String)
    id!: string;

    @field(ConfigFile, true)
    files!: ConfigFile[];
}
