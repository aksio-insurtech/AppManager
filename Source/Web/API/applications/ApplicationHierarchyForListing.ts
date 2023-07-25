/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { field } from '@aksio/fundamentals';

import { EnvironmentOnApplication } from './EnvironmentOnApplication';

export class ApplicationHierarchyForListing {

    @field(String)
    id!: string;

    @field(String)
    name!: string;

    @field(EnvironmentOnApplication, true)
    environments!: EnvironmentOnApplication[];
}
