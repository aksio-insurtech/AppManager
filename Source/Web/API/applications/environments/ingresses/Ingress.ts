/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { field } from '@aksio/cratis-fundamentals';

import { CustomDomain } from './CustomDomain';
import { IngressRoute } from './IngressRoute';

export class Ingress {

    @field(String)
    id!: string;

    @field(String)
    name!: string;

    @field(CustomDomain, true)
    customDomains!: CustomDomain[];

    @field(IngressRoute, true)
    routes!: IngressRoute[];
}
