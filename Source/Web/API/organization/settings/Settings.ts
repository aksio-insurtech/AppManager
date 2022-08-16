/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { AzureSubscription } from './AzureSubscription';
import { AzureServicePrincipal } from './AzureServicePrincipal';

export type Settings = {
    azureSubscriptions: AzureSubscription[];
    pulumiOrganization: string;
    pulumiAccessToken: string;
    mongoDBOrganizationId: string;
    mongoDBPublicKey: string;
    mongoDBPrivateKey: string;
    servicePrincipal: AzureServicePrincipal;
    elasticUrl: string;
    elasticApiKey: string;
};
