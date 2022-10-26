/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { field } from '@aksio/cratis-fundamentals';


export class IngressRoute {

    @field(String)
    path!: string;

    @field(String)
    targetMicroservice!: string;

    @field(String)
    targetPath!: string;
}
