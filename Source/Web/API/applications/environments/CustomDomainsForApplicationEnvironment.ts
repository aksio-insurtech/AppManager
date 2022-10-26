/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { field } from '@aksio/cratis-fundamentals';

import { CustomDomain } from './CustomDomain';

export class CustomDomainsForApplicationEnvironment {

    @field(String)
    id!: string;

    @field(CustomDomain, true)
    domains!: CustomDomain[];
}
