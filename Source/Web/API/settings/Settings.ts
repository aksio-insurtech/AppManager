/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { field } from '@aksio/fundamentals';

import { AzureSubscription } from './AzureSubscription';
import { AzureServicePrincipal } from './AzureServicePrincipal';

export class Settings {

    @field(AzureSubscription, true)
    azureSubscriptions!: AzureSubscription[];

    @field(String)
    pulumiOrganization!: string;

    @field(String)
    pulumiAccessToken!: string;

    @field(String)
    mongoDBOrganizationId!: string;

    @field(String)
    mongoDBPublicKey!: string;

    @field(String)
    mongoDBPrivateKey!: string;

    @field(AzureServicePrincipal)
    servicePrincipal!: AzureServicePrincipal;
}
