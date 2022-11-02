/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { field } from '@aksio/cratis-fundamentals';

import { TenantInEnvironment } from './TenantInEnvironment';
import { IngressInEnvironment } from './IngressInEnvironment';
import { MicroserviceInEnvironment } from './MicroserviceInEnvironment';

export class EnvironmentOnApplication {

    @field(String)
    environmentId!: string;

    @field(String)
    name!: string;

    @field(Date)
    lastUpdated!: Date;

    @field(Date)
    lastConsolidation!: Date;

    @field(TenantInEnvironment, true)
    tenants!: TenantInEnvironment[];

    @field(IngressInEnvironment, true)
    ingresses!: IngressInEnvironment[];

    @field(MicroserviceInEnvironment, true)
    microservices!: MicroserviceInEnvironment[];

    @field(Boolean)
    hasChanges!: boolean;
}
