/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { field } from '@aksio/fundamentals';

import { IngressRoute } from './IngressRoute';

export class Ingress {

    @field(String)
    id!: string;

    @field(String)
    name!: string;

    @field(IngressRoute, true)
    routes!: IngressRoute[];
}
