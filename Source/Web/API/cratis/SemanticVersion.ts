/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { field } from '@aksio/cratis-fundamentals';


export class SemanticVersion {

    @field(Number)
    major!: number;

    @field(Number)
    minor!: number;

    @field(Number)
    patch!: number;

    @field(String)
    preRelease!: string;

    @field(String)
    metadata!: string;
}
