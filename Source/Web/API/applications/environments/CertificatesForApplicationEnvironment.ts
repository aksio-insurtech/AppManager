/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { field } from '@aksio/cratis-fundamentals';

import { Certificate } from './Certificate';

export class CertificatesForApplicationEnvironment {

    @field(String)
    id!: string;

    @field(Certificate, true)
    certificates!: Certificate[];
}
