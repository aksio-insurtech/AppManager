/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { field } from '@aksio/cratis-fundamentals';

import { TenantKey } from './TenantKey';

export class Tenant {

    @field(TenantKey)
    id!: TenantKey;

    @field(String)
    name!: string;

    @field(String)
    onBehalfOf!: string;

    @field(String)
    domain!: string;

    @field(String)
    certificateId!: string;
}
